using Akka.Actor;
using Akka.Configuration;
using Akka.Event;
using Akka.Pattern;
using Civitas.Api;
using Civitas.Api.Application.Health;
using Civitas.Api.Core.Entities;
using Civitas.Api.Core.Interfaces;
using Civitas.Api.Endpoints;
using Civitas.Api.Infrastructure.Actors;
using Civitas.Api.Infrastructure.Repositories;
using Prometheus;
using StackExchange.Redis;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Akka config loader
builder.Services.AddSingleton<AkkaConfigLoader>();
builder.Services.AddSingleton(_ =>
{
    var configLoader = _.GetRequiredService<AkkaConfigLoader>();
    return configLoader.LoadConfig();
});

// Register Akka system
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<Config>();
    return ActorSystem.Create("CivitasSystem", config);
});

// App services
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthorization();

// Register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetValue<string>("Redis:ConnectionString");
    if (string.IsNullOrEmpty(configuration))
        throw new ArgumentNullException($"Redis connection string is missing in configuration.");

    return ConnectionMultiplexer.Connect(configuration);
});

// Register MethodRegistry
builder.Services.AddSingleton<IMethodRegistry, MethodRegistry>();

// Register ReliableDeliveryActor with BackoffSupervisor
builder.Services.AddSingleton<IActorRef>(provider =>
{
    var system = provider.GetRequiredService<ActorSystem>();
    var registry = provider.GetRequiredService<IMethodRegistry>();
    var config = provider.GetRequiredService<Config>();

    // Create CircuitBreaker with settings from the config
    var maxFailures = config.GetInt("akka.circuit-breaker.max-failures");
    var callTimeout = config.GetTimeSpan("akka.circuit-breaker.call-timeout");
    var resetTimeout = config.GetTimeSpan("akka.circuit-breaker.reset-timeout");

    var breaker = new CircuitBreaker(
            system.Scheduler, maxFailures, callTimeout, resetTimeout)
        .OnOpen(() => system.Log.Warning("Circuit breaker opened"))
        .OnHalfOpen(() => system.Log.Info("Circuit breaker is half-open"))
        .OnClose(() => system.Log.Info("Circuit breaker closed"));

    var backoffProps = Backoff.OnFailure(
        Props.Create(() => new ReliableDeliveryActor(breaker, registry)),
        childName: "reliableDeliveryActor",
        minBackoff: TimeSpan.FromSeconds(3),
        maxBackoff: TimeSpan.FromSeconds(10),
        randomFactor: 0.2,
        maxNrOfRetries: 2
    );

    var supervisorProps = BackoffSupervisor.Props(backoffProps);
    return system.ActorOf(supervisorProps, "reliable-delivery-supervisor");
});

// Register culture settings
var cultureConfig = builder.Configuration.GetSection("CultureSettings");
var cultureInfo = new CultureInfo(cultureConfig["Culture"]!)
{
    DateTimeFormat =
    {
        ShortDatePattern = cultureConfig["ShortDatePattern"]!,
        LongDatePattern = cultureConfig["LongDatePattern"]!
    }
};
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Swagger
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerConfiguration(builder.Configuration);
}

var app = builder.AddHealth().Build();

// --- Prometheus: expose /metrics endpoint ---
app.UseMetricServer(); // This will expose /metrics automatically
app.UseHttpMetrics();  // Collects default HTTP metrics for requests

// Middleware and endpoints
app.UseMiddlewareConfiguration(app.Environment, builder.Configuration);
app.UseRouting();
app.UseAuthorization();
app.MapEmployeeEndpoints();
app.MapAccessControlEndpoints();
app.MapSalaryEndpoints();
app.UseHealth();

RegisterMethodHandlers(app.Services);

app.Run();

// ---------------------------------------------
// Local Method for Handler Registration
// ---------------------------------------------
void RegisterMethodHandlers(IServiceProvider services)
{
    var registry = services.GetRequiredService<IMethodRegistry>();
    var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();

    registry.Register("EmployeeRepository.AddEmployee", async payload =>
    {
        if (payload is not Employee employee)
            return false;

        using var scope = scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        await repository.AddEmployeeAsync(employee);
        return true;
    });
}

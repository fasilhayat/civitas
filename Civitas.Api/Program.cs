using Akka.Actor;
using Akka.Configuration;
using Akka.Pattern;
using Civitas.Api;
using Civitas.Api.Core.Interfaces;
using Civitas.Api.Endpoints;
using Civitas.Api.Infrastructure.Actors;
using Civitas.Api.Infrastructure.Repositories;
using StackExchange.Redis;
using System.Globalization;
using Civitas.Api.Core.Entities;

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

// Register ReliableDeliveryActor
builder.Services.AddSingleton<IActorRef>(provider =>
{
    var system = provider.GetRequiredService<ActorSystem>();
    var registry = provider.GetRequiredService<IMethodRegistry>();

    var breaker = new CircuitBreaker(
        scheduler: system.Scheduler,
        maxFailures: 5,
        callTimeout: TimeSpan.FromSeconds(10),
        resetTimeout: TimeSpan.FromSeconds(30)
    );

    var props = Props.Create(() => new ReliableDeliveryActor(breaker, registry));
    return system.ActorOf(props, "reliable-delivery-actor");
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

var app = builder.Build();

// Middleware and endpoints
app.UseMiddlewareConfiguration(app.Environment, builder.Configuration);
app.UseRouting();
app.UseAuthorization();
app.MapEmployeeEndpoints();
app.MapAccessControlEndpoints();
app.MapSalaryEndpoints();

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
        var employeeRepo = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        await employeeRepo.AddEmployeeAsync(employee);
        return true;
    });
}


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

// Register AkkaConfigLoader as a singleton so it can be injected
builder.Services.AddSingleton<AkkaConfigLoader>();

// Load Akka.NET configuration from 'akka.conf'
builder.Services.AddSingleton(_ =>
{
    var configLoader = _.GetRequiredService<AkkaConfigLoader>();
    var config = configLoader.LoadConfig(); // Ensure LoadConfig reads this file
    return config;
});

// Add Services
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthorization();

// Akka
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

builder.Services.AddSingleton<IMethodRegistry, MethodRegistry>();
//builder.Services.AddSingleton(provider =>
//{
//    var registry = provider.GetRequiredService<IMethodRegistry>();
//    var employeeRepo = provider.GetRequiredService<IEmployeeRepository>();

//    registry.Register("EmployeeRepository.AddEmployee", async payload =>
//    {
//        if (payload is not Employee employee) return false;

//        await employeeRepo.AddEmployeeAsync(employee);
//        return true;
//    });

//    return registry;
//});

// Register Akka ActorSystem
builder.Services.AddSingleton(_ =>
{
    var system = ActorSystem.Create("CivitasSystem", _.GetRequiredService<Config>());
    return system;
});

// Add Redis ConnectionMultiplexer using the configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetValue<string>("Redis:ConnectionString");
    if (string.IsNullOrEmpty(configuration))
    {
        throw new ArgumentNullException($"Redis connection string is missing in configuration.");
    }

    return ConnectionMultiplexer.Connect(configuration);
});

var cultureConfig = builder.Configuration.GetSection("CultureSettings");
var cultureInfo = new CultureInfo(cultureConfig["Culture"]!)
{
    DateTimeFormat =
    {
        ShortDatePattern = cultureConfig["ShortDatePattern"]!,
        LongDatePattern = cultureConfig["LongDatePattern"]!
    }
};

// Apply the culture globally
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add Swagger
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerConfiguration(builder.Configuration);
}

var app = builder.Build();

// Use Middlewares
app.UseMiddlewareConfiguration(app.Environment, builder.Configuration);

// Add routing middleware
app.UseRouting();
app.UseAuthorization();

// Map endpoints
app.MapEmployeeEndpoints();
app.MapAccessControlEndpoints();
app.MapSalaryEndpoints();


// Start the application
app.Run();
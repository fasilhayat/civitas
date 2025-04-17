using System.Globalization;
using Civitas.Api;
using Civitas.Api.Endpoints;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthorization();

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
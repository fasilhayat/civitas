//using System.Globalization;

//var builder = WebApplication.CreateBuilder(args);

//// Register Swagger services
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Enable Swagger UI in development
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Minimal API endpoints
//app.MapGet("/", () => Results.Ok("Welcome to Civitas API (Minimal API Style)"));

//app.MapGet("/employees", () =>
//{
//    var employees = new[]
//    {
//        new { Id = 1, Name = "Alice", Department = "HR" },
//        new { Id = 2, Name = "Bob", Department = "IT" }
//    };

//    return Results.Ok(employees);
//});

//app.Run();

using System.Globalization;
using Civitas.Api;
using Civitas.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthorization();

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

// Start the application
app.Run();

var builder = WebApplication.CreateBuilder(args);

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Minimal API endpoints
app.MapGet("/", () => Results.Ok("👋 Welcome to Civitas API (Minimal API Style)"));

app.MapGet("/employees", () =>
{
    var employees = new[]
    {
        new { Id = 1, Name = "Alice", Department = "HR" },
        new { Id = 2, Name = "Bob", Department = "IT" }
    };

    return Results.Ok(employees);
});

app.Run();
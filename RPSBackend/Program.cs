using RpsBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Enable controllers (MVC-style endpoints)
builder.Services.AddControllers();
builder.Services.AddScoped<RpsAiService>();
builder.Services.AddScoped<AlgorithmTestingService>();

// Optional: Generates an OpenAPI JSON at /openapi/v1.json
// You can ignore this if you don't use it.
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Exposes the raw OpenAPI JSON (not Swagger UI)
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map your controllers in /Controllers folder
app.MapControllers();

app.Run();

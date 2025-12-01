using Microsoft.EntityFrameworkCore;
using RpsBackend.Data;
using RpsBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// This will read from:
// appsettings.json
// appsettings.{Environment}.json  (e.g. Development)
// user-secrets (if configured)
// env vars (for ConnectionStrings:DefaultConnection)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Using connection string: {connectionString}");

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Controllers
builder.Services.AddControllers();

// Your services
builder.Services.AddScoped<RpsGameService>();
builder.Services.AddScoped<AlgorithmTestingService>();
builder.Services.AddScoped<StatsGatheringService>();

// CORS compliance
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:5173"); // your frontend dev server
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

// Dev-only stuff if you want later
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

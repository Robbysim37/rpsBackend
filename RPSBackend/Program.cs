using Microsoft.EntityFrameworkCore;
using RpsBackend.Data;
using RpsBackend.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

// Controllers
builder.Services.AddControllers();

// Your services
builder.Services.AddScoped<RpsGameService>();
builder.Services.AddScoped<AlgorithmTestingService>();

var app = builder.Build();

// Dev-only stuff if you want later
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

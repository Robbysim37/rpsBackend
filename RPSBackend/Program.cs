using Microsoft.EntityFrameworkCore;
using RpsBackend.Data;
using RpsBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
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

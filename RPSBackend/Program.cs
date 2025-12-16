using Microsoft.EntityFrameworkCore;
using RpsBackend.Data;
using RpsBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RpsBackend.Config;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var connectionString =
    Environment.GetEnvironmentVariable("DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Controllers
builder.Services.AddControllers();

// Services
builder.Services.AddScoped<RpsGameService>();
builder.Services.AddScoped<AlgorithmTestingService>();
builder.Services.AddScoped<StatsGatheringService>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<AuthService>();

// Bind Jwt settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

// Register JWT generator service
builder.Services.AddScoped<IJwt, JwtService>();

// JWT Bearer validation middleware
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = jwt.Issuer,

          ValidateAudience = true,
          ValidAudience = jwt.Audience,

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),

          ValidateLifetime = true,
          ClockSkew = TimeSpan.FromMinutes(2)
      };

      options.Events = new JwtBearerEvents
      {
          OnAuthenticationFailed = ctx =>
          {
              Console.WriteLine("AUTH FAILED: " + ctx.Exception.ToString());
              return Task.CompletedTask;
          },
          OnChallenge = ctx =>
          {
              Console.WriteLine($"CHALLENGE: {ctx.Error} | {ctx.ErrorDescription}");
              return Task.CompletedTask;
          }
      };
  });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://rps-frontend-tan.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

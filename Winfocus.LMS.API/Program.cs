using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Winfocus.LMS.API.Middlewares;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;
using Winfocus.LMS.Infrastructure.DataSeeders;
using Winfocus.LMS.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Configure Serilog to write logs into a folder inside the project
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/lms-log-.txt",          // relative path inside project
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 60,         // keep last 60 days
        fileSizeLimitBytes: 10_000_000,     // 10 MB per file
        rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Host.UseSerilog();

// EF Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<JwtService>();

// Register password hasher for User entity
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Configure JWT authentication
var jwtKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured. Please set 'Jwt:Key' in your configuration.");
var issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
var audience = config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

var key = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations and seed initial data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        db.Database.Migrate();
        CountryDataSeeder.Seed(db);
        RoleDataSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database migration or seeding failed. Application will stop.");
        throw; // rethrow to stop the app cleanly
    }
}

// Use global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

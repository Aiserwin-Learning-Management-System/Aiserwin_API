using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Winfocus.LMS.API.Middleware;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;
using Winfocus.LMS.Infrastructure.DataSeeders;
using Winfocus.LMS.Infrastructure.Repositories;
using Winfocus.LMS.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// =====================
// Serilog Configuration
// =====================
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/lms-log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 60,
        fileSizeLimitBytes: 10_000_000,
        rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Host.UseSerilog();

// =====================
// Database
// =====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// =====================
// Dependency Injection
// =====================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IModeOfStudyService, ModeOfStudyService>();
builder.Services.AddScoped<IModeOfStudyRepository, ModeOfStudyRepository>();
builder.Services.AddScoped<ICentreService, CentreService>();
builder.Services.AddScoped<ICentreRepository, CentreRepository>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// =====================
// JWT Authentication
// =====================
var jwtKey = config["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");

var issuer = config["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");

var audience = config["Jwt:Audience"]
    ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

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
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =====================
// Global Exception Middleware (FIRST)
// =====================
app.UseMiddleware<GlobalExceptionMiddleware>();

// =====================
// Database Migration + Seeding
// =====================
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
        logger.LogCritical(ex, "Database migration or seeding failed.");
        throw;
    }
}

// =====================
// HTTP Pipeline
// =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

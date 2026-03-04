using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;
using Winfocus.LMS.API.Middleware;
using Winfocus.LMS.Application.Configuration;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;
using Winfocus.LMS.Infrastructure.DataSeeders;
using Winfocus.LMS.Infrastructure.Repositories;
using Winfocus.LMS.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

#region Serilog Configuration

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

#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

#endregion

#region Database

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
}

#endregion

#region Dependency Injection

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
builder.Services.AddScoped<ICenterService, CentreService>();
builder.Services.AddScoped<ICentreRepository, CentreRepository>();
builder.Services.AddScoped<ISyllabusService, SyllabusService>();
builder.Services.AddScoped<ISyllabusRepository, SyllabusRepository>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IStreamService, StreamService>();
builder.Services.AddScoped<IStreamRepository, StreamRepository>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IBatchTimingSundayService, BatchTimingSundayService>();
builder.Services.AddScoped<IBatchTimingSundayRepository, BatchTimingSundayRepository>();
builder.Services.AddScoped<IBatchTimingSaturdayService, BatchTimingSaturdayService>();
builder.Services.AddScoped<IBatchTimingSaturdayRepository, BatchTimingSaturdayRepository>();
builder.Services.AddScoped<IBatchTimingMTFService, BatchTimingMTFService>();
builder.Services.AddScoped<IBatchTimingMTFRepository, BatchTiminingMTFRepository>();
builder.Services.AddScoped<IStudentAcademicdetailsService, StudentAcademicdetailsService>();
builder.Services.AddScoped<IStudentAcademicdetailsRepository, StudentAcademicdetailsRepository>();
builder.Services.AddScoped<IStudentPersonaldetailsService, StudentPersonaldetailService>();
builder.Services.AddScoped<IStudentPersonaldetailsRepository, StudentPersonaldetailsRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IUserActivationTokenRepository, UserActivationTokenRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAcademicYearService, AcademiYearService>();
builder.Services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
builder.Services.AddScoped<IFeeRepository, FeeRepository>();
builder.Services.AddScoped<IFeeService, FeeService>();
builder.Services.AddScoped<IUserLoginLogRepository, UserLoginLogRepository>();
builder.Services.AddScoped<IUserLoginLogService, UserLoginLogService>();
builder.Services.AddScoped<IUsernameGeneratorService, UsernameGeneratorService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IBatchService, BatchService>();
builder.Services.AddScoped<IBatchRepository, BatchRepository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();

#endregion

#region JWT Authentication

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

#endregion

#region Controllers

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This stops the infinite loop during JSON serialization
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

#endregion

#region API Versioning

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#endregion

#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 1. Define the Security Scheme (The "Authorize" button)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text box below. \r\n\r\nExample: '12345abcdef'",
    });

    // 2. Define the Security Requirement (Apply to all endpoints)
    options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        },
    });

    // This helps Swagger find the XML comments if you have them enabled
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

#endregion

#region Rate Limiting

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("SetPasswordPolicy", config =>
    {
        config.PermitLimit = 1;                 // 5 requests
        config.Window = TimeSpan.FromMinutes(1); // per minute
        config.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 2;
    });
});

#endregion

var app = builder.Build();

#region Global Exception Middleware

app.UseMiddleware<GlobalExceptionMiddleware>();

#endregion

#region Database Migration + Seeding

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
       // db.Database.Migrate();
        CountryDataSeeder.Seed(db);
       // StateDataSeeder.Seed(db);
        RoleDataSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database migration or seeding failed.");
        throw;
    }
}

#endregion

#region Swagger UI

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    var provider = app.Services
        .GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"Winfocus LMS API {description.GroupName.ToUpper()}");
        }
    });
}

#endregion

#region Pipeline

app.UseCors("AllowAngularApp");

app.UseAuthentication();

app.UseMiddleware<SessionValidationMiddleware>();

app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

#endregion

app.Run();

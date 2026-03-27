using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;
using Winfocus.LMS.API.Authorization;
using Winfocus.LMS.API.Middleware;
using Winfocus.LMS.Application.Configuration;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Application.Mapping;
using Winfocus.LMS.Application.Services;
using Winfocus.LMS.Application.Settings;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;
using Winfocus.LMS.Infrastructure.DataSeeders;
using Winfocus.LMS.Infrastructure.Repositories;
using Winfocus.LMS.Infrastructure.Security;
using Winfocus.LMS.Application.Mapping;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.Configure<FileUploadSettings>(
    builder.Configuration.GetSection(FileUploadSettings.SectionName));

#region Serilog Configuration

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "/home/logs/lms-log-.txt",
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
        var allowedOrigins = builder.Environment.IsDevelopment()
            ? new[] { "http://localhost:4200" }
            : new[] { "https://icy-plant-0ad05eb00.4.azurestaticapps.net", "http://localhost:4200" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

#endregion

#region Database

if (!builder.Environment.IsEnvironment("Testing"))
{
    var connStr = config.GetConnectionString("DefaultConnection");
    Log.Information("Connection string length: {Length}", connStr?.Length);
    Log.Information(
        "Contains server keyword: {HasServer}",
        connStr?.Contains("aiserwin-sql-dev"));
    Log.Information(
        "Contains password end: {HasEnd}",
        connStr?.Contains("SQL;"));

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connStr));
}

#endregion

#region Dependency Injection

builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

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
builder.Services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserScopeService, UserScopeService>();

builder.Services.AddScoped<IDoubtClearingRepository, DoubtClearingRepository>();
builder.Services.AddScoped<IDoubtClearingService, DoubtClearingService>();
builder.Services.AddScoped<IStaffCategoryRepository, StaffCategoryRepository>();
builder.Services.AddScoped<IStaffCategoryService, StaffCategoryService>();
builder.Services.AddScoped<IFieldGroupRepository, FieldGroupRepository>();
builder.Services.AddScoped<IFieldGroupServices, FieldGroupsService>();
builder.Services.AddScoped<IFieldValueValidatorService, FieldValueValidatorService>();
builder.Services.AddScoped<IStaffRegistrationRepository, StaffRegistrationRepository>();
builder.Services.AddScoped<IStaffRegistrationService, StaffRegistrationService>();

builder.Services.AddScoped<IFormFieldRepository, FormFieldRepository>();
builder.Services.AddScoped<IFormFieldService, FormFieldService>();
builder.Services.AddScoped<IFieldValueValidatorService, FieldValueValidatorService>();
builder.Services.AddScoped<IRegistrationFormRepository, RegistrationFormRepository>();
builder.Services.AddScoped<IRegistrationFormService, RegistrationFormService>();
builder.Services.AddScoped<IPageHeadingRepository, PageHeadingRepository>();
builder.Services.AddScoped<IPageHeadingService, PageHeadingService>();
builder.Services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
builder.Services.AddScoped<IOperatorDashboardRepository, OperatorDashboardRepository>();
builder.Services.AddScoped<IOperatorDashboardService, OperatorDashboardService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<INavigationService, NavigationService>();

builder.Services.AddScoped<IDtpAdminRepository, DtpAdminRepository>();
builder.Services.AddScoped<IDtpAdminService, DtpAdminService>();
builder.Services.AddScoped<IExamGradeRepository, ExamGradeRepository>();
builder.Services.AddScoped<IExamGradeService, ExamGradeService>();

builder.Services.AddScoped<IQuestionReviewRepository, QuestionReviewRepository>();
builder.Services.AddScoped<IQuestionReviewService, QuestionReviewService>();
builder.Services.AddScoped<IQuestionCorrectionService, QuestionCorrectionService>();
builder.Services.AddScoped<IExamUnitRepository, ExamUnitRepository>();
builder.Services.AddScoped<IExamUnitService, ExamUnitService>();
builder.Services.AddScoped<IExamSubjectRepository, ExamSubjectRepository>();
builder.Services.AddScoped<IExamSubjectService, ExamSubjectService>();

builder.Services.AddScoped<IQuestionTypeConfigRepository, QuestionTypeConfigRepository>();
builder.Services.AddScoped<IQuestionTypeConfigService, QuestionTypeConfigService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IExamChapterRepository, ExamChapterRepository>();
builder.Services.AddScoped<IExamChapterService, ExamChapterService>();

builder.Services.AddScoped<IOperatorStatsRepository, OperatorStatsRepository>();
builder.Services.AddScoped<IOperatorStatsService, OperatorStatsService>();
builder.Services.AddScoped<IQuestionConfigurationRepository, QuestionConfigurationRepository>();
builder.Services.AddScoped<IQuestionConfigurationService, QuestionConfigurationService>();

builder.Services.AddScoped<IExamSyllabusRepository, ExamSyllabusRepository>();
builder.Services.AddScoped<IExamSyllabusService, ExamSyllabusService>();
builder.Services.AddScoped<IContentResourceTypeRepository, ContentResourceTypeRepository>();
builder.Services.AddScoped<IContentResourceTypeService, ContentResourceTypeService>();

builder.Services.AddScoped<IDarRepository, DarRepository>();
builder.Services.AddScoped<IDarService, DarService>();

var fileUploadSettings = builder.Configuration
    .GetSection(FileUploadSettings.SectionName)
    .Get<FileUploadSettings>();

if (fileUploadSettings?.UseAzureBlob == true)
{
    builder.Services.AddScoped<IFileStorageService, AzureBlobStorageService>();
    Log.Information(
        "File storage provider: Azure Blob Storage. "
        + "Container: {Container}",
        fileUploadSettings.AzureBlobContainerName);
}
else
{
    builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
    Log.Information("File storage provider: Local Disk");
}

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly",
        policy => policy.RequireRole("SuperAdmin"));

    options.AddPolicy("CountryAdminOnly", policy =>
    {
        policy.RequireRole("CountryAdmin");
        policy.Requirements.Add(new ScopeRequirement());
    });

    options.AddPolicy("CenterAdminOnly", policy =>
    {
        policy.RequireRole("CenterAdmin");
        policy.Requirements.Add(new ScopeRequirement());
    });

    options.AddPolicy("StaffOnly", policy =>
    {
        policy.RequireRole("Staff");
        policy.Requirements.Add(new ScopeRequirement());
    });

    options.AddPolicy("CanCreateStudent", policy =>
    {
        policy.RequireClaim("Permission", "CanCreateStudent");
    });

    options.AddPolicy("CanViewStudent", policy =>
    {
        policy.RequireClaim("Permission", "CanViewStudent");
    });

    options.AddPolicy("CanDeleteStudent", policy =>
    {
        policy.RequireClaim("Permission", "CanDeleteStudent");
    });
    options.AddPolicy("CanUpdateStudent", policy =>
    {
        policy.RequireClaim("Permission", "CanUpdateStudent");
    });

});

#endregion

#region Controllers

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // prevents infinite loop
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        // enums serialize as "Full" instead of 1
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
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
        config.PermitLimit = 1;                 // 1 requests
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
    var logger = scope.ServiceProvider
        .GetRequiredService<ILogger<Program>>();

    try
    {
        db.Database.Migrate();
        CountryDataSeeder.Seed(db);
        StateDataSeeder.Seed(db);
        RoleDataSeeder.Seed(db);
        StaffTypeDataSeeder.Seed(db);
        PermissionSeeder.Seed(db);
        CenterDataSeeder.Seed(db);
        AdminUserSeeder.Seed(db);
        PageHeadingSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database migration or seeding failed.");
        throw;
    }

    var resolvedFileSettings = app.Configuration
        .GetSection(FileUploadSettings.SectionName)
        .Get<FileUploadSettings>();

    // Only create local directories when using local storage
    if (resolvedFileSettings?.UseAzureBlob != true)
    {
        string persistentRoot;
        if (string.IsNullOrEmpty(resolvedFileSettings?.StorageRootPath))
        {
            persistentRoot = app.Environment.ContentRootPath;
        }
        else
        {
            persistentRoot = Environment.OSVersion.Platform == PlatformID.Unix
                ? "/home/data"
                : resolvedFileSettings.StorageRootPath;
        }

        var studentFilesPath = Path.Combine(
            persistentRoot, "StudentFiles");
        var photosPath = Path.Combine(studentFilesPath, "Photos");
        var signaturesPath = Path.Combine(
            studentFilesPath, "Signatures");

        Directory.CreateDirectory(studentFilesPath);
        Directory.CreateDirectory(photosPath);
        Directory.CreateDirectory(signaturesPath);

        logger.LogInformation(
            "Student file directories ensured at: {Path}",
            studentFilesPath);
    }
    else
    {
        logger.LogInformation(
            "Using Azure Blob Storage � skipping local "
            + "directory creation.");
    }
}

#endregion

#region Swagger UI

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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

app.UseStaticFiles();

var pipelineFileConfig = app.Configuration
    .GetSection(FileUploadSettings.SectionName)
    .Get<FileUploadSettings>();

// Only serve StudentFiles from disk when using local storage
if (pipelineFileConfig?.UseAzureBlob != true)
{
    var localFileRoot = string.IsNullOrEmpty(
        pipelineFileConfig?.StorageRootPath)
        ? app.Environment.ContentRootPath
        : pipelineFileConfig.StorageRootPath;

    var studentFilesDir = Path.Combine(localFileRoot, "StudentFiles");

    if (Directory.Exists(studentFilesDir))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider =
                new Microsoft.Extensions.FileProviders
                    .PhysicalFileProvider(studentFilesDir),
            RequestPath = "/StudentFiles",
        });
    }
}
else
{
    Log.Information(
        "Azure Blob Storage active � StudentFiles served "
        + "directly from blob URLs.");
}

app.UseAuthentication();
app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

#endregion

app.Run();

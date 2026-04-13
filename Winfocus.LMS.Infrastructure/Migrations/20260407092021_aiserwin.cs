using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class aiserwin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    GroupName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guidelines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guidelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageHeadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PageKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MainHeading = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubHeading = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "User Management"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageHeadings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PlaceholderText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentPhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentSignaturePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAcceptedTermsAndConditions = table.Column<bool>(type: "bit", nullable: false),
                    IsAcceptedAgreement = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentPersonalDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MobileWhatsapp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileBotim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileComera = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictOrLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emirates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPersonalDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherDocumentInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofType = table.Column<int>(type: "int", nullable: false),
                    ProofNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherDocumentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherProfessionalDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HighestQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalTeachingExperience = table.Column<int>(type: "int", nullable: false),
                    HasOnlineTeachingExperience = table.Column<bool>(type: "bit", nullable: false),
                    HasOfflineTeachingExperience = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailableForDemoClass = table.Column<bool>(type: "bit", nullable: false),
                    ComputerLiteracy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherProfessionalDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeachingTools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingTools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserActivationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivationTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserActiveSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LoginAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LogoutAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActiveSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeOfStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeOfStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeOfStudies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FieldGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Placeholder = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ValidationRegex = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinLength = table.Column<int>(type: "int", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFields_FieldGroups_FieldGroupId",
                        column: x => x.FieldGroupId,
                        principalTable: "FieldGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    StaffCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationForms_StaffCategories_StaffCategoryId",
                        column: x => x.StaffCategoryId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TeacherProfessionalDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeacherScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherAvailability_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                        column: x => x.TeacherProfessionalDetailId,
                        principalTable: "TeacherProfessionalDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherAvailability_TeacherSchedule_TeacherScheduleId",
                        column: x => x.TeacherScheduleId,
                        principalTable: "TeacherSchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModeOfStudyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_States_ModeOfStudies_ModeOfStudyId",
                        column: x => x.ModeOfStudyId,
                        principalTable: "ModeOfStudies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOptions_FormFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationFormGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFormGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                        column: x => x.FieldGroupId,
                        principalTable: "FieldGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationFormGroups_RegistrationForms_FormId",
                        column: x => x.FormId,
                        principalTable: "RegistrationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffRegistrations_RegistrationForms_FormId",
                        column: x => x.FormId,
                        principalTable: "RegistrationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffRegistrations_StaffCategories_StaffCategoryId",
                        column: x => x.StaffCategoryId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Centres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CenterType = table.Column<int>(type: "int", nullable: false),
                    ModeOfStudyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CenterCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Centres_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Centres_ModeOfStudies_ModeOfStudyId",
                        column: x => x.ModeOfStudyId,
                        principalTable: "ModeOfStudies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Centres_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmploymentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkMode = table.Column<int>(type: "int", nullable: false),
                    ReportingManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    AlternativeMobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AlternativeEmailAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DistrictOrLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RefernceContactNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefernceContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsWillingToWorkWeekends = table.Column<bool>(type: "bit", nullable: false),
                    HasInternetAndSystemAvailability = table.Column<bool>(type: "bit", nullable: false),
                    IsDeclared = table.Column<bool>(type: "bit", nullable: false),
                    IsSignedAgreement = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdministrativeRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsTermsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractDuration = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherRegistrations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherRegistrations_StaffCategories_EmploymentTypeId",
                        column: x => x.EmploymentTypeId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherRegistrations_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherRegistrations_TeacherDocumentInfo_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "TeacherDocumentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherRegistrations_TeacherProfessionalDetail_ProfessionalDetailId",
                        column: x => x.ProfessionalDetailId,
                        principalTable: "TeacherProfessionalDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherRegistrations_TeacherSchedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "TeacherSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationFormFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFormFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFormFields_FormFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationFormFields_RegistrationFormGroups_FormGroupId",
                        column: x => x.FormGroupId,
                        principalTable: "RegistrationFormGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationFormFields_RegistrationForms_FormId",
                        column: x => x.FormId,
                        principalTable: "RegistrationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffRegistrationValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    RegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRegistrationValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffRegistrationValues_FormFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffRegistrationValues_StaffRegistrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "StaffRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Syllabuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllabuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Syllabuses_Centres_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Centres_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centres",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_StaffCategories_StaffCategoryId",
                        column: x => x.StaffCategoryId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherAcademicRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MarksPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Subjects = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAcademicRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherAcademicRecords_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    TeacherProfessionalDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherLanguages_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                        column: x => x.TeacherProfessionalDetailId,
                        principalTable: "TeacherProfessionalDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherLanguages_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherTools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherProfessionalDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherTools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherTools_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                        column: x => x.TeacherProfessionalDetailId,
                        principalTable: "TeacherProfessionalDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherTools_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherTools_TeachingTools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "TeachingTools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherWorkHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobProfile = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReasonForLeaving = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherWorkHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherWorkHistories_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSyllabi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherProfessionalDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSyllabi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherSyllabi_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSyllabi_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                        column: x => x.TeacherProfessionalDetailId,
                        principalTable: "TeacherProfessionalDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherSyllabi_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Streams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreamCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streams_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherProfessionalDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPreferredGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredGrades_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredGrades_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                        column: x => x.TeacherProfessionalDetailId,
                        principalTable: "TeacherProfessionalDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherPreferredGrades_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherTaughtGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherTaughtGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherTaughtGrades_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherTaughtGrades_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreferredBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreferredBatches_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchTimingMTFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTimingMTFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BatchTimingSaturdays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTimingSaturdays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BatchTimingSundays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTimingSundays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentResourceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChapterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentResourceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Streams_StreamId",
                        column: x => x.StreamId,
                        principalTable: "Streams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoubtClearing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubtClearing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoubtClearing_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UnitNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamUnits_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeePlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuitionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationinYears = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsInstallmentAllowed = table.Column<bool>(type: "bit", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeePlans_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeePlans_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentAcademicDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModeOfStudyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PastYearPerformance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PastSchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PastSchoolLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emirates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAcademicDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_Centres_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_ModeOfStudies_ModeOfStudyId",
                        column: x => x.ModeOfStudyId,
                        principalTable: "ModeOfStudies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_Streams_StreamId",
                        column: x => x.StreamId,
                        principalTable: "Streams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentAcademicDetails_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubjectBatchTimingMTFs",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingMTFsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectBatchTimingMTFBatchTimingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubjectBatchTimingMTFSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectBatchTimingMTFs", x => new { x.SubjectId, x.BatchTimingId });
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingMTFs_BatchTimingMTFs_BatchTimingMTFsId",
                        column: x => x.BatchTimingMTFsId,
                        principalTable: "BatchTimingMTFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingMTFs_SubjectBatchTimingMTFs_SubjectBatchTimingMTFSubjectId_SubjectBatchTimingMTFBatchTimingId",
                        columns: x => new { x.SubjectBatchTimingMTFSubjectId, x.SubjectBatchTimingMTFBatchTimingId },
                        principalTable: "SubjectBatchTimingMTFs",
                        principalColumns: new[] { "SubjectId", "BatchTimingId" });
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingMTFs_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectBatchTimingSaturdays",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingSundayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectBatchTimingSaturdayBatchTimingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubjectBatchTimingSaturdaySubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectBatchTimingSaturdays", x => new { x.SubjectId, x.BatchTimingId });
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingSaturdays_BatchTimingSaturdays_BatchTimingSundayId",
                        column: x => x.BatchTimingSundayId,
                        principalTable: "BatchTimingSaturdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdaySubjectId_SubjectBatchTimingSaturdayBatchT~",
                        columns: x => new { x.SubjectBatchTimingSaturdaySubjectId, x.SubjectBatchTimingSaturdayBatchTimingId },
                        principalTable: "SubjectBatchTimingSaturdays",
                        principalColumns: new[] { "SubjectId", "BatchTimingId" });
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingSaturdays_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectBatchTimingSundays",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingSundayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectBatchTimingSundayBatchTimingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubjectBatchTimingSundaySubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectBatchTimingSundays", x => new { x.SubjectId, x.BatchTimingId });
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingSundays_BatchTimingSundays_BatchTimingSundayId",
                        column: x => x.BatchTimingSundayId,
                        principalTable: "BatchTimingSundays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingSundays_SubjectBatchTimingSundays_SubjectBatchTimingSundaySubjectId_SubjectBatchTimingSundayBatchTimingId",
                        columns: x => new { x.SubjectBatchTimingSundaySubjectId, x.SubjectBatchTimingSundayBatchTimingId },
                        principalTable: "SubjectBatchTimingSundays",
                        principalColumns: new[] { "SubjectId", "BatchTimingId" });
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingSundays_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherProfessionalDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPreferredSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredSubjects_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                        column: x => x.TeacherProfessionalDetailId,
                        principalTable: "TeacherProfessionalDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherPreferredSubjects_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherTaughtSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherTaughtSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherTaughtSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherTaughtSubjects_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamChapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ChapterNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamChapters_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeInstallments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeePlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstallmentNo = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueAfterDays = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeInstallments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeInstallments_FeePlans_FeePlanId",
                        column: x => x.FeePlanId,
                        principalTable: "FeePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeePlanDiscount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeePlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeePlanDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeePlanDiscount_FeePlans_FeePlanId",
                        column: x => x.FeePlanId,
                        principalTable: "FeePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentAcademicDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentPersonalDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentDocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Isscholershipstudent = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationStatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsManualdiscountRequest = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_StudentAcademicDetails_StudentAcademicDetailsId",
                        column: x => x.StudentAcademicDetailsId,
                        principalTable: "StudentAcademicDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudentDocuments_StudentDocumentsId",
                        column: x => x.StudentDocumentsId,
                        principalTable: "StudentDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudentPersonalDetails_StudentPersonalDetailsId",
                        column: x => x.StudentPersonalDetailsId,
                        principalTable: "StudentPersonalDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypeConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypeConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_ContentResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "ContentResourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_ExamChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ExamChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChapterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CompletedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Instructions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TaskCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_ContentResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "ContentResourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_ExamChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ExamChapters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignments_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentAcademicCouses",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAcademicCouses", x => new { x.StudentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_StudentAcademicCouses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAcademicCouses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentBatchTimingMTFs",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingMTFId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBatchTimingMTFs", x => new { x.StudentId, x.BatchTimingMTFId });
                    table.ForeignKey(
                        name: "FK_StudentBatchTimingMTFs_BatchTimingMTFs_BatchTimingMTFId",
                        column: x => x.BatchTimingMTFId,
                        principalTable: "BatchTimingMTFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentBatchTimingMTFs_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentBatchTimingSaturdays",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingSaturdayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBatchTimingSaturdays", x => new { x.StudentId, x.BatchTimingSaturdayId });
                    table.ForeignKey(
                        name: "FK_StudentBatchTimingSaturdays_BatchTimingSaturdays_BatchTimingSaturdayId",
                        column: x => x.BatchTimingSaturdayId,
                        principalTable: "BatchTimingSaturdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentBatchTimingSaturdays_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentBatchTimingSundays",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingSundayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBatchTimingSundays", x => new { x.StudentId, x.BatchTimingSundayId });
                    table.ForeignKey(
                        name: "FK_StudentBatchTimingSundays_BatchTimingSundays_BatchTimingSundayId",
                        column: x => x.BatchTimingSundayId,
                        principalTable: "BatchTimingSundays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentBatchTimingSundays_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourseDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeePlanDiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourseDiscounts_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCourseDiscounts_FeePlanDiscount_FeePlanDiscountId",
                        column: x => x.FeePlanDiscountId,
                        principalTable: "FeePlanDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudentCourseDiscounts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentFeeSelections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeePlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YearlyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SelectedDurationYears = table.Column<int>(type: "int", nullable: false),
                    TotalBeforeDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalInstallments = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeeSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFeeSelections_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentFeeSelections_FeePlans_FeePlanId",
                        column: x => x.FeePlanId,
                        principalTable: "FeePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFeeSelections_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModeOfStudyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamQuestionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExamDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMark = table.Column<double>(type: "float", nullable: false),
                    PassingMark = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Centres_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exams_ContentResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "ContentResourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exams_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_ExamChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ExamChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exams_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_ModeOfStudies_ModeOfStudyId",
                        column: x => x.ModeOfStudyId,
                        principalTable: "ModeOfStudies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_QuestionTypeConfigs_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypeConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exams_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_Streams_StreamId",
                        column: x => x.StreamId,
                        principalTable: "Streams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exams_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_ContentResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "ContentResourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_ExamChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ExamChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_QuestionTypeConfigs_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypeConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DailyActivityReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportDate = table.Column<DateOnly>(type: "date", nullable: false),
                    QuestionsTyped = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TimeSpentHours = table.Column<decimal>(type: "decimal(4,1)", nullable: false, defaultValue: 0m),
                    IssuesFaced = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyActivityReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyActivityReports_StaffRegistrations_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "StaffRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyActivityReports_TaskAssignments_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskAssignments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentFeeDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentFeeSelectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeeDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFeeDiscounts_StudentFeeSelections_StudentFeeSelectionId",
                        column: x => x.StudentFeeSelectionId,
                        principalTable: "StudentFeeSelections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentInstallments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentFeeSelectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstallmentNo = table.Column<int>(type: "int", nullable: false),
                    DueAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInstallments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentInstallments_StudentFeeSelections_StudentFeeSelectionId",
                        column: x => x.StudentFeeSelectionId,
                        principalTable: "StudentFeeSelections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionLabel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marks = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CorrectAnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionOptions_CorrectOptionId",
                        column: x => x.CorrectOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_StaffRegistrations_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "StaffRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_TaskAssignments_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionReviews_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_SubjectId",
                table: "Batches",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTimingMTFs_SubjectId",
                table: "BatchTimingMTFs",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTimingSaturdays_SubjectId",
                table: "BatchTimingSaturdays",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTimingSundays_SubjectId",
                table: "BatchTimingSundays",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Centres_CountryId",
                table: "Centres",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Centres_ModeOfStudyId",
                table: "Centres",
                column: "ModeOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_Centres_StateId",
                table: "Centres",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentResourceTypes_ChapterId",
                table: "ContentResourceTypes",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentResourceTypes_Name_WhereNotDeleted",
                table: "ContentResourceTypes",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_GradeId",
                table: "Courses",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StreamId",
                table: "Courses",
                column: "StreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubjectId",
                table: "Courses",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyActivityReports_OperatorId_ReportDate_WhereNotDeleted",
                table: "DailyActivityReports",
                columns: new[] { "OperatorId", "ReportDate" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_DailyActivityReports_Status",
                table: "DailyActivityReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DailyActivityReports_TaskId",
                table: "DailyActivityReports",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubtClearing_SubjectId",
                table: "DoubtClearing",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamChapters_UnitId_ChapterNumber_WhereNotDeleted",
                table: "ExamChapters",
                columns: new[] { "UnitId", "ChapterNumber" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ExamChapters_UnitId_Name_WhereNotDeleted",
                table: "ExamChapters",
                columns: new[] { "UnitId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamId",
                table: "ExamQuestions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamId_QuestionId_UQ",
                table: "ExamQuestions",
                columns: new[] { "ExamId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CenterId",
                table: "Exams",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ChapterId",
                table: "Exams",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CountryId",
                table: "Exams",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseId",
                table: "Exams",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExamDate",
                table: "Exams",
                column: "ExamDate");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_GradeId",
                table: "Exams",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ModeOfStudyId",
                table: "Exams",
                column: "ModeOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_QuestionTypeId",
                table: "Exams",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ResourceTypeId",
                table: "Exams",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_StateId",
                table: "Exams",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_StreamId",
                table: "Exams",
                column: "StreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SubjectId",
                table: "Exams",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SyllabusId",
                table: "Exams",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_UnitId",
                table: "Exams",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamUnits_SubjectId_Name_WhereNotDeleted",
                table: "ExamUnits",
                columns: new[] { "SubjectId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ExamUnits_SubjectId_UnitNumber_WhereNotDeleted",
                table: "ExamUnits",
                columns: new[] { "SubjectId", "UnitNumber" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FeeInstallments_FeePlanId",
                table: "FeeInstallments",
                column: "FeePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FeePlanDiscount_FeePlanId",
                table: "FeePlanDiscount",
                column: "FeePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FeePlans_CourseId",
                table: "FeePlans",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_FeePlans_SubjectId",
                table: "FeePlans",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldGroups_DisplayOrder",
                table: "FieldGroups",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_FieldGroups_GroupName_WhereNotDeleted",
                table: "FieldGroups",
                column: "GroupName",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptions_FieldId_DisplayOrder",
                table: "FieldOptions",
                columns: new[] { "FieldId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptions_FieldId_OptionValue_Unique",
                table: "FieldOptions",
                columns: new[] { "FieldId", "OptionValue" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_FieldName_WhereNotDeleted",
                table: "FormFields",
                column: "FieldName",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_GroupId_DisplayOrder",
                table: "FormFields",
                columns: new[] { "FieldGroupId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SyllabusId",
                table: "Grades",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Guidelines_Title",
                table: "Guidelines",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_ModeOfStudies_CountryId",
                table: "ModeOfStudies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PageHeadings_ModuleName",
                table: "PageHeadings",
                column: "ModuleName");

            migrationBuilder.CreateIndex(
                name: "IX_PageHeadings_PageKey_Unique",
                table: "PageHeadings",
                column: "PageKey",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PreferredBatches_BatchId",
                table: "PreferredBatches",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_AcademicYearId",
                table: "QuestionConfigurations",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_ChapterId",
                table: "QuestionConfigurations",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_GradeId",
                table: "QuestionConfigurations",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_QuestionCode",
                table: "QuestionConfigurations",
                column: "QuestionCode",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_QuestionTypeId",
                table: "QuestionConfigurations",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_ResourceTypeId",
                table: "QuestionConfigurations",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_SubjectId",
                table: "QuestionConfigurations",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_SyllabusId_AcademicYearId_GradeId_SubjectId_UnitId_ChapterId_QuestionTypeId",
                table: "QuestionConfigurations",
                columns: new[] { "SyllabusId", "AcademicYearId", "GradeId", "SubjectId", "UnitId", "ChapterId", "QuestionTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionConfigurations_UnitId",
                table: "QuestionConfigurations",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReviews_QuestionId",
                table: "QuestionReviews",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReviews_ReviewerId",
                table: "QuestionReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CorrectOptionId",
                table: "Questions",
                column: "CorrectOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_OperatorId_Status",
                table: "Questions",
                columns: new[] { "OperatorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TaskId_Status",
                table: "Questions",
                columns: new[] { "TaskId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeConfigs_ChapterId",
                table: "QuestionTypeConfigs",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeConfigs_GradeId",
                table: "QuestionTypeConfigs",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeConfigs_ResourceTypeId",
                table: "QuestionTypeConfigs",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeConfigs_SubjectId",
                table: "QuestionTypeConfigs",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeConfigs_SyllabusId_GradeId_SubjectId_UnitId_ChapterId_ResourceTypeId_Name",
                table: "QuestionTypeConfigs",
                columns: new[] { "SyllabusId", "GradeId", "SubjectId", "UnitId", "ChapterId", "ResourceTypeId", "Name" },
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeConfigs_UnitId",
                table: "QuestionTypeConfigs",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RegFormFields_FormId_DisplayOrder",
                table: "RegistrationFormFields",
                columns: new[] { "FormId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RegFormFields_FormId_FieldId_Unique",
                table: "RegistrationFormFields",
                columns: new[] { "FormId", "FieldId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegFormFields_GroupId_DisplayOrder",
                table: "RegistrationFormFields",
                columns: new[] { "FormGroupId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFormFields_FieldId",
                table: "RegistrationFormFields",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_RegFormGroups_FormId_DisplayOrder",
                table: "RegistrationFormGroups",
                columns: new[] { "FormId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RegFormGroups_FormId_FieldGroupId_Unique",
                table: "RegistrationFormGroups",
                columns: new[] { "FormId", "FieldGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFormGroups_FieldGroupId",
                table: "RegistrationFormGroups",
                column: "FieldGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationForms_FormName",
                table: "RegistrationForms",
                column: "FormName");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationForms_OneActivePerCategory",
                table: "RegistrationForms",
                column: "StaffCategoryId",
                unique: true,
                filter: "[IsActive] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCategories_IsActive",
                table: "StaffCategories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCategories_Name_WhereNotDeleted",
                table: "StaffCategories",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_FormId",
                table: "StaffRegistrations",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_StaffCategoryId",
                table: "StaffRegistrations",
                column: "StaffCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_Status",
                table: "StaffRegistrations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_UserId",
                table: "StaffRegistrations",
                column: "UserId",
                unique: true,
                filter: "UserId IS NOT NULL AND IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegValues_FieldId",
                table: "StaffRegistrationValues",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegValues_RegId_FieldId_Unique",
                table: "StaffRegistrationValues",
                columns: new[] { "RegistrationId", "FieldId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_States_ModeOfStudyId",
                table: "States",
                column: "ModeOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_Streams_GradeId_Name",
                table: "Streams",
                columns: new[] { "GradeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicCouses_CourseId",
                table: "StudentAcademicCouses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_AcademicYearId",
                table: "StudentAcademicDetails",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_CenterId",
                table: "StudentAcademicDetails",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_CountryId",
                table: "StudentAcademicDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_GradeId",
                table: "StudentAcademicDetails",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_ModeOfStudyId",
                table: "StudentAcademicDetails",
                column: "ModeOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_StateId",
                table: "StudentAcademicDetails",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_StreamId",
                table: "StudentAcademicDetails",
                column: "StreamId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_SubjectId",
                table: "StudentAcademicDetails",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_SyllabusId",
                table: "StudentAcademicDetails",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatchTimingMTFs_BatchTimingMTFId",
                table: "StudentBatchTimingMTFs",
                column: "BatchTimingMTFId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatchTimingSaturdays_BatchTimingSaturdayId",
                table: "StudentBatchTimingSaturdays",
                column: "BatchTimingSaturdayId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatchTimingSundays_BatchTimingSundayId",
                table: "StudentBatchTimingSundays",
                column: "BatchTimingSundayId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseDiscounts_CourseId",
                table: "StudentCourseDiscounts",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseDiscounts_FeePlanDiscountId",
                table: "StudentCourseDiscounts",
                column: "FeePlanDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseDiscounts_StudentId_CourseId_DiscountName",
                table: "StudentCourseDiscounts",
                columns: new[] { "StudentId", "CourseId", "DiscountName" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeDiscounts_StudentFeeSelectionId",
                table: "StudentFeeDiscounts",
                column: "StudentFeeSelectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeSelections_CourseId",
                table: "StudentFeeSelections",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeSelections_FeePlanId",
                table: "StudentFeeSelections",
                column: "FeePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeSelections_StudentId_CourseId",
                table: "StudentFeeSelections",
                columns: new[] { "StudentId", "CourseId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstallments_StudentFeeSelectionId_InstallmentNo",
                table: "StudentInstallments",
                columns: new[] { "StudentFeeSelectionId", "InstallmentNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentAcademicDetailsId",
                table: "Students",
                column: "StudentAcademicDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentDocumentsId",
                table: "Students",
                column: "StudentDocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentPersonalDetailsId",
                table: "Students",
                column: "StudentPersonalDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId_Unique",
                table: "Students",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingMTFs_BatchTimingMTFsId",
                table: "SubjectBatchTimingMTFs",
                column: "BatchTimingMTFsId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingMTFs_SubjectBatchTimingMTFSubjectId_SubjectBatchTimingMTFBatchTimingId",
                table: "SubjectBatchTimingMTFs",
                columns: new[] { "SubjectBatchTimingMTFSubjectId", "SubjectBatchTimingMTFBatchTimingId" });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSaturdays_BatchTimingSundayId",
                table: "SubjectBatchTimingSaturdays",
                column: "BatchTimingSundayId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdaySubjectId_SubjectBatchTimingSaturdayBatchTimingId",
                table: "SubjectBatchTimingSaturdays",
                columns: new[] { "SubjectBatchTimingSaturdaySubjectId", "SubjectBatchTimingSaturdayBatchTimingId" });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSundays_BatchTimingSundayId",
                table: "SubjectBatchTimingSundays",
                column: "BatchTimingSundayId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSundays_SubjectBatchTimingSundaySubjectId_SubjectBatchTimingSundayBatchTimingId",
                table: "SubjectBatchTimingSundays",
                columns: new[] { "SubjectBatchTimingSundaySubjectId", "SubjectBatchTimingSundayBatchTimingId" });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_AcademicYearId",
                table: "Syllabuses",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_CenterId",
                table: "Syllabuses",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_ChapterId",
                table: "TaskAssignments",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_Deadline",
                table: "TaskAssignments",
                column: "Deadline");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_GradeId",
                table: "TaskAssignments",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_OperatorId_Status",
                table: "TaskAssignments",
                columns: new[] { "OperatorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_ResourceTypeId",
                table: "TaskAssignments",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_SubjectId",
                table: "TaskAssignments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_SyllabusId",
                table: "TaskAssignments",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_UnitId",
                table: "TaskAssignments",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAcademicRecords_TeacherRegistrationId",
                table: "TeacherAcademicRecords",
                column: "TeacherRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAvailability_TeacherProfessionalDetailId",
                table: "TeacherAvailability",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAvailability_TeacherScheduleId",
                table: "TeacherAvailability",
                column: "TeacherScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLanguages_TeacherProfessionalDetailId",
                table: "TeacherLanguages",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLanguages_TeacherRegistrationId_Language",
                table: "TeacherLanguages",
                columns: new[] { "TeacherRegistrationId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredGrades_GradeId",
                table: "TeacherPreferredGrades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredGrades_TeacherProfessionalDetailId",
                table: "TeacherPreferredGrades",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredGrades_TeacherRegistrationId_ExamGradeId",
                table: "TeacherPreferredGrades",
                columns: new[] { "TeacherRegistrationId", "GradeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredSubjects_SubjectId",
                table: "TeacherPreferredSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredSubjects_TeacherProfessionalDetailId",
                table: "TeacherPreferredSubjects",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredSubjects_TeacherRegistrationId_ExamSubjectId",
                table: "TeacherPreferredSubjects",
                columns: new[] { "TeacherRegistrationId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_CountryId",
                table: "TeacherRegistrations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_DocumentsId",
                table: "TeacherRegistrations",
                column: "DocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_EmailAddress",
                table: "TeacherRegistrations",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_EmployeeId",
                table: "TeacherRegistrations",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_EmploymentTypeId",
                table: "TeacherRegistrations",
                column: "EmploymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_ProfessionalDetailId",
                table: "TeacherRegistrations",
                column: "ProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_ScheduleId",
                table: "TeacherRegistrations",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_StateId",
                table: "TeacherRegistrations",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSyllabi_SyllabusId",
                table: "TeacherSyllabi",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSyllabi_TeacherProfessionalDetailId",
                table: "TeacherSyllabi",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSyllabi_TeacherRegistrationId_ExamSyllabusId",
                table: "TeacherSyllabi",
                columns: new[] { "TeacherRegistrationId", "SyllabusId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtGrades_GradeId",
                table: "TeacherTaughtGrades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtGrades_TeacherRegistrationId_ExamGradeId",
                table: "TeacherTaughtGrades",
                columns: new[] { "TeacherRegistrationId", "GradeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtSubjects_SubjectId",
                table: "TeacherTaughtSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtSubjects_TeacherRegistrationId_ExamSubjectId",
                table: "TeacherTaughtSubjects",
                columns: new[] { "TeacherRegistrationId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTools_TeacherProfessionalDetailId",
                table: "TeacherTools",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTools_TeacherRegistrationId_ToolId",
                table: "TeacherTools",
                columns: new[] { "TeacherRegistrationId", "ToolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTools_ToolId",
                table: "TeacherTools",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherWorkHistories_TeacherRegistrationId",
                table: "TeacherWorkHistories",
                column: "TeacherRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivationTokens_Token",
                table: "UserActivationTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivationTokens_UserId",
                table: "UserActivationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActiveSessions_SessionId",
                table: "UserActiveSessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActiveSessions_UserId",
                table: "UserActiveSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActiveSessions_UserId_Active_Revoked_Expires",
                table: "UserActiveSessions",
                columns: new[] { "UserId", "IsActive", "IsRevoked", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_LoginTimestamp",
                table: "UserLoginLogs",
                column: "LoginTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_UserId_LoginTimestamp",
                table: "UserLoginLogs",
                columns: new[] { "Id", "LoginTimestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CenterId",
                table: "Users",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_StaffCategoryId",
                table: "Users",
                column: "StaffCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Subjects_SubjectId",
                table: "Batches",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTimingMTFs_Subjects_SubjectId",
                table: "BatchTimingMTFs",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTimingSaturdays_Subjects_SubjectId",
                table: "BatchTimingSaturdays",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTimingSundays_Subjects_SubjectId",
                table: "BatchTimingSundays",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentResourceTypes_ExamChapters_ChapterId",
                table: "ContentResourceTypes",
                column: "ChapterId",
                principalTable: "ExamChapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamUnits_Subjects_SubjectId",
                table: "ExamUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Subjects_SubjectId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Centres_Countries_CountryId",
                table: "Centres");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeOfStudies_Countries_CountryId",
                table: "ModeOfStudies");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Countries_CountryId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Countries_CountryId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Centres_ModeOfStudies_ModeOfStudyId",
                table: "Centres");

            migrationBuilder.DropForeignKey(
                name: "FK_States_ModeOfStudies_ModeOfStudyId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_Centres_States_StateId",
                table: "Centres");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentResourceTypes_ExamChapters_ChapterId",
                table: "ContentResourceTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ExamChapters_ChapterId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Grades_GradeId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_StaffRegistrations_OperatorId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TaskAssignments_TaskId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "DailyActivityReports");

            migrationBuilder.DropTable(
                name: "DoubtClearing");

            migrationBuilder.DropTable(
                name: "ExamQuestions");

            migrationBuilder.DropTable(
                name: "FeeInstallments");

            migrationBuilder.DropTable(
                name: "FieldOptions");

            migrationBuilder.DropTable(
                name: "Guidelines");

            migrationBuilder.DropTable(
                name: "PageHeadings");

            migrationBuilder.DropTable(
                name: "PreferredBatches");

            migrationBuilder.DropTable(
                name: "QuestionConfigurations");

            migrationBuilder.DropTable(
                name: "QuestionReviews");

            migrationBuilder.DropTable(
                name: "RegistrationFormFields");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "StaffRegistrationValues");

            migrationBuilder.DropTable(
                name: "StudentAcademicCouses");

            migrationBuilder.DropTable(
                name: "StudentBatchTimingMTFs");

            migrationBuilder.DropTable(
                name: "StudentBatchTimingSaturdays");

            migrationBuilder.DropTable(
                name: "StudentBatchTimingSundays");

            migrationBuilder.DropTable(
                name: "StudentCourseDiscounts");

            migrationBuilder.DropTable(
                name: "StudentFeeDiscounts");

            migrationBuilder.DropTable(
                name: "StudentInstallments");

            migrationBuilder.DropTable(
                name: "SubjectBatchTimingMTFs");

            migrationBuilder.DropTable(
                name: "SubjectBatchTimingSaturdays");

            migrationBuilder.DropTable(
                name: "SubjectBatchTimingSundays");

            migrationBuilder.DropTable(
                name: "TeacherAcademicRecords");

            migrationBuilder.DropTable(
                name: "TeacherAvailability");

            migrationBuilder.DropTable(
                name: "TeacherLanguages");

            migrationBuilder.DropTable(
                name: "TeacherPreferredGrades");

            migrationBuilder.DropTable(
                name: "TeacherPreferredSubjects");

            migrationBuilder.DropTable(
                name: "TeacherSyllabi");

            migrationBuilder.DropTable(
                name: "TeacherTaughtGrades");

            migrationBuilder.DropTable(
                name: "TeacherTaughtSubjects");

            migrationBuilder.DropTable(
                name: "TeacherTools");

            migrationBuilder.DropTable(
                name: "TeacherWorkHistories");

            migrationBuilder.DropTable(
                name: "UserActivationTokens");

            migrationBuilder.DropTable(
                name: "UserActiveSessions");

            migrationBuilder.DropTable(
                name: "UserLoginLogs");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "RegistrationFormGroups");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "FormFields");

            migrationBuilder.DropTable(
                name: "FeePlanDiscount");

            migrationBuilder.DropTable(
                name: "StudentFeeSelections");

            migrationBuilder.DropTable(
                name: "BatchTimingMTFs");

            migrationBuilder.DropTable(
                name: "BatchTimingSaturdays");

            migrationBuilder.DropTable(
                name: "BatchTimingSundays");

            migrationBuilder.DropTable(
                name: "TeachingTools");

            migrationBuilder.DropTable(
                name: "TeacherRegistrations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "QuestionTypeConfigs");

            migrationBuilder.DropTable(
                name: "FieldGroups");

            migrationBuilder.DropTable(
                name: "FeePlans");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "TeacherDocumentInfo");

            migrationBuilder.DropTable(
                name: "TeacherProfessionalDetail");

            migrationBuilder.DropTable(
                name: "TeacherSchedule");

            migrationBuilder.DropTable(
                name: "StudentAcademicDetails");

            migrationBuilder.DropTable(
                name: "StudentDocuments");

            migrationBuilder.DropTable(
                name: "StudentPersonalDetails");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Streams");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ModeOfStudies");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "ExamChapters");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "StaffRegistrations");

            migrationBuilder.DropTable(
                name: "RegistrationForms");

            migrationBuilder.DropTable(
                name: "TaskAssignments");

            migrationBuilder.DropTable(
                name: "ContentResourceTypes");

            migrationBuilder.DropTable(
                name: "ExamUnits");

            migrationBuilder.DropTable(
                name: "Syllabuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AcademicYears");

            migrationBuilder.DropTable(
                name: "Centres");

            migrationBuilder.DropTable(
                name: "StaffCategories");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionOptions");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmploymentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkMode = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    IdProofType = table.Column<int>(type: "int", nullable: false),
                    IdProofNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComputerLiteracy = table.Column<int>(type: "int", nullable: false),
                    HighestQualification = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SalaryStructure = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PaymentCycle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContractDuration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReportingManager = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdCardPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsTermsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeclarationAccepted = table.Column<bool>(type: "bit", nullable: false),
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
                        name: "FK_TeacherRegistrations_StaffCategories_EmploymentTypeId",
                        column: x => x.EmploymentTypeId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_TeacherLanguages_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_TeacherPreferredGrades_ExamGrades_ExamGradeId",
                        column: x => x.ExamGradeId,
                        principalTable: "ExamGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredGrades_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPreferredSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_TeacherPreferredSubjects_ExamSubjects_ExamSubjectId",
                        column: x => x.ExamSubjectId,
                        principalTable: "ExamSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherPreferredSubjects_TeacherRegistrations_TeacherRegistrationId",
                        column: x => x.TeacherRegistrationId,
                        principalTable: "TeacherRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSyllabi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamSyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_TeacherSyllabi_ExamSyllabuses_ExamSyllabusId",
                        column: x => x.ExamSyllabusId,
                        principalTable: "ExamSyllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSyllabi_TeacherRegistrations_TeacherRegistrationId",
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
                    ExamGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_TeacherTaughtGrades_ExamGrades_ExamGradeId",
                        column: x => x.ExamGradeId,
                        principalTable: "ExamGrades",
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
                name: "TeacherTaughtSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_TeacherTaughtSubjects_ExamSubjects_ExamSubjectId",
                        column: x => x.ExamSubjectId,
                        principalTable: "ExamSubjects",
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
                name: "TeacherTools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    TeacherRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLanguages_TeacherRegistrationId_Language",
                table: "TeacherLanguages",
                columns: new[] { "TeacherRegistrationId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredGrades_ExamGradeId",
                table: "TeacherPreferredGrades",
                column: "ExamGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredGrades_TeacherRegistrationId_ExamGradeId",
                table: "TeacherPreferredGrades",
                columns: new[] { "TeacherRegistrationId", "ExamGradeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredSubjects_ExamSubjectId",
                table: "TeacherPreferredSubjects",
                column: "ExamSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredSubjects_TeacherRegistrationId_ExamSubjectId",
                table: "TeacherPreferredSubjects",
                columns: new[] { "TeacherRegistrationId", "ExamSubjectId" },
                unique: true);

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
                name: "IX_TeacherSyllabi_ExamSyllabusId",
                table: "TeacherSyllabi",
                column: "ExamSyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSyllabi_TeacherRegistrationId_ExamSyllabusId",
                table: "TeacherSyllabi",
                columns: new[] { "TeacherRegistrationId", "ExamSyllabusId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtGrades_ExamGradeId",
                table: "TeacherTaughtGrades",
                column: "ExamGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtGrades_TeacherRegistrationId_ExamGradeId",
                table: "TeacherTaughtGrades",
                columns: new[] { "TeacherRegistrationId", "ExamGradeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtSubjects_ExamSubjectId",
                table: "TeacherTaughtSubjects",
                column: "ExamSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTaughtSubjects_TeacherRegistrationId_ExamSubjectId",
                table: "TeacherTaughtSubjects",
                columns: new[] { "TeacherRegistrationId", "ExamSubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTools_TeacherRegistrationId_ToolId",
                table: "TeacherTools",
                columns: new[] { "TeacherRegistrationId", "ToolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTools_ToolId",
                table: "TeacherTools",
                column: "ToolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "TeacherRegistrations");
        }
    }
}

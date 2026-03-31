using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class teachettable_modified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComputerLiteracy",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "ContractDuration",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "HighestQualification",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "IdCardPath",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "IdProofNumber",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "IdProofType",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "PaymentCycle",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "ReportingManager",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "SalaryStructure",
                table: "TeacherRegistrations");

            migrationBuilder.RenameColumn(
                name: "MaritalStatus",
                table: "TeacherRegistrations",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeclarationAccepted",
                table: "TeacherRegistrations",
                newName: "IsWillingToWorkWeekends");

            migrationBuilder.AddColumn<string>(
                name: "AdministrativeRemarks",
                table: "TeacherRegistrations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfJoining",
                table: "TeacherRegistrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclarationDate",
                table: "TeacherRegistrations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentsId",
                table: "TeacherRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactNumber",
                table: "TeacherRegistrations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasInternetAndSystemAvailability",
                table: "TeacherRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "TeacherRegistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                table: "TeacherRegistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfessionalDetailId",
                table: "TeacherRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReportingManagerId",
                table: "TeacherRegistrations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleId",
                table: "TeacherRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherProfessionalDetailId",
                table: "TeacherPreferredSubjects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherProfessionalDetailId",
                table: "TeacherPreferredGrades",
                type: "uniqueidentifier",
                nullable: true);

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
                name: "TeacherDocumentInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCardPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    University = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfPassing = table.Column<int>(type: "int", nullable: false),
                    HasTeachingCertification = table.Column<bool>(type: "bit", nullable: false),
                    AdditionalCourses = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "TeacherAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TeacherScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherAvailability_TeacherSchedule_TeacherScheduleId",
                        column: x => x.TeacherScheduleId,
                        principalTable: "TeacherSchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_DocumentsId",
                table: "TeacherRegistrations",
                column: "DocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_ProfessionalDetailId",
                table: "TeacherRegistrations",
                column: "ProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_ScheduleId",
                table: "TeacherRegistrations",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredSubjects_TeacherProfessionalDetailId",
                table: "TeacherPreferredSubjects",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPreferredGrades_TeacherProfessionalDetailId",
                table: "TeacherPreferredGrades",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAcademicRecords_TeacherRegistrationId",
                table: "TeacherAcademicRecords",
                column: "TeacherRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAvailability_TeacherScheduleId",
                table: "TeacherAvailability",
                column: "TeacherScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherWorkHistories_TeacherRegistrationId",
                table: "TeacherWorkHistories",
                column: "TeacherRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPreferredGrades_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherPreferredGrades",
                column: "TeacherProfessionalDetailId",
                principalTable: "TeacherProfessionalDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPreferredSubjects_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherPreferredSubjects",
                column: "TeacherProfessionalDetailId",
                principalTable: "TeacherProfessionalDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherRegistrations_TeacherDocumentInfo_DocumentsId",
                table: "TeacherRegistrations",
                column: "DocumentsId",
                principalTable: "TeacherDocumentInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherRegistrations_TeacherProfessionalDetail_ProfessionalDetailId",
                table: "TeacherRegistrations",
                column: "ProfessionalDetailId",
                principalTable: "TeacherProfessionalDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherRegistrations_TeacherSchedule_ScheduleId",
                table: "TeacherRegistrations",
                column: "ScheduleId",
                principalTable: "TeacherSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPreferredGrades_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherPreferredGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPreferredSubjects_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherPreferredSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherRegistrations_TeacherDocumentInfo_DocumentsId",
                table: "TeacherRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherRegistrations_TeacherProfessionalDetail_ProfessionalDetailId",
                table: "TeacherRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherRegistrations_TeacherSchedule_ScheduleId",
                table: "TeacherRegistrations");

            migrationBuilder.DropTable(
                name: "TeacherAcademicRecords");

            migrationBuilder.DropTable(
                name: "TeacherAvailability");

            migrationBuilder.DropTable(
                name: "TeacherDocumentInfo");

            migrationBuilder.DropTable(
                name: "TeacherProfessionalDetail");

            migrationBuilder.DropTable(
                name: "TeacherWorkHistories");

            migrationBuilder.DropTable(
                name: "TeacherSchedule");

            migrationBuilder.DropIndex(
                name: "IX_TeacherRegistrations_DocumentsId",
                table: "TeacherRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TeacherRegistrations_ProfessionalDetailId",
                table: "TeacherRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TeacherRegistrations_ScheduleId",
                table: "TeacherRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TeacherPreferredSubjects_TeacherProfessionalDetailId",
                table: "TeacherPreferredSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TeacherPreferredGrades_TeacherProfessionalDetailId",
                table: "TeacherPreferredGrades");

            migrationBuilder.DropColumn(
                name: "AdministrativeRemarks",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "DateOfJoining",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "DeclarationDate",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "DocumentsId",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "EmergencyContactNumber",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "HasInternetAndSystemAvailability",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "ProfessionalDetailId",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "ReportingManagerId",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "TeacherProfessionalDetailId",
                table: "TeacherPreferredSubjects");

            migrationBuilder.DropColumn(
                name: "TeacherProfessionalDetailId",
                table: "TeacherPreferredGrades");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TeacherRegistrations",
                newName: "MaritalStatus");

            migrationBuilder.RenameColumn(
                name: "IsWillingToWorkWeekends",
                table: "TeacherRegistrations",
                newName: "IsDeclarationAccepted");

            migrationBuilder.AddColumn<int>(
                name: "ComputerLiteracy",
                table: "TeacherRegistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContractDuration",
                table: "TeacherRegistrations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HighestQualification",
                table: "TeacherRegistrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdCardPath",
                table: "TeacherRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdProofNumber",
                table: "TeacherRegistrations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdProofType",
                table: "TeacherRegistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentCycle",
                table: "TeacherRegistrations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "TeacherRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportingManager",
                table: "TeacherRegistrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalaryStructure",
                table: "TeacherRegistrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDtpModuleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guidelines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Guidelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Syllabus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Stream = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Course = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Chapter = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CompletedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Instructions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_TaskAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_StaffRegistrations_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "StaffRegistrations",
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
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionLabel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReviewerRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                name: "IX_DailyActivityReports_OperatorId_ReportDate_Unique",
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
                name: "IX_Guidelines_Category_DisplayOrder",
                table: "Guidelines",
                columns: new[] { "Category", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Guidelines_Title",
                table: "Guidelines",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId_DisplayOrder",
                table: "QuestionOptions",
                columns: new[] { "QuestionId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId_Label_Unique",
                table: "QuestionOptions",
                columns: new[] { "QuestionId", "OptionLabel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReviews_QuestionId",
                table: "QuestionReviews",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReviews_ReviewerId",
                table: "QuestionReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_OperatorId_Status",
                table: "Questions",
                columns: new[] { "OperatorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TaskId_Status",
                table: "Questions",
                columns: new[] { "TaskId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_Deadline",
                table: "TaskAssignments",
                column: "Deadline");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_OperatorId_Status",
                table: "TaskAssignments",
                columns: new[] { "OperatorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_Priority",
                table: "TaskAssignments",
                column: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyActivityReports");

            migrationBuilder.DropTable(
                name: "Guidelines");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "QuestionReviews");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "TaskAssignments");
        }
    }
}

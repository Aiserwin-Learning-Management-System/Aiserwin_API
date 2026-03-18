using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedQuestionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_QuestionTypeConfigs_ExamGrades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "ExamGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_ExamSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "ExamSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_ExamSyllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "ExamSyllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionTypeConfigs_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionTypeConfigs");
        }
    }
}

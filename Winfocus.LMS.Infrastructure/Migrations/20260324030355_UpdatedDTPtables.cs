using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDTPtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_QuestionConfigurations_ExamGrades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "ExamGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_ExamSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "ExamSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_ExamSyllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "ExamSyllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionConfigurations_QuestionTypeConfigs_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypeConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionConfigurations");
        }
    }
}

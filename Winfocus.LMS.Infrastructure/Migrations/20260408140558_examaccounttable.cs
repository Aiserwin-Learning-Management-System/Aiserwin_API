using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class examaccounttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivationStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivationEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionTypeConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamAccounts_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamAccounts_ContentResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "ContentResourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamAccounts_ExamChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ExamChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExamAccounts_ExamUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ExamUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamAccounts_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExamAccounts_QuestionTypeConfigs_QuestionTypeConfigId",
                        column: x => x.QuestionTypeConfigId,
                        principalTable: "QuestionTypeConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamAccounts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamAccounts_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_BatchId",
                table: "ExamAccounts",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_ChapterId",
                table: "ExamAccounts",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_ExamId",
                table: "ExamAccounts",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_QuestionTypeConfigId",
                table: "ExamAccounts",
                column: "QuestionTypeConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_ResourceTypeId",
                table: "ExamAccounts",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_StudentId",
                table: "ExamAccounts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_SubjectId",
                table: "ExamAccounts",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAccounts_UnitId",
                table: "ExamAccounts",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamAccounts");
        }
    }
}

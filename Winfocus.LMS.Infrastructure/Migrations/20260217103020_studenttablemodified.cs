using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class studenttablemodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAcademicCouses",
                table: "StudentAcademicCouses");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicCouses_StudentId",
                table: "StudentAcademicCouses");

            migrationBuilder.AddColumn<bool>(
                name: "Isscholershipstudent",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAcademicCouses",
                table: "StudentAcademicCouses",
                columns: new[] { "StudentId", "CourseId" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentBatchTimingMTFs");

            migrationBuilder.DropTable(
                name: "StudentBatchTimingSaturdays");

            migrationBuilder.DropTable(
                name: "StudentBatchTimingSundays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAcademicCouses",
                table: "StudentAcademicCouses");

            migrationBuilder.DropColumn(
                name: "Isscholershipstudent",
                table: "Students");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAcademicCouses",
                table: "StudentAcademicCouses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicCouses_StudentId",
                table: "StudentAcademicCouses",
                column: "StudentId");
        }
    }
}

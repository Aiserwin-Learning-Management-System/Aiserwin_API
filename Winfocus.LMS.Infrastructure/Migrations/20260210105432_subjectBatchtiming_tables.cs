using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class subjectBatchtiming_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BatchTime",
                table: "BatchTimingSundays",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "BatchTimingSundays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BatchTime",
                table: "BatchTimingSaturdays",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "BatchTimingSaturdays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BatchTime",
                table: "BatchTimingMTFs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "BatchTimingMTFs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SubjectBatchTimingMTFs",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchTimingSundayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectBatchTimingMTFs", x => new { x.SubjectId, x.BatchTimingId });
                    table.ForeignKey(
                        name: "FK_SubjectBatchTimingMTFs_BatchTimingSaturdays_BatchTimingSundayId",
                        column: x => x.BatchTimingSundayId,
                        principalTable: "BatchTimingSaturdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    BatchTimingSundayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    BatchTimingSundayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        name: "FK_SubjectBatchTimingSundays_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchTimingSundays_SubjectId",
                table: "BatchTimingSundays",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTimingSaturdays_SubjectId",
                table: "BatchTimingSaturdays",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTimingMTFs_SubjectId",
                table: "BatchTimingMTFs",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingMTFs_BatchTimingSundayId",
                table: "SubjectBatchTimingMTFs",
                column: "BatchTimingSundayId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSaturdays_BatchTimingSundayId",
                table: "SubjectBatchTimingSaturdays",
                column: "BatchTimingSundayId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSundays_BatchTimingSundayId",
                table: "SubjectBatchTimingSundays",
                column: "BatchTimingSundayId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTimingMTFs_Subjects_SubjectId",
                table: "BatchTimingMTFs",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTimingSaturdays_Subjects_SubjectId",
                table: "BatchTimingSaturdays",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTimingSundays_Subjects_SubjectId",
                table: "BatchTimingSundays",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchTimingMTFs_Subjects_SubjectId",
                table: "BatchTimingMTFs");

            migrationBuilder.DropForeignKey(
                name: "FK_BatchTimingSaturdays_Subjects_SubjectId",
                table: "BatchTimingSaturdays");

            migrationBuilder.DropForeignKey(
                name: "FK_BatchTimingSundays_Subjects_SubjectId",
                table: "BatchTimingSundays");

            migrationBuilder.DropTable(
                name: "SubjectBatchTimingMTFs");

            migrationBuilder.DropTable(
                name: "SubjectBatchTimingSaturdays");

            migrationBuilder.DropTable(
                name: "SubjectBatchTimingSundays");

            migrationBuilder.DropIndex(
                name: "IX_BatchTimingSundays_SubjectId",
                table: "BatchTimingSundays");

            migrationBuilder.DropIndex(
                name: "IX_BatchTimingSaturdays_SubjectId",
                table: "BatchTimingSaturdays");

            migrationBuilder.DropIndex(
                name: "IX_BatchTimingMTFs_SubjectId",
                table: "BatchTimingMTFs");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "BatchTimingSundays");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "BatchTimingSaturdays");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "BatchTimingMTFs");

            migrationBuilder.AlterColumn<string>(
                name: "BatchTime",
                table: "BatchTimingSundays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "BatchTime",
                table: "BatchTimingSaturdays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "BatchTime",
                table: "BatchTimingMTFs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}

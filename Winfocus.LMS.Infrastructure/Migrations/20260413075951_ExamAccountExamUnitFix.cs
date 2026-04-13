using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExamAccountExamUnitFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAccounts_ExamUnits_UnitId",
                table: "ExamAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAccounts_ExamUnits_UnitId",
                table: "ExamAccounts",
                column: "UnitId",
                principalTable: "ExamUnits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAccounts_ExamUnits_UnitId",
                table: "ExamAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAccounts_ExamUnits_UnitId",
                table: "ExamAccounts",
                column: "UnitId",
                principalTable: "ExamUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

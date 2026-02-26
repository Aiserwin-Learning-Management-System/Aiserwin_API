using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixGradeStreamRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Streams_Grades_GradeId",
                table: "Streams");

            migrationBuilder.DropIndex(
                name: "IX_Streams_GradeId",
                table: "Streams");

            migrationBuilder.CreateIndex(
                name: "IX_Streams_GradeId_Name",
                table: "Streams",
                columns: new[] { "GradeId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Streams_Grades_GradeId",
                table: "Streams",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Streams_Grades_GradeId",
                table: "Streams");

            migrationBuilder.DropIndex(
                name: "IX_Streams_GradeId_Name",
                table: "Streams");

            migrationBuilder.CreateIndex(
                name: "IX_Streams_GradeId",
                table: "Streams",
                column: "GradeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Streams_Grades_GradeId",
                table: "Streams",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

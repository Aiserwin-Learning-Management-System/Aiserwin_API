using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseForeignKeyToStudentFeeSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeSelections_CourseId",
                table: "StudentFeeSelections",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFeeSelections_Courses_CourseId",
                table: "StudentFeeSelections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentFeeSelections_Courses_CourseId",
                table: "StudentFeeSelections");

            migrationBuilder.DropIndex(
                name: "IX_StudentFeeSelections_CourseId",
                table: "StudentFeeSelections");
        }
    }
}

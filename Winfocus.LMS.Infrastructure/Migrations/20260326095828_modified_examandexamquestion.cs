using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modified_examandexamquestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Grades_GradeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Syllabuses_SyllabusId",
                table: "Exams");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamGrades_GradeId",
                table: "Exams",
                column: "GradeId",
                principalTable: "ExamGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamSyllabuses_SyllabusId",
                table: "Exams",
                column: "SyllabusId",
                principalTable: "ExamSyllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamGrades_GradeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamSyllabuses_SyllabusId",
                table: "Exams");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Grades_GradeId",
                table: "Exams",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Syllabuses_SyllabusId",
                table: "Exams",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

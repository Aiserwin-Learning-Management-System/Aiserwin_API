using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modified_exammodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Countries_CountryId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Courses_CourseId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamGrades_GradeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamSyllabuses_SyllabusId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamUnits_UnitId",
                table: "Exams");

            migrationBuilder.AddColumn<Guid>(
                name: "CorrectOptionId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CorrectOptionId",
                table: "Questions",
                column: "CorrectOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExamDate",
                table: "Exams",
                column: "ExamDate");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamId_QuestionId_UQ",
                table: "ExamQuestions",
                columns: new[] { "ExamId", "QuestionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Countries_CountryId",
                table: "Exams",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Courses_CourseId",
                table: "Exams",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamUnits_UnitId",
                table: "Exams",
                column: "UnitId",
                principalTable: "ExamUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionOptions_CorrectOptionId",
                table: "Questions",
                column: "CorrectOptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Countries_CountryId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Courses_CourseId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamGrades_GradeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamSyllabuses_SyllabusId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamUnits_UnitId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionOptions_CorrectOptionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CorrectOptionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ExamDate",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_ExamQuestions_ExamId_QuestionId_UQ",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "CorrectOptionId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Countries_CountryId",
                table: "Exams",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Courses_CourseId",
                table: "Exams",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamGrades_GradeId",
                table: "Exams",
                column: "GradeId",
                principalTable: "ExamGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamSyllabuses_SyllabusId",
                table: "Exams",
                column: "SyllabusId",
                principalTable: "ExamSyllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamUnits_UnitId",
                table: "Exams",
                column: "UnitId",
                principalTable: "ExamUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

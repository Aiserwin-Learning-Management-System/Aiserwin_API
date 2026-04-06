using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tablemodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamChapters_ExamUnits_UnitId",
                table: "ExamChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicYearId",
                table: "Syllabuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "StudentFeeDiscounts",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "StudentCourseDiscounts",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_AcademicYearId",
                table: "Syllabuses",
                column: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamChapters_ExamUnits_UnitId",
                table: "ExamChapters",
                column: "UnitId",
                principalTable: "ExamUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups",
                column: "FieldGroupId",
                principalTable: "FieldGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamChapters_ExamUnits_UnitId",
                table: "ExamChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.DropIndex(
                name: "IX_Syllabuses_AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "StudentFeeDiscounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "StudentCourseDiscounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamChapters_ExamUnits_UnitId",
                table: "ExamChapters",
                column: "UnitId",
                principalTable: "ExamUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups",
                column: "FieldGroupId",
                principalTable: "FieldGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

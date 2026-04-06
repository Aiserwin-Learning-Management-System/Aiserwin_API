using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAcademicYearFkAndModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // STEP 1: Insert placeholder AcademicYear with zero GUID.
            // This must exist before the FK can be added because existing Centres rows
            // have AcademicYearId = 00000000-0000-0000-0000-000000000000.
            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM [AcademicYears] WHERE [Id] = '00000000-0000-0000-0000-000000000000')
                BEGIN
                    INSERT INTO [AcademicYears] ([Id], [Name], [StartDate], [EndDate], [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy])
                    VALUES ('00000000-0000-0000-0000-000000000000', 'Not Specified', '1900-01-01', '2099-12-31', 1, 0, GETUTCDATE(), '00000000-0000-0000-0000-000000000000', NULL, NULL);
                END;");

            // STEP 2: Add AcademicYearId column + index + FK to Centres.
            // This was part of the failed migration tablesmodifiedfordtp.
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicYearId",
                table: "Centres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Centres_AcademicYearId",
                table: "Centres",
                column: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Centres_AcademicYears_AcademicYearId",
                table: "Centres",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // STEP 3: Fix decimal precision for FeePlanDiscount.DiscountPercent.
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "FeePlanDiscount",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            // STEP 4: Fix global query filter mismatches.
            // Entities with soft-delete filters (Question, FormField, FieldGroup) cause null-reference
            // warnings when their children have required navigations to them. Changing OnDelete to
            // SetNull prevents orphaned records and resolves EF Core's query filter warnings.

            // FieldOption → FormField: Cascade → SetNull
            migrationBuilder.DropForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions");
            migrationBuilder.AddForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // QuestionOption → Question: Cascade → SetNull
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");
            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // QuestionReview → Question: Restrict → SetNull
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews");
            migrationBuilder.AddForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // RegistrationFormField → FormField: Restrict → SetNull
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields");
            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // RegistrationFormGroup → FieldGroup: Restrict → SetNull
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups");
            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups",
                column: "FieldGroupId",
                principalTable: "FieldGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // StaffRegistrationValue → FormField: Restrict → SetNull
            migrationBuilder.DropForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues");
            migrationBuilder.AddForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // ExamChapter → ExamUnit: navigation made optional via EntityConfiguration
            // (no FK change needed; ExamUnitId is still required in the table)
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert decimal precision
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "FeePlanDiscount",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            // Revert FK onDelete behaviors
            migrationBuilder.DropForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions");
            migrationBuilder.AddForeignKey(
                name: "FK_FieldOptions_FormFields_FieldId",
                table: "FieldOptions",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");
            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews");
            migrationBuilder.AddForeignKey(
                name: "FK_QuestionReviews_Questions_QuestionId",
                table: "QuestionReviews",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields");
            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormFields_FormFields_FieldId",
                table: "RegistrationFormFields",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups");
            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                table: "RegistrationFormGroups",
                column: "FieldGroupId",
                principalTable: "FieldGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues");
            migrationBuilder.AddForeignKey(
                name: "FK_StaffRegistrationValues_FormFields_FieldId",
                table: "StaffRegistrationValues",
                column: "FieldId",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Remove AcademicYearId from Centres
            migrationBuilder.DropForeignKey(
                name: "FK_Centres_AcademicYears_AcademicYearId",
                table: "Centres");
            migrationBuilder.DropIndex(name: "IX_Centres_AcademicYearId", table: "Centres");
            migrationBuilder.DropColumn(name: "AcademicYearId", table: "Centres");
        }
    }
}

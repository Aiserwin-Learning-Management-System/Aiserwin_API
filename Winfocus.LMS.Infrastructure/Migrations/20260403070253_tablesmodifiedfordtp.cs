using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tablesmodifiedfordtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamGrades_GradeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamSyllabuses_SyllabusId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamUnits_ExamSubjects_SubjectId",
                table: "ExamUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionConfigurations_ExamGrades_GradeId",
                table: "QuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionConfigurations_ExamSubjects_SubjectId",
                table: "QuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionConfigurations_ExamSyllabuses_SyllabusId",
                table: "QuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeConfigs_ExamGrades_GradeId",
                table: "QuestionTypeConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeConfigs_ExamSubjects_SubjectId",
                table: "QuestionTypeConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeConfigs_ExamSyllabuses_SyllabusId",
                table: "QuestionTypeConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ExamGrades_GradeId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ExamSubjects_SubjectId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_ExamSyllabuses_SyllabusId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPreferredGrades_ExamGrades_ExamGradeId",
                table: "TeacherPreferredGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPreferredSubjects_ExamSubjects_ExamSubjectId",
                table: "TeacherPreferredSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSyllabi_ExamSyllabuses_ExamSyllabusId",
                table: "TeacherSyllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherTaughtGrades_ExamGrades_ExamGradeId",
                table: "TeacherTaughtGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherTaughtSubjects_ExamSubjects_ExamSubjectId",
                table: "TeacherTaughtSubjects");

            migrationBuilder.DropTable(
                name: "ExamSubjects");

            migrationBuilder.DropTable(
                name: "ExamGrades");

            migrationBuilder.DropTable(
                name: "ExamSyllabuses");

            migrationBuilder.DropColumn(
                name: "EmergencyContactNumber",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "AdditionalCourses",
                table: "TeacherProfessionalDetail");

            migrationBuilder.DropColumn(
                name: "HasTeachingCertification",
                table: "TeacherProfessionalDetail");

            migrationBuilder.DropColumn(
                name: "University",
                table: "TeacherProfessionalDetail");

            migrationBuilder.DropColumn(
                name: "YearOfPassing",
                table: "TeacherProfessionalDetail");

            migrationBuilder.DropColumn(
                name: "Mode",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "ExamSubjectId",
                table: "TeacherTaughtSubjects",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherTaughtSubjects_ExamSubjectId",
                table: "TeacherTaughtSubjects",
                newName: "IX_TeacherTaughtSubjects_SubjectId");

            migrationBuilder.RenameColumn(
                name: "ExamGradeId",
                table: "TeacherTaughtGrades",
                newName: "GradeId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherTaughtGrades_ExamGradeId",
                table: "TeacherTaughtGrades",
                newName: "IX_TeacherTaughtGrades_GradeId");

            migrationBuilder.RenameColumn(
                name: "ExamSyllabusId",
                table: "TeacherSyllabi",
                newName: "SyllabusId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSyllabi_ExamSyllabusId",
                table: "TeacherSyllabi",
                newName: "IX_TeacherSyllabi_SyllabusId");

            migrationBuilder.RenameColumn(
                name: "DeclarationDate",
                table: "TeacherRegistrations",
                newName: "JoiningDate");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "TeacherRegistrations",
                newName: "ResidentialAddress");

            migrationBuilder.RenameColumn(
                name: "ExamSubjectId",
                table: "TeacherPreferredSubjects",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherPreferredSubjects_ExamSubjectId",
                table: "TeacherPreferredSubjects",
                newName: "IX_TeacherPreferredSubjects_SubjectId");

            migrationBuilder.RenameColumn(
                name: "ExamGradeId",
                table: "TeacherPreferredGrades",
                newName: "GradeId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherPreferredGrades_ExamGradeId",
                table: "TeacherPreferredGrades",
                newName: "IX_TeacherPreferredGrades_GradeId");

            migrationBuilder.RenameColumn(
                name: "IdCardPath",
                table: "TeacherDocumentInfo",
                newName: "ProofNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherProfessionalDetailId",
                table: "TeacherTools",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherProfessionalDetailId",
                table: "TeacherSyllabi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlternativeEmailAddress",
                table: "TeacherRegistrations",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlternativeMobileNumber",
                table: "TeacherRegistrations",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ContractDuration",
                table: "TeacherRegistrations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "TeacherRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DistrictOrLocation",
                table: "TeacherRegistrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclared",
                table: "TeacherRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSignedAgreement",
                table: "TeacherRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "TeacherRegistrations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefernceContactName",
                table: "TeacherRegistrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefernceContactNumber",
                table: "TeacherRegistrations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "TeacherRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherProfessionalDetailId",
                table: "TeacherLanguages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProofType",
                table: "TeacherDocumentInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherProfessionalDetailId",
                table: "TeacherAvailability",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicYearId",
                table: "Syllabuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModeOfStudyId",
                table: "Exams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceTypeId",
                table: "Exams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "Exams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Exams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicYearId",
                table: "Centres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTools_TeacherProfessionalDetailId",
                table: "TeacherTools",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSyllabi_TeacherProfessionalDetailId",
                table: "TeacherSyllabi",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_CountryId",
                table: "TeacherRegistrations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherRegistrations_StateId",
                table: "TeacherRegistrations",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLanguages_TeacherProfessionalDetailId",
                table: "TeacherLanguages",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAvailability_TeacherProfessionalDetailId",
                table: "TeacherAvailability",
                column: "TeacherProfessionalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_AcademicYearId",
                table: "Syllabuses",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ModeOfStudyId",
                table: "Exams",
                column: "ModeOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ResourceTypeId",
                table: "Exams",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_StateId",
                table: "Exams",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SubjectId",
                table: "Exams",
                column: "SubjectId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ContentResourceTypes_ResourceTypeId",
                table: "Exams",
                column: "ResourceTypeId",
                principalTable: "ContentResourceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Grades_GradeId",
                table: "Exams",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ModeOfStudies_ModeOfStudyId",
                table: "Exams",
                column: "ModeOfStudyId",
                principalTable: "ModeOfStudies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_States_StateId",
                table: "Exams",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Subjects_SubjectId",
                table: "Exams",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Syllabuses_SyllabusId",
                table: "Exams",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamUnits_Subjects_SubjectId",
                table: "ExamUnits",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionConfigurations_Grades_GradeId",
                table: "QuestionConfigurations",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionConfigurations_Subjects_SubjectId",
                table: "QuestionConfigurations",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionConfigurations_Syllabuses_SyllabusId",
                table: "QuestionConfigurations",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeConfigs_Grades_GradeId",
                table: "QuestionTypeConfigs",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeConfigs_Subjects_SubjectId",
                table: "QuestionTypeConfigs",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeConfigs_Syllabuses_SyllabusId",
                table: "QuestionTypeConfigs",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Grades_GradeId",
                table: "TaskAssignments",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Subjects_SubjectId",
                table: "TaskAssignments",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Syllabuses_SyllabusId",
                table: "TaskAssignments",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAvailability_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherAvailability",
                column: "TeacherProfessionalDetailId",
                principalTable: "TeacherProfessionalDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherLanguages_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherLanguages",
                column: "TeacherProfessionalDetailId",
                principalTable: "TeacherProfessionalDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPreferredGrades_Grades_GradeId",
                table: "TeacherPreferredGrades",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPreferredSubjects_Subjects_SubjectId",
                table: "TeacherPreferredSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherRegistrations_Countries_CountryId",
                table: "TeacherRegistrations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherRegistrations_States_StateId",
                table: "TeacherRegistrations",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSyllabi_Syllabuses_SyllabusId",
                table: "TeacherSyllabi",
                column: "SyllabusId",
                principalTable: "Syllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSyllabi_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherSyllabi",
                column: "TeacherProfessionalDetailId",
                principalTable: "TeacherProfessionalDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherTaughtGrades_Grades_GradeId",
                table: "TeacherTaughtGrades",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherTaughtSubjects_Subjects_SubjectId",
                table: "TeacherTaughtSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherTools_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherTools",
                column: "TeacherProfessionalDetailId",
                principalTable: "TeacherProfessionalDetail",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Centres_AcademicYears_AcademicYearId",
                table: "Centres");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ContentResourceTypes_ResourceTypeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Grades_GradeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ModeOfStudies_ModeOfStudyId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_States_StateId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Subjects_SubjectId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Syllabuses_SyllabusId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamUnits_Subjects_SubjectId",
                table: "ExamUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionConfigurations_Grades_GradeId",
                table: "QuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionConfigurations_Subjects_SubjectId",
                table: "QuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionConfigurations_Syllabuses_SyllabusId",
                table: "QuestionConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeConfigs_Grades_GradeId",
                table: "QuestionTypeConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeConfigs_Subjects_SubjectId",
                table: "QuestionTypeConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeConfigs_Syllabuses_SyllabusId",
                table: "QuestionTypeConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Grades_GradeId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Subjects_SubjectId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Syllabuses_SyllabusId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAvailability_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherAvailability");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherLanguages_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherLanguages");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPreferredGrades_Grades_GradeId",
                table: "TeacherPreferredGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPreferredSubjects_Subjects_SubjectId",
                table: "TeacherPreferredSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherRegistrations_Countries_CountryId",
                table: "TeacherRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherRegistrations_States_StateId",
                table: "TeacherRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSyllabi_Syllabuses_SyllabusId",
                table: "TeacherSyllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSyllabi_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherSyllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherTaughtGrades_Grades_GradeId",
                table: "TeacherTaughtGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherTaughtSubjects_Subjects_SubjectId",
                table: "TeacherTaughtSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherTools_TeacherProfessionalDetail_TeacherProfessionalDetailId",
                table: "TeacherTools");

            migrationBuilder.DropIndex(
                name: "IX_TeacherTools_TeacherProfessionalDetailId",
                table: "TeacherTools");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSyllabi_TeacherProfessionalDetailId",
                table: "TeacherSyllabi");

            migrationBuilder.DropIndex(
                name: "IX_TeacherRegistrations_CountryId",
                table: "TeacherRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TeacherRegistrations_StateId",
                table: "TeacherRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TeacherLanguages_TeacherProfessionalDetailId",
                table: "TeacherLanguages");

            migrationBuilder.DropIndex(
                name: "IX_TeacherAvailability_TeacherProfessionalDetailId",
                table: "TeacherAvailability");

            migrationBuilder.DropIndex(
                name: "IX_Syllabuses_AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ModeOfStudyId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ResourceTypeId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_StateId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_SubjectId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Centres_AcademicYearId",
                table: "Centres");

            migrationBuilder.DropColumn(
                name: "TeacherProfessionalDetailId",
                table: "TeacherTools");

            migrationBuilder.DropColumn(
                name: "TeacherProfessionalDetailId",
                table: "TeacherSyllabi");

            migrationBuilder.DropColumn(
                name: "AlternativeEmailAddress",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "AlternativeMobileNumber",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "ContractDuration",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "DistrictOrLocation",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "IsDeclared",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "IsSignedAgreement",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "RefernceContactName",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "RefernceContactNumber",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "TeacherRegistrations");

            migrationBuilder.DropColumn(
                name: "TeacherProfessionalDetailId",
                table: "TeacherLanguages");

            migrationBuilder.DropColumn(
                name: "ProofType",
                table: "TeacherDocumentInfo");

            migrationBuilder.DropColumn(
                name: "TeacherProfessionalDetailId",
                table: "TeacherAvailability");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.DropColumn(
                name: "ModeOfStudyId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ResourceTypeId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Centres");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "TeacherTaughtSubjects",
                newName: "ExamSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherTaughtSubjects_SubjectId",
                table: "TeacherTaughtSubjects",
                newName: "IX_TeacherTaughtSubjects_ExamSubjectId");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "TeacherTaughtGrades",
                newName: "ExamGradeId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherTaughtGrades_GradeId",
                table: "TeacherTaughtGrades",
                newName: "IX_TeacherTaughtGrades_ExamGradeId");

            migrationBuilder.RenameColumn(
                name: "SyllabusId",
                table: "TeacherSyllabi",
                newName: "ExamSyllabusId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSyllabi_SyllabusId",
                table: "TeacherSyllabi",
                newName: "IX_TeacherSyllabi_ExamSyllabusId");

            migrationBuilder.RenameColumn(
                name: "ResidentialAddress",
                table: "TeacherRegistrations",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "JoiningDate",
                table: "TeacherRegistrations",
                newName: "DeclarationDate");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "TeacherPreferredSubjects",
                newName: "ExamSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherPreferredSubjects_SubjectId",
                table: "TeacherPreferredSubjects",
                newName: "IX_TeacherPreferredSubjects_ExamSubjectId");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "TeacherPreferredGrades",
                newName: "ExamGradeId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherPreferredGrades_GradeId",
                table: "TeacherPreferredGrades",
                newName: "IX_TeacherPreferredGrades_ExamGradeId");

            migrationBuilder.RenameColumn(
                name: "ProofNumber",
                table: "TeacherDocumentInfo",
                newName: "IdCardPath");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactNumber",
                table: "TeacherRegistrations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                table: "TeacherRegistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalCourses",
                table: "TeacherProfessionalDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasTeachingCertification",
                table: "TeacherProfessionalDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "TeacherProfessionalDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YearOfPassing",
                table: "TeacherProfessionalDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExamSyllabuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSyllabuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSyllabuses_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SyllabusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamGrades_ExamSyllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "ExamSyllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSubjects_ExamGrades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "ExamGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamGrades_SyllabusId_Name_WhereNotDeleted",
                table: "ExamGrades",
                columns: new[] { "SyllabusId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_GradeId_Name_WhereNotDeleted",
                table: "ExamSubjects",
                columns: new[] { "GradeId", "Name" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSyllabuses_AcademicYearId",
                table: "ExamSyllabuses",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSyllabuses_Name_AcademicYearId_WhereNotDeleted",
                table: "ExamSyllabuses",
                columns: new[] { "Name", "AcademicYearId" },
                unique: true,
                filter: "[IsDeleted] = 0");

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
                name: "FK_ExamUnits_ExamSubjects_SubjectId",
                table: "ExamUnits",
                column: "SubjectId",
                principalTable: "ExamSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionConfigurations_ExamGrades_GradeId",
                table: "QuestionConfigurations",
                column: "GradeId",
                principalTable: "ExamGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionConfigurations_ExamSubjects_SubjectId",
                table: "QuestionConfigurations",
                column: "SubjectId",
                principalTable: "ExamSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionConfigurations_ExamSyllabuses_SyllabusId",
                table: "QuestionConfigurations",
                column: "SyllabusId",
                principalTable: "ExamSyllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeConfigs_ExamGrades_GradeId",
                table: "QuestionTypeConfigs",
                column: "GradeId",
                principalTable: "ExamGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeConfigs_ExamSubjects_SubjectId",
                table: "QuestionTypeConfigs",
                column: "SubjectId",
                principalTable: "ExamSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeConfigs_ExamSyllabuses_SyllabusId",
                table: "QuestionTypeConfigs",
                column: "SyllabusId",
                principalTable: "ExamSyllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_ExamGrades_GradeId",
                table: "TaskAssignments",
                column: "GradeId",
                principalTable: "ExamGrades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_ExamSubjects_SubjectId",
                table: "TaskAssignments",
                column: "SubjectId",
                principalTable: "ExamSubjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_ExamSyllabuses_SyllabusId",
                table: "TaskAssignments",
                column: "SyllabusId",
                principalTable: "ExamSyllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPreferredGrades_ExamGrades_ExamGradeId",
                table: "TeacherPreferredGrades",
                column: "ExamGradeId",
                principalTable: "ExamGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPreferredSubjects_ExamSubjects_ExamSubjectId",
                table: "TeacherPreferredSubjects",
                column: "ExamSubjectId",
                principalTable: "ExamSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSyllabi_ExamSyllabuses_ExamSyllabusId",
                table: "TeacherSyllabi",
                column: "ExamSyllabusId",
                principalTable: "ExamSyllabuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherTaughtGrades_ExamGrades_ExamGradeId",
                table: "TeacherTaughtGrades",
                column: "ExamGradeId",
                principalTable: "ExamGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherTaughtSubjects_ExamSubjects_ExamSubjectId",
                table: "TeacherTaughtSubjects",
                column: "ExamSubjectId",
                principalTable: "ExamSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

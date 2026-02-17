using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class studentchnages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_BatchTimingMTFs_BatchTimingMTFId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_BatchTimingSaturdays_BatchTimingSaturdayId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_BatchTimingSundays_BatchTimingSundayId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_Courses_CourseId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_PreferredBatchId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicDetails_BatchTimingMTFId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicDetails_BatchTimingSaturdayId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicDetails_BatchTimingSundayId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicDetails_CourseId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropColumn(
                name: "BatchTimingMTFId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropColumn(
                name: "BatchTimingSaturdayId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropColumn(
                name: "BatchTimingSundayId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "StudentAcademicDetails");

            migrationBuilder.RenameColumn(
                name: "StudentSignature",
                table: "StudentDocuments",
                newName: "StudentSignaturePath");

            migrationBuilder.RenameColumn(
                name: "StudentPhoto",
                table: "StudentDocuments",
                newName: "StudentPhotoPath");

            migrationBuilder.RenameColumn(
                name: "PreferredBatchTime",
                table: "StudentAcademicDetails",
                newName: "PreferredBatchTimeId");

            migrationBuilder.RenameColumn(
                name: "PreferredBatchId",
                table: "StudentAcademicDetails",
                newName: "BatchId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAcademicDetails_PreferredBatchId",
                table: "StudentAcademicDetails",
                newName: "IX_StudentAcademicDetails_BatchId");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptedAgreement",
                table: "StudentDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptedTermsAndConditions",
                table: "StudentDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_PreferredBatchTimeId",
                table: "StudentAcademicDetails",
                column: "PreferredBatchTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_BatchId",
                table: "StudentAcademicDetails",
                column: "BatchId",
                principalTable: "PreferredBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_PreferredBatchTimeId",
                table: "StudentAcademicDetails",
                column: "PreferredBatchTimeId",
                principalTable: "PreferredBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_BatchId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_PreferredBatchTimeId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicDetails_PreferredBatchTimeId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsAcceptedAgreement",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "IsAcceptedTermsAndConditions",
                table: "StudentDocuments");

            migrationBuilder.RenameColumn(
                name: "StudentSignaturePath",
                table: "StudentDocuments",
                newName: "StudentSignature");

            migrationBuilder.RenameColumn(
                name: "StudentPhotoPath",
                table: "StudentDocuments",
                newName: "StudentPhoto");

            migrationBuilder.RenameColumn(
                name: "PreferredBatchTimeId",
                table: "StudentAcademicDetails",
                newName: "PreferredBatchTime");

            migrationBuilder.RenameColumn(
                name: "BatchId",
                table: "StudentAcademicDetails",
                newName: "PreferredBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAcademicDetails_BatchId",
                table: "StudentAcademicDetails",
                newName: "IX_StudentAcademicDetails_PreferredBatchId");

            migrationBuilder.AddColumn<Guid>(
                name: "BatchTimingMTFId",
                table: "StudentAcademicDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BatchTimingSaturdayId",
                table: "StudentAcademicDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BatchTimingSundayId",
                table: "StudentAcademicDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "StudentAcademicDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_BatchTimingMTFId",
                table: "StudentAcademicDetails",
                column: "BatchTimingMTFId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_BatchTimingSaturdayId",
                table: "StudentAcademicDetails",
                column: "BatchTimingSaturdayId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_BatchTimingSundayId",
                table: "StudentAcademicDetails",
                column: "BatchTimingSundayId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_CourseId",
                table: "StudentAcademicDetails",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_BatchTimingMTFs_BatchTimingMTFId",
                table: "StudentAcademicDetails",
                column: "BatchTimingMTFId",
                principalTable: "BatchTimingMTFs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_BatchTimingSaturdays_BatchTimingSaturdayId",
                table: "StudentAcademicDetails",
                column: "BatchTimingSaturdayId",
                principalTable: "BatchTimingSaturdays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_BatchTimingSundays_BatchTimingSundayId",
                table: "StudentAcademicDetails",
                column: "BatchTimingSundayId",
                principalTable: "BatchTimingSundays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_Courses_CourseId",
                table: "StudentAcademicDetails",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_PreferredBatchId",
                table: "StudentAcademicDetails",
                column: "PreferredBatchId",
                principalTable: "PreferredBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

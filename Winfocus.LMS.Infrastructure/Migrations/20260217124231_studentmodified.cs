using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class studentmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentAcademicDetails_AcademicDetailsId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentPersonalDetails_PersonalDetailsId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicDetailsId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PersonalDetailsId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AcademicDetailsId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PersonalDetailsId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StudentPersonalId",
                table: "Students",
                newName: "StudentPersonalDetailsId");

            migrationBuilder.RenameColumn(
                name: "StudentAcademicId",
                table: "Students",
                newName: "StudentAcademicDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentAcademicDetailsId",
                table: "Students",
                column: "StudentAcademicDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentPersonalDetailsId",
                table: "Students",
                column: "StudentPersonalDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentAcademicDetails_StudentAcademicDetailsId",
                table: "Students",
                column: "StudentAcademicDetailsId",
                principalTable: "StudentAcademicDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentPersonalDetails_StudentPersonalDetailsId",
                table: "Students",
                column: "StudentPersonalDetailsId",
                principalTable: "StudentPersonalDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentAcademicDetails_StudentAcademicDetailsId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentPersonalDetails_StudentPersonalDetailsId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentAcademicDetailsId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentPersonalDetailsId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StudentPersonalDetailsId",
                table: "Students",
                newName: "StudentPersonalId");

            migrationBuilder.RenameColumn(
                name: "StudentAcademicDetailsId",
                table: "Students",
                newName: "StudentAcademicId");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicDetailsId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PersonalDetailsId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicDetailsId",
                table: "Students",
                column: "AcademicDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_PersonalDetailsId",
                table: "Students",
                column: "PersonalDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentAcademicDetails_AcademicDetailsId",
                table: "Students",
                column: "AcademicDetailsId",
                principalTable: "StudentAcademicDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentPersonalDetails_PersonalDetailsId",
                table: "Students",
                column: "PersonalDetailsId",
                principalTable: "StudentPersonalDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

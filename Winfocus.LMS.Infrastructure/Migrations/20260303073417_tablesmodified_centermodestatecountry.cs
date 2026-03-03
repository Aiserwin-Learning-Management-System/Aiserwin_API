using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tablesmodified_centermodestatecountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeOfStudies_States_StateId",
                table: "ModeOfStudies");

            migrationBuilder.DropColumn(
                name: "PreferredBatchTimeId",
                table: "StudentAcademicDetails");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "ModeOfStudies",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_ModeOfStudies_StateId",
                table: "ModeOfStudies",
                newName: "IX_ModeOfStudies_CountryId");

            migrationBuilder.AddColumn<string>(
                name: "PreferredTime",
                table: "StudentAcademicDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ModeOfStudyId",
                table: "States",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_States_ModeOfStudyId",
                table: "States",
                column: "ModeOfStudyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeOfStudies_Countries_CountryId",
                table: "ModeOfStudies",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_States_ModeOfStudies_ModeOfStudyId",
                table: "States",
                column: "ModeOfStudyId",
                principalTable: "ModeOfStudies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeOfStudies_Countries_CountryId",
                table: "ModeOfStudies");

            migrationBuilder.DropForeignKey(
                name: "FK_States_ModeOfStudies_ModeOfStudyId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_ModeOfStudyId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "PreferredTime",
                table: "StudentAcademicDetails");

            migrationBuilder.DropColumn(
                name: "ModeOfStudyId",
                table: "States");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "ModeOfStudies",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_ModeOfStudies_CountryId",
                table: "ModeOfStudies",
                newName: "IX_ModeOfStudies_StateId");

            migrationBuilder.AddColumn<Guid>(
                name: "PreferredBatchTimeId",
                table: "StudentAcademicDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_ModeOfStudies_States_StateId",
                table: "ModeOfStudies",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

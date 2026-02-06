using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mastertablesmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeOfStudies_Countries_CountryId",
                table: "ModeOfStudies");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "ModeOfStudies",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_ModeOfStudies_CountryId",
                table: "ModeOfStudies",
                newName: "IX_ModeOfStudies_StateId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Centres",
                newName: "CenterType");

            migrationBuilder.AddColumn<string>(
                name: "ModeCode",
                table: "ModeOfStudies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Centres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ModeOfStudyId",
                table: "Centres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Centres_ModeOfStudyId",
                table: "Centres",
                column: "ModeOfStudyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Centres_ModeOfStudies_ModeOfStudyId",
                table: "Centres",
                column: "ModeOfStudyId",
                principalTable: "ModeOfStudies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeOfStudies_States_StateId",
                table: "ModeOfStudies",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Centres_ModeOfStudies_ModeOfStudyId",
                table: "Centres");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeOfStudies_States_StateId",
                table: "ModeOfStudies");

            migrationBuilder.DropIndex(
                name: "IX_Centres_ModeOfStudyId",
                table: "Centres");

            migrationBuilder.DropColumn(
                name: "ModeCode",
                table: "ModeOfStudies");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Centres");

            migrationBuilder.DropColumn(
                name: "ModeOfStudyId",
                table: "Centres");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "ModeOfStudies",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_ModeOfStudies_StateId",
                table: "ModeOfStudies",
                newName: "IX_ModeOfStudies_CountryId");

            migrationBuilder.RenameColumn(
                name: "CenterType",
                table: "Centres",
                newName: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeOfStudies_Countries_CountryId",
                table: "ModeOfStudies",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

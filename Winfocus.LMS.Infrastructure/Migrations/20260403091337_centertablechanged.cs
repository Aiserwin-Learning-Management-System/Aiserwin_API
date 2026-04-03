using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class centertablechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Centres_AcademicYears_AcademicYearId",
                table: "Centres");

            migrationBuilder.DropIndex(
                name: "IX_Centres_AcademicYearId",
                table: "Centres");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Centres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

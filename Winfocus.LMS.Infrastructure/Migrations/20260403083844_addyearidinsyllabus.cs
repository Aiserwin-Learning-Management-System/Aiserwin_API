using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addyearidinsyllabus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Centres_CenterId",
                table: "Syllabuses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CenterId",
                table: "Syllabuses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicYearId",
                table: "Syllabuses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_Centres_CenterId",
                table: "Syllabuses",
                column: "CenterId",
                principalTable: "Centres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Centres_CenterId",
                table: "Syllabuses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CenterId",
                table: "Syllabuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicYearId",
                table: "Syllabuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_AcademicYears_AcademicYearId",
                table: "Syllabuses",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_Centres_CenterId",
                table: "Syllabuses",
                column: "CenterId",
                principalTable: "Centres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

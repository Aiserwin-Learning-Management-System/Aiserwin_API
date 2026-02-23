using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedcenterIdinsyllabus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Centres_CenterId",
                table: "Syllabuses");

            migrationBuilder.DropIndex(
                name: "IX_Syllabuses_CenterId",
                table: "Syllabuses");

            migrationBuilder.DropColumn(
                name: "CenterId",
                table: "Syllabuses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CenterId",
                table: "Syllabuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_CenterId",
                table: "Syllabuses",
                column: "CenterId");

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

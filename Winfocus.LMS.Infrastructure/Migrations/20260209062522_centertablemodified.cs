using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class centertablemodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "Centres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Centres_StateId",
                table: "Centres",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Centres_States_StateId",
                table: "Centres",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Centres_States_StateId",
                table: "Centres");

            migrationBuilder.DropIndex(
                name: "IX_Centres_StateId",
                table: "Centres");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Centres");
        }
    }
}

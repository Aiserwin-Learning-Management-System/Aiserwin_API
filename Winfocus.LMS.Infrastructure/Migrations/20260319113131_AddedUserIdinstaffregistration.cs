using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdinstaffregistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "StaffRegistrations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_UserId",
                table: "StaffRegistrations",
                column: "UserId",
                unique: true,
                filter: "UserId IS NOT NULL AND IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StaffRegistrations_UserId",
                table: "StaffRegistrations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StaffRegistrations");
        }
    }
}

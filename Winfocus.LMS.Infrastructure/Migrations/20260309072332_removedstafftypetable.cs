using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedstafftypetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_StaffType_StaffTypeId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "StaffType");

            migrationBuilder.RenameColumn(
                name: "StaffTypeId",
                table: "Users",
                newName: "StaffCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_StaffTypeId",
                table: "Users",
                newName: "IX_Users_StaffCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StaffCategories_StaffCategoryId",
                table: "Users",
                column: "StaffCategoryId",
                principalTable: "StaffCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_StaffCategories_StaffCategoryId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "StaffCategoryId",
                table: "Users",
                newName: "StaffTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_StaffCategoryId",
                table: "Users",
                newName: "IX_Users_StaffTypeId");

            migrationBuilder.CreateTable(
                name: "StaffType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffType", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StaffType_StaffTypeId",
                table: "Users",
                column: "StaffTypeId",
                principalTable: "StaffType",
                principalColumn: "Id");
        }
    }
}

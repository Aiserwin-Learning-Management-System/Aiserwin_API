using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedgenderinstudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "StudentPersonalDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "StudentPersonalDetails");
        }
    }
}

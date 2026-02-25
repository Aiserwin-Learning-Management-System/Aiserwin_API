using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedcolumnsincountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_IsoAlpha3",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_IsoNumeric",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "IsoAlpha3",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "IsoNumeric",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "IsoAlpha3",
                table: "Countries",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IsoNumeric",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoAlpha3",
                table: "Countries",
                column: "IsoAlpha3",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoNumeric",
                table: "Countries",
                column: "IsoNumeric",
                unique: true);
        }
    }
}

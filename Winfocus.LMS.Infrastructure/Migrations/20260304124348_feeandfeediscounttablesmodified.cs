using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class feeandfeediscounttablesmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationinYears",
                table: "FeePlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "FeePlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "FeePlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FeePlans_SubjectId",
                table: "FeePlans",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeePlans_Subjects_SubjectId",
                table: "FeePlans",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeePlans_Subjects_SubjectId",
                table: "FeePlans");

            migrationBuilder.DropIndex(
                name: "IX_FeePlans_SubjectId",
                table: "FeePlans");

            migrationBuilder.DropColumn(
                name: "DurationinYears",
                table: "FeePlans");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "FeePlans");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "FeePlans");
        }
    }
}

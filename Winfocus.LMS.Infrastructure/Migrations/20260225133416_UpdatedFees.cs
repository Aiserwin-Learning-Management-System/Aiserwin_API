using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseFee",
                table: "StudentFeeSelections",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsManualDiscountActive",
                table: "StudentFeeSelections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScholarshipActive",
                table: "StudentFeeSelections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeasonalActive",
                table: "StudentFeeSelections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ScholarshipPercent",
                table: "StudentFeeSelections",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SeasonalPercent",
                table: "StudentFeeSelections",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeasonalDiscountActive",
                table: "FeePlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeSelections_FeePlanId",
                table: "StudentFeeSelections",
                column: "FeePlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFeeSelections_FeePlans_FeePlanId",
                table: "StudentFeeSelections",
                column: "FeePlanId",
                principalTable: "FeePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFeeSelections_Students_StudentId",
                table: "StudentFeeSelections",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentFeeSelections_FeePlans_FeePlanId",
                table: "StudentFeeSelections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentFeeSelections_Students_StudentId",
                table: "StudentFeeSelections");

            migrationBuilder.DropIndex(
                name: "IX_StudentFeeSelections_FeePlanId",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "BaseFee",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "IsManualDiscountActive",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "IsScholarshipActive",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "IsSeasonalActive",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "ScholarshipPercent",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "SeasonalPercent",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "IsSeasonalDiscountActive",
                table: "FeePlans");
        }
    }
}

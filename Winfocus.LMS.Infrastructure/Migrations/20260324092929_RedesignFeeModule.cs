using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RedesignFeeModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentInstallments_StudentFeeSelectionId",
                table: "StudentInstallments");

            migrationBuilder.DropIndex(
                name: "IX_StudentFeeSelections_StudentId_CourseId",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "StudentInstallments");

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
                name: "PaymentMode",
                table: "StudentFeeSelections");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "StudentInstallments",
                newName: "PaidAmount");

            migrationBuilder.RenameColumn(
                name: "SeasonalPercent",
                table: "StudentFeeSelections",
                newName: "YearlyFee");

            migrationBuilder.RenameColumn(
                name: "ScholarshipPercent",
                table: "StudentFeeSelections",
                newName: "TotalDiscountAmount");

            migrationBuilder.RenameColumn(
                name: "ManualDiscountPercent",
                table: "StudentFeeSelections",
                newName: "TotalBeforeDiscount");

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "StudentInstallments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DueAmount",
                table: "StudentInstallments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "StudentInstallments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "StudentInstallments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudentInstallments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "StudentFeeSelections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "StudentFeeSelections",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SelectedDurationYears",
                table: "StudentFeeSelections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "StudentFeeSelections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudentFeeSelections",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDiscountPercent",
                table: "StudentFeeSelections",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalInstallments",
                table: "StudentFeeSelections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentType",
                table: "FeePlans",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "StudentCourseDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeePlanDiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourseDiscounts_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCourseDiscounts_FeePlanDiscount_FeePlanDiscountId",
                        column: x => x.FeePlanDiscountId,
                        principalTable: "FeePlanDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudentCourseDiscounts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentFeeDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentFeeSelectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeeDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFeeDiscounts_StudentFeeSelections_StudentFeeSelectionId",
                        column: x => x.StudentFeeSelectionId,
                        principalTable: "StudentFeeSelections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstallments_StudentFeeSelectionId_InstallmentNo",
                table: "StudentInstallments",
                columns: new[] { "StudentFeeSelectionId", "InstallmentNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeSelections_StudentId_CourseId",
                table: "StudentFeeSelections",
                columns: new[] { "StudentId", "CourseId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseDiscounts_CourseId",
                table: "StudentCourseDiscounts",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseDiscounts_FeePlanDiscountId",
                table: "StudentCourseDiscounts",
                column: "FeePlanDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseDiscounts_StudentId_CourseId_DiscountName",
                table: "StudentCourseDiscounts",
                columns: new[] { "StudentId", "CourseId", "DiscountName" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeDiscounts_StudentFeeSelectionId",
                table: "StudentFeeDiscounts",
                column: "StudentFeeSelectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCourseDiscounts");

            migrationBuilder.DropTable(
                name: "StudentFeeDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_StudentInstallments_StudentFeeSelectionId_InstallmentNo",
                table: "StudentInstallments");

            migrationBuilder.DropIndex(
                name: "IX_StudentFeeSelections_StudentId_CourseId",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "StudentInstallments");

            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "StudentInstallments");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "StudentInstallments");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "StudentInstallments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentInstallments");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "SelectedDurationYears",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "TotalDiscountPercent",
                table: "StudentFeeSelections");

            migrationBuilder.DropColumn(
                name: "TotalInstallments",
                table: "StudentFeeSelections");

            migrationBuilder.RenameColumn(
                name: "PaidAmount",
                table: "StudentInstallments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "YearlyFee",
                table: "StudentFeeSelections",
                newName: "SeasonalPercent");

            migrationBuilder.RenameColumn(
                name: "TotalDiscountAmount",
                table: "StudentFeeSelections",
                newName: "ScholarshipPercent");

            migrationBuilder.RenameColumn(
                name: "TotalBeforeDiscount",
                table: "StudentFeeSelections",
                newName: "ManualDiscountPercent");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "StudentInstallments",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "StudentFeeSelections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                table: "FeePlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstallments_StudentFeeSelectionId",
                table: "StudentInstallments",
                column: "StudentFeeSelectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeSelections_StudentId_CourseId",
                table: "StudentFeeSelections",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);
        }
    }
}

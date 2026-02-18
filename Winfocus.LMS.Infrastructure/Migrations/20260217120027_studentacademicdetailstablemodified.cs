using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class studentacademicdetailstablemodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_BatchId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_PreferredBatchTimeId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicDetails_BatchId",
                table: "StudentAcademicDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicDetails_PreferredBatchTimeId",
                table: "StudentAcademicDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_BatchId",
                table: "StudentAcademicDetails",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicDetails_PreferredBatchTimeId",
                table: "StudentAcademicDetails",
                column: "PreferredBatchTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_BatchId",
                table: "StudentAcademicDetails",
                column: "BatchId",
                principalTable: "PreferredBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicDetails_PreferredBatches_PreferredBatchTimeId",
                table: "StudentAcademicDetails",
                column: "PreferredBatchTimeId",
                principalTable: "PreferredBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiedtaskassignmenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_StaffRegistrations_OperatorId",
                table: "TaskAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Users_OperatorId",
                table: "TaskAssignments",
                column: "OperatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Users_OperatorId",
                table: "TaskAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_StaffRegistrations_OperatorId",
                table: "TaskAssignments",
                column: "OperatorId",
                principalTable: "StaffRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedResourcetype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionTypeId",
                table: "TaskAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "TaskAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaskCode",
                table: "TaskAssignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ChapterId",
                table: "ContentResourceTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ContentResourceTypes_ChapterId",
                table: "ContentResourceTypes",
                column: "ChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentResourceTypes_ExamChapters_ChapterId",
                table: "ContentResourceTypes",
                column: "ChapterId",
                principalTable: "ExamChapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentResourceTypes_ExamChapters_ChapterId",
                table: "ContentResourceTypes");

            migrationBuilder.DropIndex(
                name: "IX_ContentResourceTypes_ChapterId",
                table: "ContentResourceTypes");

            migrationBuilder.DropColumn(
                name: "QuestionTypeId",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "TaskCode",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "ContentResourceTypes");
        }
    }
}

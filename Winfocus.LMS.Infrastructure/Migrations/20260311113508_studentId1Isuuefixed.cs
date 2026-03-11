using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class studentId1Isuuefixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicCouses_Students_StudentId",
                table: "StudentAcademicCouses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicCouses_Students_StudentId1",
                table: "StudentAcademicCouses");

            migrationBuilder.DropIndex(
                name: "IX_StudentAcademicCouses_StudentId1",
                table: "StudentAcademicCouses");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "StudentAcademicCouses");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicCouses_Students_StudentId",
                table: "StudentAcademicCouses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAcademicCouses_Students_StudentId",
                table: "StudentAcademicCouses");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId1",
                table: "StudentAcademicCouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAcademicCouses_StudentId1",
                table: "StudentAcademicCouses",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicCouses_Students_StudentId",
                table: "StudentAcademicCouses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAcademicCouses_Students_StudentId1",
                table: "StudentAcademicCouses",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}

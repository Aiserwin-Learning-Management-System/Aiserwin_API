using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removecodeinmastertables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "SyllabusCode",
                table: "Syllabuses");

            migrationBuilder.DropColumn(
                name: "SubjectCode",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "StreamCode",
                table: "Streams");

            migrationBuilder.DropColumn(
                name: "StateCode",
                table: "States");

            migrationBuilder.DropColumn(
                name: "ModeCode",
                table: "ModeOfStudies");

            migrationBuilder.DropColumn(
                name: "GradeCode",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Centres");

            migrationBuilder.DropColumn(
                name: "BatchCode",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "SyllabusName",
                table: "Syllabuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SubjectName",
                table: "Subjects",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "StreamName",
                table: "Streams",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "StateName",
                table: "States",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ModeName",
                table: "ModeOfStudies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "GradeName",
                table: "Grades",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "Courses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BatchName",
                table: "Batches",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Syllabuses",
                newName: "SyllabusName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Subjects",
                newName: "SubjectName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Streams",
                newName: "StreamName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "States",
                newName: "StateName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ModeOfStudies",
                newName: "ModeName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Grades",
                newName: "GradeName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Courses",
                newName: "CourseName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Batches",
                newName: "BatchName");

            migrationBuilder.AddColumn<string>(
                name: "SyllabusCode",
                table: "Syllabuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubjectCode",
                table: "Subjects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreamCode",
                table: "Streams",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateCode",
                table: "States",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModeCode",
                table: "ModeOfStudies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GradeCode",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Centres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BatchCode",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);
        }
    }
}

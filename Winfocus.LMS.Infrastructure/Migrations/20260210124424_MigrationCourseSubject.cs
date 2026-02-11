using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationCourseSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourse_Courses_CourseId",
                table: "StreamCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourse_Streams_StreamId",
                table: "StreamCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Courses_CourseId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamCourse",
                table: "StreamCourse");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Subjects");

            migrationBuilder.RenameTable(
                name: "StreamCourse",
                newName: "StreamCourses");

            migrationBuilder.RenameIndex(
                name: "IX_StreamCourse_CourseId",
                table: "StreamCourses",
                newName: "IX_StreamCourses_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamCourses",
                table: "StreamCourses",
                columns: new[] { "StreamId", "CourseId" });

            migrationBuilder.CreateTable(
                name: "CourseSubjects",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubjects", x => new { x.CourseId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_CourseSubjects_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubjects_SubjectId",
                table: "CourseSubjects",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_StreamCourses_Courses_CourseId",
                table: "StreamCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamCourses_Streams_StreamId",
                table: "StreamCourses",
                column: "StreamId",
                principalTable: "Streams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourses_Courses_CourseId",
                table: "StreamCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourses_Streams_StreamId",
                table: "StreamCourses");

            migrationBuilder.DropTable(
                name: "CourseSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamCourses",
                table: "StreamCourses");

            migrationBuilder.RenameTable(
                name: "StreamCourses",
                newName: "StreamCourse");

            migrationBuilder.RenameIndex(
                name: "IX_StreamCourses_CourseId",
                table: "StreamCourse",
                newName: "IX_StreamCourse_CourseId");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamCourse",
                table: "StreamCourse",
                columns: new[] { "StreamId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StreamCourse_Courses_CourseId",
                table: "StreamCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamCourse_Streams_StreamId",
                table: "StreamCourse",
                column: "StreamId",
                principalTable: "Streams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Courses_CourseId",
                table: "Subjects",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

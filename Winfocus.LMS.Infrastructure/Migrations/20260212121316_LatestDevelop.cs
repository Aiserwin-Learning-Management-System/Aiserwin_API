using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LatestDevelop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Streams_StreamId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourses_Courses_CourseId",
                table: "StreamCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourses_Streams_StreamId",
                table: "StreamCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectBatchTimingMTFs_BatchTimingSaturdays_BatchTimingSundayId",
                table: "SubjectBatchTimingMTFs");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Subjects_Courses_CourseId",
            //    table: "Subjects");

            migrationBuilder.DropTable(
                name: "CourseSubjects");

            migrationBuilder.DropIndex(
                name: "IX_Streams_GradeId",
                table: "Streams");

            //migrationBuilder.DropColumn(
            //    name: "CourseId",
            //    table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "BatchTimingSundayId",
                table: "SubjectBatchTimingMTFs",
                newName: "BatchTimingMTFsId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectBatchTimingMTFs_BatchTimingSundayId",
                table: "SubjectBatchTimingMTFs",
                newName: "IX_SubjectBatchTimingMTFs_BatchTimingMTFsId");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectBatchTimingSundayBatchTimingId",
                table: "SubjectBatchTimingSundays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectBatchTimingSundaySubjectId",
                table: "SubjectBatchTimingSundays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectBatchTimingSaturdayBatchTimingId",
                table: "SubjectBatchTimingSaturdays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectBatchTimingSaturdaySubjectId",
                table: "SubjectBatchTimingSaturdays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectBatchTimingMTFBatchTimingId",
                table: "SubjectBatchTimingMTFs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectBatchTimingMTFSubjectId",
                table: "SubjectBatchTimingMTFs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreamName",
                table: "Streams",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StreamCode",
                table: "Streams",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CourseName",
                table: "Courses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSundays_SubjectBatchTimingSundaySubjectId_SubjectBatchTimingSundayBatchTimingId",
                table: "SubjectBatchTimingSundays",
                columns: new[] { "SubjectBatchTimingSundaySubjectId", "SubjectBatchTimingSundayBatchTimingId" });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdaySubjectId_SubjectBatchTimingSaturdayBatchTimingId",
                table: "SubjectBatchTimingSaturdays",
                columns: new[] { "SubjectBatchTimingSaturdaySubjectId", "SubjectBatchTimingSaturdayBatchTimingId" });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectBatchTimingMTFs_SubjectBatchTimingMTFSubjectId_SubjectBatchTimingMTFBatchTimingId",
                table: "SubjectBatchTimingMTFs",
                columns: new[] { "SubjectBatchTimingMTFSubjectId", "SubjectBatchTimingMTFBatchTimingId" });

            migrationBuilder.CreateIndex(
                name: "IX_Streams_GradeId",
                table: "Streams",
                column: "GradeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubjectId",
                table: "Courses",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Streams_StreamId",
                table: "Courses",
                column: "StreamId",
                principalTable: "Streams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamCourses_Courses_CourseId",
                table: "StreamCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamCourses_Streams_StreamId",
                table: "StreamCourses",
                column: "StreamId",
                principalTable: "Streams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectBatchTimingMTFs_BatchTimingMTFs_BatchTimingMTFsId",
                table: "SubjectBatchTimingMTFs",
                column: "BatchTimingMTFsId",
                principalTable: "BatchTimingMTFs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectBatchTimingMTFs_SubjectBatchTimingMTFs_SubjectBatchTimingMTFSubjectId_SubjectBatchTimingMTFBatchTimingId",
                table: "SubjectBatchTimingMTFs",
                columns: new[] { "SubjectBatchTimingMTFSubjectId", "SubjectBatchTimingMTFBatchTimingId" },
                principalTable: "SubjectBatchTimingMTFs",
                principalColumns: new[] { "SubjectId", "BatchTimingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdaySubjectId_SubjectBatchTimingSaturdayBatchT~",
                table: "SubjectBatchTimingSaturdays",
                columns: new[] { "SubjectBatchTimingSaturdaySubjectId", "SubjectBatchTimingSaturdayBatchTimingId" },
                principalTable: "SubjectBatchTimingSaturdays",
                principalColumns: new[] { "SubjectId", "BatchTimingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectBatchTimingSundays_SubjectBatchTimingSundays_SubjectBatchTimingSundaySubjectId_SubjectBatchTimingSundayBatchTimingId",
                table: "SubjectBatchTimingSundays",
                columns: new[] { "SubjectBatchTimingSundaySubjectId", "SubjectBatchTimingSundayBatchTimingId" },
                principalTable: "SubjectBatchTimingSundays",
                principalColumns: new[] { "SubjectId", "BatchTimingId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Streams_StreamId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourses_Courses_CourseId",
                table: "StreamCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamCourses_Streams_StreamId",
                table: "StreamCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectBatchTimingMTFs_BatchTimingMTFs_BatchTimingMTFsId",
                table: "SubjectBatchTimingMTFs");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectBatchTimingMTFs_SubjectBatchTimingMTFs_SubjectBatchTimingMTFSubjectId_SubjectBatchTimingMTFBatchTimingId",
                table: "SubjectBatchTimingMTFs");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdaySubjectId_SubjectBatchTimingSaturdayBatchT~",
                table: "SubjectBatchTimingSaturdays");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectBatchTimingSundays_SubjectBatchTimingSundays_SubjectBatchTimingSundaySubjectId_SubjectBatchTimingSundayBatchTimingId",
                table: "SubjectBatchTimingSundays");

            migrationBuilder.DropIndex(
                name: "IX_SubjectBatchTimingSundays_SubjectBatchTimingSundaySubjectId_SubjectBatchTimingSundayBatchTimingId",
                table: "SubjectBatchTimingSundays");

            migrationBuilder.DropIndex(
                name: "IX_SubjectBatchTimingSaturdays_SubjectBatchTimingSaturdaySubjectId_SubjectBatchTimingSaturdayBatchTimingId",
                table: "SubjectBatchTimingSaturdays");

            migrationBuilder.DropIndex(
                name: "IX_SubjectBatchTimingMTFs_SubjectBatchTimingMTFSubjectId_SubjectBatchTimingMTFBatchTimingId",
                table: "SubjectBatchTimingMTFs");

            migrationBuilder.DropIndex(
                name: "IX_Streams_GradeId",
                table: "Streams");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SubjectId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SubjectBatchTimingSundayBatchTimingId",
                table: "SubjectBatchTimingSundays");

            migrationBuilder.DropColumn(
                name: "SubjectBatchTimingSundaySubjectId",
                table: "SubjectBatchTimingSundays");

            migrationBuilder.DropColumn(
                name: "SubjectBatchTimingSaturdayBatchTimingId",
                table: "SubjectBatchTimingSaturdays");

            migrationBuilder.DropColumn(
                name: "SubjectBatchTimingSaturdaySubjectId",
                table: "SubjectBatchTimingSaturdays");

            migrationBuilder.DropColumn(
                name: "SubjectBatchTimingMTFBatchTimingId",
                table: "SubjectBatchTimingMTFs");

            migrationBuilder.DropColumn(
                name: "SubjectBatchTimingMTFSubjectId",
                table: "SubjectBatchTimingMTFs");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "BatchTimingMTFsId",
                table: "SubjectBatchTimingMTFs",
                newName: "BatchTimingSundayId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectBatchTimingMTFs_BatchTimingMTFsId",
                table: "SubjectBatchTimingMTFs",
                newName: "IX_SubjectBatchTimingMTFs_BatchTimingSundayId");

            //migrationBuilder.AddColumn<Guid>(
            //    name: "CourseId",
            //    table: "Subjects",
            //    type: "uniqueidentifier",
            //    nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreamName",
                table: "Streams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "StreamCode",
                table: "Streams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CourseName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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
                name: "IX_Streams_GradeId",
                table: "Streams",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubjects_SubjectId",
                table: "CourseSubjects",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Streams_StreamId",
                table: "Courses",
                column: "StreamId",
                principalTable: "Streams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectBatchTimingMTFs_BatchTimingSaturdays_BatchTimingSundayId",
                table: "SubjectBatchTimingMTFs",
                column: "BatchTimingSundayId",
                principalTable: "BatchTimingSaturdays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Subjects_Courses_CourseId",
            //    table: "Subjects",
            //    column: "CourseId",
            //    principalTable: "Courses",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}

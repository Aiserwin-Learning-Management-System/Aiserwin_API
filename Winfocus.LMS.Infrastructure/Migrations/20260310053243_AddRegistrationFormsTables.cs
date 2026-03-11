using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winfocus.LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationFormsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    StaffCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationForms_StaffCategories_StaffCategoryId",
                        column: x => x.StaffCategoryId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationFormGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFormGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFormGroups_FieldGroups_FieldGroupId",
                        column: x => x.FieldGroupId,
                        principalTable: "FieldGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationFormGroups_RegistrationForms_FormId",
                        column: x => x.FormId,
                        principalTable: "RegistrationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffRegistrations_RegistrationForms_FormId",
                        column: x => x.FormId,
                        principalTable: "RegistrationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffRegistrations_StaffCategories_StaffCategoryId",
                        column: x => x.StaffCategoryId,
                        principalTable: "StaffCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationFormFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFormFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFormFields_FormFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationFormFields_RegistrationFormGroups_FormGroupId",
                        column: x => x.FormGroupId,
                        principalTable: "RegistrationFormGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrationFormFields_RegistrationForms_FormId",
                        column: x => x.FormId,
                        principalTable: "RegistrationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffRegistrationValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    RegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRegistrationValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffRegistrationValues_FormFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffRegistrationValues_StaffRegistrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "StaffRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegFormFields_FormId_DisplayOrder",
                table: "RegistrationFormFields",
                columns: new[] { "FormId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RegFormFields_FormId_FieldId_Unique",
                table: "RegistrationFormFields",
                columns: new[] { "FormId", "FieldId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegFormFields_GroupId_DisplayOrder",
                table: "RegistrationFormFields",
                columns: new[] { "FormGroupId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFormFields_FieldId",
                table: "RegistrationFormFields",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_RegFormGroups_FormId_DisplayOrder",
                table: "RegistrationFormGroups",
                columns: new[] { "FormId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RegFormGroups_FormId_FieldGroupId_Unique",
                table: "RegistrationFormGroups",
                columns: new[] { "FormId", "FieldGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFormGroups_FieldGroupId",
                table: "RegistrationFormGroups",
                column: "FieldGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationForms_FormName",
                table: "RegistrationForms",
                column: "FormName");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationForms_OneActivePerCategory",
                table: "RegistrationForms",
                column: "StaffCategoryId",
                unique: true,
                filter: "[IsActive] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_FormId",
                table: "StaffRegistrations",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_StaffCategoryId",
                table: "StaffRegistrations",
                column: "StaffCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegistrations_Status",
                table: "StaffRegistrations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegValues_FieldId",
                table: "StaffRegistrationValues",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRegValues_RegId_FieldId_Unique",
                table: "StaffRegistrationValues",
                columns: new[] { "RegistrationId", "FieldId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationFormFields");

            migrationBuilder.DropTable(
                name: "StaffRegistrationValues");

            migrationBuilder.DropTable(
                name: "RegistrationFormGroups");

            migrationBuilder.DropTable(
                name: "StaffRegistrations");

            migrationBuilder.DropTable(
                name: "RegistrationForms");
        }
    }
}

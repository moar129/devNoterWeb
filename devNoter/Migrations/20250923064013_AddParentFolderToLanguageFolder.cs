using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace devNoter.Migrations
{
    /// <inheritdoc />
    public partial class AddParentFolderToLanguageFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ParentFolderId",
                table: "LanguageFolders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageFolders_ParentFolderId",
                table: "LanguageFolders",
                column: "ParentFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageFolders_LanguageFolders_ParentFolderId",
                table: "LanguageFolders",
                column: "ParentFolderId",
                principalTable: "LanguageFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageFolders_LanguageFolders_ParentFolderId",
                table: "LanguageFolders");

            migrationBuilder.DropIndex(
                name: "IX_LanguageFolders_ParentFolderId",
                table: "LanguageFolders");

            migrationBuilder.DropColumn(
                name: "ParentFolderId",
                table: "LanguageFolders");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}

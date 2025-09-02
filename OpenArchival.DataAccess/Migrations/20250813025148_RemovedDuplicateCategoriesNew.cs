using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDuplicateCategoriesNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactGroupings_ArchiveCategory_CategoryId",
                table: "ArtifactGroupings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArchiveCategory",
                table: "ArchiveCategory");

            migrationBuilder.RenameTable(
                name: "ArchiveCategory",
                newName: "ArchiveCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArchiveCategories",
                table: "ArchiveCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactGroupings_ArchiveCategories_CategoryId",
                table: "ArtifactGroupings",
                column: "CategoryId",
                principalTable: "ArchiveCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactGroupings_ArchiveCategories_CategoryId",
                table: "ArtifactGroupings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArchiveCategories",
                table: "ArchiveCategories");

            migrationBuilder.RenameTable(
                name: "ArchiveCategories",
                newName: "ArchiveCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArchiveCategory",
                table: "ArchiveCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactGroupings_ArchiveCategory_CategoryId",
                table: "ArtifactGroupings",
                column: "CategoryId",
                principalTable: "ArchiveCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

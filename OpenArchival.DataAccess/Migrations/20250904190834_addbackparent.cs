using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addbackparent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactFilePaths_ArtifactEntries_ArtifactEntryId",
                table: "ArtifactFilePaths");

            migrationBuilder.RenameColumn(
                name: "ArtifactEntryId",
                table: "ArtifactFilePaths",
                newName: "ParentArtifactEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtifactFilePaths_ArtifactEntryId",
                table: "ArtifactFilePaths",
                newName: "IX_ArtifactFilePaths_ParentArtifactEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactFilePaths_ArtifactEntries_ParentArtifactEntryId",
                table: "ArtifactFilePaths",
                column: "ParentArtifactEntryId",
                principalTable: "ArtifactEntries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactFilePaths_ArtifactEntries_ParentArtifactEntryId",
                table: "ArtifactFilePaths");

            migrationBuilder.RenameColumn(
                name: "ParentArtifactEntryId",
                table: "ArtifactFilePaths",
                newName: "ArtifactEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtifactFilePaths_ParentArtifactEntryId",
                table: "ArtifactFilePaths",
                newName: "IX_ArtifactFilePaths_ArtifactEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactFilePaths_ArtifactEntries_ArtifactEntryId",
                table: "ArtifactFilePaths",
                column: "ArtifactEntryId",
                principalTable: "ArtifactEntries",
                principalColumn: "Id");
        }
    }
}

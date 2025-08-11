using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NullableParentArtifactEntryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactFilePaths_ArtifactEntries_ParentArtifactEntryId",
                table: "ArtifactFilePaths");

            migrationBuilder.AlterColumn<int>(
                name: "ParentArtifactEntryId",
                table: "ArtifactFilePaths",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AlterColumn<int>(
                name: "ParentArtifactEntryId",
                table: "ArtifactFilePaths",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactFilePaths_ArtifactEntries_ParentArtifactEntryId",
                table: "ArtifactFilePaths",
                column: "ParentArtifactEntryId",
                principalTable: "ArtifactEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

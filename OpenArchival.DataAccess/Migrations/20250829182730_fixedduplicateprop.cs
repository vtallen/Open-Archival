using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixedduplicateprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactEntries_ArtifactGroupings_ParentArtifactGroupingId",
                table: "ArtifactEntries");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactEntries_ParentArtifactGroupingId",
                table: "ArtifactEntries");

            migrationBuilder.DropColumn(
                name: "ParentArtifactGroupingId",
                table: "ArtifactEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentArtifactGroupingId",
                table: "ArtifactEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntries_ParentArtifactGroupingId",
                table: "ArtifactEntries",
                column: "ParentArtifactGroupingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactEntries_ArtifactGroupings_ParentArtifactGroupingId",
                table: "ArtifactEntries",
                column: "ParentArtifactGroupingId",
                principalTable: "ArtifactGroupings",
                principalColumn: "Id");
        }
    }
}

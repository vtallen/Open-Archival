using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDuplicateCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactEntryTags_ArtifactGroupings_ArtifactGroupingId",
                table: "ArtifactEntryTags");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactEntryTags_ArtifactGroupingId",
                table: "ArtifactEntryTags");

            migrationBuilder.DropColumn(
                name: "ArtifactGroupingId",
                table: "ArtifactEntryTags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArtifactGroupingId",
                table: "ArtifactEntryTags",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntryTags_ArtifactGroupingId",
                table: "ArtifactEntryTags",
                column: "ArtifactGroupingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactEntryTags_ArtifactGroupings_ArtifactGroupingId",
                table: "ArtifactEntryTags",
                column: "ArtifactGroupingId",
                principalTable: "ArtifactGroupings",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArtifactGroupingId",
                table: "ArtifactEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntries_ArtifactGroupingId",
                table: "ArtifactEntries",
                column: "ArtifactGroupingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactEntries_ArtifactGroupings_ArtifactGroupingId",
                table: "ArtifactEntries",
                column: "ArtifactGroupingId",
                principalTable: "ArtifactGroupings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactEntries_ArtifactGroupings_ArtifactGroupingId",
                table: "ArtifactEntries");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactEntries_ArtifactGroupingId",
                table: "ArtifactEntries");

            migrationBuilder.DropColumn(
                name: "ArtifactGroupingId",
                table: "ArtifactEntries");
        }
    }
}

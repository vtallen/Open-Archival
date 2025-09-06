using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class properlylinktags3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactDefects_ArtifactEntries_ArtifactEntryId",
                table: "ArtifactDefects");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactDefects_ArtifactEntryId",
                table: "ArtifactDefects");

            migrationBuilder.DropColumn(
                name: "ArtifactEntryId",
                table: "ArtifactDefects");

            migrationBuilder.CreateTable(
                name: "ArtifactDefectArtifactEntry",
                columns: table => new
                {
                    ArtifactEntriesId = table.Column<int>(type: "integer", nullable: false),
                    DefectsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactDefectArtifactEntry", x => new { x.ArtifactEntriesId, x.DefectsId });
                    table.ForeignKey(
                        name: "FK_ArtifactDefectArtifactEntry_ArtifactDefects_DefectsId",
                        column: x => x.DefectsId,
                        principalTable: "ArtifactDefects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtifactDefectArtifactEntry_ArtifactEntries_ArtifactEntries~",
                        column: x => x.ArtifactEntriesId,
                        principalTable: "ArtifactEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactDefectArtifactEntry_DefectsId",
                table: "ArtifactDefectArtifactEntry",
                column: "DefectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtifactDefectArtifactEntry");

            migrationBuilder.AddColumn<int>(
                name: "ArtifactEntryId",
                table: "ArtifactDefects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactDefects_ArtifactEntryId",
                table: "ArtifactDefects",
                column: "ArtifactEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactDefects_ArtifactEntries_ArtifactEntryId",
                table: "ArtifactDefects",
                column: "ArtifactEntryId",
                principalTable: "ArtifactEntries",
                principalColumn: "Id");
        }
    }
}

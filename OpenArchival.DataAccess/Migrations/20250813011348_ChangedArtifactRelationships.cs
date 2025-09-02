using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedArtifactRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatedGroupings");

            migrationBuilder.CreateTable(
                name: "ArtifactRelationships",
                columns: table => new
                {
                    RelatedById = table.Column<int>(type: "integer", nullable: false),
                    RelatedToId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactRelationships", x => new { x.RelatedById, x.RelatedToId });
                    table.ForeignKey(
                        name: "FK_ArtifactRelationships_ArtifactEntries_RelatedById",
                        column: x => x.RelatedById,
                        principalTable: "ArtifactEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtifactRelationships_ArtifactEntries_RelatedToId",
                        column: x => x.RelatedToId,
                        principalTable: "ArtifactEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactRelationships_RelatedToId",
                table: "ArtifactRelationships",
                column: "RelatedToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtifactRelationships");

            migrationBuilder.CreateTable(
                name: "RelatedGroupings",
                columns: table => new
                {
                    ArtifactGroupingId = table.Column<int>(type: "integer", nullable: false),
                    RelatedArtifactGroupingsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedGroupings", x => new { x.ArtifactGroupingId, x.RelatedArtifactGroupingsId });
                    table.ForeignKey(
                        name: "FK_RelatedGroupings_ArtifactGroupings_ArtifactGroupingId",
                        column: x => x.ArtifactGroupingId,
                        principalTable: "ArtifactGroupings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedGroupings_ArtifactGroupings_RelatedArtifactGroupings~",
                        column: x => x.RelatedArtifactGroupingsId,
                        principalTable: "ArtifactGroupings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelatedGroupings_RelatedArtifactGroupingsId",
                table: "RelatedGroupings",
                column: "RelatedArtifactGroupingsId");
        }
    }
}

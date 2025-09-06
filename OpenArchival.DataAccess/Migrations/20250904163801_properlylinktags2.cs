using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class properlylinktags2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactAssociatedNames_ArtifactEntries_ParentArtifactEntry~",
                table: "ArtifactAssociatedNames");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactAssociatedNames_ParentArtifactEntryId",
                table: "ArtifactAssociatedNames");

            migrationBuilder.DropColumn(
                name: "ParentArtifactEntryId",
                table: "ArtifactAssociatedNames");

            migrationBuilder.CreateTable(
                name: "ArtifactEntryListedName",
                columns: table => new
                {
                    ArtifactEntriesId = table.Column<int>(type: "integer", nullable: false),
                    ListedNamesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactEntryListedName", x => new { x.ArtifactEntriesId, x.ListedNamesId });
                    table.ForeignKey(
                        name: "FK_ArtifactEntryListedName_ArtifactAssociatedNames_ListedNames~",
                        column: x => x.ListedNamesId,
                        principalTable: "ArtifactAssociatedNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtifactEntryListedName_ArtifactEntries_ArtifactEntriesId",
                        column: x => x.ArtifactEntriesId,
                        principalTable: "ArtifactEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntryListedName_ListedNamesId",
                table: "ArtifactEntryListedName",
                column: "ListedNamesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtifactEntryListedName");

            migrationBuilder.AddColumn<int>(
                name: "ParentArtifactEntryId",
                table: "ArtifactAssociatedNames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactAssociatedNames_ParentArtifactEntryId",
                table: "ArtifactAssociatedNames",
                column: "ParentArtifactEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactAssociatedNames_ArtifactEntries_ParentArtifactEntry~",
                table: "ArtifactAssociatedNames",
                column: "ParentArtifactEntryId",
                principalTable: "ArtifactEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

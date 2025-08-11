using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageLocation",
                table: "ArtifactEntries");

            migrationBuilder.AddColumn<int>(
                name: "StorageLocationId",
                table: "ArtifactEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ArtifactStorageLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Location = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactStorageLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntries_StorageLocationId",
                table: "ArtifactEntries",
                column: "StorageLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactEntries_ArtifactStorageLocations_StorageLocationId",
                table: "ArtifactEntries",
                column: "StorageLocationId",
                principalTable: "ArtifactStorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactEntries_ArtifactStorageLocations_StorageLocationId",
                table: "ArtifactEntries");

            migrationBuilder.DropTable(
                name: "ArtifactStorageLocations");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactEntries_StorageLocationId",
                table: "ArtifactEntries");

            migrationBuilder.DropColumn(
                name: "StorageLocationId",
                table: "ArtifactEntries");

            migrationBuilder.AddColumn<string>(
                name: "StorageLocation",
                table: "ArtifactEntries",
                type: "text",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactAssocaitedNames_ArtifactEntries_ParentArtifactEntry~",
                table: "ArtifactAssocaitedNames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtifactAssocaitedNames",
                table: "ArtifactAssocaitedNames");

            migrationBuilder.RenameTable(
                name: "ArtifactAssocaitedNames",
                newName: "ArtifactAssociatedNames");

            migrationBuilder.RenameColumn(
                name: "IsPublicallyVisible",
                table: "ArtifactEntries",
                newName: "IsPubliclyVisible");

            migrationBuilder.RenameIndex(
                name: "IX_ArtifactAssocaitedNames_ParentArtifactEntryId",
                table: "ArtifactAssociatedNames",
                newName: "IX_ArtifactAssociatedNames_ParentArtifactEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtifactAssociatedNames",
                table: "ArtifactAssociatedNames",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ArtifactTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactAssociatedNames_ArtifactEntries_ParentArtifactEntry~",
                table: "ArtifactAssociatedNames",
                column: "ParentArtifactEntryId",
                principalTable: "ArtifactEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactAssociatedNames_ArtifactEntries_ParentArtifactEntry~",
                table: "ArtifactAssociatedNames");

            migrationBuilder.DropTable(
                name: "ArtifactTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtifactAssociatedNames",
                table: "ArtifactAssociatedNames");

            migrationBuilder.RenameTable(
                name: "ArtifactAssociatedNames",
                newName: "ArtifactAssocaitedNames");

            migrationBuilder.RenameColumn(
                name: "IsPubliclyVisible",
                table: "ArtifactEntries",
                newName: "IsPublicallyVisible");

            migrationBuilder.RenameIndex(
                name: "IX_ArtifactAssociatedNames_ParentArtifactEntryId",
                table: "ArtifactAssocaitedNames",
                newName: "IX_ArtifactAssocaitedNames_ParentArtifactEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtifactAssocaitedNames",
                table: "ArtifactAssocaitedNames",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactAssocaitedNames_ArtifactEntries_ParentArtifactEntry~",
                table: "ArtifactAssocaitedNames",
                column: "ParentArtifactEntryId",
                principalTable: "ArtifactEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

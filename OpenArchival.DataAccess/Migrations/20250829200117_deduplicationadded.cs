using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class deduplicationadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defects",
                table: "ArtifactEntries");

            migrationBuilder.DropColumn(
                name: "ListedNames",
                table: "ArtifactEntries");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ArtifactAssociatedNames");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ArtifactAssociatedNames");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "ArtifactAssociatedNames",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ArtifactEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArtifactEntryId",
                table: "ArtifactDefects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntries_TypeId",
                table: "ArtifactEntries",
                column: "TypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactEntries_ArtifactTypes_TypeId",
                table: "ArtifactEntries",
                column: "TypeId",
                principalTable: "ArtifactTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactDefects_ArtifactEntries_ArtifactEntryId",
                table: "ArtifactDefects");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactEntries_ArtifactTypes_TypeId",
                table: "ArtifactEntries");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactEntries_TypeId",
                table: "ArtifactEntries");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactDefects_ArtifactEntryId",
                table: "ArtifactDefects");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ArtifactEntries");

            migrationBuilder.DropColumn(
                name: "ArtifactEntryId",
                table: "ArtifactDefects");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ArtifactAssociatedNames",
                newName: "LastName");

            migrationBuilder.AddColumn<List<string>>(
                name: "Defects",
                table: "ArtifactEntries",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "ListedNames",
                table: "ArtifactEntries",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ArtifactAssociatedNames",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ArtifactAssociatedNames",
                type: "text",
                nullable: true);
        }
    }
}

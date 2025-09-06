using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ArtifactGroupings");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ArtifactGroupings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactGroupings_TypeId",
                table: "ArtifactGroupings",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtifactGroupings_ArtifactTypes_TypeId",
                table: "ArtifactGroupings",
                column: "TypeId",
                principalTable: "ArtifactTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtifactGroupings_ArtifactTypes_TypeId",
                table: "ArtifactGroupings");

            migrationBuilder.DropIndex(
                name: "IX_ArtifactGroupings_TypeId",
                table: "ArtifactGroupings");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ArtifactGroupings");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ArtifactGroupings",
                type: "text",
                nullable: true);
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenArchival.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchiveCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FieldSeparator = table.Column<string>(type: "text", nullable: false),
                    FieldNames = table.Column<List<string>>(type: "text[]", nullable: false),
                    FieldDescriptions = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtifactDefects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactDefects", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ArtifactGroupings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    IsPublicallyVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IdentifierFields = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactGroupings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtifactGroupings_ArchiveCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ArchiveCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtifactEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtifactNumber = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StorageLocationId = table.Column<int>(type: "integer", nullable: false),
                    ListedNames = table.Column<List<string>>(type: "text[]", nullable: true),
                    AssociatedDates = table.Column<List<DateTime>>(type: "timestamp with time zone[]", nullable: true),
                    Defects = table.Column<List<string>>(type: "text[]", nullable: true),
                    Links = table.Column<List<string>>(type: "text[]", nullable: true),
                    ParentArtifactGroupingId = table.Column<int>(type: "integer", nullable: true),
                    FileTextContent = table.Column<string>(type: "text", nullable: true),
                    IsPubliclyVisible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtifactEntries_ArtifactGroupings_ParentArtifactGroupingId",
                        column: x => x.ParentArtifactGroupingId,
                        principalTable: "ArtifactGroupings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArtifactEntries_ArtifactStorageLocations_StorageLocationId",
                        column: x => x.StorageLocationId,
                        principalTable: "ArtifactStorageLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtifactEntryTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ArtifactGroupingId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactEntryTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtifactEntryTags_ArtifactGroupings_ArtifactGroupingId",
                        column: x => x.ArtifactGroupingId,
                        principalTable: "ArtifactGroupings",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "ArtifactAssociatedNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentArtifactEntryId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactAssociatedNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtifactAssociatedNames_ArtifactEntries_ParentArtifactEntry~",
                        column: x => x.ParentArtifactEntryId,
                        principalTable: "ArtifactEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtifactFilePaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentArtifactEntryId = table.Column<int>(type: "integer", nullable: true),
                    OriginalName = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactFilePaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtifactFilePaths_ArtifactEntries_ParentArtifactEntryId",
                        column: x => x.ParentArtifactEntryId,
                        principalTable: "ArtifactEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArtifactEntryArtifactEntryTag",
                columns: table => new
                {
                    ArtifactEntriesId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactEntryArtifactEntryTag", x => new { x.ArtifactEntriesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ArtifactEntryArtifactEntryTag_ArtifactEntries_ArtifactEntri~",
                        column: x => x.ArtifactEntriesId,
                        principalTable: "ArtifactEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtifactEntryArtifactEntryTag_ArtifactEntryTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ArtifactEntryTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactAssociatedNames_ParentArtifactEntryId",
                table: "ArtifactAssociatedNames",
                column: "ParentArtifactEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntries_ParentArtifactGroupingId",
                table: "ArtifactEntries",
                column: "ParentArtifactGroupingId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntries_StorageLocationId",
                table: "ArtifactEntries",
                column: "StorageLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntryArtifactEntryTag_TagsId",
                table: "ArtifactEntryArtifactEntryTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactEntryTags_ArtifactGroupingId",
                table: "ArtifactEntryTags",
                column: "ArtifactGroupingId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactFilePaths_ParentArtifactEntryId",
                table: "ArtifactFilePaths",
                column: "ParentArtifactEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtifactGroupings_CategoryId",
                table: "ArtifactGroupings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedGroupings_RelatedArtifactGroupingsId",
                table: "RelatedGroupings",
                column: "RelatedArtifactGroupingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtifactAssociatedNames");

            migrationBuilder.DropTable(
                name: "ArtifactDefects");

            migrationBuilder.DropTable(
                name: "ArtifactEntryArtifactEntryTag");

            migrationBuilder.DropTable(
                name: "ArtifactFilePaths");

            migrationBuilder.DropTable(
                name: "ArtifactTypes");

            migrationBuilder.DropTable(
                name: "RelatedGroupings");

            migrationBuilder.DropTable(
                name: "ArtifactEntryTags");

            migrationBuilder.DropTable(
                name: "ArtifactEntries");

            migrationBuilder.DropTable(
                name: "ArtifactGroupings");

            migrationBuilder.DropTable(
                name: "ArtifactStorageLocations");

            migrationBuilder.DropTable(
                name: "ArchiveCategory");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace OpenArchival.DataAccess;

public class ArchiveDbContext : DbContext
{
    public DbSet<ArtifactGrouping> ArtifactGroupings { get; set; }

    public DbSet<ArtifactEntry> ArtifactEntries { get; set; }

    public DbSet<ArtifactEntryTag> ArtifactEntryTags { get; set; }

    public DbSet<ArchiveCategory> ArchiveCategories { get; set; }

    public DbSet<ListedName> ArtifactAssociatedNames { get; set; }

    public DbSet<FilePathListing> ArtifactFilePaths { get; set; }

    public DbSet<ArchiveCategory> ArtifactGroupingCategories { get; set; }

    public DbSet<ArtifactDefect> ArtifactDefects { get; set; }

    public DbSet<ArtifactStorageLocation> ArtifactStorageLocations { get; set; }

    public DbSet<ArtifactType> ArtifactTypes { get; set; }

    public ArchiveDbContext(DbContextOptions<ArchiveDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ArtifactGrouping>()
            .HasMany(p => p.RelatedArtifactGroupings)
            .WithMany()
            .UsingEntity(j => j.ToTable("RelatedGroupings"));

        modelBuilder.Entity<ArtifactEntry>()
            .HasMany(p => p.Files)
            .WithOne(p => p.ParentArtifactEntry)
            .HasForeignKey(p => p.ParentArtifactEntryId)
            .IsRequired(false);

        modelBuilder.Entity<ArtifactGrouping>()
            .OwnsOne(p => p.IdentifierFields)
            .ToJson();

        modelBuilder.Entity<ArtifactEntry>(builder =>
        {
            builder.Property(p => p.FileTextContent)
                .HasConversion
                (
                    v => JsonSerializer.Serialize
                        (v, new JsonSerializerOptions()),

                    v => JsonSerializer.Deserialize<Dictionary<string, string>>
                        (v, new JsonSerializerOptions()) ?? new Dictionary<string, string>()
                );
        });

        var dictionaryComparer = new ValueComparer<Dictionary<string, string>>(
            (dictionary1, dictionary2) => dictionary1.OrderBy(pair => pair.Key)
                                                      .SequenceEqual(dictionary2.OrderBy(pair => pair.Key)),

            dictionary => dictionary.Aggregate(
                0,
                (aggregatedHash, pair) => HashCode.Combine(aggregatedHash, pair.Key.GetHashCode(), pair.Value.GetHashCode())),

            sourceDictionary => new Dictionary<string, string>(sourceDictionary)
        );

        modelBuilder.Entity<ArtifactEntry>(builder =>
        {
            builder.Property(p => p.FileTextContent)
                .HasConversion
                (
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, new JsonSerializerOptions()) ?? new Dictionary<string, string>()
                ).Metadata
                .SetValueComparer(dictionaryComparer);
        });
    }
}

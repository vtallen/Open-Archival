using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OpenArchival.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace OpenArchival.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ArtifactGrouping> ArtifactGroupings { get; set; }

    public DbSet<ArtifactEntry> ArtifactEntries { get; set; }

    public DbSet<ArtifactEntryTag> ArtifactEntryTags { get; set; }

    public DbSet<ArchiveCategory> ArchiveCategories { get; set; }

    public DbSet<ListedName> ArtifactAssociatedNames { get; set; }

    public DbSet<FilePathListing> ArtifactFilePaths { get; set; }

    public DbSet<ArtifactDefect> ArtifactDefects { get; set; }

    public DbSet<ArtifactStorageLocation> ArtifactStorageLocations { get; set; }

    public DbSet<ArtifactType> ArtifactTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        /*
        modelBuilder.Entity<ArtifactEntry>()
            .HasMany(p => p.Files)
            .WithOne(p => p.ParentArtifactEntry)
            .IsRequired(false);
        */

        // Make other associations
        modelBuilder.Entity<ArtifactEntry>()
            .HasMany(a => a.RelatedTo)
            .WithMany(a => a.RelatedBy)
            .UsingEntity(j => j.ToTable("ArtifactRelationships"));

        modelBuilder.Entity<ArtifactEntry>()
            .HasOne(a => a.StorageLocation)
            .WithMany(l => l.ArtifactEntries);

        modelBuilder.Entity<ArtifactGrouping>()
            .OwnsOne(p => p.IdentifierFields)
            .ToJson();
        
        modelBuilder.Entity<ArtifactGrouping>()
            .HasMany(grouping => grouping.ChildArtifactEntries)
            .WithOne(entry => entry.ArtifactGrouping)
            .HasForeignKey(entry => entry.ArtifactGroupingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ArtifactGrouping>()
            .Navigation(g => g.IdentifierFields)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        var dictionaryComparer = new ValueComparer<Dictionary<string, string>>(
            (dictionary1, dictionary2) => dictionary1.OrderBy(pair => pair.Key)
                                                      .SequenceEqual(dictionary2.OrderBy(pair => pair.Key)),

            dictionary => dictionary.Aggregate(
                0,
                (aggregatedHash, pair) => HashCode.Combine(aggregatedHash, pair.Key.GetHashCode(), pair.Value.GetHashCode())),

            sourceDictionary => new Dictionary<string, string>(sourceDictionary)
        );
    }
}

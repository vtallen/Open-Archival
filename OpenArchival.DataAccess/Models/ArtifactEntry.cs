using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenArchival.DataAccess;

public class ArtifactEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>
    /// This value gets appended on the end of the contianing ArtifactGrouping's 
    /// Category value
    /// </summary>
    public string? ArtifactIdentifier 
    {
        get
        {
            return (ArtifactGrouping is not null) 
                ? ModelHelpers.MakeIdentifier(ArtifactGrouping.IdentifierFields.Values, ArtifactGrouping.Category.FieldSeparator, ArtifactNumber) 
                : null;
        }
    }

    public string? ArtifactNumber { get; set; } 

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required ArtifactStorageLocation StorageLocation { get; set; }

    //public List<ArtifactEntryTag>? Tags { get; set; } = [];

    public List<ArtifactEntryTag> Tags { get; set; } = [];

    public List<ListedName>? ListedNames { get; set; } = [];

    public List<DateTime>? AssociatedDates { get; set; } = [];

    public List<ArtifactDefect>? Defects { get; set; } = [];

    public List<string>? Links { get; set; } = [];
    
    public required List<FilePathListing> Files { get; set; } = [];

    public string? FileTextContent { get; set; } = null;

    public required ArtifactType Type { get; set; }

    public bool IsPubliclyVisible { get; set; }


    // Relationships this artifact has TO other artifacts
    public List<ArtifactEntry> RelatedTo { get; set; } = [];

    // Relationships other artifacts have TO this artifact
    public List<ArtifactEntry> RelatedBy { get; set; } = [];


    public int ArtifactGroupingId { get; set; }

    public required ArtifactGrouping ArtifactGrouping { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"--- ArtifactEntry (ID: {Id}) ---");
        sb.AppendLine($"  Title: {Title}");
        sb.AppendLine($"  ArtifactIdentifier: {ArtifactIdentifier ?? "N/A"}");
        sb.AppendLine($"  ArtifactNumber: {ArtifactNumber ?? "N/A"}");
        sb.AppendLine($"  StorageLocation: {StorageLocation}"); // Assumes ArtifactStorageLocation has a useful ToString()
        sb.AppendLine($"  IsPubliclyVisible: {IsPubliclyVisible}");

        // Handle Description (it could be long, so you might truncate it if needed)
        sb.AppendLine($"  Description: {(string.IsNullOrWhiteSpace(Description) ? "N/A" : Description)}");

        // Handle Lists
        sb.AppendLine($"  Tags: {(Tags is not null && Tags.Any() ? string.Join(", ", Tags) : "None")}");
        sb.AppendLine($"  ListedNames: {(ListedNames is not null && ListedNames.Any() ? string.Join(", ", ListedNames) : "None")}");
        sb.AppendLine($"  AssociatedDates: {(AssociatedDates is not null && AssociatedDates.Any() ? string.Join(", ", AssociatedDates.Select(d => d.ToShortDateString())) : "None")}");
        sb.AppendLine($"  Defects: {(Defects is not null && Defects.Any() ? string.Join(", ", Defects) : "None")}");
        sb.AppendLine($"  Links: {(Links is not null && Links.Any() ? string.Join(", ", Links) : "None")}");
        sb.AppendLine($"  Files: {(Files is not null && Files.Any() ? string.Join(", ", Files) : "None")}");

        // Handle potentially very large text content
        sb.AppendLine($"  FileTextContent: {(string.IsNullOrEmpty(FileTextContent) ? "Not Present" : $"Present (Length: {FileTextContent.Length})")}");

        sb.Append("--------------------------");

        return sb.ToString();
    }
}


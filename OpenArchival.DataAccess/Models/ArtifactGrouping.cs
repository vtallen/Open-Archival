using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OpenArchival.DataAccess;

public class ArtifactGrouping
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? ArtifactGroupingIdentifier 
    { 
        get
        {
            return ModelHelpers.MakeIdentifier(_identifierFields.Values, Category.FieldSeparator, null);
        }
    }

    public required ArchiveCategory Category { get; set; }

    private IdentifierFields _identifierFields;
    public required IdentifierFields IdentifierFields {
        get => _identifierFields;
        set
        {
            if (value.Values.Count != Category.FieldNames.Count)
            {
                throw new ArgumentException(nameof(IdentifierFields), $"The number of field values must be equal to the field count of the {nameof(ArchiveCategory)}");
            }

            _identifierFields = value; 
        }
    }


    public required string Title { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public bool IsPublicallyVisible { get; set; }

    public required List<ArtifactEntry> ChildArtifactEntries { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Artifact Grouping Identifier: {ArtifactGroupingIdentifier}");
        sb.AppendLine($"Category:");
        sb.AppendLine(Category.ToString());
        sb.AppendLine($"Title: {Title}");
        sb.AppendLine($"Description: {Description}");
        sb.AppendLine($"Type:{Type}");
        sb.AppendLine($"Publically Visible: {IsPublicallyVisible}");
        sb.AppendLine($"Artifact Entries:");
        foreach (var artifact in ChildArtifactEntries)
        {
            sb.AppendLine(artifact.ToString());
            sb.AppendLine();
        }

        return sb.ToString();
    }

    [NotMapped]
    public IEnumerable<ArtifactEntryTag> ChildTags
    {
        get
        {
            HashSet<ArtifactEntryTag> seenTags = [];
            for (int index = 0; index < ChildArtifactEntries.Count; ++index)
            {
                // Get the tags for this entry, skip if no tags 
                List<ArtifactEntryTag>? tags = ChildArtifactEntries[index].Tags;
                if (tags is null)
                {
                    continue;
                }
                
                // Only yield a tag if we have not yielded it yet
                foreach (ArtifactEntryTag tag in tags)
                {
                    if (seenTags.Contains(tag))
                    {
                        continue;
                    }

                    seenTags.Add(tag);
                    yield return tag;
                }
            }
        }
    }
}

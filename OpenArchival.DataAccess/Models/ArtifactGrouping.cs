using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OpenArchival.DataAccess;

public class ArtifactGrouping
{
    [Key]
    public required int Id { get; set; }

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

    public required List<ArtifactEntry> ChildArtifactEntries { get; set; } = new();

    public required string Title { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public List<ArtifactGrouping>? RelatedArtifactGroupings { get; set; }
    
    public bool IsPublicallyVisible { get; set; }

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

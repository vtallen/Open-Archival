using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Core;

public class ArchiveItem
{
    public required Category Category { get; set; }
    
    public required string ItemTitle { get; set; }

    public string? Description { get; set; }

    public string? StorageLocation { get; set; }

    public string? ArtifactType { get; set; }

    public List<string>? Tags { get; set; }

    public List<string>? ListedNames { get; set; }

    public List<DateTime>? AssociatedDates { get; set; }

    public List<string>? Defects { get; set; }

    public List<string>? RelatedArtifacts { get; set; }
    
    /// <summary>
    /// TODO: Implement
    /// </summary>
    public List<string>? Files { get; set; }

    public bool IsPublic { get; set; } = true;
}

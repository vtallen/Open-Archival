using OpenArchival.DataAccess;

namespace OpenArchival.Blazor;

public class ArtifactGroupingValidationModel
{
    public required ArchiveCategory Category { get; set; }

    public List<string>? IdentifierFieldValues { get; set;  }

    public List<ArtifactEntryValidationModel>? ArtifactEntries { get; set;  }

    public bool IsPublicallyVisible { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.DataAccess;

public class ArtifactEntryTag
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }

    public List<ArtifactEntry>? ArtifactEntries { get; set; }
}

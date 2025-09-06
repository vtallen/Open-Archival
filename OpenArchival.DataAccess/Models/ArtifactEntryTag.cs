using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.DataAccess;

public class ArtifactEntryTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Index(IsUnique = true)]
    public required string Name { get; set; }

    public List<ArtifactEntry> ArtifactEntries { get; set; } = [];
}

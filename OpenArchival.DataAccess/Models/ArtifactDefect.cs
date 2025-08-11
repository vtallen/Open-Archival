using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.DataAccess;

public class ArtifactDefect
{
    [Key]
    public required int Id { get; set; }

    public required string Description { get; set; }
}

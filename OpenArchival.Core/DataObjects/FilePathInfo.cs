using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database.Remove;

public class FilePathInfo
{
    public int Id { get; set; }
    public required string OriginalName { get; set; }
    public required string Path { get; set; }
}

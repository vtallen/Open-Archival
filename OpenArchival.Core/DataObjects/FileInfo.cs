using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public class FileInfo
{
    public int Id { get; set; }
    public required string Filename { get; set; }
    public required string Path { get; set; }
}

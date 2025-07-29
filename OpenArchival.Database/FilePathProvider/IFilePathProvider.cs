using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public interface IFilePathProvider
{
    public Task<bool> AddFile(string path);

    public Task<bool> RemoveFile(string path);

    public Task<IEnumerable<FileInfo>> SearchFiles(string filename);

    public Task<IEnumerable<FileInfo>> TopFiles(int resultsCount);
}

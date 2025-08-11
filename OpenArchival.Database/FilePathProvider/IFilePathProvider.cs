using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public interface IFilePathProvider
{
    public Task<bool> AddFileInfo(string path, string originalName);

    public Task<bool> RemoveFileInfo(string path, string originalName);

    public Task<IEnumerable<FilePathInfo>> SearchFiles(string filename);

    public Task<IEnumerable<FilePathInfo>> TopFiles(int resultsCount);
}

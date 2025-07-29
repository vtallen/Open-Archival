using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public interface IArtifactAssociatedNamesProvider
{
    public Task<bool> ContainsName(string name);

    public Task<bool> InsertName(string name);
    
    public Task<bool> RemoveName(string name);

    public Task<IEnumerable<string>> SearchNames(string query);

    public Task<IEnumerable<string>> TopNames(int resultCount);
}

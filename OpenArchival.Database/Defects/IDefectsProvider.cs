using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public interface IDefectsProvider
{
    public Task<bool> AddDefect(string defect);

    public Task<bool> RemoveDefect(string defect);

    public Task<bool> ContainsDefect(string defect);

    public Task<IEnumerable<string>> SearchDefects(string query);

    public Task<IEnumerable<string>> TopDefects(int resultCount);
}

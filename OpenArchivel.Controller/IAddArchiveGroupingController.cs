using OpenArchival.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Controller;

public interface IAddArchiveGroupingController
{
    public void CategoryAdded(ArchiveCategory category);
}

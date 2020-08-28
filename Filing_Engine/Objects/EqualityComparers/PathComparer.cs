using BH.oM.Adapters.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public class PathComparer : IEqualityComparer<IFileSystemInfo>
    {
        public bool Equals(IFileSystemInfo x, IFileSystemInfo y)
        {
            return x.IFullPath() == y.IFullPath();
        }

        public int GetHashCode(IFileSystemInfo obj)
        {
            return obj.IFullPath().GetHashCode();
        }
    }
}

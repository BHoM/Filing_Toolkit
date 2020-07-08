using BH.oM.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public class PathComparer : IEqualityComparer<IFileSystemInfo >
    {
        public bool Equals(IFileSystemInfo  x, IFileSystemInfo  y)
        {
            return x.FullPath() == y.FullPath();
        }

        public int GetHashCode(IFileSystemInfo  obj)
        {
            return obj.FullPath().GetHashCode();
        }
    }
}

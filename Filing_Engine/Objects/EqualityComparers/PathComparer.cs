using BH.oM.Adapters.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public class PathComparer : IEqualityComparer<IFSInfo>
    {
        public bool Equals(IFSInfo x, IFSInfo y)
        {
            return x.IFullPath() == y.IFullPath();
        }

        public int GetHashCode(IFSInfo obj)
        {
            return obj.IFullPath().GetHashCode();
        }
    }
}

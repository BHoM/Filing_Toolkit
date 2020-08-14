using BH.oM.Adapters.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public class PathComparer : IEqualityComparer<IInfo>
    {
        public bool Equals(IInfo x, IInfo y)
        {
            return x.IFullPath() == y.IFullPath();
        }

        public int GetHashCode(IInfo obj)
        {
            return obj.IFullPath().GetHashCode();
        }
    }
}

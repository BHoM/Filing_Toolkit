using BH.oM.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public class PathComparer : IEqualityComparer<IFile>
    {
        public bool Equals(IFile x, IFile y)
        {
            return x.FullPath() == y.FullPath();
        }

        public int GetHashCode(IFile obj)
        {
            return obj.FullPath().GetHashCode();
        }
    }
}

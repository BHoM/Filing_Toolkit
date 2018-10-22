using BH.Engine.Filing;
using BH.oM.Base;
using BH.oM.Filing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids)
        {
            if (type == typeof(Directory))
            {
                return new List<File>() { FileSystem.DirectoryInfo.FromDirectoryName(Path).ToBHoM() };
            } else if (type == typeof(File))
            {
                return new List<File>() { FileSystem.FileInfo.FromFileName(Path).ToBHoM() };
            }
            return new List<IBHoMObject>();
        }
    }
}

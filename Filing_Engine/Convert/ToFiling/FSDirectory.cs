using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Convert
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Converts the provided DirectoryInfo into a BH.oM.Adapters.Filing.Directory." +
            "\nTo populate its `Content` property you need to pull the Directory.")]
        public static oM.Adapters.Filing.FSDirectory ToFiling(this DirectoryInfo di)
        {
            if (di == null) return null;

            oM.Adapters.Filing.FSDirectory bd = new oM.Adapters.Filing.FSDirectory();

            bd.ParentDirectory = di.Parent.ToFiling();
            bd.Name = di.Name;
            bd.Exists = di.Exists;
            bd.IsReadOnly = di.Attributes.HasFlag(FileAttributes.ReadOnly);
            bd.Attributes = di.Attributes;
            bd.CreationTimeUtc = di.CreationTimeUtc;
            bd.CustomData["CreationTime"] = di.CreationTime;
            bd.CustomData["CreationTimeUtc"] = di.CreationTimeUtc;
            bd.CustomData["LastAccessTime"] = di.LastAccessTime;
            bd.CustomData["LastAccessTimeUtc"] = di.LastAccessTimeUtc;
            bd.CustomData["LastWriteTime"] = di.LastWriteTime;
            bd.CustomData["LastWriteTimeUtc"] = di.LastWriteTimeUtc;
            bd.ModifiedTimeUtc = di.LastWriteTimeUtc;

            return bd;
        }

        /***************************************************/
    }
}

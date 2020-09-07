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

        [Description("Converts the provided FileInfo into a BH.oM.Adapters.Filing.File." +
            "\nTo populate its `Content` property you need to pull the file.")]
        public static oM.Adapters.Filing.FSFile ToFiling(this FileInfo fi)
        {
            if (fi == null) return null;

            oM.Adapters.Filing.FSFile bf = new oM.Adapters.Filing.FSFile();

            bf.ParentDirectory = fi.Directory.ToFiling();
            bf.Name = fi.Name;
            bf.Exists = fi.Exists;
            bf.IsReadOnly = fi.IsReadOnly;
            bf.Size = (int)(fi.Length & 0xFFFFFFFF);
            bf.Attributes = fi.Attributes;
            bf.CreationTimeUtc = fi.CreationTimeUtc;
            bf.CustomData["CreationTime"] = fi.CreationTime;
            bf.CustomData["CreationTimeUtc"] = fi.CreationTimeUtc;
            bf.CustomData["LastAccessTime"] = fi.LastAccessTime;
            bf.CustomData["LastAccessTimeUtc"] = fi.LastAccessTimeUtc;
            bf.CustomData["LastWriteTime"] = fi.LastWriteTime;
            bf.CustomData["LastWriteTimeUtc"] = fi.LastWriteTimeUtc;
            bf.ModifiedTimeUtc = fi.LastWriteTimeUtc;

            return bf;
        }

        /***************************************************/
    }
}

using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Query
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Whether the oM.Filing.FileInfo points to a File or a Directory.")]
        [Output("True if points to a File, false if it points to a Directory.")]
        public static bool IsFile(this oM.Filing.FileInfo fi)
        {
            return IsFile(fi.IFullPath());
        }

        [Description("Whether the string fullPath points to a File or a Directory.")]
        [Output("True if points to a File, false if it points to a Directory.")]
        public static bool IsFile(this string fullPath)
        {
            FileAttributes attr = System.IO.File.GetAttributes(fullPath);
            return !((attr & FileAttributes.Directory) == FileAttributes.Directory);
        }

        /***************************************************/


    }
}

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
    public static partial class Query
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Whether the string fullPath points to a Directory that exists.")]
        [Output("True if points to a Directory, false if it points to a File or if the Directory doesn't exist.")]
        public static bool IsExistingDir(this string fullPath)
        {
            if (System.IO.Directory.Exists(fullPath))
            {
                var di = new DirectoryInfo(fullPath);
                FileAttributes attr = di.Attributes;
                return ((attr & FileAttributes.Directory) == FileAttributes.Directory);
            }

            return false;
        }

        /***************************************************/


    }
}

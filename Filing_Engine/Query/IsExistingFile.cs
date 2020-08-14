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

        [Description("Whether the string fullPath points to a File that exists.")]
        [Output("True if points to a File, false if it points to a Directory or if the file doesn't exist.")]
        public static bool IsExistingFile(this string fullPath)
        {
            if (System.IO.File.Exists(fullPath))
            {
                FileAttributes attr = System.IO.File.GetAttributes(fullPath);
                return !((attr & FileAttributes.Directory) == FileAttributes.Directory);
            }

            return false;
        }

        /***************************************************/


    }
}

using BH.oM.Base;
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

        [Description("Combines two paths.")]
        public static string PathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /***************************************************/

        [Description("Combines two paths.")]
        public static string PathCombine(List<string> paths)
        {
            return Path.Combine(paths.ToArray());
        }
    }
}

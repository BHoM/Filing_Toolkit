using BH.oM.Filing;
using System;
using System.Collections.Generic;
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

        public static string Path(this IFile file, string seperator = "/")
        {
            if (file.ParentDirectory == null) return file.Name;
            return file.ParentDirectory.Path(seperator) + seperator + file.Name;
        }

        /***************************************************/
    }
}

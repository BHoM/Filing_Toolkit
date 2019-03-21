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
            if (!CheckLoops(file)) throw new ArgumentException("Circular directory hierarchy");
            if (file.ParentDirectory == null) return file.Name;
            return file.ParentDirectory.Path(seperator) + seperator + file.Name;
        }

        /***************************************************/
        /*** Private Methods                             ***/
        /***************************************************/
        private static bool CheckLoops(this IFile file, HashSet<Directory> encountered = null)
        {
            if (encountered == null) encountered = new HashSet<Directory>();
            if (file.ParentDirectory == null) return true;
            if (encountered.Contains(file.ParentDirectory)) return false;

            encountered.Add(file.ParentDirectory);

            return CheckLoops(file.ParentDirectory, encountered);
        }

        /***************************************************/
    }
}

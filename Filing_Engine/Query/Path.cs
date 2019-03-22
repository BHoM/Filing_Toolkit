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
            if (!IsAcyclic(file)) throw new ArgumentException("Circular directory hierarchy");
            if (file.ParentDirectory == null) return file.Name;
            return file.ParentDirectory.Path(seperator) + seperator + file.Name;
        }
        

        /***************************************************/

        public static bool IsAcyclic(this IFile file)
        {
            return IsAcyclic(file as dynamic, new HashSet<Directory>());
        }

        /***************************************************/

        private static bool IsAcyclic(IFile file, HashSet<Directory> encountered)
        {
            if (file.ParentDirectory == null) return true;

            if (encountered.Contains(file.ParentDirectory)) return false;

            encountered.Add(file.ParentDirectory);

            return IsAcyclic(file.ParentDirectory, encountered);
        }

        /***************************************************/
    }
}

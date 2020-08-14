using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Description("Test whether the file hierarchy is acyclic, i.e. has no loops in it.")]
        [Input("file", "The file to test.")]
        [Output("Whether the file hierachy is Acyclic.")]
        public static bool IsAcyclic(this IInfo file)
        {
            return IsAcyclic(file as dynamic, new HashSet<IInfo>());
        }

        /***************************************************/

        private static bool IsAcyclic(IInfo file, HashSet<IInfo> encountered)
        {
            if (file?.ParentDirectory == null) return true;

            if (encountered.Contains(file.ParentDirectory)) return false;

            encountered.Add(file.ParentDirectory);

            return IsAcyclic(file.ParentDirectory, encountered);
        }

        /***************************************************/
    }
}

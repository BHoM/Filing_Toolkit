using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Description("Get the path of the file.")]
        [Input("file", "The file to get the path of.")]
        [Input("separator", "The path separator to use.")]
        [Output("The path of the file separated by the supplied separator.")]
        public static string Path(this IFile file, string separator = "/")
        {
            if (!IsAcyclic(file)) throw new ArgumentException("Circular directory hierarchy");
            if (file.ParentDirectory == null) return file.Name;
            return file.ParentDirectory.Path(separator) + separator + file.Name;
        }
        

        /***************************************************/
    }
}

using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Rename a file or directory.")]
        [Input("file", "The file (or directory) to rename.")]
        [Input("name", "The new name.")]
        [Output("The moved file object.")]
        public static IContent  Rename(this IContent  fileOrDir, string name)
        {
            fileOrDir = BH.Engine.Base.Query.ShallowClone(fileOrDir);
            fileOrDir.Name = name;
            return fileOrDir;
        }

        /***************************************************/
    }
}

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
    public static partial class Modify
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Rename a file or directory.")]
        [Input("file", "The file (or directory) to rename.")]
        [Input("name", "The new name.")]
        [Output("The moved file object.")]
        public static IFile Rename(this IFile file, string name)
        {
            file = file.GetShallowClone() as IFile;
            file.Name = name;
            return file;
        }

        /***************************************************/
    }
}

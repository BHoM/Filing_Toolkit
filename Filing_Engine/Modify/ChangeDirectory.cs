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
        /**** Methods                                   ****/
        /***************************************************/

        [Description("Move a file or directory to a new parent directory.")]
        [Input("file", "The file (or directory) to move.")]
        [Input("to", "The new parent Directory.")]
        [Output("The moved file object.")]
        public static IFile  ChangeDirectory(this IFile  file, FileInfo to)
        {
            file = BH.Engine.Base.Query.ShallowClone(file);
            file.ParentDirectory = to;
            return file;
        }
        
        /***************************************************/
    }
}

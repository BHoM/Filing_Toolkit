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
    public static partial class Modify
    {
        /***************************************************/
        /**** Methods                                   ****/
        /***************************************************/

        [Description("Move a file or directory to a new parent directory.")]
        [Input("file", "The file (or directory) to move.")]
        [Input("to", "The new parent Directory.")]
        [Output("The moved file object.")]
        public static IFSContainer ChangeDirectory(this oM.Adapters.Filing.IFSContainer fileOrDir, oM.Adapters.Filing.FSDirectory to)
        {
            fileOrDir = BH.Engine.Base.Query.ShallowClone(fileOrDir);
            fileOrDir.ParentDirectory = to;
            return fileOrDir;
        }

        /***************************************************/
    }
}

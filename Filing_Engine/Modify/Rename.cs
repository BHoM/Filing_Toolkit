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
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Rename a file or directory.")]
        [Input("file", "The file (or directory) to rename.")]
        [Input("name", "The new name.")]
        [Output("The moved file object.")]
        public static IFSContainer IRename(this oM.Adapters.Filing.IFSContainer fileOrDir, string name)
        {
            fileOrDir = BH.Engine.Base.Query.ShallowClone(fileOrDir);
            return Rename(fileOrDir as dynamic, name);
        }

        /***************************************************/

        public static IFSContainer Rename(this FSFile file, string name)
        {
            file.Name = name;
            return file;
        }

        public static IFSContainer Rename(this FSDirectory directory, string name)
        {
            directory.Name = name;
            return directory;
        }
    }
}

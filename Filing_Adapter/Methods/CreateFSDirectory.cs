using System;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BH.oM.Adapters.Filing;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Create
    {
        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        [Input("parentDirectory", "Path of parent Directory of the directory. You can also specify a string path.")]
        [Input("directoryName", "Name of the directory.")]
        [Input("content", "The content of the file.")]
        [Description("Creates a oM.Adapters.Filing.File object.")]
        public static oM.Adapters.Filing.FSDirectory FSDirectory(string dirFullPath)
        {
            if (Path.HasExtension(dirFullPath))
            {
                BH.Engine.Reflection.Compute.RecordError($"{nameof(dirFullPath)} must identify a Directory. Do not include an extension.");
                return null;
            }

            return dirFullPath;
            
        }

        [Input("parentDirectory","Path of parent Directory of the directory. You can also specify a string path.")]
        [Input("directoryName", "Name of the directory.")]
        [Input("content", "The content of the file.")]
        [Description("Creates a oM.Adapters.Filing.File object.")]
        public static oM.Adapters.Filing.FSDirectory FSDirectory(oM.Adapters.Filing.FSDirectory parentDirectory, string directoryName)
        {
            if (Path.HasExtension(directoryName))
            {
                BH.Engine.Reflection.Compute.RecordError($"{nameof(directoryName)} must identify a Directory. Do not include an extension.");
                return null;
            }

            return new oM.Adapters.Filing.FSDirectory()
            {
                ParentDirectory = parentDirectory,
                Name = directoryName,
            };
        }

        /*******************************************/
    }
}

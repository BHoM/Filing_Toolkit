using System;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BH.oM.Filing;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Filing
{
    public static partial class Create
    {
        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        [Input("parentDirectory", "Path of parent Directory of the directory. You can also specify a string path.")]
        [Input("directoryName", "Name of the directory.")]
        [Input("content", "The content of the file.")]
        [Description("Creates a oM.Filing.File object.")]
        public static oM.Filing.Directory Directory(oM.Filing.Info directoryFullPath)
        {
            string dirFullPath = directoryFullPath.IFullPath();
            if (Path.HasExtension(dirFullPath))
            {
                BH.Engine.Reflection.Compute.RecordError($"{nameof(directoryFullPath)} must identify a Directory. Do not include an extension.");
                return null;
            }

            var di = new DirectoryInfo(dirFullPath);

            return new oM.Filing.Directory()
            {
                ParentDirectory = di.Parent.FullName,
                Name = di.Name,
            };
        }

        [Input("parentDirectory","Path of parent Directory of the directory. You can also specify a string path.")]
        [Input("directoryName", "Name of the directory.")]
        [Input("content", "The content of the file.")]
        [Description("Creates a oM.Filing.File object.")]
        public static oM.Filing.Directory Directory(oM.Filing.Info parentDirectory, string directoryName)
        {
            if (Path.HasExtension(directoryName))
            {
                BH.Engine.Reflection.Compute.RecordError($"{nameof(directoryName)} must identify a Directory. Do not include an extension.");
                return null;
            }

            return new oM.Filing.Directory()
            {
                ParentDirectory = parentDirectory,
                Name = directoryName,
            };
        }

        /*******************************************/
    }
}

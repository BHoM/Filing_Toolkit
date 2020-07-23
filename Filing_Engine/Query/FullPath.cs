using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        [Description("Get the full path of the file or directory.")]
        [Input("fileOrDir", "The file or Directory to get the full path of.")]
        [Output("The full path of the file or Directory.")]
        public static string FullPath(this IFileSystemInfo fileOrDir)
        {
            if (!IsAcyclic(fileOrDir)) throw new ArgumentException("Circular directory hierarchy");

            return FullPath(fileOrDir as dynamic);
        }

        [Description("Get the full path of the file, including extension.")]
        [Input("file", "The file to get the path of.")]
        [Output("The full path of the file separated by the supplied delimiter.")]
        private static string FullPath(this BH.oM.Filing.File file)
        {
            return Path.Combine(file.ParentDirectory.FullPath(), file.Name);
        }

        [Description("Get the full path of the directory.")]
        [Input("directory", "The directory to get the path of.")]
        [Output("The path of the file separated by the supplied separator.")]
        private static string FullPath(this BH.oM.Filing.DirectoryInfo directory)
        {
            if (directory.ParentDirectory == null)
                return directory.Name;

            return Path.Combine(directory.ParentDirectory.FullPath(), directory.Name);
        }


        /***************************************************/


    }
}

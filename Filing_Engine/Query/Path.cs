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

        [Description("Get the full path of the file or directory.")]
        [Input("fileOrDir", "The file or Directory to get the full path of.")]
        [Input("separator", "The path separator to use.")]
        [Output("The path of the file or Directory separated by the supplied separator.")]
        public static string FullPath(this IFile fileOrDir, string separator = "/")
        {
            if (!IsAcyclic(fileOrDir)) throw new ArgumentException("Circular directory hierarchy");

            return FullPath(fileOrDir as dynamic);
        }

        [Description("Get the full path of the file, including extension.")]
        [Input("file", "The file to get the path of.")]
        [Input("separator", "The path separator to use.")]
        [Output("The full path of the file separated by the supplied separator.")]
        public static string FullPath(this BH.oM.Filing.File file, string separator = "/")
        {
            string fileNameWithExtension = "";

            if (file.ParentDirectory == null)
            {

                if (file.Extension.StartsWith("."))
                    fileNameWithExtension = file.Name + file.Extension;

                else
                    fileNameWithExtension = file.Name + "." + file.Extension;
            }

            return file.ParentDirectory.FullPath(separator) + separator + fileNameWithExtension;
        }

        [Description("Get the full path of the directory.")]
        [Input("directory", "The directory to get the path of.")]
        [Input("separator", "The path separator to use.")]
        [Output("The path of the file separated by the supplied separator.")]
        public static string FullPath(this BH.oM.Filing.Directory directory, string separator = "/")
        {
            if (directory.ParentDirectory == null) return directory.Name;
            return directory.ParentDirectory.FullPath(separator) + separator + directory.Name;
        }


        /***************************************************/
    }
}

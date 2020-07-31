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
    public static partial class Convert
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Converts the provided File into a System.IO.FileInfo." +
            "\nAny `Content` property is lost in this conversion.")]
        public static FileInfo ToSystem(this oM.Filing.File file)
        {
            return new FileInfo(file.IFullPath());
        }

        /***************************************************/

        [Description("Converts the provided Directory into a System.IO.DirectoryInfo." +
            "\nAny `Content` property is lost in this conversion.")]
        public static DirectoryInfo ToSystem(this oM.Filing.Directory directory)
        {
            return new DirectoryInfo(directory.IFullPath());
        }

        /***************************************************/
    }
}

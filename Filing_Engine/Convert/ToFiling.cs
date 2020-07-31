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

        [Description("Converts the provided FileInfo into a BH.oM.Filing.File." +
            "\nTo populate its `Content` property you need to pull the file.")]
        public static oM.Filing.File ToFiling(this FileInfo fileInfo)
        {
            return (oM.Filing.File)fileInfo;
        }

        /***************************************************/

        [Description("Converts the provided DirectoryInfo into a BH.oM.Filing.Directory." +
            "\nTo populate its `Content` property you need to pull the Directory.")]
        public static oM.Filing.Directory ToFiling(this DirectoryInfo directoryInfo)
        {
            return (oM.Filing.Directory)directoryInfo;
        }

        /***************************************************/
    }
}

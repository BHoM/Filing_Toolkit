using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Convert
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Converts the provided FileInfo into a BH.oM.Adapters.Filing.File." +
            "\nTo populate its `Content` property you need to pull the file.")]
        public static oM.Adapters.Filing.File ToFiling(this FileInfo fileInfo)
        {
            return (oM.Adapters.Filing.File)fileInfo;
        }

        /***************************************************/

        [Description("Converts the provided DirectoryInfo into a BH.oM.Adapters.Filing.Directory." +
            "\nTo populate its `Content` property you need to pull the Directory.")]
        public static oM.Adapters.Filing.Directory ToFiling(this DirectoryInfo directoryInfo)
        {
            return (oM.Adapters.Filing.Directory)directoryInfo;
        }

        /***************************************************/
    }
}

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

        public static oM.Adapters.Filing.FileRequest ToFileRequest(this FileDirRequest fdr)
        {
            return new FileRequest()
            {
                ParentDirectory = fdr.ParentDirectory,
                IncludeFileContents = fdr.IncludeFileContents,
                SearchSubdirectories = fdr.IncludeSubdirectories,
                MaxFiles = fdr.MaxFiles,
                MaxNesting = fdr.MaxNesting,
            };
        }

        /***************************************************/
    }
}

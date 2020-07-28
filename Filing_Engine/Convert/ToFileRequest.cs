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

        public static oM.Filing.FileRequest ToFileRequest(this FileDirRequest fdr)
        {
            return new FileRequest()
            {
                FullPath = fdr.FullPath,
                IncludeFileContents = fdr.IncludeFileContents,
                SearchSubdirectories = fdr.IncludeSubdirectories,
                MaxFiles = fdr.MaxFiles,
                MaxNesting = fdr.MaxNesting,
            };
        }

        /***************************************************/
    }
}

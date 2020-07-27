using BH.oM.Base;
using BH.oM.Humans;
using System.IO.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using BH.oM.Data.Requests;

namespace BH.oM.Filing
{
    [Description("Used to query Directories or Files.")]
    public class FileRequest : IFileRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Files from this FullPath will be queried. You can also specify a string path.")]
        public virtual Info FullPath { get; set; } = "";

        [Description("Whether to include the contents of the Files.")]
        public virtual bool IncludeFileContents { get; set; } = false;

        /***************************************************/
        /**** Implicit cast                             ****/
        /***************************************************/

        public static implicit operator FileRequest(string fullPath)
        {
            if (!String.IsNullOrWhiteSpace(fullPath))
                return new FileRequest() { FullPath = fullPath };
            else
                return null;
        }
    }
}

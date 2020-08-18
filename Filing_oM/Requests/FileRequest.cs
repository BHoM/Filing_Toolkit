using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using BH.oM.Data.Requests;

namespace BH.oM.Adapters.Filing
{
    [Description("Used to query Files from a Parent directory.")]
    public class FileRequest : IFileRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Files from this location will be queried.")]
        public virtual string Location { get; set; } = "";

        [Description("If enabled, look also in subdirectories.")]
        public virtual bool SearchSubdirectories { get; set; } = false;

        [Description("If SearchSubdirectories is true, this sets the maximum subdirectiory nesting level to look in." +
            "\nDefaults to -1 which corresponds to no limit.")]
        public virtual int MaxNesting { get; set; } = -1;

        [Description("Sets the maximum number of Files to retrieve, useful when using SearchSubdirectories." +
            "\nDefaults to -1 which corresponds to no limit.")]
        public virtual int MaxFiles { get; set; } = -1;

        [Description("Whether to include the contents of the Files.")]
        public virtual bool IncludeFileContents { get; set; } = false;

        /***************************************************/
        /**** Implicit cast                             ****/
        /***************************************************/

        public static implicit operator FileRequest(string fullPath)
        {
            if (!String.IsNullOrWhiteSpace(fullPath))
                return new FileRequest() { Location = fullPath };
            else
                return null;
        }
    }
}

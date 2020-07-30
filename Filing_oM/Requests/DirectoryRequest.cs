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

namespace BH.oM.Filing
{
    [Description("Used to query Directories or Files.")]
    public class DirectoryRequest : IDirectoryRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Directory and/or Files from this FullPath will be queried." +
            "\nYou can also specify a string path.")]
        public virtual Info ParentDirectory { get; set; } = "";

        [Description("If enabled, look also in subdirectories.")]
        public virtual bool IncludeSubdirectories { get; set; } = false;

        [Description("If IncludeSubdirectories is true, this sets the maximum subdirectiory nesting level to look in." +
            "\nDefaults to -1 which corresponds to no limit.")]
        public virtual int MaxNesting { get; set; } = -1;

        [Description("Sets the maximum number of Directories to retrieve, useful when using IncludeSubdirectories." +
            "\nDefaults to -1 which corresponds to no limit.")]
        public virtual int MaxDirectories { get; set; } = -1;

        [Description("These files or directories will be excluded from the results, useful when using IncludeSubdirectories." +
            "\nYou can also specify string Full Paths.")]
        public virtual List<IInfo> Exclusions { get; set; } = new List<IInfo>();

        /***************************************************/
        /**** Implicit cast                             ****/
        /***************************************************/

        public static implicit operator DirectoryRequest(string fullPath)
        {
            if (!String.IsNullOrWhiteSpace(fullPath))
                return new DirectoryRequest() { ParentDirectory = fullPath };
            else
                return null;
        }
    }
}

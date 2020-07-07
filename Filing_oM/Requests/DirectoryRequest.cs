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
    public class DirectoryRequest : IRequest, IFileDirRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Directories included in Directory will be pulled. You can also specify a string path.")]
        public virtual Directory Directory { get; set; } = "";

        [Description("Sets the maximum number of Directories to retrieve." +
            "\nDefaults to -1 which corresponds to no limit.")]
        public virtual int MaxDirectories { get; set; } = -1;

        [Description("If enabled, look also in subdirectories.")]
        public virtual bool IncludeSubdirectories { get; set; } = false;

        [Description("If IncludeSubdirectories is true, this sets the maximum subdirectiory nesting level to look in." +
            "\nDefaults to -1 which corresponds to no limit.")]
        public virtual int MaxNesting { get; set; } = -1;

        [Description("These directories will be excluded from the results. You can also specify string Full Paths.")]
        public virtual List<IFile> Exclusions { get; set; }
    }
}

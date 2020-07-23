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
    public interface IFileDirRequest : IRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Files or directories included in this Directory will be pulled. You can also specify a string path.")]
        DirectoryInfo FullPath { get; set; }

        [Description("If enabled, look also in subdirectories.")]
        bool IncludeSubdirectories { get; set; }

        [Description("If IncludeSubdirectories is true, this sets the maximum subdirectiory nesting level to look in." +
            "\nDefaults to -1 which corresponds to infinite.")]
        int MaxNesting { get; set; }

        [Description("These files or directories will be excluded from the results. You can also specify string Full Paths.")]
        List<IFileSystemInfo > Exclusions { get; set; }
    }
}

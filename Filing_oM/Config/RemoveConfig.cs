using BH.oM.Adapter;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.Filing
{
    public class RemoveConfig : ActionConfig
    {
        [Description("Keeps the warnings about Deletion off.")]
        public bool DisableWarnings { get; set; } = false;

        [Description("Whether to include Hidden files.")]
        public bool IncludeHiddenFiles { get; set; } = false;
    }
}

using BH.oM.Adapter;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class PullConfig : ActionConfig
    {
        [Description("Whether to include Hidden files.")]
        public bool IncludeHiddenFiles { get; set; } = false;

        [Description("Whether to include System files.")]
        public bool IncludeSystemFiles { get; set; } = false;
    }
}

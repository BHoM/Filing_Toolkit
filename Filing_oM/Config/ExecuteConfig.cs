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
    public class ExecuteConfig : ActionConfig
    {
        [Description("Keeps the warnings about off.")]
        public bool DisableWarnings { get; set; } = false;
    }
}

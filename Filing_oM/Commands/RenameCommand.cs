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
    public class RenameCommand : IMRCCommand
    {
        [Description("Full paths of the items to be Renamed.")]
        public string FullPath { get; set; }

        [Description("The new Full paths of the Files. Files will be Renamed.")]
        public string TargetFullPath { get; set; }

        [Description("Whether to override the target.")]
        public bool OverwriteTarget { get; set; } = false;
    }
}

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
    public class MoveCommand : IMRCCommand
    {
        [Description("Full paths of the items to be Moved.")]
        public string FullPath { get; set; }

        [Description("The new Full paths of the Files. Files will be Moved there.")]
        public string TargetFullPath { get; set; }

        [Description("Whether to create the target Directory if it doesn't exist.")]
        public bool CreateDirectoryIfNotExist { get; set; } = true;

        [Description("Whether to override the target.")]
        public bool OverwriteTarget { get; set; } = false;
    }
}

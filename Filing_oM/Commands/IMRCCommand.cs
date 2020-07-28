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
    [Description("Interface for Move, Rename and Copy commands.")]
    public interface IMRCCommand : IExecuteCommand
    {
        string FullPath { get; set; }

        string TargetFullPath { get; set; }

        bool OverwriteTarget { get; set; }
    }
}

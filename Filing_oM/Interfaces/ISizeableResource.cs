using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.Filing
{
    [Description("Identifies a resource whose Size can be obtained.")]
    public interface ISizeableResource : IResource
    {
        [Description("Size of the resource in Bytes.")]
        int Size { get; set; }
    }
}

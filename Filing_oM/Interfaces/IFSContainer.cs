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
    [Description("Identifies a FileSystem-hosted Resource (a File or Directory or other) that holds some content.")]
    public interface IFSContainer : IFSInfo, IContainableResource, IBHoMObject
    {
    }
}

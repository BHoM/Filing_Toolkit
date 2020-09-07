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
    [Description("Identifies a Resource (a File or Directory or other) that holds some content.")]
    public interface IContainer : ILocatableResource, IContainableResource, IBHoMObject
    {
    }
}

using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Convert
    {
        public static oM.Adapters.Filing.IFSContainer ToFiling(this oM.Adapters.Filing.IContainer iContainer)
        {
            oM.Adapters.Filing.IFSContainer fscont = (iContainer as ILocatableResource).ToFiling();

            fscont.Content = iContainer.Content;

            return fscont;
        }
    }
}

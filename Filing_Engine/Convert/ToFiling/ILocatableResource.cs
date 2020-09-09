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
        [Description("Attempts conversion of a generic Resource to a File-system Resource.")]
        public static oM.Adapters.Filing.IFSContainer ToFiling(this ILocatableResource iLocRes)
        {
            try
            {
                FSFile file = (FSFile)Path.Combine(iLocRes.Location, iLocRes.Name);
                return file;
            }
            catch { }

            try
            {
                FSDirectory dir = (FSDirectory)Path.Combine(iLocRes.Location, iLocRes.Name);
                return dir;
            }
            catch { }

            BH.Engine.Reflection.Compute.RecordError("The provided resource has an invalid path.");

            return null;
        }
    }
}

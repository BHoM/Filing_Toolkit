using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using BH.oM.Data.Requests;

namespace BH.oM.Adapters.Filing
{
    [Description("Used to read contents of one or more files.")]
    public class FileContentRequest : IRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("The content from this File will be queried.\n" +
            "You can also specify the file with a string Path. ")]
        public virtual File File { get; set; }

        [Description("Only objects of a Type specified in this list will be returned.")]
        public virtual List<Type> Types { get; set; } = new List<Type>();

        [Description("Only BHoMObjects whose CustomData contains these key/value pairs will be returned.")]
        public virtual List<string> CustomDataKeys { get; set; } = new List<string>();

        [Description("Only BHoMObjects that own a Fragment of one of these Types will be returned.")]
        public virtual List<Type> FragmentTypes { get; set; } = new List<Type>();
    }
}

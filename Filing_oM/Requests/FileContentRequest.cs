using BH.oM.Base;
using BH.oM.Humans;
using System.IO.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using BH.oM.Data.Requests;

namespace BH.oM.Filing
{
    [Description("Used to read contents of one or more files.")]
    public class FileContentRequest : IRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("You can also specify Full Paths as string. The content from these Files will be pulled and concatenated.")]
        public virtual List<File> Files { get; set; }

        [Description("Only objects of a Type specified in this list will be returned.")]
        public virtual List<Type> Types { get; set; } = new List<Type>();

        [Description("Only BHoMObjects whose CustomData contains these key/value pairs will be returned.")]
        public virtual List<string> CustomDataKeys { get; set; } = new List<string>();

        [Description("Only BHoMObjects that own a Fragment of one of these Types will be returned.")]
        public virtual List<Type> FragmentTypes { get; set; } = new List<Type>();

        [Description("Maximum number of objects to be retrieved from each file." +
            "Defaults to -1 which corresponds to no limit.")]
        public virtual int MaxObjects { get; set; } = -1;
    }
}

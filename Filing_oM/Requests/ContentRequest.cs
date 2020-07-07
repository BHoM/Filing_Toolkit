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
    public class ContentRequest : IRequest
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("You can also specify Full Paths as string. The content from this Files will be pulled.")]
        public virtual List<File> Files { get; set; }

        [Description("Maximum number of objects to be retrieved." +
            "Defaults to -1 which corresponds to no limit.")]
        public virtual int MaxObjects { get; set; } = -1;
    }
}

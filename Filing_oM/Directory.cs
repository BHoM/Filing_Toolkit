using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace BH.oM.Adapters.Filing
{
    [Description("A Directory. It can include the content of the Directory.")]
    public class Directory : BHoMObject, IDirectory, ILocatableResource, IContainableResource
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Full path of parent Directory. You can also specify a string path.")]
        public virtual string Location { get; set; }

        [Description("Name of the directory.")]
        public new string Name { get; set; }

        [Description("The content of the Directory. This is populated only once Pulled.")]
        public virtual List<object> Content { get; set; } = new List<object>();
    }
}

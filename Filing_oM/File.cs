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
    [Description("A File. It can include the content of the File.")]
    public class File : BHoMObject, IFile, ILocatableResource, IContainableResource
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Full path of parent location of the File. You can also specify a string path.")]
        public virtual string Location { get; set; }

        [Description("Name of the file, INCLUDING Extension.")]
        public new string Name { get; set; }

        [Description("The content of the file.")]
        public virtual List<object> Content { get; set; } = new List<object>();
    }
}

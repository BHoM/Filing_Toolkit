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
    [Description("Identifies objects that store information on a File or Directory under a FileSystem, plus its contents.")]
    public interface IFileSystemContainer : IFileSystemInfo, IBHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        List<object> Content { get; set; }

        /***************************************************/
    }
}
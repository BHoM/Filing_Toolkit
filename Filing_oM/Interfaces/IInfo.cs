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
    [Description("Contains the information and attributes of a File or Directory, but it does not contain any content. Rehash of the .NET's base class 'FileSystemInfo' in BHoM flavour.")]
    public interface IInfo  : IObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/
      
        [Description("Parent Directory of the File or Directory.")]
        Directory ParentDirectory { get; set; }

        [Description("Name of the file (WITH Extension) or of the Directory.")]
        string Name { get; set; }

        bool? Exists { get; set; }

        FileAttributes Attributes { get; set; }

        DateTime CreationTimeUtc { get; set; }
        DateTime ModifiedTimeUtc { get; set; }

        int Size { get; set; }

        string Owner { get; set; }

        /***************************************************/
    }
}

using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    [Description("Points to a File or Directory, but does not store the contents. Rehash of the .NET's class 'FileSystemInfo' in BHoM flavour.")]
    public interface IFile  : IObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/
      
        [Description("Parent Directory of the File or Directory.")]
        FileInfo ParentDirectory { get; set; }

        [Description("Name of the file (WITHOUT Extension) or of the Directory.")]
        string Name { get; set; }

        bool Exists { get; set; }

        FileAttributes Attributes { get; set; }

        DateTime CreationTime { get; set; }
        DateTime CreationTimeUtc { get; set; }
        DateTime LastAccessTime { get; set; }
        DateTime LastAccessTimeUtc { get; set; }
        DateTime LastWriteTime { get; set; }
        DateTime LastWriteTimeUtc { get; set; }

        Human Owner { get; set; }

        /***************************************************/
    }
}

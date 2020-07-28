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
    public interface IInfo  : IObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/
      
        [Description("Parent Directory of the File or Directory.")]
        Info ParentDirectory { get; set; }

        [Description("Name of the file (WITH Extension) or of the Directory.")]
        string Name { get; set; }

        bool Exists { get; set; }

        FileAttributes Attributes { get; set; }

        DateTime CreationTime { get; set; }
        DateTime CreationTimeUtc { get; set; }
        DateTime LastAccessTime { get; set; }
        DateTime LastAccessTimeUtc { get; set; }
        DateTime LastWriteTime { get; set; }
        DateTime LastWriteTimeUtc { get; set; }

        string Owner { get; set; }

        /***************************************************/
    }
}

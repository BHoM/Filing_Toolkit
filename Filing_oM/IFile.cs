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
    public interface IFile : IObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/
      
        string FullName { get; set; }
        string Name { get; set; }
        string Extension { get; set; }

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

using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class Directory : BHoMObject, IFile

    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public virtual string FullName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Extension { get; set; }

        public virtual bool Exists { get; set; }

        public virtual FileAttributes Attributes { get; set; }

        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime CreationTimeUtc { get; set; }
        public virtual DateTime LastAccessTime { get; set; }
        public virtual DateTime LastAccessTimeUtc { get; set; }
        public virtual DateTime LastWriteTime { get; set; }
        public virtual DateTime LastWriteTimeUtc { get; set; }

        public virtual Human Owner { get; set; }

        /***************************************************/

    }
}

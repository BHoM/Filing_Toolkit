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
    public class Directory : IContent
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Full path of parent Directory. You can also specify a string path.")]
        public virtual Directory ParentDirectory { get; set; }

        [Description("Name of the directory.")]
        public virtual string Name { get; set; }

        [Description("Gets a value indicating whether a file exists.")]
        public virtual bool? Exists { get; set; } = null;

        [Description("Whether the folder is read only.")]
        public virtual bool IsReadOnly { get; set; } = false;

        [Description("Size of the folder. This is never automatically computed.")]
        public virtual int Length { get; set; } = 0;

        [Description("Attributes indicating if ReadOnly, Hidden, System File, etc.")]
        public virtual FileAttributes Attributes { get; set; }

        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime CreationTimeUtc { get; set; }
        public virtual DateTime LastAccessTime { get; set; }
        public virtual DateTime LastAccessTimeUtc { get; set; }
        public virtual DateTime LastWriteTime { get; set; }
        public virtual DateTime LastWriteTimeUtc { get; set; }

        [Description("User owning the Directory, if any, or the user who created the Directory.")]
        public virtual string Owner { get; set; }

        [Description("The content of the Directory. This is populated only once Pulled.")]
        public virtual List<object> Content { get; set; } = new List<object>();

        [Description("If hosted on a File System that supports it, this contains the id on that system")]
        public object SystemId { get; set; }


        /***************************************************/
        /**** Explicit cast                             ****/
        /***************************************************/

        public static explicit operator Directory(System.IO.DirectoryInfo di)
        {
            return di != null ? new Directory()
            {
                ParentDirectory = (Directory)di.Parent,

                Name = di.Name,

                Exists = di.Exists,
                IsReadOnly = di.Attributes.HasFlag(FileAttributes.ReadOnly),

                Attributes = di.Attributes,
                CreationTime = di.CreationTime,
                CreationTimeUtc = di.CreationTimeUtc,
                LastAccessTime = di.LastAccessTime,
                LastAccessTimeUtc = di.LastAccessTimeUtc,
                LastWriteTime = di.LastWriteTime,
                LastWriteTimeUtc = di.LastWriteTimeUtc,
            } : null;
        }

        /***************************************************/
        /**** Implicit cast                             ****/
        /***************************************************/

        public static implicit operator Directory(string fileFullPath)
        {
            if (!String.IsNullOrWhiteSpace(fileFullPath))
            {
                var di = new System.IO.DirectoryInfo(fileFullPath);
                return (Directory)di;
            }
            else
                return null;
        }

        /***************************************************/
        /**** ToString                                  ****/
        /***************************************************/

        public override string ToString()
        {
            return Path.Combine(this.ParentDirectory?.ToString() ?? "", this.Name);
        }
    }
}

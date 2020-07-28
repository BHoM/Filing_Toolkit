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

namespace BH.oM.Filing
{
    [Description("A File. It can include the content of the File.")]
    public class File : IContent 
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Full path of parent Directory of the File. You can also specify a string path.")]
        public virtual Info ParentDirectory { get; set; }

        [Description("Name of the file, INCLUDING Extension.")]
        public virtual string Name { get; set; }

        [Description("Gets a value indicating whether a file exists.")]
        public virtual bool Exists { get; set; } = false;

        [Description("Gets or sets a value that determines if the current file is read only.")]
        public virtual bool IsReadOnly { get; set; } = false;

        [Description("Gets the size, in bytes, of the current file.")]
        public virtual long Length { get; set; } = 0;

        [Description("Attributes indicating if ReadOnly, Hidden, System File, etc.")]
        public virtual FileAttributes Attributes { get; set; }

        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime CreationTimeUtc { get; set; }
        public virtual DateTime LastAccessTime { get; set; }
        public virtual DateTime LastAccessTimeUtc { get; set; }
        public virtual DateTime LastWriteTime { get; set; }
        public virtual DateTime LastWriteTimeUtc { get; set; }

        [Description("User owning the file, if any, or the user who created the File.")]
        public virtual string Owner { get; set; }

        [Description("The content of the file.")]
        public virtual List<object> Content { get; set; } = new List<object>();

        /***************************************************/
        /**** Explicit cast                             ****/
        /***************************************************/

        public static explicit operator File(Info bi)
        {
            return bi != null ? new File()
            {
                ParentDirectory = bi.ParentDirectory,

                Name = bi.Name,

                Exists = bi.Exists,

                Attributes = bi.Attributes,
                CreationTime = bi.CreationTime,
                CreationTimeUtc = bi.CreationTimeUtc,
                LastAccessTime = bi.LastAccessTime,
                LastAccessTimeUtc = bi.LastAccessTimeUtc,
                LastWriteTime = bi.LastWriteTime,
                LastWriteTimeUtc = bi.LastWriteTimeUtc,
            } : null;
        }

        public static explicit operator File(System.IO.FileInfo fi)
        {
            return fi != null ? new File()
            {
                ParentDirectory = (Info)fi.Directory,

                Name = fi.Name,

                Exists = fi.Exists,

                IsReadOnly = fi.IsReadOnly,

                Length = fi.Length,

                Attributes = fi.Attributes,
                CreationTime = fi.CreationTime,
                CreationTimeUtc = fi.CreationTimeUtc,
                LastAccessTime = fi.LastAccessTime,
                LastAccessTimeUtc = fi.LastAccessTimeUtc,
                LastWriteTime = fi.LastWriteTime,
                LastWriteTimeUtc = fi.LastWriteTimeUtc,
            } : null;
        }

        /***************************************************/
        /**** Implicit cast                             ****/
        /***************************************************/

        public static implicit operator File(string fileFullPath)
        {
            if (!String.IsNullOrWhiteSpace(fileFullPath))
                return (File)new System.IO.FileInfo(fileFullPath);
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
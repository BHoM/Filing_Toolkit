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
    public class File : BHoMObject, IFile
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Full path of parent Directory of the File.")]
        public virtual Directory ParentDirectory { get; set; }

        [Description("Name of the file, WITHOUT Extension.")]
        public override string Name { get; set; }

        [Description("Extension of the File.")]
        public virtual string Extension { get; set; }

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

        [Description("User owning the file, if any, or the user who created the object File.")]
        public virtual Human Owner { get; set; }


        /***************************************************/
        /**** Explicit cast                             ****/
        /***************************************************/

        public static explicit operator File(System.IO.FileInfo fi)
        {
            return fi != null ? new File()
            {
                ParentDirectory = (Directory)fi.Directory,

                Name = Path.GetFileNameWithoutExtension(fi.Name),

                Extension = Path.GetExtension(fi.Name).Replace(".", ""),

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
                return (File)new FileInfo(fileFullPath);
            else
                return null;
        }
    }
}

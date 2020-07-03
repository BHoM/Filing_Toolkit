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
    public class File : IObject, IFile
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Gets the full path of the directory or file.")]
        public virtual string FullName { get; set; }

        [Description("The name of the file.")]
        public virtual string Name { get; set; }
        [Description("The extension of the file.")]
        public virtual string Extension { get; set; }


        [Description("Gets a value indicating whether a file exists.")]
        public virtual bool Exists { get; set; } = false;
        [Description("Gets or sets a value that determines if the current file is read only.")]
        public bool IsReadOnly { get; set; } = false;

        [Description("Gets the size, in bytes, of the current file.")]
        public long Length { get; set; } = 0;

        
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

        public static explicit operator File(System.IO.FileInfo fileInfo)
        {
            return new File()
            {
                FullName = fileInfo.FullName,

                Name = fileInfo.Name,
                Extension = fileInfo.Extension,

                Exists = fileInfo.Exists,
                IsReadOnly = fileInfo.IsReadOnly,

                Length = fileInfo.Length,

                Attributes = fileInfo.Attributes,
                CreationTime = fileInfo.CreationTime,
                CreationTimeUtc = fileInfo.CreationTimeUtc,
                LastAccessTime = fileInfo.LastAccessTime,
                LastAccessTimeUtc = fileInfo.LastAccessTimeUtc,
                LastWriteTime = fileInfo.LastWriteTime,
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc,
            };
        }
    }
}

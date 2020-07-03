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
    public class Directory : IObject, IFile

    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Gets the full path of the directory or file.")]
        public virtual string FullName { get; set; }

        [Description("The name of the directory.")]
        public virtual string Name { get; set; }

        [Description("Gets a value indicating whether the directory exists.")]
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


        [Description(@"The parent directory, or null if the path is null or if the file path denotes a root (such as '\', 'C:', or * '\\server\share').")]
        public DirectoryInfo Parent { get; }
       
     
        //
        // Summary:
        //     Gets the root portion of the directory.
        //
        // Returns:
        //     An object that represents the root of the directory.
        //
        // Exceptions:
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission.
        public DirectoryInfo Root { get; }


        /***************************************************/
        /**** Explicit cast                             ****/
        /***************************************************/

        public static explicit operator Directory(System.IO.DirectoryInfo directoryInfo)
        {
            return new Directory()
            {
                FullName = directoryInfo.FullName,

                Name = directoryInfo.Name,

                Exists = directoryInfo.Exists,

                Attributes = directoryInfo.Attributes,
                CreationTime = directoryInfo.CreationTime,
                CreationTimeUtc = directoryInfo.CreationTimeUtc,
                LastAccessTime = directoryInfo.LastAccessTime,
                LastAccessTimeUtc = directoryInfo.LastAccessTimeUtc,
                LastWriteTime = directoryInfo.LastWriteTime,
                LastWriteTimeUtc = directoryInfo.LastWriteTimeUtc,
            };
        }

    }
}

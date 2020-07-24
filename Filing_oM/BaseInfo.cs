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
    [Description("Contains the information and attributes of a File or Directory, but it does not contain any content. Rehash of the .NET's base class 'FileSystemInfo' in BHoM flavour.")]
    public class BaseInfo : IInfo
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Full path of parent Directory.")]
        public virtual BaseInfo ParentDirectory { get; set; }

        [Description("Name of the Directory or File.")]
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


        [Description(@"Root folder, such as '\', 'C:', or * '\\server\share'.")]
        public BaseInfo Root { get; }


        /***************************************************/
        /**** Explicit cast                             ****/
        /***************************************************/

        public static explicit operator BaseInfo(System.IO.FileInfo fi)
        {
            return fi != null ? new BaseInfo()
            {
                ParentDirectory = (BaseInfo)fi.Directory,

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

        public static explicit operator BaseInfo(System.IO.DirectoryInfo di)
        {
            return di != null ? new BaseInfo()
            {
                ParentDirectory = (BaseInfo)di.Parent,

                Name = di.Name,

                Exists = di.Exists,

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

        public static implicit operator BaseInfo(string directoryFullPath)
        {
            if (!String.IsNullOrWhiteSpace(directoryFullPath))
                return (BaseInfo)new System.IO.DirectoryInfo(directoryFullPath);
            else
                return null;
        }
    }
}

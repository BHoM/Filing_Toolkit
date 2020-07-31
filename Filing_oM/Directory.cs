using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace BH.oM.Filing
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


        /***************************************************/
        /**** Explicit cast                             ****/
        /***************************************************/

        public static explicit operator Directory(System.IO.DirectoryInfo di)
        {
            return di != null ? new Directory()
            {
                ParentDirectory = (Directory)System.IO.Directory.GetParent(di.FullName),

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
            if (String.IsNullOrWhiteSpace(fileFullPath))
                return null;

            Uri uri = new Uri(fileFullPath);

            string parent = "";
            string name = uri.AbsoluteUri;

            if (uri.IsFile)
            {
                string root = new Uri(Path.GetPathRoot(uri.LocalPath)).AbsoluteUri;

                parent = name != root ? new Uri(uri, "..").LocalPath : "";
                name = name != root ? new Uri(name).Segments.Last().TrimEnd('/') : new Uri(name).LocalPath;
            }
            else
            {
                string root = uri.GetComponents(UriComponents.SchemeAndServer,
                                      UriFormat.SafeUnescaped);

                parent = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

                for (int i = 0; i < uri.Segments.Length - 1; i++)
                {
                    parent += uri.Segments[i];
                }

                parent = parent != root ? parent.Trim("/".ToCharArray()) : ""; // remove trailing `/`
                name = parent != root ? uri.Segments.Last().TrimEnd('/') : root;

            }

            Directory dir = new Directory()
            {
                ParentDirectory = parent,
                Name = name,
            };

            //Exists = di.Exists,
            //    IsReadOnly = di.Attributes.HasFlag(FileAttributes.ReadOnly),

            //    Attributes = di.Attributes,
            //    CreationTime = di.CreationTime,
            //    CreationTimeUtc = di.CreationTimeUtc,
            //    LastAccessTime = di.LastAccessTime,
            //    LastAccessTimeUtc = di.LastAccessTimeUtc,
            //    LastWriteTime = di.LastWriteTime,
            //    LastWriteTimeUtc = di.LastWriteTimeUtc,

            return dir;


        }

        /***************************************************/
        /**** ToString                                  ****/
        /***************************************************/

        public override string ToString()
        {
            //string path = Path.GetFullPath(this.ParentDirectory?.ToString() ?? "");

            Uri parentUri = null;

            Uri res = null;

            Uri.TryCreate(this.ParentDirectory?.ToString() ?? "", UriKind.RelativeOrAbsolute, out parentUri);

            Uri.TryCreate(parentUri, this.Name, out res);

            return res?.AbsolutePath ?? "";

            return Path.Combine(Path.GetFullPath(this.ParentDirectory?.ToString() ?? ""), this.Name ?? "");
        }
    }
}

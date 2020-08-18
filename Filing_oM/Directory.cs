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
    public class Directory : BHoMObject, IContent
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Full path of parent Directory. You can also specify a string path.")]
        public virtual Directory ParentDirectory { get; set; }

        [Description("Name of the directory.")]
        public new string Name { get; set; }

        [Description("Gets a value indicating whether a file exists.")]
        public virtual bool? Exists { get; set; } = null;

        [Description("Whether the folder is read only.")]
        public virtual bool? IsReadOnly { get; set; } = null;

        [Description("Size of the folder. This is never automatically computed.")]
        public virtual int Size { get; set; } = 0;

        [Description("Attributes indicating if ReadOnly, Hidden, System File, etc.")]
        public virtual FileAttributes Attributes { get; set; }

        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime CreationTimeUtc { get; set; }

        public virtual DateTime LastWriteTime { get; set; }
        public virtual DateTime ModifiedTimeUtc { get; set; }

        [Description("User owning the Directory, if any, or the user who created the Directory.")]
        public virtual string Owner { get; set; }

        [Description("The content of the Directory. This is populated only once Pulled.")]
        public virtual List<object> Content { get; set; } = new List<object>();


        /***************************************************/
        /**** Implicit cast                             ****/
        /***************************************************/

        public static implicit operator Directory(string dirFullPath)
        {
            if (String.IsNullOrWhiteSpace(dirFullPath))
                return null;

            Uri uri = new Uri(dirFullPath);

            string parent = "";
            string name = "";

            name = uri.Segments.Last();

            string root = "";
            if (uri.IsFile)
                root = uri.Segments[1];
            else
                root = uri.GetComponents(UriComponents.SchemeAndServer,
                                                    UriFormat.SafeUnescaped);
            int i = 0;
            if (uri.IsFile)
                i = 1;

            for (; i < uri.Segments.Length - 1; i++)
            {
                parent += uri.Segments[i];
            }

            if (uri.IsFile)
                parent = parent.Replace("file:///", "");

            if (parent != root)
                name = uri.Segments.Last().TrimEnd('/');
            else
                parent = "";


            return new Directory() { ParentDirectory = parent, Name = name };
        }

        /***************************************************/
        /**** ToString                                  ****/
        /***************************************************/

        public override string ToString()
        {
            return Path.Combine(this.ParentDirectory?.ToString() ?? "", this.Name ?? "");
        }
    }
}

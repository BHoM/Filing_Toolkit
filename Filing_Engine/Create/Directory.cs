using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Create
    {
        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        [Description("Create a Directory (folder) object by its name and parent.")]
        [Input("name", "The name of the Directory.")]
        [Input("parent", "The parent Directory.")]
        [Output("A new Directory object.")]
        public static Directory Directory(string name , Directory parent = null)
        {
            return new Directory {
                Name = name,
                ParentDirectory = parent
            };
        }

        /*******************************************/

        [Description("Create a Directory (folder) object by a list of names, forming a hierarchy.")]
        [Input("pathParts", "A list of directory names, the first will have no parent and subsequent Directories' parents will be the one created for the preceding part.")]
        [Output("The leaf Directory of the directories created (its name will be the last string in the list).")]
        public static Directory Directory(string[] pathParts)
        {
            Directory last = null;
            foreach(string name in pathParts)
            {
                Directory dir = Directory(name, last);
                last = dir;
            }
            return last;
        }

        /*******************************************/

        [Description("Create a Directory (folder) object by a delimited path, forming a hierarchy based on the parts thereof.")]
        [Input("path", "A file path separated by a delimeter ('/' by default).")]
        [Input("separator", "The separator to split the path by.")]
        [Output("The leaf Directory of the directories created (its name will be the last segment in the supplied path).")]
        public static Directory Directory(string path, string separator = "/")
        {
            return Directory(path.Split(new string[] { separator },StringSplitOptions.RemoveEmptyEntries).ToArray());
        }

        /*******************************************/
    }
}

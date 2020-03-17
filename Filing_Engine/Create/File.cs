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

        [Description("Create a File object by its name and parent.")]
        [Input("name", "The name of the Directory.")]
        [Input("directory", "The parent Directory.")]
        [Input("contents", "The contents of the file as a string.")]
        [Input("encoding", "The encoding of the contents, defaults to UTF-8.")]
        [Output("A new File object.")]
        public static File File(string name , Directory directory = null, string contents = "", Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            return File(
                name, 
                directory,
                encoding.GetBytes(contents)
            );
        }

        /*******************************************/

        [Description("Create a File object by its name and parent.")]
        [Input("name", "The name of the Directory.")]
        [Input("directory", "The parent Directory.")]
        [Input("contents", "The contents of the file as bytes.")]
        [Output("A new File object.")]
        public static File File(string name , Directory directory = null, byte[] contents = null)
        {
            return new File()
            {
                Name = name, 
                ParentDirectory = directory,
                Contents = contents
            };
        }

        /*******************************************/
    }
}

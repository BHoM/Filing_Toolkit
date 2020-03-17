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
    public static partial class Query
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Get the contents of the file as a string.")]
        [Input("file", "The file to get the contents of.")]
        [Input("encoding", "The encoding to use to decode the data, if null (default) discovery will be attempted and default to UTF-8 if encoding can't be discovered.")]
        [Output("The contents of the file.")]
        public static string Contents(this File file, Encoding encoding = null)
        {
            byte[] contents = file.Contents;
            if (contents == null) return null;
            if (encoding == null && ( encoding = file.Encoding() ) == null)
            {
                Reflection.Compute.RecordNote($"Could not determine encoding for file {file.Name}, assuming UTF-8");
                encoding = System.Text.Encoding.UTF8;
            }
            return encoding.GetString(contents);
        }

        /***************************************************/
    }
}

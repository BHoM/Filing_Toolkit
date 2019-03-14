using BH.oM.Filing;
using System;
using System.Collections.Generic;
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

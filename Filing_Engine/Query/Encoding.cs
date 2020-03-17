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

        [Description("Try to get the encoding of the file.")]
        [Input("file", "The file to get the encoding of.")]
        [Output("The encoding of the file if it can be discovered, null if unknown.")]
        public static Encoding Encoding(this File file)
        {
            byte[] contents = file.Contents;
            if (contents == null) return null;
            foreach (EncodingInfo candidate in System.Text.Encoding.GetEncodings())
            {
                byte[] pre = candidate.GetEncoding().GetPreamble();
                int len = pre.Length;
                if (len == 0) continue;
                byte[] check = new byte[len];
                Array.ConstrainedCopy(contents, 0, check, 0, len);
                if (pre.SequenceEqual(check))
                {
                    return candidate.GetEncoding();
                }
            }
            return null;
        }

        /***************************************************/
    }
}

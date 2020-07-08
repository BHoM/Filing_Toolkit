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
        public static Encoding Encoding(this FileInfo file)
        {
            byte[] contents = file.ContentsAsByteArray();
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


        private static Encoding FromEnum(Encodings encodingEnumValue)
        {
            switch (encodingEnumValue)
            {
                case Encodings.FromFile: return null;
                case Encodings.ASCII: return System.Text.Encoding.ASCII;
                case Encodings.BigEndianUnicode: return System.Text.Encoding.BigEndianUnicode;
                case Encodings.Unicode: return System.Text.Encoding.Unicode;
                case Encodings.UTF32: return System.Text.Encoding.UTF32;
                case Encodings.UTF7: return System.Text.Encoding.UTF7;
                case Encodings.UTF8: return System.Text.Encoding.UTF8;

                default: return System.Text.Encoding.UTF8;
            }
        }
    }
}

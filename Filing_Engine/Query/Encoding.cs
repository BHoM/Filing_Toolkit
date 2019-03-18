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

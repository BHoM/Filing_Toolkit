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

        [Description("Get the contents of the file as a string, reading from its location.")]
        [Input("file", "The file to get the contents of.")]
        [Input("encoding", "The encoding to use to decode the data. If null (default) discovery will be attempted; defaults to UTF-8 if it can't be discovered.")]
        [Output("The contents of the file.")]
        public static string ContentsAsString(this File file, Encodings encoding = Encodings.FromFile)
        {
            byte[] contents = file.ContentsAsByteArray();

            System.Text.Encoding sysEncoding = null;
            if (encoding == Encodings.FromFile)
            {
                sysEncoding = file.Encoding();
                if (sysEncoding == null)
                {
                    Reflection.Compute.RecordNote($"Could not determine encoding for file {file.Name}, assuming UTF-8.");
                    sysEncoding = System.Text.Encoding.UTF8;
                }
            }
            else
                sysEncoding = FromEnum(encoding);

            return sysEncoding.GetString(contents);
        }


        [Description("Get the contents of the file as a byteArray, reading from its location.")]
        [Input("file", "The file to get the contents of.")]
        [Output("The contents of the file.")]
        public static byte[] ContentsAsByteArray(this File bhomFile)
        {
            byte[] contents = null;

            try
            {
                using (var stream = System.IO.File.OpenRead(bhomFile.FullPath()))
                {
                    byte[] buff = new byte[stream.Length];
                    int read = stream.Read(buff, 0, (int)stream.Length);
                    contents = buff;
                }
            }
            catch
            {
                Engine.Reflection.Compute.RecordError("Unable to read file: " + bhomFile.FullPath());
            }

            return contents;
        }

        /***************************************************/
    }
}

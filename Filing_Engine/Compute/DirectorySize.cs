using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static class Compute
    {
        [Description("Returns the size of the directory. The calculation might take some time.")]
        public static long DirectorySize(BH.oM.Filing.Directory directory)
        {
            return DirectorySize(directory.ToSystem());
        }

        private static long DirectorySize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirectorySize(di);
            }
            return size;
        }
    }
}

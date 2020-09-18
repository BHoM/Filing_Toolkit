using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Compute
    {
        [Description("Creates Directories and subdirectories for the specified path, if they do not exist. " +
            "E.g. `C:\folder2\folder1` will create both folder2 and folder1 if they do not exist.")]
        public static void CreateFolders(string parentFolder, bool active = false)
        {
            if (active)
                System.IO.Directory.CreateDirectory(parentFolder);
        }
    }
}

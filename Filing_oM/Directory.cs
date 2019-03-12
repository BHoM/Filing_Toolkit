using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class Directory : File
    {
        public IEnumerable<Directory> SubDirectories { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}

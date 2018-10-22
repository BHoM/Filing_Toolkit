using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter;
using BH.oM.Base;
using System.IO.Abstractions;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        public virtual IFileSystem FileSystem { get; protected set; } = new FileSystem();
        public string Path { get; private set; }
        public FilingAdapter(string path)
        {
            Path = path;
        }
    }
}

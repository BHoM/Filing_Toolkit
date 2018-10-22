using BH.Adapter.Filing;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filing_Test
{
    class FakeFilingAdapter : FilingAdapter
    {

        public FakeFilingAdapter(IFileSystem fileSystem, string path) : base(path)
        {
            FileSystem = fileSystem;
        }
    }
}

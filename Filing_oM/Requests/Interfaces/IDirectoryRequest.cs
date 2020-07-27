using BH.oM.Base;
using BH.oM.Humans;
using System.IO.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using BH.oM.Data.Requests;

namespace BH.oM.Filing
{
    public interface IDirectoryRequest : IFilingRequest
    {
        bool IncludeSubdirectories { get; set; }

        int MaxNesting { get; set; }

        int MaxDirectories { get; set; }

        List<IInfo> Exclusions { get; set; }
    }
}

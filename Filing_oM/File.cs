using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class File : BHoMObject
    {
        public string Path { get; set; }
        public DateTime Created { get; set; }
    }
}

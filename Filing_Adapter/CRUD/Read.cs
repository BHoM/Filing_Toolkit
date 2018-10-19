using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids)
        {
            return new List<BHoMObject>();
        }
    }
}

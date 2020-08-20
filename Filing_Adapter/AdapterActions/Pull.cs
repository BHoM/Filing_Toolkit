using BH.Engine.Adapters.Filing;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Adapters.Filing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            PullConfig pullConfig = actionConfig as PullConfig ?? new PullConfig();

            IFilingRequest iFr = request as IFilingRequest;
            if (iFr == null)
            {
                BH.Engine.Reflection.Compute.RecordWarning($"Filing_Adapter only supports request inheriting {nameof(IFilingRequest)}.");
                return new List<object>();
            }

            return Read(request as dynamic, pullConfig);
        }

        /***************************************************/

    }
}

using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Filing;
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
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        public override bool Move(IBHoMAdapter to, IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig pullConfig = null, PushType pushType = PushType.AdapterDefault, ActionConfig pushConfig = null)
        {
            if (pullConfig == null) pullConfig = new FilingConfig { ReadFiles = true };
            return base.Move(to, request, pullType, pullConfig, pushType, pushConfig);
        }

        /***************************************************/
    }
}

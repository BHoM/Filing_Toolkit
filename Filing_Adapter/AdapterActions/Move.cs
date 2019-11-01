using BH.Engine.Reflection;
using BH.oM.Data.Requests;
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
        
        public override bool Move(BHoMAdapter to, IRequest query, Dictionary<string, object> pullConfig = null, Dictionary<string, object> pushConfig = null)
        {
            // Force pulling contents when pulling to another adapter
            if (pullConfig == null) pullConfig = new Dictionary<string, object>();
            pullConfig["ReadFiles"] = true;

            return base.Move(to, query, pullConfig, pushConfig);
        }

        /***************************************************/
    }
}

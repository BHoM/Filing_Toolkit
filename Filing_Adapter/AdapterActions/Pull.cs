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

            // Usual needed check onto badly automated FilterRequest
            FilterRequest fr = request as FilterRequest;
            if (fr != null && !fr.Equalities.Any() && string.IsNullOrWhiteSpace(fr.Tag) && fr.Type == null)
                request = null;

            if (request == null && string.IsNullOrWhiteSpace(m_defaultFilePath))
            {
                BH.Engine.Reflection.Compute.RecordWarning($"Please specify a valid Request, or a Default Filepath in the Filing_Adapter.");
                return new List<object>();
            }

            IRequest ifr = request;

            if (request == null && !string.IsNullOrWhiteSpace(m_defaultFilePath))
            {
                ifr = new FileContentRequest() { File = m_defaultFilePath };
            }

            return Read(ifr as dynamic, pullConfig);
        }

        /***************************************************/

    }
}

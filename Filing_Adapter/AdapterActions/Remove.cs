using BH.Engine.Filing;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Filing;
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

        public override int Remove(IRequest request, ActionConfig actionConfig = null)
        {
            RemoveRequest removeRequest = request as RemoveRequest;
            if (removeRequest == null)
            {
                BH.Engine.Reflection.Compute.RecordWarning($"Please specify a valid {nameof(RemoveRequest)}.");
                return 0;
            }

            oM.Filing.RemoveConfig removeConfig = actionConfig as oM.Filing.RemoveConfig ?? new RemoveConfig();

            if (m_Remove_enableDeleteWarning && !removeConfig.DisableWarnings)
            {
                BH.Engine.Reflection.Compute.RecordWarning($"This Action can delete files and folders with their contents." +
                    $"\nMake sure that you know what you are doing. Re-enable the component to continue.");

                m_Remove_enableDeleteWarning = false;

                return 0;
            }

            if (m_Remove_enableDeleteAlert && (removeConfig.IncludeHiddenFiles))
            {
                BH.Engine.Reflection.Compute.RecordError($"Your {nameof(removeConfig)} is set to {nameof(removeConfig.IncludeHiddenFiles)}" +
                    $"\nMake sure you know what you are doing. To continue, re-enable the component.");

                m_Remove_enableDeleteAlert = false;

                return 0;
            }


            int deletedCount = Delete(request as dynamic, removeConfig);

            if (deletedCount > 0)
            {
                m_Remove_enableDeleteWarning = true; // re-enable the warning if any object was successfully deleted.
            }

            m_Remove_enableDeleteAlert = true; // always re-enable the alert for hidden files.

            return deletedCount;
        }

        /***************************************************/
    }
}

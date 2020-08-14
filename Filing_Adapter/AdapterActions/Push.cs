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

        public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            oM.Adapters.Filing.PushConfig pushConfig = actionConfig as oM.Adapters.Filing.PushConfig ?? new PushConfig();

            if (pushType == PushType.AdapterDefault)
                pushType = m_AdapterSettings.DefaultPushType;

            if (pushType == PushType.FullPush)
            {
                BH.Engine.Reflection.Compute.RecordWarning($"The specified {nameof(PushType)} {nameof(PushType.FullPush)} is not supported.");
                return new List<object>();
            }
                

            if (pushType == PushType.DeleteThenCreate)
                if (m_Push_enableDeleteWarning && !pushConfig.DisableWarnings)
                {
                    BH.Engine.Reflection.Compute.RecordWarning($"You have selected the {nameof(PushType)} {nameof(PushType.DeleteThenCreate)}." +
                        $"\nThis has the potential of deleting files and folders with their contents." +
                        $"\nMake sure that you know what you are doing. This warning will not be repeated." +
                        $"\nRe-enable the component to continue.");

                    m_Push_enableDeleteWarning = false;

                    return new List<object>();
                }

            IEnumerable<oM.Adapters.Filing.IContent> files = objects.OfType<oM.Adapters.Filing.IContent>();
            if (objects.Where(o => o != null).Count() != files.Count())
                BH.Engine.Reflection.Compute.RecordWarning($"File Adapter can only push objects of type {nameof(oM.Adapters.Filing.File)}." +
                    $"\nYou can append BHoMObjects to be pushed in its {nameof(oM.Adapters.Filing.IContent.Content)} property.");

            List<BH.oM.Adapters.Filing.IContent> createdFiles = new List<oM.Adapters.Filing.IContent>();
            createdFiles = Create(files, pushType, pushConfig);

            return createdFiles.OfType<object>().ToList();
        }

        /***************************************************/

    }
}

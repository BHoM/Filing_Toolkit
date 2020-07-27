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

        public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            oM.Filing.PushConfig pushConfig = actionConfig as oM.Filing.PushConfig;

            if (pushType == PushType.AdapterDefault)
                pushType = m_AdapterSettings.DefaultPushType;

            if (pushType == PushType.DeleteThenCreate)
                if (m_enableDeleteWarning)
                {
                    BH.Engine.Reflection.Compute.RecordWarning($"You have selected the {nameof(PushType)} {nameof(PushType.DeleteThenCreate)}." +
                        $"\nThis has the potential of deleting files and folders with their contents." +
                        $"\nMake sure that you know what you are doing. This wanrning is not repeated. Re-enable the component to continue.");

                    m_enableDeleteWarning = false;

                    return new List<object>();
                }

            IEnumerable<oM.Filing.IContent> files = objects.OfType<oM.Filing.IContent>();
            if (objects.Where(o => o != null).Count() != files.Count())
                BH.Engine.Reflection.Compute.RecordWarning($"File Adapter can only push objects of type {nameof(oM.Filing.File)}." +
                    $"\nYou can append BHoMObjects to be pushed in its {nameof(oM.Filing.IContent.Content)} property.");

            List<BH.oM.Filing.IContent> createdFiles = new List<oM.Filing.IContent>();
            createdFiles = Create(files, pushType, pushConfig);

            return createdFiles.OfType<object>().ToList();
        }

        /***************************************************/

    }
}

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
using BH.oM.Base;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            PushConfig pushConfig = actionConfig as PushConfig ?? new PushConfig();
            m_defaultFilePath = pushConfig.DefaultFilePath;

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

            List<IResource> createdFiles = new List<IResource>();

            List<IResource> filesOrDirs = objects.OfType<IResource>().ToList();
            List<object> remainder = objects.Except(filesOrDirs).ToList();

            if (remainder.Any())
            {
                BH.Engine.Reflection.Compute.RecordNote($"Objects that are not Files or Directories " +
                    $"\nwill be Pushed using the Filing Adapter default filePath: `{m_defaultFilePath}`." +
                    $"\nUse the PushConfig to specify a different filePath for them.");
                string defaultDirectory = Path.GetDirectoryName(m_defaultFilePath);
                string defaultFileName = Path.GetFileName(m_defaultFilePath);

                FSFile file = CreateFSFile(defaultDirectory, defaultFileName, remainder);
                filesOrDirs.Add(file);
            }

            foreach (IResource fileOrDir in filesOrDirs)
            {
                IResource created = Create(fileOrDir as dynamic, pushType, pushConfig);
                createdFiles.Add(created);
            }

            return createdFiles.OfType<object>().ToList();
        }

        /***************************************************/

    }
}

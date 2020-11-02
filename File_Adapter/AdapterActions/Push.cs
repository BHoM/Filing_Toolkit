/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.Engine.Adapters.File;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Adapters.File;
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

namespace BH.Adapter.File
{
    public partial class FileAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            PushConfig pushConfig = actionConfig as PushConfig ?? new PushConfig();

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
            List<object> remainder = objects.Where(v => !filesOrDirs.Contains(v)).ToList();

            if (m_defaultFilePath == null && remainder.Any())
            {
                BH.Engine.Reflection.Compute.RecordError($"To Push objects that are not of type `{nameof(BH.oM.Adapters.File.File)}` or `{nameof(BH.oM.Adapters.File.Directory)}`," +
                    $"\nyou need to specify a target Location by creating the {nameof(FileAdapter)} through the constructor with inputs.");
                return null;
            }

            if (m_defaultFilePath != null)
            {
                pushConfig.PushContentOnly = true;

                if (filesOrDirs.Any())
                    BH.Engine.Reflection.Compute.RecordWarning($"Objects of type `{nameof(BH.oM.Adapters.File.File)}` or `{nameof(BH.oM.Adapters.File.Directory)}`," +
                   $"\nwill be appended to the target file specified in the adapter constructor.\n" +
                   $"If you want to target multiple files, you need create the {nameof(FileAdapter)} through the constructor without inputs.");
            }
             
            if (remainder.Any())
            {
                string defaultDirectory = Path.GetDirectoryName(m_defaultFilePath);
                string defaultFileName = Path.GetFileName(m_defaultFilePath);

                FSFile file = CreateFSFile(defaultDirectory, defaultFileName, remainder);

                IResource created = Create(file, pushType, pushConfig);

                if (created != null)
                    filesOrDirs.Add(created);
            }

            foreach (IResource fileOrDir in filesOrDirs)
            {
                if (fileOrDir == null)
                    continue;

                IResource created = Create(fileOrDir as dynamic, pushType, pushConfig);
                createdFiles.Add(created);
            }

            return createdFiles.OfType<object>().ToList();
        }

        /***************************************************/

    }
}

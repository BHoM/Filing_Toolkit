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

using BH.Adapter;
using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.Reflection.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace BH.Adapter.File
{
    public partial class FileAdapter : BHoMAdapter
    {
        [Input("defaultFilepath", "Default filePath, including file extension. " +
            "\nWhen Pushing, this is used for pushing objects that are not BHoM `File` or `Directory`." +
            "\nWhen Pulling, if no request is specified, a FileContentRequest is automatically generated with this location." +
            "\nBy default this is `C:/temp/Filing_Adapter-objects.json`.")]
        [PreviousVersion("4.0", "BH.Adapter.Filing.FilingAdapter(System.String)")]
        public FileAdapter(string defaultLocation = "C:/temp/Filing_Adapter-objects.json")
        {
            m_defaultFilePath = defaultLocation;

            ProcessExtension(m_defaultFilePath);

            // By default, if they exist already, the files to be created are wiped out and then re-created.
            this.m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.UpdateOrCreateOnly;
        }

        [PreviousVersion("4.0", "BH.Adapter.FileAdapter.FileAdapter(System.String, System.String)")]
        public FileAdapter(string folder = null, string fileName = "")
        {
            if (folder == null)
                folder = @"C:/temp/";

            if (string.IsNullOrEmpty(fileName))
                fileName = "Filing_Adapter-objects.json";

            if (folder.Count() > 2 && folder.ElementAt(1) != ':')
                folder = Path.Combine(@"C:\ProgramData\BHoM\DataSets", folder);

            m_defaultFilePath = Path.Combine(folder, fileName);

            ProcessExtension(m_defaultFilePath);

            // By default, if they exist already, the files to be created are wiped out and then re-created.
            this.m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.UpdateOrCreateOnly;
        }

        private bool m_Push_enableDeleteWarning = true;
        private bool m_Remove_enableDeleteWarning = true;
        private bool m_Remove_enableDeleteAlert = true;
        private bool m_Execute_enableWarning = true;
        private string m_defaultFilePath = null;


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private bool ProcessExtension(string filePath)
        {
            string ext = Path.GetExtension(filePath);

            if (!Path.HasExtension(m_defaultFilePath))
            {
                Engine.Reflection.Compute.RecordNote($"No extension specified in the FileName input. Default is .json.");
                ext = ".json";
                filePath += ext;
            }

            if (ext != ".json" && ext != ".bson")
            {
                Engine.Reflection.Compute.RecordError($"File_Adapter currently supports only .json and .bson extension types.\nSpecified file extension: {ext}");
                return false;
            }

            return true;
        }
    }
}

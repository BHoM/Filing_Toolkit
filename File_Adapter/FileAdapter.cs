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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;

namespace BH.Adapter.File
{
    public partial class FileAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Initialises the File_Adapter without a target location. Allows to target multiple files. Target file locations will have to be specified in the Adapter Action.")]
        public FileAdapter()
        {
            // By default, if they exist already, the files to be created are wiped out and then re-created.
            this.m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.UpdateOrCreateOnly;
        }

        [Description("Initialises the File_Adapter with a target location.")]
        [Input("defaultFilepath", "Default filePath, including file extension. " +
            "\nWhen Pushing, this is used for pushing objects that are not BHoM `File` or `Directory`." +
            "\nWhen Pulling, if no request is specified, a FileContentRequest is automatically generated with this location." +
            "\nBy default this is `C:\\temp\\Filing_Adapter-objects.json`.")]
        [PreviousVersion("4.0", "BH.Adapter.Filing.FilingAdapter(System.String)")]
        public FileAdapter(string targetLocation)
        {
            Init(targetLocation);
        }

        [PreviousVersion("4.0", "BH.Adapter.FileAdapter.FileAdapter(System.String, System.String)")]
        public FileAdapter(string folder, string fileName)
        {
            if (folder?.Count() > 2 && folder?.ElementAt(1) != ':')
                folder = Path.Combine(@"C:\ProgramData\BHoM\DataSets", folder);

            string location = Path.Combine(folder, fileName);

            Init(location);
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private bool m_Push_enableDeleteWarning = true;
        private bool m_Remove_enableDeleteWarning = true;
        private bool m_Remove_enableDeleteAlert = true;
        private bool m_Execute_enableWarning = true;
        private string m_defaultFilePath = null;


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        // Initialisation method for when the File Adapter is instantiated with a location.
        private bool Init(string location)
        {
            location = location.Replace("/", "\\");

            if (string.IsNullOrWhiteSpace(location))
            {
                BH.Engine.Reflection.Compute.RecordError("Please specifiy a valid target location.");
                return true;
            }

            m_defaultFilePath = location;

            if (!ProcessExtension(ref m_defaultFilePath))
                return false;

            BH.Engine.Reflection.Compute.RecordNote($"The adapter will always target the input location `{location}`." +
                $"\nTo target multiple Files, use the {this.GetType().Name} constructor with no input.");

            // By default, the objects are appendend to the file if it exists already.
            this.m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateOnly;

            m_resourceWatcherThread?.Interrupt();
            m_resourceWatcherThread = new Thread(WatchResource);
            m_resourceWatcherThread.Start();

            return true;
        }

        /***************************************************/

        // Checks on the file extension.
        private bool ProcessExtension(ref string filePath)
        {
            string ext = Path.GetExtension(filePath);

            if (!Path.HasExtension(m_defaultFilePath))
            {
                Engine.Reflection.Compute.RecordNote($"No extension specified in the FileName input. Defaulting to .json.");
                ext = ".json";
                filePath += ext;
            }

            if (ext != ".json")
            {
                Engine.Reflection.Compute.RecordError($"File_Adapter currently supports only .json extension type.\nSpecified file extension is `{ext}`.");
                return false;
            }

            return true;
        }

        /***************************************************/

        // Watches the file specified in the m_defaultFilePath.
        private void WatchResource()
        {
            string directory = m_defaultFilePath.Remove(m_defaultFilePath.Count() - Path.GetFileName(m_defaultFilePath).Count());
            string fileName = Path.GetFileName(m_defaultFilePath);

            // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = directory;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch this file.
                watcher.Filter = fileName;//"*.json";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                while (true) { } // Keep alive.
            }
        }

        /***************************************************/

        Thread m_resourceWatcherThread;

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            BH.Engine.Reflection.Compute.RecordWarning($"File: {e.FullPath} {e.ChangeType}");
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            //m_resourceWatcherThread.Interrupt();
            BH.Engine.Reflection.Compute.RecordWarning($"File: {e.OldFullPath} renamed to {e.FullPath}");
            //throw new Exception("Renamed");
        }
    }
}

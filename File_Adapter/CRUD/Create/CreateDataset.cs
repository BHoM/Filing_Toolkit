/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using BH.oM.Base;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.Engine.Serialiser;
using BH.oM.Adapter;
using BH.Engine.Adapters.File;
using BH.oM.Adapters.File;
using System.Text.Json;
using BH.Engine.Diffing;
using BH.oM.Diffing;

namespace BH.Adapter.File
{
    public partial class FileAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public FSFile CreateDataset(BH.oM.Data.Library.Dataset dataset, string fullPath, PushType pushType, PushConfig pushConfig)
        {
            if (dataset == null)
                return null;

            bool fileExisted = System.IO.File.Exists(fullPath);

            // For Dataset objects, use the non-JSON compliant "Dataset" serialization style.
            // This is a newline-separated concatenation of individual JSON-serialized objects.
            string text = String.Join(Environment.NewLine, dataset.ToJson());

            bool filecreated = true;
            try
            {
                if (string.IsNullOrWhiteSpace(m_defaultFilePath))
                {
                    // The file object does not contain any path, so we write an unique File under the default file path.

                    if (pushType == PushType.DeleteThenCreate)
                    {
                        if (fileExisted)
                            System.IO.File.Delete(fullPath);

                        WriteTextFile(fullPath, text, true);
                    }
                    else if (pushType == PushType.UpdateOnly)
                    {
                        // Overwrite existing file.
                        if (fileExisted)
                            WriteTextFile(fullPath, text, true);
                        else
                            BH.Engine.Reflection.Compute.RecordNote($"Dataset {fullPath} was not updated as no file existed at that location.");
                    }
                    else if (pushType == PushType.UpdateOrCreateOnly)
                    {
                        // Overwrite existing file. If it doesn't exist, create it.
                        WriteTextFile(fullPath, text, true);
                    }
                    else if (pushType == PushType.CreateOnly || pushType == PushType.CreateNonExisting)
                    {
                        // Create only if file didn't exist. Do not touch existing ones.
                        if (!fileExisted)
                            WriteTextFile(fullPath, text, true);
                        else
                            BH.Engine.Reflection.Compute.RecordNote($"Dataset {fullPath} was not created as it existed already (Pushtype {pushType.ToString()} was specified).");
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for Dataset objects.");
                        filecreated = false;
                    }
                }
            }
            catch (Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError(e.Message);
            }

            if (filecreated)
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(fullPath);
                oM.Adapters.File.FSFile createdFile = fileinfo.ToFiling();
                createdFile.Content = new List<object>() { dataset };

                return createdFile;
            }

            BH.Engine.Reflection.Compute.RecordError($"Could not create Dataset file `{fullPath}`.");
            return null;
        }

        /***************************************************/

        public static void WriteTextFile(string fullPath, string text, bool replaceContent, bool createParentDirectories = true)
        {
            if (createParentDirectories)
            {
                System.IO.FileInfo file = new System.IO.FileInfo(fullPath);
                file.Directory.Create(); // If the directory already exists, this method does nothing.
            }

            // Treat the "empty input text" case.
            if (string.IsNullOrWhiteSpace(text))
            {
                if (replaceContent)
                    System.IO.File.WriteAllText(fullPath, text); // still write the file
                else
                    System.IO.File.AppendAllText(fullPath, text); // still append empty text (modifying the file)

                return;
            }

            // If replaceContent is true, or file doesn't exist, or file exists but is empty, then replace all content.
            if (replaceContent || !System.IO.File.Exists(fullPath) || !System.IO.File.ReadAllText(fullPath).Any())
                System.IO.File.WriteAllText(fullPath, text);
            else
            {
                System.IO.File.AppendAllText(fullPath, text); // still append empty text (modifying the file)

            }
        }
    }
}



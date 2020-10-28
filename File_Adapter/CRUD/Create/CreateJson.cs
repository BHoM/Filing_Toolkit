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

namespace BH.Adapter.File
{
    public partial class FileAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static FSFile CreateJson(FSFile file, PushType pushType, PushConfig pushConfig)
        {
            string fullPath = file.IFullPath();
            bool fileExisted = System.IO.File.Exists(fullPath);

            // Put together all of the file content.
            List<string> allLines = new List<string>();
            string json = "";

            if (file.Content != null)
            {
                if (!pushConfig.UseDatasetSerialization)
                {
                    allLines.AddRange(file.Content.Where(c => c != null).Select(obj => obj.ToJson() + ","));

                    // Remove the trailing comma if there is only one element.
                    if (string.IsNullOrWhiteSpace(allLines.ElementAtOrDefault(1)) && !string.IsNullOrWhiteSpace(allLines.FirstOrDefault()))
                        allLines[0] = allLines[0].Remove(allLines.FirstOrDefault().Length - 1);

                    // Join all between square brackets to make a valid JSON array.
                    json = String.Join(Environment.NewLine, allLines);
                    json = "[" + json + "]";
                }
                else
                {
                    // Use the non-JSON compliant "Dataset" serialization style.
                    // This is a newline-separated concatenation of individual JSON-serialized objects.
                    allLines.AddRange(file.Content.Where(c => c != null).Select(obj => obj.ToJson()));
                    json = String.Join(Environment.NewLine, allLines);
                }
            }

            bool filecreated = true;
            try
            {
                // Clarify if we are considering the Push in terms of content or of Files.
                if (!pushConfig.PushContentOnly) // We are talking about Files.
                {
                    if (pushType == PushType.DeleteThenCreate)
                    {
                        if (fileExisted)
                            System.IO.File.Delete(fullPath);

                        System.IO.File.WriteAllText(fullPath, json);
                    }
                    else if (pushType == PushType.UpdateOnly)
                    {
                        // Overwrite existing file.
                        if (fileExisted)
                            System.IO.File.WriteAllText(fullPath, json);
                        else
                            BH.Engine.Reflection.Compute.RecordNote($"File {fullPath} was not updated as no file existed at that location.");
                    }
                    else if (pushType == PushType.UpdateOrCreateOnly)
                    {
                        // Overwrite existing file. If it doesn't exist, create it.
                        System.IO.File.WriteAllText(fullPath, json);
                    }
                    else if (pushType == PushType.CreateOnly || pushType == PushType.CreateNonExisting)
                    {
                        if (!fileExisted)
                            System.IO.File.WriteAllText(fullPath, json); // Create only if file didn't exist. Do not touch existing ones.
                        else
                            BH.Engine.Reflection.Compute.RecordNote($"File {fullPath} was not created as it existed already (Pushtype {pushType.ToString()} was specified).");
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .json files.");
                        filecreated = false;
                    }
                }
                else // We are talking about File content.
                {
                    if (pushType == PushType.DeleteThenCreate)
                    {
                        // Replace all content.
                        System.IO.File.WriteAllText(fullPath, json);
                    }
                    else if (pushType == PushType.CreateOnly || pushType == PushType.CreateNonExisting || pushType == PushType.UpdateOnly || pushType == PushType.UpdateOrCreateOnly)
                    {
                        if (!fileExisted)
                            System.IO.File.WriteAllText(fullPath, json); 
                        else
                            System.IO.File.AppendAllText(fullPath, json);
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .json files.");
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
                createdFile.Content = file.Content;

                return createdFile;
            }

            BH.Engine.Reflection.Compute.RecordError($"Could not create {file.ToString()}");
            return null;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static string BeautifyJson(string jsonString)
        {
            JsonDocument doc = JsonDocument.Parse(
                jsonString,
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = true
                }
            );
            MemoryStream memoryStream = new MemoryStream();
            using (
                var utf8JsonWriter = new Utf8JsonWriter(
                    memoryStream,
                    new JsonWriterOptions
                    {
                        Indented = true
                    }
                )
            )
            {
                doc.WriteTo(utf8JsonWriter);
            }
            return new System.Text.UTF8Encoding()
                .GetString(memoryStream.ToArray());
        }
    }
}


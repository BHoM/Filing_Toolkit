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
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.Engine.Serialiser;
using BH.oM.Adapter;
using BH.Engine.Filing;
using BH.oM.Filing;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<BH.oM.Filing.IContent> CreateBson(IEnumerable<BH.oM.Filing.File> files, PushType pushType, PushConfig pushConfig)
        {
            List<BH.oM.Filing.IContent> createdFiles = new List<oM.Filing.IContent>();

            foreach (var file in files)
            {
                string fullPath = file.IFullPath();
                bool fileExisted = System.IO.File.Exists(fullPath);

                bool filecreated = true;

                try
                {
                    if (pushType == PushType.DeleteThenCreate)
                    {
                        if (fileExisted)
                            System.IO.File.Delete(fullPath);

                        WriteBsonWithStream(file, fullPath, FileMode.CreateNew);
                    }
                    else if ((pushType == PushType.UpdateOnly && fileExisted) || pushType == PushType.UpdateOrCreateOnly)
                    {
                        if (fileExisted && pushConfig.AppendContent)
                        {
                            // Append the text to existing file.
                            WriteBsonWithStream(file, fullPath, FileMode.Append);
                        }
                        else
                        {
                            // Override existing file.
                            WriteBsonWithStream(file, fullPath, FileMode.Create);
                        }
                    }
                    else if (pushType == PushType.CreateOnly)
                    {
                        // Create only if file didn't exist. Do not touch existing ones.
                        if (!fileExisted)
                            WriteBsonWithStream(file, fullPath, FileMode.CreateNew);
                        else
                            BH.Engine.Reflection.Compute.RecordNote($"File {fullPath} was not created as it existed already (Pushtype {pushType.ToString()} was specified).");
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .bson files.");
                        filecreated = false;
                    }

                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordError(e.Message);
                    continue;
                }

                if (filecreated)
                {
                    System.IO.FileInfo fileinfo = new System.IO.FileInfo(fullPath);
                    oM.Filing.File createdFile = fileinfo.ToFiling();
                    createdFile.Content = file.Content;

                    createdFiles.Add(createdFile);
                }
            }

            return createdFiles;
        }

        public void WriteBsonWithStream(oM.Filing.File file, string fullPath, FileMode fileMode)
        {
            FileStream stream = new FileStream(fullPath, FileMode.CreateNew);
            var writer = new BsonBinaryWriter(stream);
            BsonSerializer.Serialize(writer, typeof(object), file);
            stream.Flush();
            stream.Close();
        }

        /***************************************************/
    }
}


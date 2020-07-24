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
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
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
        /**** Public Methods                            ****/
        /***************************************************/

        protected List<BH.oM.Filing.IContent> Create(IEnumerable<IContent> filesOrDirs, PushType pushType)
        {
            List<BH.oM.Filing.IContent> createdFiles = new List<oM.Filing.IContent>();

            var groupedPerExtension = filesOrDirs.GroupBy(f => Path.GetExtension(f.IFullPath())).ToDictionary(g => g.Key, g => g.ToList());

            List<IContent> jsons = new List<IContent>();
            if (groupedPerExtension.TryGetValue(".json", out jsons))
                createdFiles.AddRange(CreateJson(jsons.OfType<oM.Filing.File>(), pushType).Cast<IContent>());

            List<IContent> bsons = new List<IContent>();
            if (groupedPerExtension.TryGetValue(".bson", out bsons))
                createdFiles.AddRange(CreateBson(bsons.OfType<oM.Filing.File>(), pushType).Cast<IContent>());

            List<IContent> folders = new List<IContent>();
            if (groupedPerExtension.TryGetValue("", out folders))
                createdFiles.AddRange(CreateDirectory(folders.OfType<oM.Filing.File>(), pushType).Cast<IContent>());

            return createdFiles;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<oM.Filing.File> CreateJson(IEnumerable<BH.oM.Filing.File> files, PushType pushType)
        {
            List<oM.Filing.File> createdFiles = new List<oM.Filing.File>();

            foreach (var file in files)
            {
                string fullPath = file.IFullPath();

                bool filecreated = true;
                try
                {
                    if (pushType == PushType.DeleteThenCreate) // Overwrite existing files. TODO CHECK: does this resets the create date? I need that it does.
                    {
                        var currentTime = DateTime.Now;
                        var currentTimeUTC = DateTime.UtcNow;

                        System.IO.File.WriteAllLines(fullPath, file.Content.Select(obj => obj.ToJson()));
                        System.IO.File.SetCreationTime(fullPath, currentTime);
                        System.IO.File.SetCreationTimeUtc(fullPath, currentTimeUTC);
                    }
                    else if (pushType == PushType.UpdateOnly) // Append the text to existing files.
                        System.IO.File.AppendAllLines(fullPath, file.Content.Select(obj => obj.ToJson()));
                    else if (pushType == PushType.CreateOnly) // Create only files that didn't exist. Do not append anything to existing ones.
                    {
                        if (!System.IO.File.Exists(fullPath))
                            System.IO.File.WriteAllLines(fullPath, file.Content.Select(obj => obj.ToJson()));
                        else
                            BH.Engine.Reflection.Compute.RecordNote($"File {fullPath} was not created as it existed already (Pushtype {pushType.ToString()} was specified).");
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .json files.");
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
                    oM.Filing.File createdFile = (oM.Filing.File)fileinfo;
                    createdFile.Content = file.Content;

                    createdFiles.Add(createdFile);
                }
            }

            return createdFiles;
        }

        private List<BH.oM.Filing.IContent> CreateBson(IEnumerable<BH.oM.Filing.File> files, PushType pushType)
        {
            if (pushType != PushType.DeleteThenCreate && pushType != PushType.UpdateOnly)
            {
                BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .bson files." +
                    $"\nValid options: {PushType.DeleteThenCreate} or {PushType.UpdateOnly}.");
            }

            List<BH.oM.Filing.IContent> createdFiles = new List<oM.Filing.IContent>();

            bool clearfile = pushType == PushType.DeleteThenCreate ? true : false;

            foreach (var file in files)
            {
                string fullPath = file.IFullPath();
                try
                {
                    FileStream stream = new FileStream(fullPath, clearfile ? FileMode.Create : FileMode.Append);
                    var writer = new BsonBinaryWriter(stream);
                    BsonSerializer.Serialize(writer, typeof(object), files);
                    stream.Flush();
                    stream.Close();

                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordError(e.Message);
                    continue;
                }

                System.IO.FileInfo fileinfo = new System.IO.FileInfo(fullPath);
                oM.Filing.File createdFile = (oM.Filing.File)fileinfo;
                createdFile.Content = file.Content;

                createdFiles.Add(createdFile);
            }

            return createdFiles;
        }

        private List<BH.oM.Filing.IContent> CreateDirectory(IEnumerable<BH.oM.Filing.File> directory, PushType pushType)
        {
            if (pushType != PushType.DeleteThenCreate && pushType != PushType.UpdateOnly)
            {
                BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .bson files." +
                    $"\nValid options: {PushType.DeleteThenCreate} or {PushType.UpdateOnly}.");
            }

            List<BH.oM.Filing.IContent> createdDirs = new List<oM.Filing.IContent>();

            bool clearfile = pushType == PushType.DeleteThenCreate ? true : false;

            foreach (var dir in directory)
            {
                string fullPath = dir.IFullPath();
                bool exists = System.IO.Directory.Exists(fullPath);

                bool directoryCreated = true;

                try
                {
                    if (pushType == PushType.DeleteThenCreate) // Deletes and recreates the directory.
                    {
                        if (exists)
                            System.IO.Directory.Delete(fullPath, true); // Deletes the directory and all contents. To make things safer, a Warning is exposed in the Push before proceeding.

                        System.IO.Directory.CreateDirectory(fullPath);
                    }
                    else if (pushType == PushType.CreateOnly || pushType == PushType.UpdateOnly) // Create only directories that didn't exist.
                    {
                        if (!exists)
                            System.IO.Directory.CreateDirectory(fullPath);
                        else
                        {
                            BH.Engine.Reflection.Compute.RecordNote($"File {fullPath} was not created as it existed already (Pushtype {pushType.ToString()} was specified).");
                            directoryCreated = false;
                        }
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .json files.");
                        directoryCreated = false;
                    }
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordError(e.Message);
                    continue;
                }

                if (directoryCreated)
                {
                    System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(fullPath);
                    oM.Filing.Directory createdDir = (oM.Filing.Directory)dirInfo;
                    createdDirs.Add(createdDir);
                }
            }

            return createdDirs;
        }


        /***************************************************/
    }
}


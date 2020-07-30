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
using BH.Engine.Filing;
using BH.oM.Filing;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<BH.oM.Filing.IContent> CreateDirectory(IEnumerable<BH.oM.Filing.Directory> directory, PushType pushType, PushConfig pushConfig)
        {
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
                    else if (pushType == PushType.CreateOnly || pushType == PushType.CreateNonExisting || pushType == PushType.UpdateOrCreate) 
                    {
                        // Create only directories that didn't exist.
                        if (!exists)
                            System.IO.Directory.CreateDirectory(fullPath);
                        else
                        {
                            BH.Engine.Reflection.Compute.RecordNote($"Directory {fullPath} was not created as it existed already (Pushtype {pushType.ToString()} was specified).");
                            directoryCreated = false;
                        }
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for {nameof(BH.oM.Filing.Directory)} objects.");
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


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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.Engine.Serialiser;
using BH.oM.Adapter;
using BH.Engine.Adapters.File;
using BH.oM.Adapters.File;

namespace BH.Adapter.File
{
    public partial class FileAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public IFSContainer Create(ILocatableResource resource, PushType pushType, PushConfig pushConfig)
        {
            if (resource == null)
                return null;

            IFSContainer fileOrDir = resource.ToFiling();
            return Create(fileOrDir, pushType, pushConfig);
        }

        /***************************************************/

        public IFSContainer Create(IFSContainer fileOrDir, PushType pushType, PushConfig pushConfig)
        {
            pushConfig = pushConfig ?? new PushConfig();

            if (fileOrDir == null)
                return null;

            if (fileOrDir is IFile)
            {
                // Check if any dataset is specified in the file content.
                List<BH.oM.Data.Library.Dataset> datasets = fileOrDir.Content.OfType<BH.oM.Data.Library.Dataset>().ToList();

                if (datasets != null && datasets.Any())
                {
                    if (fileOrDir.Content.Except(datasets).Any() || datasets.Count > 1)
                    {
                        BH.Engine.Reflection.Compute.RecordError($"A file that contains an object of type {nameof(BH.oM.Data.Library.Dataset)} cannot contain any other object or Dataset.");
                        return null;
                    }
                    else
                        return CreateDataset(datasets.FirstOrDefault(), fileOrDir.IFullPath(), pushType, pushConfig) as IFSContainer;
                }

                return CreateJson((FSFile)fileOrDir, pushType, pushConfig) as IFSContainer;
            }

            if (fileOrDir is IDirectory)
                return CreateDirectory((FSDirectory)fileOrDir, pushType, pushConfig) as IFSContainer;

            BH.Engine.Reflection.Compute.RecordError($"Could not create {fileOrDir.ToString()}.");
            return null;
        }

        /***************************************************/
    }
}



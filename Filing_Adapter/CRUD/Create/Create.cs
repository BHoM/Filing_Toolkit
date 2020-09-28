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
using BH.Engine.Adapters.Filing;
using BH.oM.Adapters.Filing;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IFSContainer Create(ILocatableResource resource, PushType pushType, PushConfig pushConfig)
        {
            if (resource == null)
                return null; 

            IFSContainer fileOrDir = resource.ToFiling();
            return Create(fileOrDir,  pushType,  pushConfig);
        }

        /***************************************************/

        public static IFSContainer Create(IFSContainer fileOrDir, PushType pushType, PushConfig pushConfig)
        {
            pushConfig = pushConfig ?? new PushConfig();

            if (fileOrDir == null)
                return null;

            string extension = Path.GetExtension(fileOrDir.IFullPath());

            if (extension == ".json")
                return CreateJson((FSFile)fileOrDir, pushType, pushConfig) as IFSContainer;

            if (extension == ".bson")
                return CreateJson((FSFile)fileOrDir, pushType, pushConfig) as IFSContainer;

            if (fileOrDir is IDirectory)
                return CreateDirectory((FSDirectory)fileOrDir, pushType, pushConfig) as IFSContainer;

            BH.Engine.Reflection.Compute.RecordError($"Could not create {fileOrDir.ToString()}.");
            return null;
        }

        /***************************************************/
    }
}


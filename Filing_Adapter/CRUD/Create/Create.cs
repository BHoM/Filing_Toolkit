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

        protected List<BH.oM.Adapters.Filing.IFSContainer> Create(IEnumerable<ILocatableResource> resources, PushType pushType, PushConfig pushConfig)
        {
            IEnumerable<IFSContainer> filesOrDirs = resources.Select(r => r.ToFiling());
            return Create(filesOrDirs,  pushType,  pushConfig);
        }

        protected List<BH.oM.Adapters.Filing.IFSContainer> Create(IEnumerable<IFSContainer> filesOrDirs, PushType pushType, PushConfig pushConfig)
        {
            pushConfig = pushConfig ?? new PushConfig();

            List<BH.oM.Adapters.Filing.IFSContainer> createdFiles = new List<oM.Adapters.Filing.IFSContainer>();

            var groupedPerExtension = filesOrDirs.GroupBy(f => Path.GetExtension(f.IFullPath())).ToDictionary(g => g.Key, g => g.ToList());

            List<IFSContainer> jsons = new List<IFSContainer>();
            if (groupedPerExtension.TryGetValue(".json", out jsons))
                createdFiles.AddRange(CreateJson(jsons.OfType<oM.Adapters.Filing.FSFile>(), pushType, pushConfig).Cast<IFSContainer>());

            List<IFSContainer> bsons = new List<IFSContainer>();
            if (groupedPerExtension.TryGetValue(".bson", out bsons))
                createdFiles.AddRange(CreateBson(bsons.OfType<oM.Adapters.Filing.FSFile>(), pushType, pushConfig).Cast<IFSContainer>());

            List<IFSContainer> remaining = new List<IFSContainer>();
            if (groupedPerExtension.TryGetValue("", out remaining))
            {
                createdFiles.AddRange(CreateDirectory(remaining.OfType<oM.Adapters.Filing.FSDirectory>(), pushType, pushConfig).Cast<IFSContainer>());
            }

            return createdFiles;
        }

        /***************************************************/
    }
}


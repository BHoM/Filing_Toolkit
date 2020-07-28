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

        protected List<BH.oM.Filing.IContent> Create(IEnumerable<IContent> filesOrDirs, PushType pushType, PushConfig pushConfig)
        {
            pushConfig = pushConfig ?? new PushConfig();

            List<BH.oM.Filing.IContent> createdFiles = new List<oM.Filing.IContent>();

            var groupedPerExtension = filesOrDirs.GroupBy(f => Path.GetExtension(f.IFullPath())).ToDictionary(g => g.Key, g => g.ToList());

            List<IContent> jsons = new List<IContent>();
            if (groupedPerExtension.TryGetValue(".json", out jsons))
                createdFiles.AddRange(CreateJson(jsons.OfType<oM.Filing.File>(), pushType, pushConfig).Cast<IContent>());

            List<IContent> bsons = new List<IContent>();
            if (groupedPerExtension.TryGetValue(".bson", out bsons))
                createdFiles.AddRange(CreateBson(bsons.OfType<oM.Filing.File>(), pushType, pushConfig).Cast<IContent>());

            List<IContent> remaining = new List<IContent>();
            if (groupedPerExtension.TryGetValue("", out remaining))
            {
                createdFiles.AddRange(CreateDirectory(remaining.OfType<oM.Filing.Directory>(), pushType, pushConfig).Cast<IContent>());
            }

            return createdFiles;
        }

        /***************************************************/
    }
}

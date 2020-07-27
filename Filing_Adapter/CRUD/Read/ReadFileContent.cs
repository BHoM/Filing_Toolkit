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

using BH.oM.Adapter;
using BH.oM.Base;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.oM.Filing;
using BH.Engine.Filing;


namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        protected IEnumerable<object> Read(FileContentRequest fcr)
        {
            List<object> output = new List<object>();

            oM.Filing.File fileInfo = fcr.File;

            string fileFullPath = fileInfo.IFullPath();

            var retrievedObjects = ReadContent(fileFullPath);

            output.AddRange(
              retrievedObjects
                  .Where(o => fcr.Types.Count > 0 ? fcr.Types.Any(t => t == o.GetType()) : true)
                  .Where(o => fcr.FragmentTypes.Count > 0 ? (o as BHoMObject)?.Fragments.Select(f => f.GetType()).Intersect(fcr.FragmentTypes).Any() ?? false : true)
                  .Where(o => fcr.CustomDataKeys.Count > 0 ? (o as BHoMObject)?.CustomData.Keys.Intersect(fcr.CustomDataKeys).Any() ?? false : true)
             );

            return output;
        }

        protected IEnumerable<object> ReadContent(string fileFullPath)
        {
            List<object> retrievedObjects = new List<object>();

            string extension = Path.GetExtension(fileFullPath);

            if (extension == ".json")
                retrievedObjects.AddRange(ReadJson(fileFullPath));
            else if (extension == ".bson")
                retrievedObjects.AddRange(ReadBson(fileFullPath));
            else
                BH.Engine.Reflection.Compute.RecordWarning($"Only JSON and BSON file formats are currently supported by the {(this as dynamic).GetType().Name}.");

            return retrievedObjects;
        }

        private IEnumerable<object> ReadJson(string fileFullPath)
        {
            string[] json = System.IO.File.ReadAllLines(fileFullPath);
            var converted = json.Select(x => Engine.Serialiser.Convert.FromJson(x)).Where(x => x != null);

            if (json.Count() == 1 && converted.Count() == 1 && converted.FirstOrDefault() is BH.oM.Base.CustomObject)
            {
                // The json is in standard JSON specification (not like in the old "File_Adapter format").
                // This results in a CustomObject being extracted, where all actual objects are in CustomData.
                // Extract actual objects from CustomData.
                converted = (converted.First() as CustomObject).CustomData.Values;
            }
            else if (converted.Count() < json.Count())
                BH.Engine.Reflection.Compute.RecordWarning("Could not convert some object to BHoMObject.");
            return converted;
        }

        private IEnumerable<object> ReadBson(string filePath)
        {
            FileStream mongoReadStream = System.IO.File.OpenRead(filePath);
            var reader = new BsonBinaryReader(mongoReadStream);
            List<BsonDocument> readBson = BsonSerializer.Deserialize(reader, typeof(object)) as List<BsonDocument>;
            return readBson.Select(x => BsonSerializer.Deserialize(x, typeof(object)));
        }
    }
}


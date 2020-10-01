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
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.oM.Adapters.Filing;
using BH.Engine.Adapters.Filing;


namespace BH.Adapter.Filing
{
    /***************************************************/
    /**** Public Methods                            ****/
    /***************************************************/

    public partial class FilingAdapter : BHoMAdapter
    {
        public IEnumerable<object> Read(FileContentRequest fcr, PullConfig pullConfig)
        {
            List<object> output = new List<object>();

            string fileFullPath = fcr.File.IFullPath();

            oM.Adapters.Filing.FSFile readFile = ReadFile(fileFullPath, true, pullConfig.IncludeHiddenFiles, pullConfig.IncludeSystemFiles);

            if (readFile == null)
                return output;

            output = readFile.Content ?? new List<object>();

            if (!output.Any())
                BH.Engine.Reflection.Compute.RecordWarning($"No content could be pulled for {fileFullPath}. Make sure it's not protected or empty.");

            return output
                  .Where(o => fcr.Types.Count > 0 ? fcr.Types.Any(t => t == o.GetType()) : true)
                  .Where(o => fcr.FragmentTypes.Count > 0 ? (o as BHoMObject)?.Fragments.Select(f => f.GetType()).Intersect(fcr.FragmentTypes).Any() ?? false : true)
                  .Where(o => fcr.CustomDataKeys.Count > 0 ? (o as BHoMObject)?.CustomData.Keys.Intersect(fcr.CustomDataKeys).Any() ?? false : true);
        }

        /***************************************************/

        public static IEnumerable<object> ReadContent(string fileFullPath)
        {
            List<object> retrievedObjects = new List<object>();

            string extension = Path.GetExtension(fileFullPath);

            if (extension == ".json")
                retrievedObjects.AddRange(RetrieveJsonContent(fileFullPath));
            else if (extension == ".bson")
                retrievedObjects.AddRange(RetrieveBsonContent(fileFullPath));
            else
                BH.Engine.Reflection.Compute.RecordWarning($"Only JSON and BSON file formats are currently supported by the {typeof(FilingAdapter).Name}.");

            return retrievedObjects;
        }

        /***************************************************/

        public static IEnumerable<object> RetrieveJsonContent(string fileFullPath)
        {
            List<object> result = new List<object>();

            string jsonText = System.IO.File.ReadAllText(fileFullPath);
            string[] jsonLines = System.IO.File.ReadAllLines(fileFullPath);

            if (string.IsNullOrWhiteSpace(jsonText) || jsonLines.Length == 0)
                return result;

            bool isDatasetJson = false;
            object converted = null;

            // Check whether the pulled Json is a "BHoM-Dataset Json".
            // This non-json compliant format is essentially a set of newline-separated json objects.
            // We can check for the first and last character in the first and second line to assume whether it can be deserialised as such.
            // This is faster than Parsing the entire document to check if it's a proper JSON.
            char line1FirstChar = jsonLines.FirstOrDefault().FirstOrDefault();
            char line1LastChar = jsonLines.FirstOrDefault().LastOrDefault();

            char line2FirstChar = default(char);
            char line2LastChar = default(char);

            if (jsonLines.Length > 1)
            {
                line2FirstChar = jsonLines[1].FirstOrDefault();
                line2LastChar = jsonLines[1].LastOrDefault();
            }

            if (line1FirstChar == '{' && line1LastChar == '}')
            {
                if (line2FirstChar != default(char) && line2FirstChar == '{' && line2LastChar != default(char) && line2LastChar == '}')

                // Assume it's a BHoM Dataset Json. Not proper Json.
                isDatasetJson = true;
            }

            //try
            //{
            //    System.Text.Json.JsonDocument.Parse(jsonText);
            //    properJson = true;
            //}
            //catch
            //{
            //    // The object was not a proper JSON. It *might* be our "BHoM-Dataset" JSON.
            //    properJson = false;
            //}

            // The problem of directly doing the following without prior validating the json
            // is that it would expose an unrelevant Error message to the UI.
            if (!isDatasetJson)
                converted = BH.Engine.Serialiser.Convert.FromJson(jsonText);

            if (isDatasetJson || converted == null)
            {
                // Try to read the file as a "BHoM-Dataset json"
                converted = jsonLines.Select(x => Engine.Serialiser.Convert.FromJson(x)).Where(x => x != null);
            }

            // If there is a single CustomObject with no name, assume it was automatically generated by the BHoM Engine deserialization.
            CustomObject cObj = (converted as CustomObject);
            if (cObj != null && cObj.Name == "" && cObj.CustomData.Count > 0)
                converted = cObj.CustomData.Values;

            if (typeof(IEnumerable).IsAssignableFrom(converted.GetType()))
                result.AddRange((converted as IEnumerable).OfType<object>());
            else
                result.Add(converted);

            return result;
        }

        /***************************************************/

        public static IEnumerable<object> RetrieveBsonContent(string filePath)
        {
            FileStream mongoReadStream = System.IO.File.OpenRead(filePath);
            var reader = new BsonBinaryReader(mongoReadStream);
            List<BsonDocument> readBson = BsonSerializer.Deserialize(reader, typeof(object)) as List<BsonDocument>;
            IEnumerable<object> output = null;

            try
            {
                output = readBson.Select(x => BsonSerializer.Deserialize(x, typeof(object)));
            }
            catch
            {
                BH.Engine.Reflection.Compute.RecordWarning($"Could not read content from Bson file `{filePath}`.");
            }

            return output;
        }
    }
}


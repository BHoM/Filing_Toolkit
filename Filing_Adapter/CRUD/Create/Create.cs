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

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected void Create(IEnumerable<BH.oM.Filing.File> files, PushType pushType)
        {
            // TODO: improve performance. Use groupby extension. Do not search full list every time.

            CreateJson(files.Where(f => Path.GetExtension(f.FullPath()) == ".json"), pushType);

            CreateBson(files.Where(f => Path.GetExtension(f.FullPath()) == ".bson"), pushType);

            CreateTxt(files.Where(f => Path.GetExtension(f.FullPath()) == ".txt"), pushType);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private bool CreateJson(IEnumerable<BH.oM.Filing.File> files, PushType pushType)
        {
            try
            {
                if (pushType == PushType.DeleteThenCreate) // Overwrite existing files.
                    files.ToList().ForEach(f => File.WriteAllLines(f.FullPath(), f.Content.Select(obj => obj.ToJson())));
                else if (pushType == PushType.UpdateOnly) // Append the text to existing files.
                    files.ToList().ForEach(f => File.WriteAllLines(f.FullPath(), f.Content.Select(obj => obj.ToJson())));
                else if (pushType == PushType.CreateOnly) // Create only files that didn't exist. Do not append anything to existing ones.
                    files.ToList().Where(f => !File.Exists(f.FullPath())).ToList().ForEach(f => File.WriteAllLines(f.FullPath(), f.Content.Select(obj => obj.ToJson())));
                else
                    BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .json files.");
            }
            catch (Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError(e.Message);
                return false;
            }

            return true;
        }

        private bool CreateBson(IEnumerable<BH.oM.Filing.File> objects, PushType pushType)
        {
            if (pushType != PushType.DeleteThenCreate && pushType != PushType.UpdateOnly)
            {
                BH.Engine.Reflection.Compute.RecordWarning($"The specified Pushtype of {pushType.ToString()} is not supported for .bson files." +
                    $"\nValid options: {PushType.DeleteThenCreate} or {PushType.UpdateOnly}.");
            }

            try
            {
                bool clearfile = pushType == PushType.DeleteThenCreate ? true : false;

                FileStream stream = new FileStream(m_FilePath, clearFile ? FileMode.Create : FileMode.Append);
                var writer = new BsonBinaryWriter(stream);
                BsonSerializer.Serialize(writer, typeof(object), objects);
                stream.Flush();
                stream.Close();
            }
            catch (Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError(e.Message);
                return false;
            }

            return true;
        }

        // Dump the provided content to a text file.
        private bool CreateTxt(IEnumerable<BH.oM.Filing.File> objects, PushType pushType)
        {
            // TODO

            return true;
        }

        /***************************************************/


    }
}


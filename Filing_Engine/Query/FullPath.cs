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
using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Query
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Get the full path.")]
        public static string IFullPath(this object obj)
        {
            return obj != null ? FullPath(obj as dynamic) ?? "" : "";
        }

        private static string FullPath(this IFSInfo baseInfo)
        {
            if (baseInfo.ParentDirectory == null)
                return baseInfo.Name;

            if (!IsAcyclic(baseInfo))
                BH.Engine.Reflection.Compute.RecordError("Circular directory hierarchy found.");

            return baseInfo.ToString();
        }

        private static string FullPath(this IResourceRequest fdr)
        {
            return FullPath(fdr.Location);
        }

        private static string FullPath(this ILocatableResource fdr)
        {
            if (fdr?.Location == null)
                return null;

            return Path.Combine(fdr.Location, string.IsNullOrWhiteSpace(fdr.Name) ? "" : fdr.Name);
        }

        private static string FullPath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            try
            {
                FileInfo fi = new FileInfo(path);
                DirectoryInfo di = new DirectoryInfo(path);

                if (fi.Exists)
                    return fi.FullName;

                if (di.Exists)
                    return di.FullName;
            }
            catch
            {
                BH.Engine.Reflection.Compute.RecordError($"Invalid path provided:\n{path}");
            }

            return null;
        }

        //Fallback
        private static string FullPath(object fileOrDir)
        {
            BH.Engine.Reflection.Compute.RecordError($"Can not compute the FullPath for an object of type {fileOrDir.GetType().Name}.");
            return null;
        }

        /***************************************************/


    }
}

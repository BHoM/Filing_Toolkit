﻿/*
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

using System;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BH.oM.Adapters.Filing;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Create
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        [Input("parentDirectory", "Path of parent Directory of the directory. You can also specify a string path.")]
        [Input("directoryName", "Name of the directory.")]
        [Input("content", "The content of the file.")]
        [Description("Creates a oM.Adapters.Filing.File object.")]
        public static oM.Adapters.Filing.FSDirectory FSDirectory(string dirFullPath)
        {
            if (Path.HasExtension(dirFullPath))
            {
                BH.Engine.Reflection.Compute.RecordError($"{nameof(dirFullPath)} must identify a Directory. Do not include an extension.");
                return null;
            }

            return dirFullPath;
            
        }

        [Input("parentDirectory","Path of parent Directory of the directory. You can also specify a string path.")]
        [Input("directoryName", "Name of the directory.")]
        [Input("content", "The content of the file.")]
        [Description("Creates a oM.Adapters.Filing.File object.")]
        public static oM.Adapters.Filing.FSDirectory FSDirectory(oM.Adapters.Filing.FSDirectory parentDirectory, string directoryName)
        {
            if (Path.HasExtension(directoryName))
            {
                BH.Engine.Reflection.Compute.RecordError($"{nameof(directoryName)} must identify a Directory. Do not include an extension.");
                return null;
            }

            return new oM.Adapters.Filing.FSDirectory()
            {
                ParentDirectory = parentDirectory,
                Name = directoryName,
            };
        }

        /*******************************************/
    }
}

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

using BH.Engine.Adapters.File;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Adapters.File;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;

namespace BH.Adapter.File
{
    public partial class FileAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            PullConfig pullConfig = actionConfig as PullConfig ?? new PullConfig();

            // Usual needed check onto badly automated FilterRequest
            FilterRequest fr = request as FilterRequest;
            if (fr != null && !fr.Equalities.Any() && string.IsNullOrWhiteSpace(fr.Tag) && fr.Type == null)
                request = null;

            if (request == null && string.IsNullOrWhiteSpace(m_defaultFilePath))
            {
                BH.Engine.Reflection.Compute.RecordWarning($"Please specify a valid Request, or a Default Filepath in the Filing_Adapter.");
                return new List<object>();
            }

            IRequest ifr = request;

            if (request == null && !string.IsNullOrWhiteSpace(m_defaultFilePath))
            {
                ifr = new FileContentRequest() { File = m_defaultFilePath };
            }

            return Read(ifr as dynamic, pullConfig);
        }

        /***************************************************/

    }
}

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
using BH.oM.Data.Requests;
using BH.oM.Filing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.Engine.Filing;
using BH.oM.Filing;


namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        protected override int Delete(IRequest request, ActionConfig actionConfig = null)
        {
            Delete(request as dynamic);

            return 0;
        }

        protected void Delete(FileInfoRequest request)
        {
            try
            {
                // Check if file exists with its full path    
                if (System.IO.File.Exists(request.FullPath()))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(Path.Combine(rootFolder, authorsFile));
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }

        }
        
    }
}


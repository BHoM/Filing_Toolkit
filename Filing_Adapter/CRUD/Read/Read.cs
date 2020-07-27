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

using BH.Engine.Filing;
using BH.oM.Adapter;
using BH.oM.Base;
using BH.oM.Filing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        protected IEnumerable<object> Read(FileDirRequest fdr)
        {
            // Recursively walk the directories to retrieve File and Directory Info.
            List<oM.Filing.IContent> output = new List<IContent>();

            string fullPath = fdr.FullPath.IFullPath();
            if (Query.IsFile(fullPath))
                output.Add(RetrieveFile(fullPath, fdr.IncludeFileContents));
            else
                WalkDirectories(output, fdr);

            return output;
        }

        protected IEnumerable<object> Read(FileRequest fr)
        {
            return Read((FileDirRequest)fr);
        }

        protected IEnumerable<object> Read(DirectoryRequest dr)
        {
            return Read((FileDirRequest)dr);
        }

        /***************************************************/

        private oM.Filing.File RetrieveFile(string fullPath, bool readContent)
        {
            return RetrieveFile(new FileInfo(fullPath), readContent);
        }

        private oM.Filing.File RetrieveFile(FileInfo fi, bool readContent)
        {
            oM.Filing.File retrievedFile = new oM.Filing.File();

            retrievedFile = (oM.Filing.File)fi;

            if (readContent)
            {
                var content = ReadContent(fi.FullName);
                retrievedFile.Content.AddRange(content);
            }

            return retrievedFile;
        }

        private bool MaxItemsReached(int maxItems, int retrievedItemsCount)
        {
            return maxItems != -1 && retrievedItemsCount >= maxItems;
        }

        private bool MaxItemsReached(int maxFiles, int retrievedFilesCount, int maxDirs, int retrivedDirsCount)
        {
            return !MaxItemsReached(maxFiles, retrievedFilesCount) && !MaxItemsReached(maxDirs, retrivedDirsCount);
        }
    }
}


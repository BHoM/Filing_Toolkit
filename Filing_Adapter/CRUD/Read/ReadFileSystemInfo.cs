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
        private List<oM.Filing.IFileSystemInfo> Read(IFileDirRequest request)
        {
            // Convert to most generic type of FileInfo request.
            FileDirRequest fdr = BH.Engine.Filing.Create.FileDirRequest(request as dynamic);

            // Recursively walk the directories to retrieve File and Directory Info.
            List<oM.Filing.IFileSystemInfo> output = new List<IFileSystemInfo>();
            WalkDirectories(output, fdr);

            return output;
        }

        private void WalkDirectories(List<oM.Filing.IFileSystemInfo> output, FileDirRequest fdr, int retrievedFiles = 0, int retrievedDirs = 0)
        {
            // Recursion stop condition.
            if (fdr.MaxNesting == 0)
                return;

            // Look in directory and, if requested, recursively in subdirectories.
            System.IO.DirectoryInfo currentDir = new System.IO.DirectoryInfo(fdr.FullPath.FullPath());
            System.IO.DirectoryInfo[] dirArray = currentDir.GetDirectories();
            foreach (System.IO.DirectoryInfo dir in dirArray)
            {
                oM.Filing.DirectoryInfo bhomDir = (oM.Filing.DirectoryInfo)dir;
                bhomDir.ParentDirectory = (oM.Filing.DirectoryInfo)dir.Parent;

                if (fdr.Exclusions.Contains(bhomDir))
                    continue;

                if (fdr.RetrieveDirectories)
                    if (!MaxItemsReached(fdr.MaxDirectories, retrievedDirs))
                    {
                        output.Add(bhomDir);
                        retrievedDirs += 1;
                    }


                if (fdr.RetrieveFiles)
                {
                    System.IO.FileInfo[] files = new FileInfo[] { };

                    try
                    {
                        files = dir.GetFiles("*.*");
                    }
                    // This is thrown if one of the files requires permissions greater than the application provides.
                    catch (UnauthorizedAccessException e)
                    {
                        // Write out the message and continue.
                        BH.Engine.Reflection.Compute.RecordNote(e.Message);
                    }

                    foreach (var f in files)
                    {
                        if (!MaxItemsReached(fdr.MaxFiles, retrievedFiles))
                        {
                            // Check exclusions
                            if (fdr.Exclusions.Contains((BH.oM.Filing.File)f))
                                continue;

                            output.Add((oM.Filing.File)f);
                            retrievedFiles += 1;
                        }
                        else
                            break;
                    }
                }


                // Recurse if requested, and if the limits are not exceeded.
                if (fdr.IncludeSubdirectories == true && MaxItemsReached(fdr.MaxFiles, retrievedFiles, fdr.MaxDirectories, retrievedDirs))
                {
                    FileDirRequest fdrRecurse = BH.Engine.Base.Query.ShallowClone(fdr);
                    fdrRecurse.FullPath = bhomDir;
                    fdrRecurse.MaxNesting -= 1;

                    Read(fdrRecurse);
                }
            }
        }

        /***************************************************/

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


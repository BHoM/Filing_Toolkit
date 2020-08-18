using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.Engine.Serialiser;
using BH.oM.Adapter;
using BH.Engine.Adapters.Filing;
using BH.oM.Adapters.Filing;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        private void WalkDirectories(List<oM.Adapters.Filing.IContent> output, FileDirRequest fdr, 
            ref int retrievedFiles, ref int retrievedDirs, 
            bool inclHidFiles = false, bool inclSysFiles = false)
        {
            // Recursion stop condition.
            if (fdr.MaxNesting == 0)
                return;

            // Look in directory and, if requested, recursively in subdirectories.
            if (string.IsNullOrWhiteSpace(fdr.Location))
            {
                BH.Engine.Reflection.Compute.RecordError($"Missing parameter {nameof(fdr.Location)} from the request.");
                return;
            }

            System.IO.DirectoryInfo currentDir = new System.IO.DirectoryInfo(fdr.Location.IFullPath());

            System.IO.DirectoryInfo[] dirArray = new System.IO.DirectoryInfo[] { };

            if (!Path.HasExtension(currentDir.FullName))
                dirArray = currentDir.GetDirectories();

            foreach (System.IO.DirectoryInfo di in dirArray)
            {
                oM.Adapters.Filing.Directory bhomDir = ReadDirectory(di.FullName, inclHidFiles, inclSysFiles);
                if (bhomDir == null)
                    continue;

                bhomDir.ParentDirectory = di.Parent.ToFiling();

                if (fdr.Exclusions != null && fdr.Exclusions.Contains(bhomDir))
                    continue;

                if (fdr.IncludeDirectories)
                    if (!MaxItemsReached(fdr.MaxDirectories, retrievedDirs))
                    {
                        output.Add(bhomDir);
                        retrievedDirs += 1;
                    }

                // Recurse if requested, and if the limits are not exceeded.
                if (fdr.IncludeSubdirectories == true && MaxItemsReached(fdr.MaxFiles, retrievedFiles, fdr.MaxDirectories, retrievedDirs))
                {
                    FileDirRequest fdrRecurse = BH.Engine.Base.Query.ShallowClone(fdr);
                    fdrRecurse.Location = bhomDir.IFullPath();
                    fdrRecurse.MaxNesting -= 1;

                    WalkDirectories(output, fdrRecurse, ref retrievedFiles, ref retrievedDirs, inclHidFiles, inclSysFiles);
                }
            }

            if (fdr.IncludeFiles)
            {
                System.IO.FileInfo[] fileInfos = new System.IO.FileInfo[] { };

                try
                {
                    fileInfos = currentDir.GetFiles("*.*");
                }
                // This is thrown if one of the files requires permissions greater than the application provides.
                catch (UnauthorizedAccessException e)
                {
                    // Write out the message and continue.
                    BH.Engine.Reflection.Compute.RecordNote(e.Message);
                }

                foreach (var fi in fileInfos)
                {
                    if (!MaxItemsReached(fdr.MaxFiles, retrievedFiles))
                    {
                        // Check exclusions
                        if (fdr.Exclusions != null && fdr.Exclusions.Contains(fi.ToFiling()))
                            continue;

                        oM.Adapters.Filing.File omFile = ReadFile(fi.FullName, fdr.IncludeFileContents, inclHidFiles, inclSysFiles);

                        if (omFile != null)
                        {
                            output.Add(omFile);
                            retrievedFiles += 1;
                        }
                    }
                    else
                        break;
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

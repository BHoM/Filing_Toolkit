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
using BH.oM.Filing;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        private void WalkDirectories(List<oM.Filing.IContent> output, FileDirRequest fdr, int retrievedFiles = 0, int retrievedDirs = 0)
        {
            // Recursion stop condition.
            if (fdr.MaxNesting == 0)
                return;

            // Look in directory and, if requested, recursively in subdirectories.
            System.IO.DirectoryInfo currentDir = new System.IO.DirectoryInfo(fdr.FullPath.IFullPath());

            System.IO.DirectoryInfo[] dirArray = new System.IO.DirectoryInfo[] { };

            if (!Path.HasExtension(currentDir.FullName))
                dirArray = currentDir.GetDirectories();

            foreach (System.IO.DirectoryInfo dir in dirArray)
            {
                oM.Filing.Directory bhomDir = (oM.Filing.Directory)dir;
                bhomDir.ParentDirectory = (oM.Filing.Info)dir.Parent;

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
                    fdrRecurse.FullPath = bhomDir.IFullPath();
                    fdrRecurse.MaxNesting -= 1;

                    WalkDirectories(output, fdrRecurse, retrievedFiles, retrievedDirs);
                }
            }

            if (fdr.IncludeFiles)
            {
                System.IO.FileInfo[] files = new System.IO.FileInfo[] { };

                try
                {
                    files = currentDir.GetFiles("*.*");
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
                        if (fdr.Exclusions != null && fdr.Exclusions.Contains((BH.oM.Filing.File)f))
                            continue;

                        oM.Filing.File omFile = RetrieveFile(f, fdr.IncludeFileContents);

                        output.Add(omFile);
                        retrievedFiles += 1;
                    }
                    else
                        break;
                }
            }
        }
    }
}

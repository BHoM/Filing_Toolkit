using BH.Engine.Filing;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Filing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            List<object> outputObjs = new List<object>();

            // //- Read config
            FilingConfig filingConfig = (actionConfig as FilingConfig) ?? new FilingConfig();

            IFileDirRequest fdr = request as IFileDirRequest;
            if (fdr != null)
                outputObjs.AddRange(GetFiles(fdr));

            return outputObjs;
        }

        /***************************************************/

        private List<oM.Filing.IFile> GetFiles(IFileDirRequest request, int retrievedFiles = 0, int retrievedDirs = 0)
        {
            List<oM.Filing.IFile> output = new List<oM.Filing.IFile>();

            // Convert to most generic type of request.
            FileAndDirRequest fdr = BH.Engine.Filing.Create.FileDirRequest(request as dynamic);

            // Recursion stop condition.
            if (request.MaxNesting == 0)
                return output;

            // Look in directory and, if requested, recursively in subdirectories.
            DirectoryInfo dirInfo = new FileInfo(fdr.Directory.FullPath()).Directory;
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                oM.Filing.Directory d = (oM.Filing.Directory)dir;
                d.ParentDirectory = (oM.Filing.Directory)dir.Parent;

                if (fdr.RetrieveDirectories)
                    if (fdr.MaxDirectories == -1 || retrievedDirs < fdr.MaxDirectories)
                    {
                        output.Add(d);
                        retrievedDirs += 1;
                    }

                if (fdr.RetrieveFiles)
                    foreach (var f in dir.GetFiles())
                    {
                        if (fdr.MaxFiles == -1 || retrievedFiles < fdr.MaxFiles)
                        {
                            output.Add((oM.Filing.File)f);
                            retrievedFiles += 1;
                        }
                        else
                            break;
                    }

                // Recurse if requested, and if the limits are not exceeded.
                if (fdr.IncludeSubdirectories == true && retrievedFiles < fdr.MaxFiles && retrievedDirs < fdr.MaxDirectories)
                {
                    FileAndDirRequest fdrRecurse = BH.Engine.Base.Query.ShallowClone(fdr);
                    fdrRecurse.MaxNesting -= 1;

                    output.AddRange(GetFiles(fdrRecurse));
                }
            }

            return output;
        }

        /***************************************************/

     

    }
}

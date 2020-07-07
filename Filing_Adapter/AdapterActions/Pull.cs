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

            IFileDirRequest fileOrDirRequest = request as IFileDirRequest;
            if (fileOrDirRequest != null)
                outputObjs.AddRange(GetFiles(fileOrDirRequest));



            return outputObjs;         
        }

        /***************************************************/

        private List<oM.Filing.IFile> GetFiles(IFileDirRequest dirReq)
        {
            DirectoryInfo directory = new FileInfo(dirReq.Directory.FullPath()).Directory;

            List<oM.Filing.IFile> files = new List<oM.Filing.IFile>();
            if (dirReq.MaxNesting == 0) return files;

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                oM.Filing.Directory d = (oM.Filing.Directory)dir;
                d.ParentDirectory = (oM.Filing.Directory)dir.Parent;

                files.Add(d);

                if (dirReq.MaxItems != -1 && files.Count > dirReq.MaxItems)
                    return files.Take(dirReq.MaxItems).ToList();

                if (dirReq.IncludeSubdirectories == true)
                {
                    DirectoryRequest dr = new DirectoryRequest()
                    {
                        Directory = dir.FullName,
                        MaxNesting = dirReq.MaxNesting - 1,
                        MaxItems = dirReq.MaxItems - files.Count,
                        IncludeSubdirectories = true
                    };

                    files.AddRange(GetFiles(dr));
                }
            }

            return files;
        }

        /***************************************************/

        private List<oM.Filing.File> GetFiles(DirectoryInfo directory, int maxDepth = -1, bool readFiles = false, oM.Filing.Directory parent = null)
        {

            List<oM.Filing.File> files = new List<oM.Filing.File>();
            if (maxDepth == 0) return files;

            files.AddRange(
                directory.GetFiles().Select(f =>
                {
                    return (oM.Filing.File)f;
                })
            );

            foreach (var dir in directory.GetDirectories())
            {
                oM.Filing.Directory d = (oM.Filing.Directory)dir;
                d.ParentDirectory = parent;
                files.AddRange(GetFiles(dir, maxDepth - 1, false, d));
            }
            return files;
        }

    }
}

using BH.Engine.Filing;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Filing;
using System;
using System.Collections;
using System.Collections.Generic;
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
            // //- Read config
            FilingConfig filingConfig = (actionConfig as FilingConfig) ?? new FilingConfig();

            Type type = null;
            FilterRequest filterReq = request as FilterRequest;
            if (filterReq != null)
                type = filterReq.Type;

            if (type == typeof(oM.Filing.Directory))
                return GetDirectories(FileSystem.DirectoryInfo.FromDirectoryName(Path), filingConfig.MaxDepth);
            else if (type == typeof(oM.Filing.File))
                return GetFiles(FileSystem.DirectoryInfo.FromDirectoryName(Path), filingConfig.MaxDepth, filingConfig.ReadFiles);

            return new List<object>();
        }

        /***************************************************/

        private List<Directory> GetDirectories(DirectoryInfoBase directory, int depth = -1, Directory parent = null)
        {
            List<Directory> directories = new List<Directory>();
            if (depth == 0) return directories;
            foreach (var dir in directory.GetDirectories())
            {
                Directory d = dir.ToFile() as Directory;
                d.ParentDirectory = parent;

                directories.Add(d);
                directories.AddRange(GetDirectories(dir, depth - 1, d));

            }
            return directories;
        }

        /***************************************************/

        private List<File> GetFiles(DirectoryInfoBase directory, int depth = -1, bool readFiles = false, Directory parent = null)
        {

            List<File> files = new List<File>();
            if (depth == 0) return files;

            files.AddRange(
                directory.GetFiles().Select(f =>
                {
                    
                    return bhomFile;
                })
            );

            foreach (var dir in directory.GetDirectories())
            {
                Directory d = dir.ToFile() as Directory;
                d.ParentDirectory = parent;
                files.AddRange(GetFiles(dir, depth - 1, false, d));
            }
            return files;
        }

    }
}

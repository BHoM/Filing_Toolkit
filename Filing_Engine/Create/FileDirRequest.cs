using System;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BH.oM.Filing;
using System.ComponentModel;

namespace BH.Engine.Filing
{
    public static partial class Create
    {
        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        public static FileDirInfoRequest FileDirRequest(FileInfoRequest fr)
        {
            return new FileDirInfoRequest()
            {
                Directory = fr.Directory,
                RetrieveDirectories = false,
                MaxFiles = fr.MaxFiles,
                IncludeSubdirectories = fr.IncludeSubdirectories,
                MaxNesting = fr.MaxNesting,
                Exclusions = fr.Exclusions
            };
        }

        public static FileDirInfoRequest FileDirRequest(DirectoryInfoRequest dr)
        {
            return new FileDirInfoRequest()
            {
                Directory = dr.Directory,
                RetrieveFiles = false,
                MaxDirectories = dr.MaxDirectories,
                IncludeSubdirectories = dr.IncludeSubdirectories,
                MaxNesting = dr.MaxNesting,
                Exclusions = dr.Exclusions
            };
        }

        [Description("Combines the two requests.")]
        public static FileDirInfoRequest FileDirRequest(FileInfoRequest fr, DirectoryInfoRequest dr)
        {
            return new FileDirInfoRequest()
            {
                // Take the shortest of the paths (closer to root)
                Directory = fr.Directory.FullPath().Length < dr.Directory.FullPath().Length ? fr.Directory : dr.Directory, 

                MaxFiles = fr.MaxFiles,

                MaxDirectories = dr.MaxDirectories,

                IncludeSubdirectories = fr.IncludeSubdirectories || dr.IncludeSubdirectories,
                
                // Take the min of the nesting maximums
                MaxNesting = Math.Min(fr.MaxNesting, dr.MaxNesting),

                Exclusions = fr.Exclusions.Concat(dr.Exclusions).ToList()
            };
        }

        /*******************************************/
    }
}

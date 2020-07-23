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

        public static FileDirRequest FileDirRequest(string path, bool pullFileContents)
        {
            return new FileDirRequest()
            {
                FullPath = path
            };
        }

        public static FileDirRequest FileDirRequest(FileInfoRequest fr)
        {
            return new FileDirRequest()
            {
                FullPath = fr.FullPath,
                RetrieveDirectories = false,
                MaxFiles = fr.MaxFiles,
                IncludeSubdirectories = fr.IncludeSubdirectories,
                MaxNesting = fr.MaxNesting,
                Exclusions = fr.Exclusions
            };
        }

        public static FileDirRequest FileDirRequest(DirectoryInfoRequest dr)
        {
            return new FileDirRequest()
            {
                FullPath = dr.FullPath,
                RetrieveFiles = false,
                MaxDirectories = dr.MaxDirectories,
                IncludeSubdirectories = dr.IncludeSubdirectories,
                MaxNesting = dr.MaxNesting,
                Exclusions = dr.Exclusions
            };
        }

        [Description("Combines the two requests.")]
        public static FileDirRequest FileDirRequest(FileInfoRequest fr, DirectoryInfoRequest dr)
        {
            return new FileDirRequest()
            {
                // Take the shortest of the paths (closer to root)
                FullPath = fr.FullPath.FullPath().Length < dr.FullPath.FullPath().Length ? fr.FullPath : dr.FullPath, 

                MaxFiles = fr.MaxFiles,

                MaxDirectories = dr.MaxDirectories,

                IncludeSubdirectories = fr.IncludeSubdirectories || dr.IncludeSubdirectories,
                
                // Take the min of the nesting maximums
                MaxNesting = Math.Min(fr.MaxNesting, dr.MaxNesting),

                Exclusions = fr.Exclusions.Concat(dr.Exclusions).ToList()
            };
        }

        public static FileDirRequest FileDirRequest(FileDirRequest fr)
        {
            return fr;
        }

        /*******************************************/
    }
}

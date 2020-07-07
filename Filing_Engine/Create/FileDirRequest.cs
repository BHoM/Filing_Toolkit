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

namespace BH.Engine.Filing
{
    public static partial class Create
    {
        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        public static FileAndDirRequest FileDirRequest(FileRequest fr)
        {
            return new FileAndDirRequest()
            {
                Directory = fr.Directory,
                RetrieveDirectories = false,
                MaxFiles = fr.MaxFiles,
                IncludeSubdirectories = fr.IncludeSubdirectories,
                MaxNesting = fr.MaxNesting,
                Exclusions = fr.Exclusions
            };
        }

        public static FileAndDirRequest FileDirRequest(DirectoryRequest dr)
        {
            return new FileAndDirRequest()
            {
                Directory = dr.Directory,
                RetrieveDirectories = false,
                MaxFiles = dr.MaxDirectories,
                IncludeSubdirectories = dr.IncludeSubdirectories,
                MaxNesting = dr.MaxNesting,
                Exclusions = dr.Exclusions
            };
        }

        /*******************************************/
    }
}

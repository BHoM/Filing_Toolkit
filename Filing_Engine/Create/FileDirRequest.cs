using System;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BH.oM.Adapters.Filing;
using System.ComponentModel;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Create
    {
        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        public static FileDirRequest FileDirRequest(string fullPath, bool includeDirectories = true, bool includeFiles = true, bool searchSubdirectories = true, bool includeFileContents = false)
        {
            return new FileDirRequest()
            {
                Location = fullPath,
                IncludeDirectories = includeDirectories,
                IncludeFiles = includeFiles,
                SearchSubdirectories = searchSubdirectories,
                IncludeFileContents = includeFileContents
            };
        }

        /*******************************************/
    }
}

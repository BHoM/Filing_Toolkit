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
        private void AddAuthor(oM.Adapters.Filing.IFSContainer retrievedFile)
        {
            string fullPath = retrievedFile.IFullPath();

            // Retrieve additional data: author/owner
            if ((retrievedFile.Attributes & FileAttributes.System) > 0)
                retrievedFile.Owner = "System";
            else
            {
                try
                {
                    retrievedFile.Owner = System.IO.File.GetAccessControl(fullPath)
                                            .GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
                }
                catch
                {
                    BH.Engine.Reflection.Compute.RecordNote($"Cannot retrieve Owner of {retrievedFile.GetType().Name} `{fullPath}`");
                }
            }
        }

        private void AddContent(oM.Adapters.Filing.FSFile retrievedFile)
        {
            string fullPath = retrievedFile.IFullPath();

            var content = ReadContent(fullPath);
            retrievedFile.Content.AddRange(content);
        }

        private void AddContent(oM.Adapters.Filing.FSDirectory retrievedDir)
        {
            string fullPath = retrievedDir.IFullPath();

            var content = new DirectoryInfo(fullPath).GetFiles("*.*");

            retrievedDir.Content.AddRange(content.Cast<IFSInfo>());
        }

    }
}

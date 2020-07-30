using BH.oM.Base;
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
        private void AddAuthor(oM.Filing.IContent retrievedFile)
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
                    BH.Engine.Reflection.Compute.RecordWarning($"Cannot retrieve Author/Owner for: `{fullPath}`");
                }
            }
        }

        private void AddContent(oM.Filing.File retrievedFile)
        {
            string fullPath = retrievedFile.IFullPath();

            var content = ReadContent(fullPath);
            retrievedFile.Content.AddRange(content);
        }

        private void AddContent(oM.Filing.Directory retrievedDir)
        {
            string fullPath = retrievedDir.IFullPath();

            var content = new DirectoryInfo(fullPath).GetFiles("*.*");

            retrievedDir.Content.AddRange(content.Cast<Info>());
        }

    }
}

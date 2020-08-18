using BH.oM.Base;
using MongoDB.Bson.Serialization;
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
        protected oM.Adapters.Filing.File ReadFile(FileRequest fr, PullConfig pc)
        {
            string fullPath = fr.Location.IFullPath();

            return ReadFile(fullPath, fr.IncludeFileContents, pc.IncludeHiddenFiles, pc.IncludeSystemFiles);
        }

        private oM.Adapters.Filing.File ReadFile(string fullPath, bool inclFileContent = false, bool inclHidFiles = false, bool inclSysFiles = false)
        {
            // Perform the "Read" = get the System.FileInfo, which will be the basis for our oM.Adapters.Filing.File
            FileInfo fi = new FileInfo(fullPath);

            // Checks on config
            if (!inclHidFiles && (fi.Attributes & FileAttributes.Hidden) > 0)
                return null;

            if (!inclSysFiles && (fi.Attributes & FileAttributes.System) > 0)
                return null;

            // Checks on FileInfo
            if ((fi.Attributes & FileAttributes.Directory) <= 0 && !fi.Exists)
                return null;

            // Convert the FileInfo to our oM.Adapters.Filing.File
            oM.Adapters.Filing.File file = fi.ToFiling();

            // Add author data if possible
            AddAuthor(file);

            // Add content data if requested and possible
            if (inclFileContent)
                AddContent(file);

            return file;
        }

        private oM.Adapters.Filing.Directory ReadDirectory(string fullPath, bool inclHidDirs = false, bool inclSysDirs = false, bool includeFolderContent = false)
        {
            // Perform the "Read" = get the System.DirectoryInfo, which will be the basis for our oM.Adapters.Filing.Directory
            DirectoryInfo di = new DirectoryInfo(fullPath);

            // Checks on config
            if (!inclHidDirs && (di.Attributes & FileAttributes.Hidden) > 0)
                return null;

            if (!inclSysDirs && (di.Attributes & FileAttributes.System) > 0)
                return null;

            // Checks on FileInfo
            if ((di.Attributes & FileAttributes.Directory) <= 0 && !di.Exists)
                return null;

            // Convert the FileInfo to our oM.Adapters.Filing.File
            oM.Adapters.Filing.Directory dir = di.ToFiling();

            // Add author data if possible
            AddAuthor(dir);

            if (includeFolderContent)
                AddContent(dir);

            return dir;
        }

    }

}

using BH.oM.Base;
using MongoDB.Bson.Serialization;
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
        protected oM.Filing.File ReadFile(FileRequest fr, PullConfig pc)
        {
            string fullPath = fr.ParentDirectory.IFullPath();

            return ReadFile(fullPath, fr.IncludeFileContents, pc.IncludeHiddenFiles, pc.IncludeSystemFiles);
        }

        private oM.Filing.File ReadFile(string fullPath, bool inclFileContent = false, bool inclHidFiles = false, bool inclSysFiles = false)
        {
            // Perform the "Read" = get the System.FileInfo, which will be the basis for our om.Filing.File
            FileInfo fi = new FileInfo(fullPath);

            // Checks on config
            if (!inclHidFiles && (fi.Attributes & FileAttributes.Hidden) > 0)
                return null;

            if (!inclSysFiles && (fi.Attributes & FileAttributes.System) > 0)
                return null;

            // Checks on FileInfo
            if ((fi.Attributes & FileAttributes.Directory) <= 0 && !fi.Exists)
                return null;

            // Convert the FileInfo to our om.Filing.File
            oM.Filing.File file = (oM.Filing.File)fi;

            // Add author data if possible
            AddAuthor(file);

            // Add content data if requested and possible
            if (inclFileContent)
                AddContent(file);

            return file;
        }

        private oM.Filing.Directory ReadDirectory(string fullPath, bool inclHidDirs = false, bool inclSysDirs = false, bool includeFolderContent = false)
        {
            // Perform the "Read" = get the System.DirectoryInfo, which will be the basis for our om.Filing.Directory
            DirectoryInfo di = new DirectoryInfo(fullPath);

            // Checks on config
            if (!inclHidDirs && (di.Attributes & FileAttributes.Hidden) > 0)
                return null;

            if (!inclSysDirs && (di.Attributes & FileAttributes.System) > 0)
                return null;

            // Checks on FileInfo
            if ((di.Attributes & FileAttributes.Directory) <= 0 && !di.Exists)
                return null;

            // Convert the FileInfo to our om.Filing.File
            oM.Filing.Directory dir = (oM.Filing.Directory)di;

            // Add author data if possible
            AddAuthor(dir);

            if (includeFolderContent)
                AddContent(dir);

            return dir;
        }

    }

}

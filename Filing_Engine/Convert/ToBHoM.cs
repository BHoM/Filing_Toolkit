using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Filing;

namespace BH.Engine.Filing
{
    public static partial class Convert 
    {

        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        public static IFile ToBHoM(this FileSystemInfoBase file)
        {
            IFile bhomObj = GetObject(file as dynamic);
            bhomObj.Name = file.Name;
            Directory lastdir = null;

            var parts = file.FullName.Split(file.FileSystem.Path.PathSeparator).ToList();
            parts.RemoveAt(parts.Count - 1);
            foreach(string dir in parts)
            {
                var newdir = new Directory { Name = dir, ParentDirectory = lastdir };
                lastdir = newdir;
            }

            bhomObj.ParentDirectory = lastdir;

            if (file.Exists)
            {
                bhomObj.Created = file.CreationTimeUtc;
                bhomObj.Modified = file.LastWriteTimeUtc;
                bhomObj.Accessed = file.LastAccessTimeUtc;
            }
            return bhomObj;
        }

        /*******************************************/

        public static IFile ToBHoM(this System.IO.FileInfo file)
        {
            return ((FileInfoBase)file).ToBHoM();
        }
        
        /*******************************************/

        public static IFile ToBHoM(this System.IO.DirectoryInfo dir)
        {
            return ((DirectoryInfoBase)dir).ToBHoM();
        }

        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static File GetObject(FileSystemInfoBase file)
        {
            return new File();
        }

        /*******************************************/

        private static Directory GetObject(DirectoryInfoBase dir)
        {
            return new Directory();
        }

        /*******************************************/
    }
}
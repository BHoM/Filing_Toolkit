using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.Filing
{
    public static partial class Convert 
    {

        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        [Description("Convert a System FileInfo object to a BHoM file.")]
        [Input("file", "The FileInfo object.")]
        [Output("A BHoM File.")]
        public static IFile ToFile(this FileSystemInfoBase file)
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

        [Description("Convert a System FileInfo object to a BHoM file.")]
        [Input("file", "The FileInfo object.")]
        [Output("A BHoM File.")]
        public static IFile ToFile(this System.IO.FileInfo file)
        {
            return ((FileInfoBase)file).ToFile();
        }

        /*******************************************/

        [Description("Convert a System DirectoryInfo object to a BHoM directory.")]
        [Input("dir", "The DirectoryInfo object.")]
        [Output("A BHoM directory.")]
        public static IFile ToFile(this System.IO.DirectoryInfo dir)
        {
            return ((DirectoryInfoBase)dir).ToFile();
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
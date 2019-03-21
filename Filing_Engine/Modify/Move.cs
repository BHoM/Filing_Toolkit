using BH.oM.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Methods                                   ****/
        /***************************************************/

        public static IFile Move(this IFile file, Directory to)
        {
            if (file.ParentDirectory != null)
            {
                file.ParentDirectory.IRemove(file);
            }
            file.ParentDirectory = to;
            to.IAdd(file);
            return file;
        }
        
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Directory IRemove(this Directory dir, IFile f)
        {
            return Remove(dir, f as dynamic);
        }

        /***************************************************/

        private static Directory Remove(this Directory dir, File f)
        {
            if (dir.Files != null)
                dir.Files.Remove(f);
            return dir;
        }

        /***************************************************/

        private static Directory Remove(this Directory dir, Directory d)
        {
            if (dir.SubDirectories != null)
                dir.SubDirectories.Remove(d);
            return dir;
        }

        /***************************************************/

        private static Directory IAdd(this Directory dir, IFile f)
        {
            return Add(dir, f as dynamic);
        }

        /***************************************************/

        private static Directory Add(this Directory dir, File f)
        {
            if (dir.Files == null)
                dir.Files = new List<File>();

            dir.Files.Add(f);
            return dir;
        }

        /***************************************************/

        private static Directory Add(this Directory dir, Directory d)
        {
            if (dir.SubDirectories == null)
                dir.SubDirectories = new List<Directory>();

            dir.SubDirectories.Add(d);
            return dir;
        }

        /***************************************************/
    }
}

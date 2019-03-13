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

        public static File ToBHoM(this FileSystemInfoBase file)
        {
            File bhomObj = GetObject(file as dynamic);
            bhomObj.Name = file.Name;
            bhomObj.Path = file.FullName;
            if (file.Exists)
            {
                bhomObj.Created = file.CreationTimeUtc;
            }
            return bhomObj;
        }

        /*******************************************/

        public static File ToBHoM(this System.IO.FileInfo file)
        {
            return ((FileInfoBase)file).ToBHoM();
        }
        
        /*******************************************/

        public static File ToBHoM(this System.IO.DirectoryInfo dir)
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

        private static File GetObject(DirectoryInfoBase dir)
        {
            return new Directory();
        }

        /*******************************************/
    }
}
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Convert 
    {
        private static oM.Filing.File GetObject(FileSystemInfoBase file)
        {
            return new oM.Filing.File();
        }

        private static oM.Filing.File GetObject(DirectoryInfoBase dir)
        {
            return new oM.Filing.Directory();
        }

        public static oM.Filing.File ToBHoM(this FileSystemInfoBase file)
        {
            var bhomObj = GetObject(file as dynamic);
            bhomObj.Name = file.Name;
            bhomObj.Path = file.FullName;
            if (file.Exists)
            {
                bhomObj.Created = file.CreationTimeUtc;
            }
            return bhomObj;
        }

        public static oM.Filing.File ToBHoM(this FileInfo file)
        {
            return ((FileInfoBase)file).ToBHoM();
        }

        public static oM.Filing.File ToBHoM(this DirectoryInfo dir)
        {
            return ((DirectoryInfoBase)dir).ToBHoM();
        }
    }
}
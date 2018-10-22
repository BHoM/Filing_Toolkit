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
    class DirectoryTraverser
    {
        private int _maxdepth;
        private DirectoryInfoBase _dir;

        public DirectoryTraverser(DirectoryInfoBase dir, int maxdepth = 10)
        {
            _maxdepth = maxdepth;
            _dir = dir;
        }

        public IEnumerable<oM.Filing.File> getContents()
        {
            if(_maxdepth > 0 && _dir.Exists)
            {
                foreach(var f in _dir.GetFileSystemInfos())
                {
                    var file = f.ToBHoM();
                    if (typeof(oM.Filing.Directory).IsAssignableFrom(file.GetType()))
                    {
                        var dir = (oM.Filing.Directory)file;
                        var traverser = new DirectoryTraverser((DirectoryInfoBase)f, _maxdepth - 1);
                        dir.Contents = new List<oM.Filing.File>(
                            traverser.getContents());
                    }
                    yield return file;
                }
            }
            yield break;
        }
    }
    public static partial class Convert 
    {
        private static oM.Filing.File GetObject(FileSystemInfoBase file)
        {
            return new oM.Filing.File();
        }

        private static oM.Filing.File GetObject(DirectoryInfoBase dir)
        {
            var out_ = new oM.Filing.Directory();
            out_.Contents = new List<oM.Filing.File>(
                new DirectoryTraverser(dir).getContents());
            return out_;
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
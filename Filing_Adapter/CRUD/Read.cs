using BH.Engine.Filing;
using BH.oM.Base;
using BH.oM.Filing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        /***************************************************/
        /**** Private Methods                          *****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids)
        {
            if (type == typeof(Directory))
            {
                return GetDirectories(FileSystem.DirectoryInfo.FromDirectoryName(Path));
            } else if (type == typeof(File))
            {
                return GetFiles(FileSystem.DirectoryInfo.FromDirectoryName(Path));
            }
            return new List<IBHoMObject>();
        }

        /***************************************************/

        private List<Directory> GetDirectories(DirectoryInfoBase directory, int depth = -1)
        {
            List<Directory> directories = new List<Directory>();
            if (depth == 0) return directories;
            foreach (var dir in directory.GetDirectories())
            {
                Directory d = dir.ToBHoM() as Directory;
                d.SubDirectories = GetDirectories(dir, depth - 1);
                d.Files = GetFiles(dir, depth - 1);

                directories.Add(d);
                directories.AddRange(d.SubDirectories);

            }
            return directories;
        }

        /***************************************************/

        private List<File> GetFiles(DirectoryInfoBase directory, int depth = -1, bool readFiles = false)
        {

            List<File> files = new List<File>();
            if (depth == 0) return files;

            files.AddRange(
                directory.GetFiles().Select(f =>
                {
                    File bhomFile = f.ToBHoM() as File;
                    if (readFiles)
                    {
                        try
                        {
                            using (var stream = f.OpenRead())
                            {
                                byte[] buff = new byte[stream.Length];
                                int read = stream.Read(buff, 0, (int)stream.Length);
                                bhomFile.Contents = buff;
                            }
                        }
                        catch
                        {
                            Engine.Reflection.Compute.RecordWarning("Unable to read file: " + bhomFile.Path);
                        }
                    }
                    return bhomFile;
                })
            );

            foreach (var dir in directory.GetDirectories())
            {
                files.AddRange(GetFiles(dir, depth - 1, readFiles));
            }
            return files;
        }

        /***************************************************/
    }
}

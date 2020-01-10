using BH.Engine.Filing;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Base;
using BH.oM.Data.Requests;
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

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList ids, ActionConfig actionConfig = null)
        {
            return Read(type, ids, actionConfig as dynamic);
        }
        protected IEnumerable<IBHoMObject> Read(Type type, IList ids, ActionConfig actionConfig = null)
        {
            if (type == typeof(Directory))
            {
                return GetDirectories(FileSystem.DirectoryInfo.FromDirectoryName(Path));
            } else if (type == typeof(File))
            {
                return GetFiles(FileSystem.DirectoryInfo.FromDirectoryName(Path));
            }
            throw new ArgumentException($"{type.ToText()} is not supported", "type");
        }

        protected IEnumerable<IBHoMObject> Read(Type type, IList ids, FilingConfig actionConfig = null)
        {
            int depth = -1;
            bool readFiles = false;
            if (actionConfig != null)
            {
                depth = actionConfig.MaxDepth;
                readFiles = actionConfig.ReadFiles;
            }

            if (type == typeof(oM.Filing.Directory))
            {
                return GetDirectories(FileSystem.DirectoryInfo.FromDirectoryName(Path), depth);
            }
            else if (type == typeof(oM.Filing.File))
            {
                return GetFiles(FileSystem.DirectoryInfo.FromDirectoryName(Path), depth, readFiles);
            }

            return new List<IBHoMObject>();
        }

        /***************************************************/

        private List<Directory> GetDirectories(DirectoryInfoBase directory, int depth = -1, Directory parent = null)
        {
            List<Directory> directories = new List<Directory>();
            if (depth == 0) return directories;
            foreach (var dir in directory.GetDirectories())
            {
                Directory d = dir.ToBHoM() as Directory;
                d.ParentDirectory = parent;

                directories.Add(d);
                directories.AddRange(GetDirectories(dir, depth - 1, d));

            }
            return directories;
        }

        /***************************************************/

        private List<File> GetFiles(DirectoryInfoBase directory, int depth = -1, bool readFiles = false, Directory parent = null)
        {

            List<File> files = new List<File>();
            if (depth == 0) return files;

            files.AddRange(
                directory.GetFiles().Select(f =>
                {
                    File bhomFile = f.ToBHoM() as File;
                    bhomFile.ParentDirectory = parent;
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
                            Engine.Reflection.Compute.RecordWarning("Unable to read file: " + bhomFile.Path());
                        }
                    }
                    return bhomFile;
                })
            );

            foreach (var dir in directory.GetDirectories())
            {
                Directory d = dir.ToBHoM() as Directory;
                d.ParentDirectory = parent;
                files.AddRange(GetFiles(dir, depth - 1, false, d));
            }
            return files;
        }

        /***************************************************/
    }
}

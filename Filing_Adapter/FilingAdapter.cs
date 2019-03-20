using BH.Adapter;
using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public virtual IFileSystem FileSystem { get; protected set; } = new FileSystem();
        public string Path { get; private set; }

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public FilingAdapter(string path)
        {
            Path = path;
        }

        /***************************************************/

        public FilingAdapter(IFileSystem filesystem, string path)
        {
            FileSystem = filesystem;
            Path = path;
        }

        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/
        
        public override IEnumerable<object> Pull(IQuery query, Dictionary<string, object> config = null)
        {
            FilterQuery filter = query as FilterQuery;
            if (filter == null)
                return new List<object>();

            int depth = -1;
            bool readFiles = false;
            if(config != null)
            {
                object _depth;

                if(config.TryGetValue("MaxDepth", out _depth) && _depth != null &&
                    (_depth is int || _depth is double || _depth is float))
                {
                    depth = (int)_depth;
                }
                object _readFiles;
                if (config.TryGetValue("ReadFiles", out _readFiles))
                {
                    readFiles = (bool)_readFiles;
                }
            }

            if(filter.Type == typeof(oM.Filing.Directory))
            {
                return GetDirectories(FileSystem.DirectoryInfo.FromDirectoryName(Path), depth);
            } else if(filter.Type == typeof(oM.Filing.File))
            {
                return GetFiles(FileSystem.DirectoryInfo.FromDirectoryName(Path), depth, readFiles);
            }

            throw new System.ArgumentException($"{query.GetType().ToText()} is not supported", "query");
        }

        /***************************************************/

        public override bool PullTo(BHoMAdapter to, oM.DataManipulation.Queries.IQuery query, Dictionary<string, object> config = null)
        {
            // Force pulling contents when pulling to another adapter
            if (config == null) config = new Dictionary<string, object>();
            config["ReadFiles"] = true;

            return base.PullTo(to, query, config);
        }

        /***************************************************/
    }
}

using BH.Adapter;
using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.Data.Requests;
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
        
        public override IEnumerable<object> Pull(IRequest query, Dictionary<string, object> config = null)
        {
            FilterRequest filter = query as FilterRequest;
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

        public override bool Move(BHoMAdapter to, IRequest query, Dictionary<string, object> pullConfig = null, Dictionary<string, object> pushConfig = null)
        {
            // Force pulling contents when pulling to another adapter
            if (pullConfig == null) pullConfig = new Dictionary<string, object>();
            pullConfig["ReadFiles"] = true;

            return base.Move(to, query, pullConfig, pushConfig);
        }

        /***************************************************/
    }
}

using BH.Engine.Reflection;
using BH.oM.Data.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
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
    }
}

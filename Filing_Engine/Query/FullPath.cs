using BH.oM.Base;
using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Query
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Get the full path.")]
        public static string IFullPath(this object obj)
        {
            return obj != null ? FullPath(obj as dynamic) ?? "" : "";
        }

        private static string FullPath(this IInfo baseInfo)
        {
            if (baseInfo.ParentDirectory == null)
                return baseInfo.Name;

            if (!IsAcyclic(baseInfo))
                BH.Engine.Reflection.Compute.RecordError("Circular directory hierarchy found.");

            return Path.Combine(baseInfo.ParentDirectory.IFullPath(), baseInfo.Name);
        }

        private static string FullPath(this IFilingRequest fdr)
        {
            return FullPath(fdr.Location);
        }

        private static string FullPath(this string path)
        {
            string fullpath = null;

            try
            {
                fullpath = new FileInfo(path).FullName;
            }
            catch
            {
                BH.Engine.Reflection.Compute.RecordError($"Invalid path provided:\n{path}");
            }
            return fullpath;
        }

        //Fallback
        private static string FullPath(object fileOrDir)
        {
            return null;
        }

        /***************************************************/


    }
}

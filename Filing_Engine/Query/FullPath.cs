using BH.oM.Base;
using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Query
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Get the full path.")]
        public static string IFullPath(this IObject iObject)
        {
            return iObject != null ? FullPath(iObject as dynamic) : null;
        }

        private static string FullPath(this IContent fileOrDir)
        {
            if (!IsAcyclic(fileOrDir)) throw new ArgumentException("Circular directory hierarchy");

            return Path.Combine(fileOrDir.ParentDirectory.FullPath(), fileOrDir.Name);
        }

        private static string FullPath(this FileDirRequest fdr)
        {
            return FullPath(fdr.ParentDirectory);
        }

        private static string FullPath(this IInfo baseInfo)
        {
            if (baseInfo.ParentDirectory == null)
                return baseInfo.Name;

            return Path.Combine(baseInfo.ParentDirectory.FullPath(), baseInfo.Name);
        }

        //Fallback
        private static string FullPath(object fileOrDir)
        {
            return null;
        }

        /***************************************************/


    }
}

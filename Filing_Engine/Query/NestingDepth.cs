using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Description("Get the nesting depth of the input File or Directory, which is the total number of parent folders.")]
        public static int NestingDepth(oM.Adapters.Filing.IFSInfo fileOrDir)
        {
            int count = 0;
            while (fileOrDir.ParentDirectory != null)
            {
                fileOrDir = (FSDirectory)fileOrDir.ParentDirectory;
                count++;
            }
            return count;
        }

        /***************************************************/
    }
}

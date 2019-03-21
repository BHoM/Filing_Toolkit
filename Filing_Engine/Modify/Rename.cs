using BH.oM.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        public static IFile Rename(this IFile file, string name)
        {
            file.Name = name;
            return file;
        }

        /***************************************************/
    }
}

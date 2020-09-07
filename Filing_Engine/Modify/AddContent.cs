using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;
using BH.Engine.Reflection;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Methods                                   ****/
        /***************************************************/

        [Description("Add the input content to the specified oM.Adapters.Filing.File.")]
        public static FSFile AddContent(this FSFile file, List<object> content)
        {
            FSFile cloned = file.DeepClone();
            cloned.Content = content;

            return cloned;
        }
    }
}

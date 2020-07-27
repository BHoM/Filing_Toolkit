using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;
using BH.Engine.Reflection;

namespace BH.Engine.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Methods                                   ****/
        /***************************************************/

        [Description("Add the input content to the specified oM.Filing.File.")]
        public static File AddContent(this File file, List<object> content)
        {
            File cloned = file.DeepClone();
            cloned.Content = content;

            return cloned;
        }
    }
}

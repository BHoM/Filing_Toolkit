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

namespace BH.Engine.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Methods                                   ****/
        /***************************************************/

        [Description("Remove content from a file.")]
        public static File RemoveContent(this File file)
        {
            File cloned = file.DeepClone();
            cloned.Content = new List<object>();

            return cloned;
        }
        
        /***************************************************/
    }
}

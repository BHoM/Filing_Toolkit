using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class FilingConfig : ActionConfig
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public int MaxDepth { get; set; } = -1;

        public bool ReadFiles { get; set; }

        /***************************************************/
    }
}

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

        public virtual int MaxDepth { get; set; } = -1;

        public virtual bool ReadFiles { get; set; }

        /***************************************************/
    }
}

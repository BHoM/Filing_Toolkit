using BH.oM.Base;
using BH.oM.Humans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class Directory : BHoMObject, IFile

    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Accessed { get; set; }

        public Human Owner { get; set; }

        public Directory ParentDirectory { get; set; }

        /***************************************************/
    }
}

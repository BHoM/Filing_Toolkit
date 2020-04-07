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

        public virtual DateTime Created { get; set; }

        public virtual DateTime Modified { get; set; }

        public virtual DateTime Accessed { get; set; }

        public virtual Human Owner { get; set; }

        public virtual Directory ParentDirectory { get; set; }

        /***************************************************/
    }
}

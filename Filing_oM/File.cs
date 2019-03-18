using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class File : BHoMObject, IFile
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public string Path { get; set; }

        /***************************************************/

        public DateTime Created { get; set; }

        /***************************************************/
            
        public DateTime Modified { get; set; }

        /***************************************************/

        public DateTime Accessed { get; set; }

        /***************************************************/

        public byte[] Contents { get; set; }

        /***************************************************/
    }
}

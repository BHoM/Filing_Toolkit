using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public interface IFile : IBHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        string Path { get; set; }

        /***************************************************/

        DateTime Created { get; set; }

        /***************************************************/
            
        DateTime Modified { get; set; }

        /***************************************************/

        DateTime Accessed { get; set; }

        /***************************************************/
    }
}

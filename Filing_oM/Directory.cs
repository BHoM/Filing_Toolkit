using BH.oM.Base;
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

        public IEnumerable<Directory> SubDirectories { get; set; }

        /***************************************************/

        public IEnumerable<File> Files { get; set; }

        /***************************************************/

        public string Path { get; set; }

        /***************************************************/

        public DateTime Created { get; set; }

        /***************************************************/

        public DateTime Modified { get; set; }

        /***************************************************/

        public DateTime Accessed { get; set; }

        /***************************************************/
    }
}

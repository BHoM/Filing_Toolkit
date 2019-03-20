using BH.oM.Filing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Create
    {
        /*******************************************/
        /**** Methods                           ****/
        /*******************************************/

        public static File File(string name , Directory directory = null, string contents = "", Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            return File(
                name, 
                directory,
                encoding.GetBytes(contents)
            );
        }

        /*******************************************/

        public static File File(string name , Directory directory = null, byte[] contents = null)
        {
            return new File()
            {
                Name = name, 
                ParentDirectory = directory,
                Contents = contents
            };
        }

        /*******************************************/
    }
}

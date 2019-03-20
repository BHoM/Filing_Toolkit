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

        public static Directory Directory(string name , Directory parent = null)
        {
            return new Directory {
                Name = name,
                ParentDirectory = parent
            };
        }

        /*******************************************/

        public static Directory Directory(string[] pathParts)
        {
            Directory last = null;
            foreach(string name in pathParts)
            {
                Directory dir = Directory(name, last);
                last = dir;
            }
            return last;
        }

        /*******************************************/

        public static Directory Directory(string path, string separator = "/")
        {
            return Directory(path.Split(new string[] { separator },StringSplitOptions.RemoveEmptyEntries).ToArray());
        }

        /*******************************************/
    }
}

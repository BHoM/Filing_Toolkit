using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
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

        public static IFileSystem FileSystem()
        {
            return new FileSystem();
        }

        /*******************************************/

        public static IFileSystem FileSystem(List<string> paths)
        {
            return new MockFileSystem(paths.ToDictionary(p => p, p => new MockFileData("")));
        }
        
        /*******************************************/

        public static IFileSystem FileSystem(Dictionary<string, MockFileData> data)
        {
            return new MockFileSystem(data);
        }

        /*******************************************/
    }
}

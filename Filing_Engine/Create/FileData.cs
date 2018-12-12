using System;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BH.Engine.Filing
{
    public static partial class Create
    {
        public static MockFileData FileData(string data = "",
            DateTime created = new DateTime(),
            DateTime modified = new DateTime(),
            DateTime accessed = new DateTime(),
            FileSecurity access = null,
            FileAttributes attributes = FileAttributes.Normal)
        {
            var fileData = new MockFileData(data)
            {
                CreationTime = created,
                LastWriteTime = modified,
                LastAccessTime = accessed,
                Attributes = attributes
            };
            if(access != null) fileData.AccessControl = access;
            return fileData;
        }
    }
}

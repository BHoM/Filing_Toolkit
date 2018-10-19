using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Convert 
    {
        public static IBHoMObject ToBHoM(this FileInfo file)
        {
            return new BHoMObject() { Name = file.Name };
        }

        public static IBHoMObject ToBHoM(this DirectoryInfo dir)
        {
            return new BHoMObject() { Name = dir.Name };
        }

    }
}

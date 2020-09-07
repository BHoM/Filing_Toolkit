﻿using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Convert
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Converts the provided File into a System.IO.FileInfo." +
            "\nAny `Content` property is lost in this conversion.")]
        public static FileInfo FromFiling(this oM.Adapters.Filing.FSFile file)
        {
            return new FileInfo(file.IFullPath());
        }

        /***************************************************/
    }
}

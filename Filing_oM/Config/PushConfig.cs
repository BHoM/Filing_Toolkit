﻿using BH.oM.Adapter;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Filing
{
    public class PushConfig : ActionConfig
    {
        [Description("When updating a File, set whether the input content should be appended to the existing or overwritten." +
            "\nBy default is `false` (= the content of the File is entirely overwritten).")]
        public bool AppendContent { get; set; } = false;

        [Description("Keeps the warnings about Deletion off.")]
        public bool DisableWarnings { get; set; } = false;
    }
}
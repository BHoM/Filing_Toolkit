using BH.oM.Adapter;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.Filing
{
    public class PushConfig : ActionConfig
    {
        [Description("Default filePath used for pushing objects that are not BHoM `File` or `Directory`.")]
        public string DefaultFilePath = "C:/temp/Filing_Adapter-objects.json";

        [Description("When updating a File, set whether the input content should be appended to the existing or overwritten." +
            "\nBy default is `false` (= the content of the File is entirely overwritten).")]
        public bool AppendContent { get; set; } = false;

        [Description("Keeps the warnings about Deletion off.")]
        public bool DisableWarnings { get; set; } = false;
    }
}

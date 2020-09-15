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
        public string DefaultFilePath { get; set; } = "C:/temp/Filing_Adapter-objects.json";

        [Description("When updating a File, set whether the input content should be appended to the existing or overwritten." +
            "\nBy default is `false` (= the content of the File is entirely overwritten).")]
        public bool AppendContent { get; set; } = false;

        [Description("When serialising to JSON, use the Dataset serialization style." +
            "\nThis serializes the individual objects, and then concatenates the strings separating with a newline." +
            "\nThe obtained format is not valid JSON. You will need to deserialize each individual line." +
            "\nThis is the current standard for Datasets.")]
        public bool UseDatasetSerialization { get; set; } = false;

        [Description("Keeps the warnings about Deletion off.")]
        public bool DisableWarnings { get; set; } = false;
    }
}

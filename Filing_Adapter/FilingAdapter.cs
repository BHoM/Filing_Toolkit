using BH.Adapter;
using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.Reflection.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {  
        public FilingAdapter()
        {
            // By default, if they exist already, the files to be created are wiped out and then re-created.
            this.m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.UpdateOrCreateOnly;
        }

        private bool m_Push_enableDeleteWarning = true;
        private bool m_Remove_enableDeleteWarning = true;
        private bool m_Remove_enableDeleteAlert = true;
        private bool m_Execute_enableWarning = true;
        private string m_defaultFilePath = null;
    }
}

using BH.Adapter;
using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.Data.Requests;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        public FilingAdapter()
        {
            // By default, if they exist already, the files to be created are wiped out and then re-created.
            this.m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.DeleteThenCreate;
        }

        private bool m_enableDeleteWarning = true;
    }
}

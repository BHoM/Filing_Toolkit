using System;
using System.Collections.Generic;
using BH.Adapter.Filing;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filing_Test
{
    [TestClass]
    public class ReadTest
    {
        private FilingAdapter adapter;

        [TestInitialize]
        public void SetUp()
        {
            adapter = new FilingAdapter();
        }

        [TestMethod]
        public void TestReturnObjects()
        {
            IEnumerable<object> objs = adapter.Pull(new FilterQuery() { Type = typeof(BHoMObject) });
            foreach (var obj in objs)
            {
                Assert.IsInstanceOfType(obj, typeof(BHoMObject));
            }
        }
    }
}

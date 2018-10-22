using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using BH.Adapter.Filing;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using BH.oM.Filing;
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
            var fs = new MockFileSystem( new Dictionary<string, MockFileData>()
            {
                { @"C:\test\test.txt", new MockFileData("Some text") },
                { @"C:\test\something else.txt", new MockFileData("Some other text") }

            });
            adapter = new FakeFilingAdapter(fs, @"C:\test");
        }

        [TestMethod]
        public void TestReturnObjects()
        {
            List<object> objs = new List<object>(
                adapter.Pull(new FilterQuery() { Type = typeof(Directory) }));
            CollectionAssert.AllItemsAreInstancesOfType(objs, typeof(File));
            Assert.AreEqual(1, objs.Count);
        }
    }
}

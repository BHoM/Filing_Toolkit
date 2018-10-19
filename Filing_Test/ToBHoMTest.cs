using System;
using System.IO;
using BH.Engine.Filing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filing_Test
{
    [TestClass]
    public class ToBHoMTest
    {
        [TestMethod]
        [DataRow("somefile.txt")]
        [DataRow("foo.exe")]
        public void TestGetFileName(string filename)
        {
            var file = new FileInfo(filename);
            var bhomfile = file.ToBHoM();
            Assert.AreEqual(filename, bhomfile.Name);
        }
    }
}

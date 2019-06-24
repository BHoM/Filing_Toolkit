using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using BH.Engine.Filing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Filing_Test
{
    [TestClass]
    public class ToBHoMTest
    {
        private MockFileSystem fs;

        [TestInitialize]
        public void SetUp()
        {
            fs = new MockFileSystem();
        }

        [TestMethod]
        [DataRow("somefile.txt", "")]
        [DataRow("foo.exe", @"file\relative\path\")]
        [DataRow("bar.zip", @"N:\path\to\")]
        public void TestGetFileName(string filename, string prefix)
        {
            var file = new MockFileInfo(fs, prefix + filename);
            var bhomfile = file.ToBHoM() as BH.oM.Base.BHoMObject;
            Assert.IsInstanceOfType(bhomfile, typeof(BH.oM.Filing.File));
            Assert.AreEqual(filename, bhomfile.Name);
        }

        [TestMethod]
        [DataRow("somedir", "")]
        [DataRow("someotherdir", @"a\relative\path\")]
        [DataRow("nested", @"K:\a\long\path\to\a\")]
        public void TestGetDirName(string dirname, string prefix)
        {
            var dir = new MockDirectoryInfo(fs, prefix + dirname);
            var bhomdir = dir.ToBHoM() as BH.oM.Base.BHoMObject;
            Assert.IsInstanceOfType(bhomdir, typeof(BH.oM.Filing.Directory));
            Assert.AreEqual(dirname, bhomdir.Name);
        }

        [TestMethod]
        [DataRow("somefile.txt")]
        [DataRow(@"path\to\foo.exe")]
        [DataRow(@"N:\another\path\to\bar.zip")]
        public void TestGetFilePath(string path)
        {
            var file = new MockFileInfo(fs, path);
            var bhomfile = file.ToBHoM();
            Assert.IsInstanceOfType(bhomfile, typeof(BH.oM.Filing.File));
            Assert.AreEqual(path, bhomfile.Path());
        }

        [TestMethod]
        [DataRow("justaword")]
        [DataRow(@"Z:\absolute\path")]
        [DataRow(@"relative\path")]
        public void TestGetDirPath(string path)
        {
            var tmp = fs.Path.GetTempPath();
            var dir = new MockDirectoryInfo(fs, path);
            var bhomdir = dir.ToBHoM();
            Assert.IsInstanceOfType(bhomdir, typeof(BH.oM.Filing.Directory));
            // use endswith because of relative paths
            Assert.IsTrue(bhomdir.Path().EndsWith(path),
                $"Expected path to end with {path} but got {bhomdir.Path()}");
        }

        [TestMethod]
        [DataRow("1991-11-21 03:52")]
        [DataRow("1997-03-19 09:31")]
        [DataRow("2004-07-04 15:12")]
        [DataRow("2016-03-17 11:41")]
        [DataRow("2049-05-16 22:00")]
        public void TestPopulateCreationTime(string t)
        {
            var date = DateTime.Parse(t);
            var data = new MockFileData("Some data")
            {
                CreationTime = date
            };

            var path = @"C:\test.txt";
            fs.AddFile(path, data);

            var file = fs.FileInfo.FromFileName(path);
            var bhomfile = file.ToBHoM();

            Assert.AreEqual(date.ToUniversalTime(), bhomfile.Created);
        }

        [TestMethod]
        public void TestNoPopulateCreationTimeIfNotExists()
        {
            var path = @"C:\test.txt";
            var file = fs.FileInfo.FromFileName(path);
            var bhomfile = file.ToBHoM();

            Assert.AreEqual(new DateTime(), bhomfile.Created);
        }
    }
}

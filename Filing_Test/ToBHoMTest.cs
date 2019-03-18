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
        private IFileInfoFactory factory;

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
            var bhomfile = file.ToBHoM();
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
            var bhomdir = dir.ToBHoM();
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
            Assert.AreEqual(path, bhomfile.Path);
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
            Assert.IsTrue(bhomdir.Path.EndsWith(path),
                $"Expected path to end with {path} but got {bhomdir.Path}");
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

        [TestMethod]
        public void TestTraverseTree()
        {
            var filesystem = new MockFileSystem(
                new Dictionary<string, MockFileData>() {
                    { @"C:\hbz\jrkw.docx", new MockFileData("") },
                    { @"C:\hwf.avi", new MockFileData("") },
                    { @"C:\rxj.mp3", new MockFileData("") },
                    { @"C:\ano\nqj.pptx", new MockFileData("") },
                    { @"C:\ano\axeg.mp3", new MockFileData("") },
                    { @"C:\ano\jhsxc.jpg", new MockFileData("") },
                    { @"C:\ano\uzz.wmv", new MockFileData("") },
                    { @"C:\ano\crz.wmv", new MockFileData("") },
                    { @"C:\ano\ywhy.pptx", new MockFileData("") },
                    { @"C:\ano\jym\iyldt.mp3", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\fkghw.docx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\zexyz.txt", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\yjrb.xlsx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\ubp.avi", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\xwapd.txt", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\aqfmy\cgteg.zip", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\aqfmy\xebkx.txt", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\aqfmy\ibq\cphw.cs", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\aqfmy\ibq\gzpiu.jpg", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\aqfmy\ibq\irzwe.wav", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\aqfmy\njk.msi", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\xoyqo.exe", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\gljx.jpg", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\xbebh.txt", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wnk.xlsx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\ksrjy\uzy.xlsx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\qqssg.jpg", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\abx.docx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\ltgwz.msi", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\ezqz\gfky.avi", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\ezqz\syj.wav", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\lcmxr.xlsx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\brniu.xlsx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\ngkxl\zydup.exe", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\vic.xlsx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\vaua.exe", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\icuo.png", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\ylz.msi", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\osuo\jlgb.wmv", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\vgwg.txt", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\iramq.msi", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\apftt.xlsx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\crft.txt", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\uxcq.docx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\vixp\qeslp.jpg", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\vixp\fymfj.pptx", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\oev.cs", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\hszs\lkb.exe", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\hszs\dsj.cs", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\wdkhq\hjj.wmv", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\pje\sknwa.wav", new MockFileData("") },
                    { @"C:\ano\jym\sbefs\hayhu.exe", new MockFileData("") },
                    { @"C:\ano\jym\bna.docx", new MockFileData("") },
                    { @"C:\ano\jym\bapyc.jpg", new MockFileData("") },
                    { @"C:\ano\eis.jpg", new MockFileData("") },
                    { @"C:\ano\ayu\xco.wmv", new MockFileData("") },
                    { @"C:\ano\huzdl.avi", new MockFileData("") },
                    { @"C:\ano\voj.png", new MockFileData("") },
                    { @"C:\ano\muo.xlsx", new MockFileData("") },
                    { @"C:\ano\jiex.txt", new MockFileData("") },
                    { @"C:\ano\sfdfe.pptx", new MockFileData("") },
                    { @"C:\cmz.jpg", new MockFileData("") },
                    { @"C:\vsis.wav", new MockFileData("") },
                    { @"C:\iqms.exe", new MockFileData("") },
                    { @"C:\bruxn.pptx", new MockFileData("") },
                    { @"C:\efdwo\kxwbr.txt", new MockFileData("") },
                    { @"C:\efdwo\wrni\oriyp.msi", new MockFileData("") },
                    { @"C:\efdwo\wrni\uvsig.pptx", new MockFileData("") },
                    { @"C:\efdwo\mahw.wav", new MockFileData("") },
                    { @"C:\efdwo\rxvqc.wmv", new MockFileData("") },
                    { @"C:\fnaw.docx", new MockFileData("") },
                    { @"C:\tzv.png", new MockFileData("") },
                    { @"C:\yvsxw.pptx", new MockFileData("") },
                    { @"C:\jqix\axmin.xlsx", new MockFileData("") },
                    { @"C:\jqix\kfs\qlx.msi", new MockFileData("") },
                    { @"C:\jqix\urj.docx", new MockFileData("") },
                    { @"C:\jqix\kykwz\mnybd.wav", new MockFileData("") },
                    { @"C:\jqix\kykwz\hly\nmv.txt", new MockFileData("") },
                    { @"C:\jqix\kykwz\cii.zip", new MockFileData("") },
                    { @"C:\jqix\hamh.exe", new MockFileData("") },
                    { @"C:\jqix\azkvl\azq.cs", new MockFileData("") },
                    { @"C:\jqix\azkvl\nhzfc\pmuwb.mp3", new MockFileData("") },
                    { @"C:\jqix\azkvl\ooqu.mp3", new MockFileData("") },
                    { @"C:\jqix\azkvl\acpv\gax.png", new MockFileData("") },
                    { @"C:\jqix\azkvl\etec.wav", new MockFileData("") },
                    { @"C:\jqix\azkvl\nagx.xlsx", new MockFileData("") },
                    { @"C:\jqix\dvh.wmv", new MockFileData("") },
                    { @"C:\nbzrh.pptx", new MockFileData("") },
                    { @"C:\ogt.xlsx", new MockFileData("") },
                    { @"C:\efjz.avi", new MockFileData("") },
                    { @"C:\qawr.exe", new MockFileData("") },
                    { @"C:\eqqcn\uftnr.png", new MockFileData("") },
                    { @"C:\eqqcn\ogmy.wmv", new MockFileData("") },
                    { @"C:\eqqcn\kqp.jpg", new MockFileData("") },
                    { @"C:\eqqcn\uphjj.zip", new MockFileData("") },
                    { @"C:\eqqcn\kiw.pptx", new MockFileData("") },
                    { @"C:\eqqcn\ypi\xytvv.msi", new MockFileData("") },
                    { @"C:\eqqcn\ypi\qql.msi", new MockFileData("") },
                    { @"C:\eqqcn\ypi\zjwm.xlsx", new MockFileData("") },
                    { @"C:\eqqcn\ypi\lat\sros.xlsx", new MockFileData("") }
                }
            );

            var root = filesystem.DirectoryInfo.FromDirectoryName(@"C:\");
            var bhomobj = (BH.oM.Filing.Directory)root.ToBHoM();
            var list = new List<BH.oM.Filing.File>(bhomobj.Contents);
            Assert.AreEqual(18, list.Count);

            var root2 = filesystem.DirectoryInfo.FromDirectoryName(@"C:\ano\jym\sbefs\pje");
            var bhomobj2 = (BH.oM.Filing.Directory)root2.ToBHoM();
            var list2 = new List<BH.oM.Filing.File>(bhomobj2.Contents);
            Assert.AreEqual(15, list2.Count);
        }
    }
}

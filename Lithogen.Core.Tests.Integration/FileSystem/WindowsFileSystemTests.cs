using Lithogen.Core.FileSystem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core.Tests.Integration.FileSystem
{
    /// <summary>
    /// A few basic tests for this FileSystem since we are going to be using it a lot.
    /// You will need a C:\temp folder to run these tests.
    /// These are really integration tests so they are marked as explicit.
    /// </summary>
    [Explicit]
    public class WindowsFileSystemTests
    {
        const string T_ParentDir = @"C:\temp\wfstestdir";
        const string T_Dir1      = @"C:\temp\wfstestdir\dir1";

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(T_ParentDir))
            {
                Directory.Delete(T_ParentDir, true);
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (Directory.Exists(T_ParentDir))
            {
                Directory.Delete(T_ParentDir, true);
            }
        }

        [Explicit]
        public class DirectoryExists
        {
            [Test]
            //[ExpectedException(typeof(ArgumentNullException))]
            public void ForNullDirectory_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                Assert.That(() => wfs.DirectoryExists(null), Throws.Exception.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("directory"));
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ForEmptyDirectory_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.DirectoryExists("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ForWhitespaceDirectory_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.DirectoryExists(" ");
            }

            [Test]
            public void ForExistingDirectory_ReturnsTrue()
            {
                var wfs = new WindowsFileSystem();
                Directory.CreateDirectory(T_ParentDir);
                Assert.IsTrue(wfs.DirectoryExists(T_ParentDir));
            }

            [Test]
            public void ForNonExistingDirectory_ReturnsFalse()
            {
                var wfs = new WindowsFileSystem();
                Assert.IsFalse(wfs.DirectoryExists(@"C:\somewhere\over\the\rainbow"));
            }
        }

        [Explicit]
        public class CreateDirectory
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ForNullDirectory_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateDirectory(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ForEmptyDirectory_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateDirectory("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ForWhitespaceDirectory_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateDirectory(" ");
            }

            [Test]
            public void WhenDirectoryAlreadyExists_Succeeds()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateDirectory(T_Dir1);
                Assert.IsTrue(Directory.Exists(T_Dir1));
                wfs.CreateDirectory(T_Dir1);
                Assert.IsTrue(Directory.Exists(T_Dir1));
            }
        }

        [Explicit]
        public class CreateParentDirectory
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ForNullDirectory_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateParentDirectory(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ForEmptyDirectory_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateParentDirectory("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void ForWhitespaceDirectory_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateDirectory(" ");
            }

            [Test]
            public void WhenDirectoryAlreadyExists_Succeeds()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateParentDirectory(T_Dir1);
                string parent = Path.GetDirectoryName(T_Dir1);
                Assert.IsTrue(Directory.Exists(parent));
            }
        }

        [Explicit]
        public class WriteAllBytes
        {
            [Test]
            public void TestSomething()
            {
            }
        }
    }
}

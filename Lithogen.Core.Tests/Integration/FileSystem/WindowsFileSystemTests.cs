using Lithogen.Core.FileSystem;
using Lithogen.Core.Tests.Support.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Integration.FileSystem
{
    /// <summary>
    /// A few basic tests for this FileSystem since we are going to be using it a lot.
    /// You will need a C:\temp folder to run these tests.
    /// These are really integration tests so they are marked as explicit.
    /// </summary>
    class WindowsFileSystemTests : FileSystemTestsBase
    {
        [SetUp]
        void Setup()
        {
            if (Directory.Exists(T_ParentDir))
            {
                Directory.Delete(T_ParentDir, true);
            }
        }

        [TestFixtureTearDown]
        void TestFixtureTearDown()
        {
            if (Directory.Exists(T_ParentDir))
            {
                Directory.Delete(T_ParentDir, true);
            }
        }

        class DirectoryExists
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                //Assert.That(() => wfs.DirectoryExists(null), Throws.Exception.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("directory"));
                wfs.DirectoryExists(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.DirectoryExists("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsWhitespace_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.DirectoryExists(" ");
            }

            [Test]
            public void WhenDirectoryAlreadyExists_ReturnsTrue()
            {
                var wfs = new WindowsFileSystem();
                Directory.CreateDirectory(T_ParentDir);
                Assert.IsTrue(wfs.DirectoryExists(T_ParentDir));
            }

            [Test]
            public void WhenDirectoryDoesNotExist_ReturnsFalse()
            {
                var wfs = new WindowsFileSystem();
                Assert.IsFalse(wfs.DirectoryExists(T_DirectoryThatDoesNotExist));
            }
        }

        class CreateDirectory
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateDirectory(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateDirectory("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsWhitespace_ThrowsArgumentOutOfRangeException()
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

        class CreateParentDirectory
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateParentDirectory(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException()
            {
                var wfs = new WindowsFileSystem();
                wfs.CreateParentDirectory("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsWhitespace_ThrowsArgumentOutOfRangeException()
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

        class WriteAllBytes
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenFilenameIsNull_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                wfs.WriteAllBytes(null, T_Bytes);
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenBytesIsNull_ThrowsArgumentNullException()
            {
                var wfs = new WindowsFileSystem();
                wfs.WriteAllBytes(T_File1, null);
            }

            [Test]
            public void WhenBytesIsEmpty_WritesZeroByteFile()
            {
                var wfs = new WindowsFileSystem();
                wfs.WriteAllBytes(T_File1, new byte[] {});
                Assert.True((new FileInfo(T_File1)).Length == 0);
            }
        }
    }
}

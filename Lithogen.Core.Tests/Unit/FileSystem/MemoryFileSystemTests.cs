using Lithogen.Core.FileSystem;
using Lithogen.Core.Tests.Support.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    class MemoryFileSystemTests : FileSystemTestsBase
    {
        class DirectoryExists
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
            {
                var mfs = new MemoryFileSystem();
                mfs.DirectoryExists(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException()
            {
                var mfs = new MemoryFileSystem();
                mfs.DirectoryExists("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsWhitespace_ThrowsArgumentOutOfRangeException()
            {
                var mfs = new MemoryFileSystem();
                mfs.DirectoryExists(" ");
            }

            [Test]
            public void WhenDirectoryAlreadyExists_ReturnsTrue()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateDirectory(T_ParentDir);
                Assert.IsTrue(mfs.DirectoryExists(T_ParentDir));
            }

            [Test]
            public void WhenDirectoryDoesNotExist_ReturnsFalse()
            {
                var mfs = new MemoryFileSystem();
                Assert.IsFalse(mfs.DirectoryExists(T_DirectoryThatDoesNotExist));
            }
        }

        class CreateDirectory
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateDirectory(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateDirectory("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsWhitespace_ThrowsArgumentOutOfRangeException()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateDirectory(" ");
            }

            [Test]
            public void WhenDirectoryAlreadyExists_Succeeds()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateDirectory(T_Dir1);
                Assert.IsTrue(mfs.DirectoryExists(T_Dir1));
                mfs.CreateDirectory(T_Dir1);
                Assert.IsTrue(mfs.DirectoryExists(T_Dir1));
            }
        }

        class CreateParentDirectory
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateParentDirectory(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateParentDirectory("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void WhenDirectoryNameIsWhitespace_ThrowsArgumentOutOfRangeException()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateDirectory(" ");
            }

            [Test]
            public void WhenDirectoryAlreadyExists_Succeeds()
            {
                var mfs = new MemoryFileSystem();
                mfs.CreateParentDirectory(T_Dir1);
                string parent = Path.GetDirectoryName(T_Dir1);
                Assert.IsTrue(mfs.DirectoryExists(parent));
            }
        }

        class WriteAllBytes
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WhenFilenameIsNull_ThrowsArgumentNullException()
            {
                var mfs = new MemoryFileSystem();
                mfs.WriteAllBytes(null, T_Bytes);
            }
        }
    }
}

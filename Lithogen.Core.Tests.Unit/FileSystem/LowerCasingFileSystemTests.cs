using Lithogen.Core.FileSystem;
using Lithogen.Interfaces.FileSystem;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    /// <summary>
    /// These tests only check the lower-casing behaviour. Parameter correctness checks
    /// etc. are presumed to be tested by testing the wrapped file system.
    /// </summary>
    public class LowerCasingFileSystemTests
    {
        public const string T_Directory = @"C:\temp\Lithogen_testdir";
        public const string T_File1 = @"C:\temp\Lithogen_testdir\MixedCaseFile.dat";
        public static readonly byte[] T_Bytes = new byte[] { 1, 2, 3 };

        [SetUp]
        public virtual void Setup()
        {
            // The MemoryFileSystem is case-sensitive and case-preserving.
            // The assertions in here rely on that.
            // We need to have a separate handle to it so that we can check that
            // the LowerCasingFileSystem is passing the right thing on.
            WrappedFS = new MemoryFileSystem();
            LowerCaseFS = new LowerCasingFileSystem(WrappedFS);
        }
        public MemoryFileSystem WrappedFS;
        public LowerCasingFileSystem LowerCaseFS;

        [Test]
        public virtual void Ctor_WhenWrappedFileSystemIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new LowerCasingFileSystem(null));
        }

        [TestCase(T_File1)]
        public virtual void FileExists_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            // It appears the only way we can test this is first to write a file...
            WriteAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(filename);
        }

        [TestCase(T_File1)]
        public virtual void DirectoryExists_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            WriteAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(filename);
            string directory = Path.GetDirectoryName(filename);
            Assert.False(WrappedFS.DirectoryExists(directory));
            Assert.True(WrappedFS.DirectoryExists(directory.ToLowerInvariant()));
            Assert.True(LowerCaseFS.DirectoryExists(directory));
        }

        [TestCase(T_Directory)]
        public virtual void CreateDirectory_WhenDirectoryIsMixed_ForcesToLowerCase(string directory)
        {
            LowerCaseFS.CreateDirectory(directory);
            Assert.False(WrappedFS.DirectoryExists(directory));
            Assert.True(WrappedFS.DirectoryExists(directory.ToLowerInvariant()));
            Assert.True(LowerCaseFS.DirectoryExists(directory));
        }

        [TestCase(T_File1)]
        public virtual void CreateParentDirectory_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            LowerCaseFS.CreateParentDirectory(filename);
            string directory = Path.GetDirectoryName(filename);
            Assert.False(WrappedFS.DirectoryExists(directory));
            Assert.True(WrappedFS.DirectoryExists(directory.ToLowerInvariant()));
            Assert.True(LowerCaseFS.DirectoryExists(directory));
        }

        [TestCase(T_File1)]
        public virtual void WriteAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            LowerCaseFS.WriteAllBytes(filename, T_Bytes);
            Assert.False(WrappedFS.FileExists(filename));
            Assert.True(WrappedFS.FileExists(filename.ToLowerInvariant()));
            Assert.True(LowerCaseFS.FileExists(filename));
        }

        [TestCase(T_File1)]
        public virtual void ReadAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            // Make a file.
            WriteAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(filename);
            var readBack = LowerCaseFS.ReadAllBytes(filename);
            CollectionAssert.AreEqual(T_Bytes, readBack);
        }

        [TestCase(T_File1)]
        public virtual void DeleteFile_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            var mock = new Mock<IFileSystem>();
            var lcFS = new LowerCasingFileSystem(mock.Object);
            mock.Setup(m => m.DeleteFile(It.Is<string>(s => s == filename.ToUpperInvariant())));
            lcFS.DeleteFile(filename);
            mock.Verify();
        }
    }
}

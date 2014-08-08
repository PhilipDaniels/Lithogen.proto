using Lithogen.Core.FileSystem;
using Lithogen.Interfaces.FileSystem;
using NSubstitute;
using NUnit.Framework;
using System;

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
            MockFS = Substitute.For<IFileSystem>();
            LowerCaseFS = new LowerCasingFileSystem(MockFS);
        }
        public IFileSystem MockFS;
        public LowerCasingFileSystem LowerCaseFS;

        [Test]
        public virtual void Ctor_WhenWrappedFileSystemIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new LowerCasingFileSystem(null));
        }

        [TestCase(T_File1)]
        public virtual void FileExists_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.FileExists(filename.ToLowerInvariant()).Returns(true);
            Assert.True(LowerCaseFS.FileExists(filename));
            MockFS.Received().FileExists(filename.ToLowerInvariant());
        }

        [TestCase(T_File1)]
        public virtual void DirectoryExists_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.DirectoryExists(filename.ToLowerInvariant()).Returns(true);
            Assert.True(LowerCaseFS.DirectoryExists(filename));
            MockFS.Received().DirectoryExists(filename.ToLowerInvariant());
        }

        [TestCase(T_Directory)]
        public virtual void CreateDirectory_WhenDirectoryIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.CreateDirectory(directory.ToLowerInvariant());
            LowerCaseFS.CreateDirectory(directory);
            MockFS.Received().CreateDirectory(directory.ToLowerInvariant());
        }

        [TestCase(T_File1)]
        public virtual void CreateParentDirectory_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.CreateParentDirectory(filename.ToLowerInvariant());
            LowerCaseFS.CreateParentDirectory(filename);
            MockFS.Received().CreateParentDirectory(filename.ToLowerInvariant());
        }

        [TestCase(T_File1)]
        public virtual void WriteAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.WriteAllBytes(filename.ToLowerInvariant(), T_Bytes);
            LowerCaseFS.WriteAllBytes(filename, T_Bytes);
            MockFS.Received().WriteAllBytes(filename.ToLowerInvariant(), T_Bytes);
        }

        [TestCase(T_File1)]
        public virtual void ReadAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.ReadAllBytes(filename.ToLowerInvariant()).Returns(T_Bytes);
            Assert.AreEqual(LowerCaseFS.ReadAllBytes(filename), T_Bytes);
            MockFS.Received().ReadAllBytes(filename.ToLowerInvariant());
        }

        [TestCase(T_File1)]
        public virtual void DeleteFile_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.DeleteFile(filename.ToLowerInvariant());
            LowerCaseFS.DeleteFile(filename);
            MockFS.Received().DeleteFile(filename.ToLowerInvariant());
        }
    }
}

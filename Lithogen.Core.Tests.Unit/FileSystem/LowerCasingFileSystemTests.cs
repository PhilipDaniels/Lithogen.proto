using Lithogen.Core.FileSystem;
using Lithogen.Interfaces.FileSystem;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    /// <summary>
    /// These tests only check the lower-casing behaviour. Parameter correctness checks
    /// etc. are presumed to be tested by testing the wrapped file system.
    /// </summary>
    public class LowerCasingFileSystemTests
    {
        public IEnumerable<string> LowerCasedFiles
        {
            get
            {
                foreach (string file in TestData.MixedCaseFilenamesWithoutDirectories)
                    yield return file.ToLowerInvariant();
            }
        }

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

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void FileExists_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.FileExists(filename.ToLowerInvariant()).Returns(true);
            Assert.True(LowerCaseFS.FileExists(filename));
            MockFS.Received().FileExists(filename.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void DirectoryExists_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.DirectoryExists(filename.ToLowerInvariant()).Returns(true);
            Assert.True(LowerCaseFS.DirectoryExists(filename));
            MockFS.Received().DirectoryExists(filename.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseDirectory)]
        public virtual void CreateDirectory_WhenDirectoryNameIsMixed_ForcesToLowerCase(string directory)
        {
            LowerCaseFS.CreateDirectory(directory);
            MockFS.Received().CreateDirectory(directory.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void CreateParentDirectory_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            LowerCaseFS.CreateParentDirectory(filename);
            MockFS.Received().CreateParentDirectory(filename.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void WriteAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            LowerCaseFS.WriteAllBytes(filename, TestData.ThreeByteArray);
            MockFS.Received().WriteAllBytes(filename.ToLowerInvariant(), TestData.ThreeByteArray);
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void ReadAllBytes_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.ReadAllBytes(filename.ToLowerInvariant()).Returns(TestData.ThreeByteArray);
            Assert.AreEqual(TestData.ThreeByteArray, LowerCaseFS.ReadAllBytes(filename));
            MockFS.Received().ReadAllBytes(filename.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void DeleteFile_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            LowerCaseFS.DeleteFile(filename);
            MockFS.Received().DeleteFile(filename.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseDirectory)]
        public virtual void DeleteDirectory_WWhenDirectoryNameIsMixed_ForcesToLowerCase(string directory)
        {
            LowerCaseFS.DeleteDirectory(directory);
            MockFS.Received().DeleteDirectory(directory.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseDirectory)]
        public virtual void EnumerateFiles1_WhenDirectoryNameIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.EnumerateFiles(directory.ToLowerInvariant()).Returns(TestData.MixedCaseFilenamesWithoutDirectories);
            CollectionAssert.AreEqual(LowerCasedFiles, LowerCaseFS.EnumerateFiles(directory));
            MockFS.Received().EnumerateFiles(directory.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseDirectory)]
        public virtual void EnumerateFiles2_WhenDirectoryNameIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.EnumerateFiles(directory.ToLowerInvariant(), "*").Returns(TestData.MixedCaseFilenamesWithoutDirectories);
            CollectionAssert.AreEqual(LowerCasedFiles, LowerCaseFS.EnumerateFiles(directory, "*"));
            MockFS.Received().EnumerateFiles(directory.ToLowerInvariant(), "*");
        }

        [TestCase(TestData.MixedCaseDirectory)]
        public virtual void EnumerateFiles3_WhenDirectoryNameIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.EnumerateFiles(directory.ToLowerInvariant(), "*", SearchOption.AllDirectories).Returns(TestData.MixedCaseFilenamesWithoutDirectories);
            CollectionAssert.AreEqual(LowerCasedFiles, LowerCaseFS.EnumerateFiles(directory, "*", SearchOption.AllDirectories));
            MockFS.Received().EnumerateFiles(directory.ToLowerInvariant(), "*", SearchOption.AllDirectories);
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void ReadAllText1_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.ReadAllText(filename.ToLowerInvariant()).Returns(TestData.UnicodeString);
            Assert.AreEqual(TestData.UnicodeString, LowerCaseFS.ReadAllText(filename));
            MockFS.Received().ReadAllText(filename.ToLowerInvariant());
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void ReadAllText2_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.ReadAllText(filename.ToLowerInvariant(), Encoding.UTF8).Returns(TestData.UnicodeString);
            Assert.AreEqual(TestData.UnicodeString, LowerCaseFS.ReadAllText(filename, Encoding.UTF8));
            MockFS.Received().ReadAllText(filename.ToLowerInvariant(), Encoding.UTF8);
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void WriteAllText1_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            LowerCaseFS.WriteAllText(filename, TestData.UnicodeString);
            MockFS.Received().WriteAllText(filename.ToLowerInvariant(), TestData.UnicodeString);
        }

        [TestCase(TestData.MixedCaseFilename)]
        public virtual void WriteAllText2_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            LowerCaseFS.WriteAllText(filename, TestData.UnicodeString, Encoding.UTF8);
            MockFS.Received().WriteAllText(filename.ToLowerInvariant(), TestData.UnicodeString, Encoding.UTF8);
        }
    }
}

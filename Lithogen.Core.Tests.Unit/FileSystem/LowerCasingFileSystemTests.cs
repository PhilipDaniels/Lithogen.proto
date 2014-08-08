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
        public const string T_Directory = @"C:\temp\Lithogen_testdir";
        public const string T_File1 = @"C:\temp\Lithogen_testdir\MixedCaseFile.dat";
        public static readonly byte[] T_Bytes = new byte[] { 1, 2, 3 };
        public static readonly string[] T_Files = new string[] { "File1.txt", "File2.txt", "File3.txt" };
        public static string T_SomeText = "Hello World";

        public IEnumerable<string> LowerCasedFiles
        {
            get
            {
                foreach (string file in T_Files)
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

        [TestCase(T_Directory)]
        public virtual void DeleteDirectory_WhenFilenameIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.DeleteDirectory(directory.ToLowerInvariant());
            LowerCaseFS.DeleteDirectory(directory);
            MockFS.Received().DeleteDirectory(directory.ToLowerInvariant());
        }

        [TestCase(T_Directory)]
        public virtual void EnumerateFiles1_WhenFilenameIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.EnumerateFiles(directory.ToLowerInvariant()).Returns(T_Files);
            CollectionAssert.AreEqual(LowerCasedFiles, LowerCaseFS.EnumerateFiles(directory));
            MockFS.Received().EnumerateFiles(directory.ToLowerInvariant());
        }

        [TestCase(T_Directory)]
        public virtual void EnumerateFiles2_WhenFilenameIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.EnumerateFiles(directory.ToLowerInvariant(), "*").Returns(T_Files);
            CollectionAssert.AreEqual(LowerCasedFiles, LowerCaseFS.EnumerateFiles(directory, "*"));
            MockFS.Received().EnumerateFiles(directory.ToLowerInvariant(), "*");
        }

        [TestCase(T_Directory)]
        public virtual void EnumerateFiles3_WhenFilenameIsMixed_ForcesToLowerCase(string directory)
        {
            MockFS.EnumerateFiles(directory.ToLowerInvariant(), "*", SearchOption.AllDirectories).Returns(T_Files);
            CollectionAssert.AreEqual(LowerCasedFiles, LowerCaseFS.EnumerateFiles(directory, "*", SearchOption.AllDirectories));
            MockFS.Received().EnumerateFiles(directory.ToLowerInvariant(), "*", SearchOption.AllDirectories);
        }

        [TestCase(T_File1)]
        public virtual void ReadAllText1_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.ReadAllText(filename.ToLowerInvariant()).Returns(T_SomeText);
            Assert.AreEqual(T_SomeText, LowerCaseFS.ReadAllText(filename));
            MockFS.Received().ReadAllText(filename.ToLowerInvariant());
        }

        [TestCase(T_File1)]
        public virtual void ReadAllText2_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.ReadAllText(filename.ToLowerInvariant(), Encoding.UTF8).Returns(T_SomeText);
            Assert.AreEqual(T_SomeText, LowerCaseFS.ReadAllText(filename, Encoding.UTF8));
            MockFS.Received().ReadAllText(filename.ToLowerInvariant(), Encoding.UTF8);
        }

        [TestCase(T_File1)]
        public virtual void WriteAllText1_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.WriteAllText(filename.ToLowerInvariant(), T_SomeText);
            LowerCaseFS.WriteAllText(filename, T_SomeText);
            MockFS.Received().WriteAllText(filename.ToLowerInvariant(), T_SomeText);
        }

        [TestCase(T_File1)]
        public virtual void WriteAllText2_WhenFilenameIsMixed_ForcesToLowerCase(string filename)
        {
            MockFS.WriteAllText(filename.ToLowerInvariant(), T_SomeText, Encoding.UTF8);
            LowerCaseFS.WriteAllText(filename, T_SomeText, Encoding.UTF8);
            MockFS.Received().WriteAllText(filename.ToLowerInvariant(), T_SomeText, Encoding.UTF8);
        }
    }
}

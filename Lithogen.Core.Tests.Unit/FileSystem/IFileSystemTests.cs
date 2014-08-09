using Lithogen.Core.FileSystem;
using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystemTests<T>
        where T : IFileSystem, new()
    {
        public virtual void DeletePhysicalTestDirectories()
        {
            if (TheFS.GetType() == typeof(WindowsFileSystem) && Directory.Exists(TestData.TestRootDirectory))
            {
                Directory.Delete(TestData.TestRootDirectory, true);
            }
        }

        public virtual void TouchFile(string filename)
        {
            TheFS.WriteAllBytes(filename, TestData.EmptyByteArray); 
            string directory = Path.GetDirectoryName(filename);
            Assert.True(TheFS.DirectoryExists(directory));
            Assert.True(TheFS.FileExists(filename));
        }

        public IEnumerable<TestCaseData> InvalidFilenamesWithExceptions
        {
            get
            {
                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));
                yield return new TestCaseData("").Throws(typeof(ArgumentOutOfRangeException));
                yield return new TestCaseData(" ").Throws(typeof(ArgumentOutOfRangeException));
            }
        }

        public IEnumerable<TestCaseData> InvalidFilenamesWithEmptyByteArraysAndExceptions
        {
            get
            {
                yield return new TestCaseData(null, TestData.EmptyByteArray).Throws(typeof(ArgumentNullException));
                yield return new TestCaseData("", TestData.EmptyByteArray).Throws(typeof(ArgumentOutOfRangeException));
                yield return new TestCaseData(" ", TestData.EmptyByteArray).Throws(typeof(ArgumentOutOfRangeException));
            }
        }

        public IEnumerable<TestCaseData> ValidByteFiles
        {
            get
            {
                // Yes, an empty byte array is considered valid by File.WriteAllBytes, so we consider it valid too.
                yield return new TestCaseData(TestData.Filename, TestData.EmptyByteArray);
                yield return new TestCaseData(TestData.Filename, TestData.OneByteArray);
                yield return new TestCaseData(TestData.Filename, TestData.ThreeByteArray);
            }
        }

        public IEnumerable<TestCaseData> ValidStringFiles
        {
            get
            {
                yield return new TestCaseData(TestData.Filename, "");
                yield return new TestCaseData(TestData.Filename, TestData.UnicodeString);
            }
        }

        [SetUp]
        public virtual void Setup()
        {
            TheFS = new T();
            DeletePhysicalTestDirectories();
        }
        public T TheFS;

        [TearDown]
        public virtual void TearDown()
        {
            DeletePhysicalTestDirectories();
        }

        #region CreateDirectory
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void CreateDirectory_WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.CreateDirectory(directory);
        }

        [TestCase(TestData.SubDirectory)]
        public virtual void CreateDirectory_WhenDirectoryAlreadyExists_DoesNotThrow(string directory)
        {
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
        }
        #endregion

        #region CreateParentDirectory
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void CreateParentDirectory_WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.CreateDirectory(directory);
        }

        [TestCase(TestData.SubDirectory)]
        public virtual void CreateParentDirectory_WhenDirectoryIsValid_CreatesParentButNotTheDirectory(string directory)
        {
            TheFS.CreateParentDirectory(directory);
            Assert.False(TheFS.DirectoryExists(directory));
            Assert.True(TheFS.DirectoryExists(Path.GetDirectoryName(directory)));
        }

        [TestCase(TestData.SubDirectory)]
        public virtual void CreateParentDirectory_WhenDirectoryAlreadyExists_DoesNotThrow(string directory)
        {
            TheFS.CreateParentDirectory(directory);
            TheFS.CreateParentDirectory(directory);
        }
        #endregion

        #region DeleteDirectory
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void DeleteDirectory_WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.DeleteDirectory(directory);
        }

        [TestCase(TestData.DirectoryThatDoesNotExist)]
        public virtual void DeleteDirectory_WhenDirectoryDoesNotExist_Succeeds(string directory)
        {
            Assert.False(TheFS.DirectoryExists(directory));
            TheFS.DeleteDirectory(directory);
            Assert.False(TheFS.DirectoryExists(directory));
        }

        [TestCase(TestData.SubDirectory)]
        public virtual void DeleteDirectory_WhenDirectoryExistsAndIsEmpty_Succeeds(string directory)
        {
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
            TheFS.DeleteDirectory(directory);
            Assert.False(TheFS.DirectoryExists(directory));
        }

        [TestCase(TestData.SubDirectory)]
        public virtual void DeleteDirectory_WhenDirectoryExistsAndContainsFiles_Succeeds(string directory)
        {
            string filename = Path.Combine(directory, "somefile.txt");
            TouchFile(filename);

            TheFS.DeleteDirectory(directory);

            Assert.False(TheFS.DirectoryExists(directory));
            Assert.False(TheFS.FileExists(filename));
        }

        [TestCase(TestData.SubDirectory)]
        public virtual void DeleteDirectory_WhenDirectoryExistsAndContainsSubDirectories_Succeeds(string directory)
        {
            string subDirectory = Path.Combine(directory, "subdir");
            string filename = Path.Combine(subDirectory, "somefile.txt");
            TouchFile(filename);
            Assert.True(TheFS.DirectoryExists(directory));
            Assert.True(TheFS.DirectoryExists(subDirectory));

            TheFS.DeleteDirectory(directory);

            Assert.False(TheFS.DirectoryExists(directory));
            Assert.False(TheFS.DirectoryExists(subDirectory));
            Assert.False(TheFS.FileExists(filename));
        }
        #endregion

        #region DeleteFile
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void DeleteFile_WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.DeleteFile(filename);
        }

        [TestCase(TestData.Filename)]
        public virtual void DeleteFile_WhenFileDoesExist_Succeeds(string filename)
        {
            TheFS.WriteAllBytes(filename, TestData.EmptyByteArray);
            TheFS.DeleteFile(filename);
            Assert.False(TheFS.FileExists(filename));
        }

        [TestCase(TestData.FileThatDoesNotExist)]
        public virtual void DeleteFile_WhenFileDoesNotExist_Succeeds(string filename)
        {
            Assert.False(TheFS.FileExists(filename));
            TheFS.DeleteFile(filename);
            Assert.False(TheFS.FileExists(filename));
        }
        #endregion

        #region DirectoryExists
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void DirectoryExists_WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.CreateDirectory(directory);
        }

        [TestCase(TestData.TestRootDirectory)]
        public virtual void DirectoryExists_WhenDirectoryAlreadyExists_ReturnsTrue(string directory)
        {
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
        }

        [TestCase(TestData.DirectoryThatDoesNotExist)]
        public virtual void DirectoryExists_WhenDirectoryDoesNotExist_ReturnsFalse(string directory)
        {
            Assert.False(TheFS.DirectoryExists(directory));
        }
        #endregion

        #region EnumerateFiles
        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(" ", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(TestData.DirectoryThatDoesNotExist, ExpectedException = typeof(DirectoryNotFoundException))]
        public virtual void EnumerateFiles_WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.EnumerateFiles(directory);
        }

        [TestCase(@"C:\", null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(@"C:\", "", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(@"C:\", " ", ExpectedException = typeof(ArgumentOutOfRangeException))]
        public virtual void EnumerateFiles_WhenPatternIsInvalid_Throws(string directory, string pattern)
        {
            TheFS.EnumerateFiles(directory, pattern);
        }

        [TestCase(@"C:\", "*", -1, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(@"C:\", "*", Int32.MaxValue, ExpectedException = typeof(ArgumentOutOfRangeException))]
        public virtual void EnumerateFiles_WhenSearchOptionIsInvalid_Throws(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.EnumerateFiles(directory, pattern, searchOption);
        }

        [TestCase(TestData.SubDirectory, "*", SearchOption.TopDirectoryOnly)]
        [TestCase(TestData.SubDirectory, "*", SearchOption.AllDirectories)]
        public virtual void EnumerateFiles_WhenDirectoryIsEmpty_ReturnsEmptyCollection(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            CollectionAssert.IsEmpty(files);
        }

        [TestCase(TestData.SubDirectory, "*", SearchOption.TopDirectoryOnly)]
        [TestCase(TestData.SubDirectory, "*", SearchOption.AllDirectories)]
        public virtual void EnumerateFiles_WhenDirectoryContainsFiles_ReturnsMatchingFiles(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "file2.dat");
            TouchFile(file1);
            TouchFile(file2);
            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(TestData.SubDirectory, "*", SearchOption.TopDirectoryOnly)]
        public virtual void EnumerateFiles_WhenDirectoryHasSubdirectoriesAndSearchingTopOnly_IgnoresSubDirectoryFiles(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "a");
            TouchFile(file1);
            TouchFile(file2);
            string file3 = Path.Combine(directory, @"subdir\file3.dat");
            string file4 = Path.Combine(directory, @"subdir2\file4.dat");
            TouchFile(file3);
            TouchFile(file4);

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(TestData.SubDirectory, "*", SearchOption.AllDirectories)]
        public virtual void EnumerateFiles_WhenDirectoryHasSubdirectoriesAndSearchingAll_ReturnsSubDirectoryFiles(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "a");
            TouchFile(file1);
            TouchFile(file2);
            string file3 = Path.Combine(directory, @"subdir\file3.dat");
            string file4 = Path.Combine(directory, @"subdir2\file4.dat");
            TouchFile(file3);
            TouchFile(file4);

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2, file3, file4 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(TestData.SubDirectory, "*", SearchOption.AllDirectories)]
        public virtual void EnumerateFiles_WhenDirectoryHasEmptySubdirectoriesAndSearchingAll_ReturnsFilesOnly(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "a");
            TouchFile(file1);
            TouchFile(file2);
            TheFS.CreateDirectory(Path.Combine(directory, "subdir1"));
            TheFS.CreateDirectory(Path.Combine(directory, "subdir2"));

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(TestData.SubDirectory, SearchOption.AllDirectories)]
        public virtual void EnumerateFiles_WhenSearchingUsingPatterns_ReturnsCorrectMatches(string directory, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "file2.dat");
            string file3 = Path.Combine(directory, "file3.txt");
            TouchFile(file1);
            TouchFile(file2);
            TouchFile(file3);

            var files = TheFS.EnumerateFiles(directory, "*.dat", searchOption);
            var expectedFiles = new string[] { file1, file2 };
            CollectionAssert.AreEquivalent(expectedFiles, files);

            files = TheFS.EnumerateFiles(directory, "*.txt", searchOption);
            expectedFiles = new string[] { file3 };
            CollectionAssert.AreEquivalent(expectedFiles, files);

            files = TheFS.EnumerateFiles(directory, "*", searchOption);
            expectedFiles = new string[] { file1, file2, file3 };
            CollectionAssert.AreEquivalent(expectedFiles, files);

            files = TheFS.EnumerateFiles(directory, "*.bbb", searchOption);
            expectedFiles = new string[] { };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }
        #endregion

        #region FileExists
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void FileExists_WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.FileExists(filename);
        }

        [TestCase(TestData.Filename)]
        public virtual void FileExists_WhenFileDoesExist_ReturnsTrue(string filename)
        {
            TouchFile(filename);
            Assert.True(TheFS.FileExists(filename));
        }

        [TestCase(TestData.FileThatDoesNotExist)]
        public virtual void FileExists_WhenFileDoesNotExist_ReturnsFalse(string filename)
        {
            Assert.False(TheFS.FileExists(filename));
        }
        #endregion

        #region ReadAllBytes
        // See also test cases for WriteAllBytes.
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void ReadAllBytes_WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.ReadAllBytes(filename);
        }

        [TestCase(TestData.FileThatDoesNotExist, ExpectedException = typeof(FileNotFoundException))]
        public virtual void ReadAllBytes_WhenFileDoesNotExist_Throws(string filename)
        {
            TheFS.ReadAllBytes(filename);
        }
        #endregion

        #region ReadAllText
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void ReadAllText_WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.ReadAllText(filename);
        }

        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void ReadAllText_WhenFilenameIsInvalidAndEncodingIsValid_Throws(string filename)
        {
            TheFS.ReadAllText(filename, Encoding.UTF8);
        }

        [TestCase(TestData.Filename)]
        public virtual void ReadAllText_WhenEncodingIsNull_ThrowsArgumentNullException(string filename)
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.ReadAllText(filename, null));
        }

        [TestCase(TestData.FileThatDoesNotExist)]
        public virtual void ReadAllText_WhenFileDoesNotExist_ThrowsFileNotFoundException(string filename)
        {
            Assert.Throws<FileNotFoundException>(() => TheFS.ReadAllText(filename));
            Assert.Throws<FileNotFoundException>(() => TheFS.ReadAllText(filename, Encoding.UTF8));
        }
        // TODO: Need more tests here!
        #endregion

        #region WriteAllBytes
        [TestCaseSource("InvalidFilenamesWithEmptyByteArraysAndExceptions")]
        public virtual void WriteAllBytes_WhenFilenameIsInvalid_Throws(string filename, byte[] bytes)
        {
            TheFS.WriteAllBytes(filename, bytes);
        }

        [TestCase(TestData.Filename, null, ExpectedException = typeof(ArgumentNullException))]
        public virtual void WriteAllBytes_WhenByteArrayIsNull_ThrowsArgumentNullException(string filename, byte[] bytes)
        {
            TheFS.WriteAllBytes(filename, bytes);
        }

        [TestCaseSource("ValidByteFiles")]
        public virtual void WriteAllBytes_WhenFilenameIsValid_CreatesParentDirectoryIfItDoesNotExist(string filename, byte[] bytes)
        {
            string parent = Path.GetDirectoryName(filename);
            Assert.False(TheFS.DirectoryExists(parent));
            TheFS.WriteAllBytes(filename, bytes);
            Assert.True(TheFS.DirectoryExists(parent));
            Assert.True(TheFS.FileExists(filename));
        }

        [TestCaseSource("ValidByteFiles")]
        public virtual void WriteAllBytes_WhenFilenameIsValid_DataReadBackInIsIdentical(string filename, byte[] bytes)
        {
            TheFS.WriteAllBytes(filename, bytes);
            byte[] writtenBytes = TheFS.ReadAllBytes(filename);
            Assert.AreEqual(bytes, writtenBytes);
        }
        #endregion

        #region WriteAllText
        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void WriteAllText_WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.WriteAllText(filename, TestData.UnicodeString);
        }

        [TestCaseSource("InvalidFilenamesWithExceptions")]
        public virtual void WriteAllText_WhenFilenameIsInvalidAndEncodingIsValid_Throws(string filename)
        {
            TheFS.WriteAllText(filename, TestData.UnicodeString, Encoding.UTF8);
        }

        [TestCase(TestData.Filename)]
        public virtual void WriteAllText_WhenEncodingIsNull_ThrowsArgumentNullException(string filename)
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(filename, TestData.UnicodeString, null));
        }

        [TestCase(TestData.Filename)]
        public virtual void WriteAllText_WhenContentIsNull_ThrowsArgumentNullException(string filename)
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(filename, null));
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(filename, null, Encoding.UTF8));
        }

        [TestCaseSource("ValidStringFiles")]
        public virtual void WriteAllText_WhenFilenameIsValid_CreatesParentDirectoryIfItDoesNotExist(string filename, string contents)
        {
            string parent = Path.GetDirectoryName(filename);
            Assert.False(TheFS.DirectoryExists(parent));
            TheFS.WriteAllText(filename, contents);
            Assert.True(TheFS.DirectoryExists(parent));
            Assert.True(TheFS.FileExists(filename));
        }

        [TestCaseSource("ValidStringFiles")]
        public virtual void WriteAllText_WhenFilenameIsValidUsingDefaultEncoding_DataReadBackInIsIdentical(string filename, string contents)
        {
            TheFS.WriteAllText(filename, contents);
            string readBack = TheFS.ReadAllText(filename);
            Assert.AreEqual(contents, readBack);
        }

        [TestCaseSource("ValidStringFiles")]
        public virtual void WriteAllText_WhenFilenameIsValidUsingSpecificEncoding_DataReadBackInIsIdentical(string filename, string contents)
        {
            TheFS.WriteAllText(filename, contents, Encoding.UTF32);
            string readBack = TheFS.ReadAllText(filename, Encoding.UTF32);
            Assert.AreEqual(contents, readBack);
        }
        #endregion
    }
}

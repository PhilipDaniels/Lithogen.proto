using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    /// <summary>
    /// We only have to test the overload that accepts all 3 parameters, assuming
    /// that the others call it.
    /// </summary>
    public abstract class IFileSystem_EnumerateFiles<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(" ", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(T_DirectoryThatDoesNotExist, ExpectedException = typeof(DirectoryNotFoundException))]
        public virtual void WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.EnumerateFiles(directory);
        }

        [TestCase(@"C:\", null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(@"C:\", "", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(@"C:\", " ", ExpectedException = typeof(ArgumentOutOfRangeException))]
        public virtual void WhenPatternIsInvalid_Throws(string directory, string pattern)
        {
            TheFS.EnumerateFiles(directory, pattern);
        }

        [TestCase(@"C:\", "*", -1, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(@"C:\", "*", Int32.MaxValue, ExpectedException = typeof(ArgumentOutOfRangeException))]
        public virtual void WhenSearchOptionIsInvalid_Throws(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.EnumerateFiles(directory, pattern, searchOption);
        }

        [TestCase(T_Dir1, "*", SearchOption.TopDirectoryOnly)]
        [TestCase(T_Dir1, "*", SearchOption.AllDirectories)]
        public virtual void WhenDirectoryIsEmpty_ReturnsEmptyCollection(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            CollectionAssert.IsEmpty(files);
        }

        [TestCase(T_Dir1, "*", SearchOption.TopDirectoryOnly)]
        [TestCase(T_Dir1, "*", SearchOption.AllDirectories)]
        public virtual void WhenDirectoryContainsFiles_ReturnsMatchingFiles(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "file2.dat");
            TheFS.WriteAllBytes(file1, new byte[] { });
            TheFS.WriteAllBytes(file2, new byte[] { });
            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(T_Dir1, "*", SearchOption.TopDirectoryOnly)]
        public virtual void WhenDirectoryHasSubdirectoriesAndSearchingTopOnly_IgnoresSubDirectoryFiles(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "a");
            TheFS.WriteAllBytes(file1, new byte[] { });
            TheFS.WriteAllBytes(file2, new byte[] { });
            string file3 = Path.Combine(directory, @"subdir\file3.dat");
            string file4 = Path.Combine(directory, @"subdir2\file4.dat");
            TheFS.WriteAllBytes(file3, new byte[] { });
            TheFS.WriteAllBytes(file4, new byte[] { });

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(T_Dir1, "*", SearchOption.AllDirectories)]
        public virtual void WhenDirectoryHasSubdirectoriesAndSearchingAll_ReturnsSubDirectoryFiles(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "a");
            TheFS.WriteAllBytes(file1, new byte[] { });
            TheFS.WriteAllBytes(file2, new byte[] { });
            string file3 = Path.Combine(directory, @"subdir\file3.dat");
            string file4 = Path.Combine(directory, @"subdir2\file4.dat");
            TheFS.WriteAllBytes(file3, new byte[] { });
            TheFS.WriteAllBytes(file4, new byte[] { });

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2, file3, file4 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(T_Dir1, "*", SearchOption.AllDirectories)]
        public virtual void WhenDirectoryHasEmptySubdirectoriesAndSearchingAll_ReturnsFilesOnly(string directory, string pattern, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "a");
            TheFS.WriteAllBytes(file1, new byte[] { });
            TheFS.WriteAllBytes(file2, new byte[] { });
            TheFS.CreateDirectory(Path.Combine(directory, "subdir1"));
            TheFS.CreateDirectory(Path.Combine(directory, "subdir2"));

            var files = TheFS.EnumerateFiles(directory, pattern, searchOption);
            var expectedFiles = new string[] { file1, file2 };
            CollectionAssert.AreEquivalent(expectedFiles, files);
        }

        [TestCase(T_Dir1, SearchOption.AllDirectories)]
        public virtual void WhenSearchingUsingPatterns_ReturnsCorrectMatches(string directory, SearchOption searchOption)
        {
            TheFS.CreateDirectory(directory);
            string file1 = Path.Combine(directory, "file1.dat");
            string file2 = Path.Combine(directory, "file2.dat");
            string file3 = Path.Combine(directory, "file3.txt");
            TheFS.WriteAllBytes(file1, new byte[] { });
            TheFS.WriteAllBytes(file2, new byte[] { });
            TheFS.WriteAllBytes(file3, new byte[] { });

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
    }
}

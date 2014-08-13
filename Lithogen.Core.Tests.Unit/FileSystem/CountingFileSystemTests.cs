using Lithogen.Core.FileSystem;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public class CountingFileSystemTests
    {
        [SetUp]
        public virtual void Setup()
        {
            TheMockFS = Substitute.For<IFileSystem>();
            TheCountingFS = new CountingFileSystem(TheMockFS);
        }
        public IFileSystem TheMockFS;
        public CountingFileSystem TheCountingFS;

        [Test]
        public virtual void Ctor_WhenWrappedFileSystemIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new CountingFileSystem(null));
        }

        [Test]
        public virtual void Ctor_WhenCalled_CreatesEmptyStatsCollections()
        {
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsNotNull(TheCountingFS.StatsByExtension);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void FileExists_WhenCalled_DoesNotChangeStats(string filename)
        {
            TheMockFS.FileExists(filename).Returns(true);
            Assert.True(TheCountingFS.FileExists(filename));
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().FileExists(filename);
        }

        [TestCase(TestData.TestRootDirectory)]
        public virtual void DirectoryExists_WhenCalled_DoesNotChangeStats(string directory)
        {
            TheMockFS.DirectoryExists(directory).Returns(true);
            Assert.True(TheCountingFS.DirectoryExists(directory));
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().DirectoryExists(directory);
        }

        [TestCase(TestData.TestRootDirectory)]
        public virtual void CreateDirectory_WhenCalled_DoesNotChangeStats(string directory)
        {
            TheCountingFS.CreateDirectory(directory);
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().CreateDirectory(directory);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void CreateParentDirectory_WhenCalled_DoesNotChangeStats(string filename)
        {
            TheCountingFS.CreateParentDirectory(filename);
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().CreateParentDirectory(filename);
        }

        void WriteAllBytesImpl(string filename, byte[] bytes)
        {
            int numBytes = bytes.Length;
            TheCountingFS.WriteAllBytes(filename, bytes);

            Assert.AreEqual(1, TheCountingFS.TotalStats.FilesWritten);
            Assert.AreEqual(0, TheCountingFS.TotalStats.FilesRead);
            Assert.AreEqual(numBytes, TheCountingFS.TotalStats.BytesWritten);
            Assert.AreEqual(0, TheCountingFS.TotalStats.BytesRead);

            Assert.AreEqual(1, TheCountingFS.StatsByExtension.Count);
            string extension = Path.GetExtension(filename);
            var stats = TheCountingFS.StatsByExtension[extension];
            Assert.IsNotNull(stats);

            Assert.AreEqual(1, stats.FilesWritten);
            Assert.AreEqual(0, stats.FilesRead);
            Assert.AreEqual(numBytes, stats.BytesWritten);
            Assert.AreEqual(0, stats.BytesRead);

            TheMockFS.Received().WriteAllBytes(filename, bytes);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void WriteAllBytes_ForEmptyByteArray_IncrementsFilesWrittenButNotBytesWritten(string filename)
        {
            WriteAllBytesImpl(filename, TestData.EmptyByteArray);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void WriteAllBytes_ForNonEmptyByteArray_IncrementsFilesWrittenAndBytesWritten(string filename)
        {
            WriteAllBytesImpl(filename, TestData.ThreeByteArray);
        }

        void ReadAllBytesImpl(string filename, byte[] bytes)
        {
            int numBytes = bytes.Length;
            TheMockFS.ReadAllBytes(filename).Returns(bytes);
            byte[] result = TheCountingFS.ReadAllBytes(filename);
            Assert.AreEqual(bytes, result);

            Assert.AreEqual(0, TheCountingFS.TotalStats.FilesWritten);
            Assert.AreEqual(1, TheCountingFS.TotalStats.FilesRead);
            Assert.AreEqual(0, TheCountingFS.TotalStats.BytesWritten);
            Assert.AreEqual(numBytes, TheCountingFS.TotalStats.BytesRead);

            Assert.AreEqual(1, TheCountingFS.StatsByExtension.Count);
            string extension = Path.GetExtension(filename);
            var stats = TheCountingFS.StatsByExtension[extension];
            Assert.IsNotNull(stats);

            Assert.AreEqual(0, stats.FilesWritten);
            Assert.AreEqual(1, stats.FilesRead);
            Assert.AreEqual(0, stats.BytesWritten);
            Assert.AreEqual(numBytes, stats.BytesRead);

            TheMockFS.Received().ReadAllBytes(filename);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void ReadAllBytes_ForEmptyByteArray_IncrementsFilesReadButNotBytesRead(string filename)
        {
            ReadAllBytesImpl(filename, TestData.EmptyByteArray);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void ReadAllBytes_ForNonEmptyByteArray_IncrementsFilesReadAndBytesRead(string filename)
        {
            ReadAllBytesImpl(filename, TestData.ThreeByteArray);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void DeleteFile_WhenCalled_DoesNotChangeStats(string filename)
        {
            TheCountingFS.DeleteFile(filename);
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().DeleteFile(filename);
        }

        [TestCase(TestData.TestRootDirectory)]
        public virtual void DeleteDirectory_WhenCalled_DoesNotChangeStats(string directory)
        {
            TheCountingFS.DeleteDirectory(directory);
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().DeleteDirectory(directory);
        }

        [TestCase(TestData.TestRootDirectory)]
        public virtual void EnumerateFiles1_WhenCalled_DoesNotChangeStats(string directory)
        {
            TheMockFS.EnumerateFiles(directory).Returns(TestData.MixedCaseFilenamesWithoutDirectories);
            var files = TheCountingFS.EnumerateFiles(directory);
            Assert.AreEqual(TestData.MixedCaseFilenamesWithoutDirectories, files);
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().EnumerateFiles(directory);
        }

        [TestCase(TestData.TestRootDirectory)]
        public virtual void EnumerateFiles2_WhenCalled_DoesNotChangeStats(string directory)
        {
            TheMockFS.EnumerateFiles(directory, "*").Returns(TestData.MixedCaseFilenamesWithoutDirectories);
            var files = TheCountingFS.EnumerateFiles(directory, "*");
            Assert.AreEqual(TestData.MixedCaseFilenamesWithoutDirectories, files);
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().EnumerateFiles(directory, "*");
        }

        [TestCase(TestData.TestRootDirectory)]
        public virtual void EnumerateFiles3_WhenCalled_DoesNotChangeStats(string directory)
        {
            TheMockFS.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Returns(TestData.MixedCaseFilenamesWithoutDirectories);
            var files = TheCountingFS.EnumerateFiles(directory, "*", SearchOption.AllDirectories);
            Assert.AreEqual(TestData.MixedCaseFilenamesWithoutDirectories, files);
            AssertZeroStats(TheCountingFS.TotalStats);
            Assert.IsEmpty(TheCountingFS.StatsByExtension);
            TheMockFS.Received().EnumerateFiles(directory, "*", SearchOption.AllDirectories);
        }

        void AssertReadAllTextChecks(string filename, int expectedFileReads, int expectedByteCount)
        {
            Assert.AreEqual(0, TheCountingFS.TotalStats.FilesWritten);
            Assert.AreEqual(expectedFileReads, TheCountingFS.TotalStats.FilesRead);
            Assert.AreEqual(0, TheCountingFS.TotalStats.BytesWritten);
            Assert.AreEqual(expectedByteCount, TheCountingFS.TotalStats.BytesRead);

            Assert.AreEqual(1, TheCountingFS.StatsByExtension.Count);
            string extension = Path.GetExtension(filename);
            var stats = TheCountingFS.StatsByExtension[extension];
            Assert.IsNotNull(stats);

            Assert.AreEqual(0, stats.FilesWritten);
            Assert.AreEqual(expectedFileReads, stats.FilesRead);
            Assert.AreEqual(0, stats.BytesWritten);
            Assert.AreEqual(expectedByteCount, stats.BytesRead);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void ReadAllText_ForDefaultEncoding_IncrementsFilesReadAndBytesReadByDefaultEncoding(string filename)
        {
            TheMockFS.ReadAllText(filename).Returns(TestData.UnicodeString);
            string result = TheCountingFS.ReadAllText(filename);
            Assert.AreEqual(TestData.UnicodeString, result);

            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(TestData.UnicodeString);
            AssertReadAllTextChecks(filename, 1, byteCount);
            TheMockFS.Received().ReadAllText(filename);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void ReadAllText_ForSpecificEncoding_IncrementsFilesReadAndBytesRead(string filename)
        {
            TheMockFS.ReadAllText(filename, Encoding.UTF32).Returns(TestData.UnicodeString);
            string result = TheCountingFS.ReadAllText(filename, Encoding.UTF32);
            Assert.AreEqual(TestData.UnicodeString, result);

            int byteCount = Encoding.UTF32.GetByteCount(TestData.UnicodeString);
            AssertReadAllTextChecks(filename, 1, byteCount);
            TheMockFS.Received().ReadAllText(filename, Encoding.UTF32);
        }

        void AssertWriteAllTextChecks(string filename, int expectedFileWrites, int expectedByteCount)
        {
            Assert.AreEqual(expectedFileWrites, TheCountingFS.TotalStats.FilesWritten);
            Assert.AreEqual(0, TheCountingFS.TotalStats.FilesRead);
            Assert.AreEqual(expectedByteCount, TheCountingFS.TotalStats.BytesWritten);
            Assert.AreEqual(0, TheCountingFS.TotalStats.BytesRead);

            Assert.AreEqual(1, TheCountingFS.StatsByExtension.Count);
            string extension = Path.GetExtension(filename);
            var stats = TheCountingFS.StatsByExtension[extension];
            Assert.IsNotNull(stats);

            Assert.AreEqual(expectedFileWrites, stats.FilesWritten);
            Assert.AreEqual(0, stats.FilesRead);
            Assert.AreEqual(expectedByteCount, stats.BytesWritten);
            Assert.AreEqual(0, stats.BytesRead);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void WriteAllText_ForDefaultEncoding_IncrementsFilesWrittenAndBytesWrittenByDefaultEncoding(string filename)
        {
            TheCountingFS.WriteAllText(filename, TestData.UnicodeString);

            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(TestData.UnicodeString);
            AssertWriteAllTextChecks(filename, 1, byteCount);
            TheMockFS.Received().WriteAllText(filename, TestData.UnicodeString);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void WriteAllText_ForSpecificEncoding_IncrementsFilesWrittenAndBytesWritten(string filename)
        {
            TheCountingFS.WriteAllText(filename, TestData.UnicodeString, Encoding.UTF32);

            int byteCount = Encoding.UTF32.GetByteCount(TestData.UnicodeString);
            AssertWriteAllTextChecks(filename, 1, byteCount);
            TheMockFS.Received().WriteAllText(filename, TestData.UnicodeString, Encoding.UTF32);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void MultipleWrite_IncrementsStatsTheRequiredNumberOfTimes(string filename)
        {
            // The tests all check for 1 write or read operation, but those tests
            // would continue to pass if the CountingFileSystem did "= 1" rather
            // than "+= 1". This test is designed to catch that error.
            const int NUM_WRITES = 5;
            for (int i = 0; i < NUM_WRITES; i++)
            {
                TheCountingFS.WriteAllText(filename, TestData.UnicodeString);
            }

            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(TestData.UnicodeString) * NUM_WRITES;
            AssertWriteAllTextChecks(filename, NUM_WRITES, byteCount);
            TheMockFS.Received(NUM_WRITES).WriteAllText(filename, TestData.UnicodeString);
        }

        [TestCase(TestData.Filename)]
        [TestCase(TestData.FilenameWithoutExtension)]
        public virtual void MultipleRead_IncrementsStatsTheRequiredNumberOfTimes(string filename)
        {
            // The tests all check for 1 write or read operation, but those tests
            // would continue to pass if the CountingFileSystem did "= 1" rather
            // than "+= 1". This test is designed to catch that error.
            TheMockFS.ReadAllText(filename).Returns(TestData.UnicodeString);
            const int NUM_READS = 5;
            for (int i = 0; i < NUM_READS; i++)
            {
                string contents = TheCountingFS.ReadAllText(filename);
                Assert.AreEqual(TestData.UnicodeString, contents);
            }

            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(TestData.UnicodeString) * NUM_READS;
            AssertReadAllTextChecks(filename, NUM_READS, byteCount);
            TheMockFS.Received(NUM_READS).ReadAllText(filename);
        }

        public static FileSystemStats CopyStats(FileSystemStats stats)
        {
            return new FileSystemStats()
                {
                    FilesRead = stats.FilesRead,
                    FilesWritten = stats.FilesWritten,
                    BytesRead = stats.BytesRead,
                    BytesWritten = stats.BytesWritten
                };
        }

        public static void AssertZeroStats(FileSystemStats stats)
        {
            Assert.IsNotNull(stats);
            Assert.AreEqual(0, stats.FilesRead);
            Assert.AreEqual(0, stats.FilesWritten);
            Assert.AreEqual(0, stats.BytesRead);
            Assert.AreEqual(0, stats.BytesWritten);
        }
    }
}

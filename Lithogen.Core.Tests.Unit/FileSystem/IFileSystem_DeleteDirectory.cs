using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_DeleteDirectory<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCaseSource("InvalidFilenames")]
        public virtual void WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.DeleteDirectory(directory);
        }

        [TestCase(T_DirectoryThatDoesNotExist)]
        public virtual void WhenDirectoryDoesNotExist_Succeeds(string directory)
        {
            Assert.False(TheFS.DirectoryExists(directory)); 
            TheFS.DeleteDirectory(directory);
            Assert.False(TheFS.DirectoryExists(directory));
        }

        [TestCase(T_Dir1)]
        public virtual void WhenDirectoryExistsAndIsEmpty_Succeeds(string directory)
        {
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
            TheFS.DeleteDirectory(directory);
            Assert.False(TheFS.DirectoryExists(directory));
        }

        [TestCase(T_Dir1)]
        public virtual void WhenDirectoryExistsAndContainsFiles_Succeeds(string directory)
        {
            string filename = Path.Combine(directory, "somefile.txt");
            CreateDirectoryWithFile(directory, filename);

            TheFS.DeleteDirectory(directory);

            Assert.False(TheFS.DirectoryExists(directory));
            Assert.False(TheFS.FileExists(filename));
        }

        [TestCase(T_Dir1)]
        public virtual void WhenDirectoryExistsAndContainsSubDirectories_Succeeds(string directory)
        {
            string subDirectory = Path.Combine(directory, "subdir");
            string filename = Path.Combine(subDirectory, "somefile.txt");
            CreateDirectoryWithFile(subDirectory, filename);
            Assert.True(TheFS.DirectoryExists(directory));

            TheFS.DeleteDirectory(directory);

            Assert.False(TheFS.DirectoryExists(directory));
            Assert.False(TheFS.DirectoryExists(subDirectory));
            Assert.False(TheFS.FileExists(filename));
        }

        void CreateDirectoryWithFile(string directory, string filename)
        {
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
            TheFS.WriteAllBytes(filename, new byte[] { });
            Assert.True(TheFS.FileExists(filename));
        }
    }
}

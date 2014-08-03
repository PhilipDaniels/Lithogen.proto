using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_FileExists<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCaseSource("InvalidFilenames")]
        public virtual void WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.FileExists(filename);
        }

        [TestCase(T_File1)]
        public virtual void WhenFileDoesExist_ReturnsTrue(string filename)
        {
            TheFS.WriteAllBytes(filename, new byte[] { });
            Assert.True(TheFS.FileExists(filename));
        }

        [TestCase(T_FileThatDoesNotExist)]
        public virtual void WhenFileDoesNotExist_ReturnsFalse(string filename)
        {
            Assert.False(TheFS.FileExists(filename));
        }
    }
}

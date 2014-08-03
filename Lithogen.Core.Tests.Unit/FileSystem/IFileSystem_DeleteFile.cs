using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_DeleteFile<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCaseSource("InvalidFilenames")]
        public virtual void WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.DeleteFile(filename);
        }

        [TestCase(T_File1)]
        public virtual void WhenFileDoesExist_Succeeds(string filename)
        {
            TheFS.WriteAllBytes(filename, new byte[] { });
            TheFS.DeleteFile(filename);
            Assert.False(TheFS.FileExists(filename));
        }

        [TestCase(T_FileThatDoesNotExist)]
        public virtual void WhenFileDoesNotExist_Succeeds(string filename)
        {
            Assert.False(TheFS.FileExists(filename));
            TheFS.DeleteFile(filename);
            Assert.False(TheFS.FileExists(filename));
        }
    }
}

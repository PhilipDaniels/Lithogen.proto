using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    // Signature: void WriteAllBytes(string filename, byte[] bytes);

    public abstract class IFileSystem_WriteAllBytes<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCaseSource("InvalidFilenames")]
        public virtual void WhenFilenameIsInvalid_Throws(string filename, byte[] bytes)
        {
            TheFS.WriteAllBytes(filename, bytes);
        }
    }
}

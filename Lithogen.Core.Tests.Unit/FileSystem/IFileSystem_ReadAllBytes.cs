using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_ReadAllBytes<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCaseSource("InvalidFilenames")]
        public virtual void WhenFilenameIsInvalid_Throws(string filename)
        {
            TheFS.ReadAllBytes(filename);
        }

        [TestCase(T_FileThatDoesNotExist, ExpectedException = typeof(FileNotFoundException))]
        public virtual void WhenFileDoesNotExist_Throws(string filename)
        {
            TheFS.ReadAllBytes(filename);
        }

        // See also test cases in WriteAllBytes.
    }
}

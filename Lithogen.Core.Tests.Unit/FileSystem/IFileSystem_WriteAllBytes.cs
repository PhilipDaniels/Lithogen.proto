using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    // Signature: void WriteAllBytes(string filename, byte[] bytes);

    public abstract class IFileSystem_WriteAllBytes<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCaseSource("InvalidFilenamesWithEmptyArrays")]
        public virtual void WhenFilenameIsInvalid_Throws(string filename, byte[] bytes)
        {
            TheFS.WriteAllBytes(filename, bytes);
        }

        [TestCase(T_File1, null, ExpectedException = typeof(ArgumentNullException))]
        public virtual void WhenByteArrayIsNull_ThrowsArgumentNullException(string filename, byte[] bytes)
        {
            TheFS.WriteAllBytes(filename, bytes);
        }

        [TestCaseSource("ValidByteFiles")]
        public virtual void WhenFilenameIsValid_CreatesParentDirectoryIfItDoesNotExist(string filename, byte[] bytes)
        {
            string parent = Path.GetDirectoryName(filename);
            Assert.False(TheFS.DirectoryExists(parent));
            TheFS.WriteAllBytes(filename, bytes);
            Assert.True(TheFS.DirectoryExists(parent));
            Assert.True(TheFS.FileExists(filename));
        }

        [TestCaseSource("ValidByteFiles")]
        public virtual void WhenFilenameIsValid_DataReadBackInIsIdentical(string filename, byte[] bytes)
        {
            TheFS.WriteAllBytes(filename, bytes);
            byte[] writtenBytes = TheFS.ReadAllBytes(filename);
            Assert.AreEqual(bytes, writtenBytes);
        }
    }
}

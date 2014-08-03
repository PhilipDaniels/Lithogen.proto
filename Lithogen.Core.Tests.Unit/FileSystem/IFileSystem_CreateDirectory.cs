using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_CreateDirectory<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCaseSource("InvalidFilenames")]
        public virtual void WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.CreateDirectory(directory);
        }

        [TestCase(T_Dir1)]
        public virtual void WhenDirectoryAlreadyExists_DoesNotThrow(string directory)
        {
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
        }
    }
}

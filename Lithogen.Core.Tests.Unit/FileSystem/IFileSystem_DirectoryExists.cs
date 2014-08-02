using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_DirectoryExists<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(" ", ExpectedException = typeof(ArgumentOutOfRangeException))]
        public virtual void WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.CreateDirectory(directory);
        }

        [TestCase(T_ParentDir)]
        public virtual void WhenDirectoryAlreadyExists_ReturnsTrue(string directory)
        {
            TheFS.CreateDirectory(directory);
            Assert.True(TheFS.DirectoryExists(directory));
        }

        [TestCase(T_DirectoryThatDoesNotExist)]
        public virtual void WhenDirectoryDoesNotExist_ReturnsFalse(string directory)
        {
            Assert.False(TheFS.DirectoryExists(directory));
        }
    }
}

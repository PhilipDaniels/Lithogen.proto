using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_DirectoryExists<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [Test]
        public virtual void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.DirectoryExists(null));
        }

        [TestCase("")]
        [TestCase(" ")]
        public virtual void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException(string directory)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.DirectoryExists(directory));
        }

        [Test]
        public virtual void WhenDirectoryAlreadyExists_ReturnsTrue()
        {
            TheFS.CreateDirectory(T_ParentDir);
            Assert.True(TheFS.DirectoryExists(T_ParentDir));
        }

        [Test]
        public virtual void WhenDirectoryDoesNotExist_ReturnsFalse()
        {
            Assert.False(TheFS.DirectoryExists(T_DirectoryThatDoesNotExist));
        }
    }
}

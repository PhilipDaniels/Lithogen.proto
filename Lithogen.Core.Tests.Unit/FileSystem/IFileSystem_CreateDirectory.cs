using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_CreateDirectory<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [Test]
        public virtual void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.CreateDirectory(null));
        }

        [TestCase("")]
        [TestCase(" ")]
        public virtual void WhenDirectoryNameIsEmptyOrWhitespace_ThrowsArgumentOutOfRangeException(string directory)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.CreateDirectory(directory));
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

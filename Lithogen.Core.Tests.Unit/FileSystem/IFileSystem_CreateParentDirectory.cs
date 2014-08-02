using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_CreateParentDirectory<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [Test]
        public virtual void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.CreateParentDirectory(null));
        }

        [TestCase("")]
        [TestCase(" ")]
        public virtual void WhenDirectoryNameIsEmptyOrWhitespace_ThrowsArgumentOutOfRangeException(string directory)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.CreateParentDirectory(directory));
        }

        [TestCase(T_Dir1)]
        public virtual void WhenDirectoryIsValid_CreatesParentButNotTheDirectory(string directory)
        {
            TheFS.CreateParentDirectory(directory);
            Assert.False(TheFS.DirectoryExists(directory));
            Assert.True(TheFS.DirectoryExists(Path.GetDirectoryName(directory)));
        }

        [TestCase(T_Dir1)]
        public virtual void WhenDirectoryAlreadyExists_DoesNotThrow(string directory)
        {
            TheFS.CreateParentDirectory(directory);
            TheFS.CreateParentDirectory(directory);
        }
    }
}

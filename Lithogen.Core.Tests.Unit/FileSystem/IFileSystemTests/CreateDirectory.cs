using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit.FileSystem.IFileSystemTests
{
    public abstract class CreateDirectory<T> : IFileSystemBase<T>
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

        [Test]
        public virtual void WhenDirectoryAlreadyExists_Succeeds()
        {
            TheFS.CreateDirectory(T_Dir1);
            Assert.True(TheFS.DirectoryExists(T_Dir1));
            TheFS.CreateDirectory(T_Dir1);
            Assert.True(TheFS.DirectoryExists(T_Dir1));
        }
    }
}

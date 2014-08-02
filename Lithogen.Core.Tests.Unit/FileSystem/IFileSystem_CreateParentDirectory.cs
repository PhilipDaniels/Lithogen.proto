using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_CreateParentDirectory<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(" ", ExpectedException = typeof(ArgumentOutOfRangeException))]
        public virtual void WhenDirectoryNameIsInvalid_Throws(string directory)
        {
            TheFS.CreateDirectory(directory);
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

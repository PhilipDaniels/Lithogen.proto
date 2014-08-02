using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_CreateParentDirectory_Base<T> : IFileSystem_Base<T>
        where T : IFileSystem, new()
    {
        [Test]
        public virtual void WhenDirectoryNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.CreateParentDirectory(null));
        }

        [TestCase("")]
        [TestCase(" ")]
        public virtual void WhenDirectoryNameIsEmpty_ThrowsArgumentOutOfRangeException(string directory)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.CreateParentDirectory(directory));
        }

        [Test]
        public virtual void WhenDirectoryAlreadyExists_Succeeds()
        {
            TheFS.CreateParentDirectory(T_Dir1);
            string parent = Path.GetDirectoryName(T_Dir1);
            Assert.True(TheFS.DirectoryExists(parent));
            TheFS.CreateParentDirectory(T_Dir1);
        }
    }
}

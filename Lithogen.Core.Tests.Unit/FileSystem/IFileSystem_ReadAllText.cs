using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    /// <summary>
    /// We only have to test the overload that accepts all 3 parameters, assuming
    /// that the others call it.
    /// </summary>
    public abstract class IFileSystem_ReadAllText<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [Test]
        public virtual void WhenArgumentsAreInvalid_Throws()
        {
            // Check filename.
            Assert.Throws<ArgumentNullException>(() => TheFS.ReadAllText(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.ReadAllText(""));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.ReadAllText(" "));
            Assert.Throws<FileNotFoundException>(() => TheFS.ReadAllText(T_FileThatDoesNotExist));

            // Check filename on the other overload.
            Assert.Throws<ArgumentNullException>(() => TheFS.ReadAllText(null, Encoding.UTF8));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.ReadAllText("", Encoding.UTF8));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.ReadAllText(" ", Encoding.UTF8));
            Assert.Throws<FileNotFoundException>(() => TheFS.ReadAllText(T_FileThatDoesNotExist, Encoding.UTF8));

            // Check encoding.
            Assert.Throws<ArgumentNullException>(() => TheFS.ReadAllText(T_File1, null));
        }

        // See also checks in WriteAllText.
    }
}

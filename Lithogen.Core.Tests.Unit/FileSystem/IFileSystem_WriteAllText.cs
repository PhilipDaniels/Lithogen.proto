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
    public abstract class IFileSystem_WriteAllText<T> : IFileSystemBase<T>
        where T : IFileSystem, new()
    {
        [Test]
        public virtual void WhenArgumentsAreInvalid_Throws()
        {
            // Check filename.
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(null, T_String));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.WriteAllText("", T_String));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.WriteAllText(" ", T_String));
            
            // Check filename on the other overload.
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(null, T_String, Encoding.UTF8));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.WriteAllText("", T_String, Encoding.UTF8));
            Assert.Throws<ArgumentOutOfRangeException>(() => TheFS.WriteAllText(" ", T_String, Encoding.UTF8));

            // Check encoding.
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(T_File1, T_String, null));
        }

        [TestCase(T_File1)]
        public virtual void WhenContentIsNull_ThrowsArgumentNullException(string filename)
        {
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(filename, null));
            Assert.Throws<ArgumentNullException>(() => TheFS.WriteAllText(filename, null, Encoding.UTF8));
        }

        [TestCaseSource("ValidStringFiles")]
        public virtual void WhenFilenameIsValid_CreatesParentDirectoryIfItDoesNotExist(string filename, string contents)
        {
            string parent = Path.GetDirectoryName(filename);
            Assert.False(TheFS.DirectoryExists(parent));
            TheFS.WriteAllText(filename, contents);
            Assert.True(TheFS.DirectoryExists(parent));
            Assert.True(TheFS.FileExists(filename));
        }

        [TestCaseSource("ValidStringFiles")]
        public virtual void WhenFilenameIsValidUsingDefaultEncoding_DataReadBackInIsIdentical(string filename, string contents)
        {
            TheFS.WriteAllText(filename, contents);
            string readBack = TheFS.ReadAllText(filename);
            Assert.AreEqual(contents, readBack);
        }

        [TestCaseSource("ValidStringFiles")]
        public virtual void WhenFilenameIsValidUsingSpecificEncoding_DataReadBackInIsIdentical(string filename, string contents)
        {
            TheFS.WriteAllText(filename, contents, Encoding.UTF32);
            string readBack = TheFS.ReadAllText(filename, Encoding.UTF32);
            Assert.AreEqual(contents, readBack);
        }
    }
}

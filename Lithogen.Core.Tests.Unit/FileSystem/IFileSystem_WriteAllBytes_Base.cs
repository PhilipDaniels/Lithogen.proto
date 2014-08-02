using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystem_WriteAllBytes_Base<T> : IFileSystem_Base<T>
        where T : IFileSystem, new()
    {
        //[Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void WhenFilenameIsNull_ThrowsArgumentNullException()
        //{
        //    TheFS.WriteAllBytes(null, T_Bytes);
        //}
    }
}

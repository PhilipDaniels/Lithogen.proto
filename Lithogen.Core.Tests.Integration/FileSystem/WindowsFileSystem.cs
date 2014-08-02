using Lithogen.Core.FileSystem;
using Lithogen.Core.Tests.Unit.FileSystem;
using NUnit.Framework;
using System;
using System.IO;

namespace Lithogen.Core.Tests.Integration.FileSystem
{
    /*
    public class WindowsFileSystem_CreateDirectory : IFileSystem_CreateDirectory_Base<WindowsFileSystem>
    {
        [Test]
        public override void WhenDirectoryAlreadyExists_Succeeds()
        {
            base.WhenDirectoryAlreadyExists_Succeeds();
            // Ensure the physical directories really exist.
            Assert.True(Directory.Exists(T_Dir1));
            Assert.True(Directory.Exists(Path.GetDirectoryName(T_Dir1)));
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            DeletePhysicalTestDirectories();
        }
    }

    public class WindowsFileSystem_CreateParentDirectory : IFileSystem_CreateParentDirectory_Base<WindowsFileSystem>
    {
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            DeletePhysicalTestDirectories();
        }
    }

    public class WindowsFileSystem_DirectoryExists : IFileSystem_DirectoryExists_Base<WindowsFileSystem>
    {
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            DeletePhysicalTestDirectories();
        }
    }
     */ 
}

using Lithogen.Core.FileSystem;
using Lithogen.Core.Tests.Unit.FileSystem;
using NUnit.Framework;
using System.IO;

namespace Lithogen.Core.Tests.Integration.FileSystem
{
    public class WindowsFileSystem_CreateDirectory : IFileSystem_CreateDirectory<WindowsFileSystem>
    {
        [TestCase(T_Dir1)]
        public override void WhenDirectoryAlreadyExists_DoesNotThrow(string directory)
        {
            base.WhenDirectoryAlreadyExists_DoesNotThrow(directory);
            // Ensure the physical directories really exist.
            Assert.True(Directory.Exists(directory));
        }

        [TearDown]
        public void TearDown()
        {
            DeletePhysicalTestDirectories();
        }
    }

    public class WindowsFileSystem_CreateParentDirectory : IFileSystem_CreateParentDirectory<WindowsFileSystem>
    {
        [TestCase(T_Dir1)]
        public override void WhenDirectoryIsValid_CreatesParentButNotTheDirectory(string directory)
        {
            base.WhenDirectoryIsValid_CreatesParentButNotTheDirectory(directory);
            CheckPhysicalDirectories(directory);
        }

        [TestCase(T_Dir1)]
        public override void WhenDirectoryAlreadyExists_DoesNotThrow(string directory)
        {
            base.WhenDirectoryAlreadyExists_DoesNotThrow(directory);
            CheckPhysicalDirectories(directory);
        }

        void CheckPhysicalDirectories(string directory)
        {
            // Ensure the physical directories really exist/not exist as appropriate.
            Assert.False(Directory.Exists(directory));
            Assert.True(Directory.Exists(Path.GetDirectoryName(directory)));
        }

        [TearDown]
        public void TearDown()
        {
            DeletePhysicalTestDirectories();
        }
    }

    public class WindowsFileSystem_DirectoryExists : IFileSystem_DirectoryExists<WindowsFileSystem>
    {
        [TestCase(T_ParentDir)]
        public override void WhenDirectoryAlreadyExists_ReturnsTrue(string directory)
        {
            base.WhenDirectoryAlreadyExists_ReturnsTrue(directory);
            // Ensure the physical directory really does exist.
            Assert.True(Directory.Exists(directory));
        }

        [TestCase(T_ParentDir)]
        public override void WhenDirectoryDoesNotExist_ReturnsFalse(string directory)
        {
            base.WhenDirectoryDoesNotExist_ReturnsFalse(directory);
            // Ensure the physical directory really does not exist.
            Assert.False(Directory.Exists(directory));
        }

        [TearDown]
        public void TearDown()
        {
            DeletePhysicalTestDirectories();
        }
    }
}

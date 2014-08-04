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
    }

    public class WindowsFileSystem_DeleteDirectory : IFileSystem_DeleteDirectory<WindowsFileSystem>
    {
        [TestCase(T_DirectoryThatDoesNotExist)]
        public override void WhenDirectoryDoesNotExist_Succeeds(string directory)
        {
            Assert.False(Directory.Exists(directory));
            base.WhenDirectoryDoesNotExist_Succeeds(directory);
            Assert.False(Directory.Exists(directory));
        }

        [TestCase(T_Dir1)]
        public override void WhenDirectoryExistsAndIsEmpty_Succeeds(string directory)
        {
            base.WhenDirectoryExistsAndIsEmpty_Succeeds(directory);
            // Check the directory is really gone.
            Assert.False(Directory.Exists(directory));
        }

        [TestCase(T_Dir1)]
        public override void WhenDirectoryExistsAndContainsFiles_Succeeds(string directory)
        {
            base.WhenDirectoryExistsAndContainsFiles_Succeeds(directory);
            // Check the directory is really gone.
            Assert.False(Directory.Exists(directory));
        }

        [TestCase(T_Dir1)]
        public override void WhenDirectoryExistsAndContainsSubDirectories_Succeeds(string directory)
        {
            base.WhenDirectoryExistsAndContainsSubDirectories_Succeeds(directory);
            // Check the directory is really gone.
            Assert.False(Directory.Exists(directory));
        }
    }

    public class WindowsFileSystem_DeleteFile : IFileSystem_DeleteFile<WindowsFileSystem>
    {
        [TestCase(T_File1)]
        public override void WhenFileDoesExist_Succeeds(string filename)
        {
            base.WhenFileDoesExist_Succeeds(filename);
            // Ensure the file really does not exist.
            Assert.False(File.Exists(filename));
        }

        [TestCase(T_FileThatDoesNotExist)]
        public override void WhenFileDoesNotExist_Succeeds(string filename)
        {
            base.WhenFileDoesNotExist_Succeeds(filename);
            // Ensure the file really does not exist.
            Assert.False(File.Exists(filename));
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
    }

    public class WindowsFileSystem_EnumerateFiles : IFileSystem_EnumerateFiles<WindowsFileSystem>
    {
    }

    public class WindowsFileSystem_FileExists : IFileSystem_FileExists<WindowsFileSystem>
    {
        [TestCase(T_File1)]
        public override void WhenFileDoesExist_ReturnsTrue(string filename)
        {
            base.WhenFileDoesExist_ReturnsTrue(filename);
            // Ensure the file really does exist.
            Assert.True(File.Exists(filename));
        }

        [TestCase(T_FileThatDoesNotExist)]
        public override void WhenFileDoesNotExist_ReturnsFalse(string filename)
        {
            base.WhenFileDoesNotExist_ReturnsFalse(filename);
            // Ensure the file really does not exist.
            Assert.False(File.Exists(filename));
        }
    }

    public class WindowsFileSystem_ReadAllBytes : IFileSystem_ReadAllBytes<WindowsFileSystem>
    {
    }

    public class WindowsFileSystem_ReadAllText : IFileSystem_ReadAllText<WindowsFileSystem>
    {
    }

    public class WindowsFileSystem_WriteAllBytes : IFileSystem_WriteAllBytes<WindowsFileSystem>
    {
    }

    public class WindowsFileSystem_WriteAllText : IFileSystem_WriteAllText<WindowsFileSystem>
    {
    }
}

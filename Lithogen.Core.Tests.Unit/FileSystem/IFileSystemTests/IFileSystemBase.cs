using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem.IFileSystemTests
{
    public abstract class IFileSystemBase<T> where T : IFileSystem, new()
    {
        public static readonly string T_ParentDir = @"C:\temp\Lithogen_testdir";
        public static readonly string T_Dir1 = @"C:\temp\Lithogen_testdir\dir1";
        public static readonly string T_File1 = @"C:\temp\Lithogen_testdir\file1.data";
        public static readonly string T_DirectoryThatDoesNotExist = @"C:\somewhere\over\the\rainbow";

        public static readonly byte[] T_Bytes = new byte[] { 1, 2, 3 };

        [SetUp]
        public virtual void Setup()
        {
            TheFS = new T();
        }
        public T TheFS;

        public virtual void DeletePhysicalTestDirectories()
        {
            if (Directory.Exists(T_ParentDir))
            {
                Directory.Delete(T_ParentDir, true);
            }
        }
    }
}

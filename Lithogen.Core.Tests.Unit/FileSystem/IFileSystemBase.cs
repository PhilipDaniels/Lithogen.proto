using Lithogen.Core.FileSystem;
using Lithogen.Interfaces.FileSystem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lithogen.Core.Tests.Unit.FileSystem
{
    public abstract class IFileSystemBase<T> where T : IFileSystem, new()
    {
        public const string T_ParentDir = @"C:\temp\Lithogen_testdir";
        public const string T_Dir1 = @"C:\temp\Lithogen_testdir\dir1";
        public const string T_File1 = @"C:\temp\Lithogen_testdir\file1.data";
        public const string T_DirectoryThatDoesNotExist = @"C:\somewhere\over\the\rainbow";
        public const string T_FileThatDoesNotExist = @"C:\somewhere\over\the\rainbow\file.data";
        public static readonly byte[] T_EmptyBytes = new byte[] { };
        public static readonly byte[] T_OneByte = new byte[] { 1 };
        public static readonly byte[] T_ThreeBytes = new byte[] { 1, 2, 3 };

        [SetUp]
        public virtual void Setup()
        {
            TheFS = new T();
            DeletePhysicalTestDirectories();
        }
        public T TheFS;

        [TearDown]
        public virtual void TearDown()
        {
            DeletePhysicalTestDirectories();
        }

        public virtual void DeletePhysicalTestDirectories()
        {
            if (TheFS.GetType() == typeof(WindowsFileSystem) && Directory.Exists(T_ParentDir))
            {
                Directory.Delete(T_ParentDir, true);
            }
        }

        public IEnumerable<TestCaseData> InvalidFilenames
        {
            get
            {
                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));
                yield return new TestCaseData("").Throws(typeof(ArgumentOutOfRangeException));
                yield return new TestCaseData(" ").Throws(typeof(ArgumentOutOfRangeException));
            }
        }

        public IEnumerable<TestCaseData> InvalidFilenamesWithEmptyArrays
        {
            get
            {
                yield return new TestCaseData(null, T_EmptyBytes).Throws(typeof(ArgumentNullException));
                yield return new TestCaseData("", T_EmptyBytes).Throws(typeof(ArgumentOutOfRangeException));
                yield return new TestCaseData(" ", T_EmptyBytes).Throws(typeof(ArgumentOutOfRangeException));
            }
        }

        public IEnumerable<TestCaseData> ValidByteFiles
        {
            get
            {
                // Yes, an empty byte array is considered valid by File.WriteAllBytes, so we consider it valid too.
                yield return new TestCaseData(T_File1, T_EmptyBytes);  
                yield return new TestCaseData(T_File1, T_OneByte);
                yield return new TestCaseData(T_File1, T_ThreeBytes);
            }
        }
    }
}

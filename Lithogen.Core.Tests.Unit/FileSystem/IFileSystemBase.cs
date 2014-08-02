﻿using Lithogen.Interfaces.FileSystem;
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

        public IEnumerable<TestCaseData> InvalidFilenames
        {
            get
            {
                yield return new TestCaseData(null, T_Bytes).Throws(typeof(ArgumentNullException));
                yield return new TestCaseData("", T_Bytes).Throws(typeof(ArgumentOutOfRangeException));
                yield return new TestCaseData(" ", T_Bytes).Throws(typeof(ArgumentOutOfRangeException));
            }
        }
    }
}

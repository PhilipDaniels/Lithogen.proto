using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core.Tests.Unit
{
    public static class TestData
    {
        public static readonly byte[] EmptyByteArray = new byte[] { };
        public static readonly byte[] OneByteArray = new byte[] { 1 };
        public static readonly byte[] ThreeByteArray = new byte[] { 1, 2, 3 };

        public const string UnicodeString = @"Hello world in Japanese ハローワールド";

        public const string TestRootDirectory = @"C:\temp\Lithogen_testdir";
        public const string SubDirectory = @"C:\temp\Lithogen_testdir\dir1";
        public const string Filename = @"C:\temp\Lithogen_testdir\file1.data";
        public const string MixedCaseFilename = @"C:\temp\Lithogen_testdir\MixedCaseFile.dat";
        public const string MixedCaseDirectory = @"C:\temp\Lithogen_testdir\MixedCaseDir";
        public const string FilenameWithoutExtension = @"C:\temp\Lithogen_testdir\FileWithoutExtension";
        public const string DirectoryThatDoesNotExist = @"C:\somewhere\over\the\rainbow";
        public const string FileThatDoesNotExist = @"C:\somewhere\over\the\rainbow\file.data";
        public static readonly string[] MixedCaseFilenamesWithoutDirectories = new string[] { "File1.txt", "File2.txt", "File3.txt" };
    }
}

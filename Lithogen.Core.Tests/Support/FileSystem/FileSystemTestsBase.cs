
namespace Lithogen.Core.Tests.Support.FileSystem
{
    public abstract class FileSystemTestsBase
    {
        // Making these static means the inner test classes don't need to inherit their outer class.
        public static readonly string T_ParentDir = @"C:\temp\Lithogen_testdir";
        public static readonly string T_Dir1 = @"C:\temp\Lithogen_testdir\dir1";
        public static readonly string T_File1 = @"C:\temp\Lithogen_testdir\file1.data";
        public static readonly string T_DirectoryThatDoesNotExist = @"C:\somewhere\over\the\rainbow";

        public static readonly byte[] T_Bytes = new byte[] { 1, 2, 3 };
    }
}

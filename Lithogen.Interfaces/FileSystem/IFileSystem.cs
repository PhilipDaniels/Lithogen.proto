using System.Collections.Generic;
using System.IO;

namespace Lithogen.Interfaces.FileSystem
{
    /// <summary>
    /// Defines the basic operations that a Lithogen file system must support.
    /// Designed to be as minimal as possible, it maps closely to the Windows
    /// file system, yet allows a completely in-memory implementation to be
    /// created fairly easily.
    /// </summary>
    public interface IFileSystem
    {
        bool FileExists(string filename);
        bool DirectoryExists(string directory); 

        void CreateDirectory(string directory);
        void CreateParentDirectory(string filename);

        void WriteAllBytes(string filename, byte[] bytes);
        byte[] ReadAllBytes(string filename);

        void DeleteFile(string filename);
        void DeleteDirectory(string directory);

        IEnumerable<string> EnumerateFiles(string directory);
        IEnumerable<string> EnumerateFiles(string directory, string searchPattern);
        IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption);

        //string ReadAllText(string filename);  // This variant attempts to guess the encoding.
        //string ReadAllText(string filename, Encoding encoding);

        //void WriteAllText(string filename, string contents);                    // Write UTF-8 text without a BOM.
        //void WriteAllText(string filename, string contents, Encoding encoding); // Write encoded text with a BOM.

        //Stream Open(string filename); reader or writer?  See File.Open(...);
    }
}

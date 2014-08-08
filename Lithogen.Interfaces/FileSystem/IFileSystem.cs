using System.Collections.Generic;
using System.IO;
using System.Text;

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

        /// <summary>
        /// Reads a file and returns the context as a string. Attempts to guess
        /// the encoding.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>Contents of the file.</returns>
        string ReadAllText(string filename);

        /// <summary>
        /// Reads a file and returns the context as a string. Attempts to guess
        /// the encoding.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>Contents of the file.</returns>
        string ReadAllText(string filename, Encoding encoding);

        /// <summary>
        /// Writes text to a file. This version writes UTF-8 text without a BOM.
        /// </summary>
        /// <param name="filename">The file to write.</param>
        /// <param name="contents">The contents to write.</param>
        void WriteAllText(string filename, string contents);

        /// <summary>
        /// Writes text to a file in the specified encoding.
        /// </summary>
        /// <param name="filename">The file to write.</param>
        /// <param name="contents">The contents to write.</param>
        /// <param name="encoding">The encoding.</param>
        void WriteAllText(string filename, string contents, Encoding encoding);

        //Stream Open(string filename); reader or writer?  See File.Open(...);
    }
}

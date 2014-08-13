using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// Defines the basic operations that a Lithogen file system must support.
    /// Designed to be as minimal as possible, it maps closely to the Windows
    /// file system, yet allows a completely in-memory implementation to be
    /// created fairly easily.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Check for existence of a file.
        /// </summary>
        /// <param name="filename">The file to check.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        bool FileExists(string filename);

        /// <summary>
        /// Check for existence of a directory.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns>True if the directory exists, false otherwise.</returns>
        bool DirectoryExists(string directory);

        /// <summary>
        /// Creates the specified directory.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        void CreateDirectory(string directory);

        /// <summary>
        /// Creates the parent directory of the specified file.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="filename">The filename whose parent directory should be created.</param>
        void CreateParentDirectory(string filename);

        /// <summary>
        /// Writes bytes to the specified file.
        /// If the file exists it is overwritten.
        /// Any needed parent directories will be created.
        /// </summary>
        /// <param name="filename">The file to write to.</param>
        /// <param name="bytes">The bytes to write. Cannot be null, but can be empty.</param>
        void WriteAllBytes(string filename, byte[] bytes);

        /// <summary>
        /// Reads a file as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>The file contents as a byte array.</returns>
        byte[] ReadAllBytes(string filename);

        /// <summary>
        /// Deletes the specified file.
        /// This method can be called on a file that does not exist.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        void DeleteFile(string filename);

        /// <summary>
        /// Deletes the specified directory and all subdirectories.
        /// This method can be called on a directory that does not exist.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        void DeleteDirectory(string directory);

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        IEnumerable<string> EnumerateFiles(string directory);

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) that match a pattern in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <param name="searchPattern">Search pattern to match against files in the directory.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        IEnumerable<string> EnumerateFiles(string directory, string searchPattern);

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) that match a pattern in a specified path,
        /// and optionally searches subdirectories.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <param name="searchPattern">Search pattern to match against files in the directory.</param>
        /// <param name="searchOption">Whether to search the top directory or subdirectories.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Reads a file and returns the context as a string. Attempts to guess
        /// the encoding.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>Contents of the file.</returns>
        string ReadAllText(string filename);

        /// <summary>
        /// Reads a file and returns the context as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="encoding">The assumed encoding of the file.</param>
        /// <returns>Contents of the file, converted to the specified encoding.</returns>
        string ReadAllText(string filename, Encoding encoding);

        /// <summary>
        /// Writes text to a file. This version writes UTF-8 text without a BOM.
        /// If the file exists it is overwritten.
        /// Any needed parent directories will be created.
        /// </summary>
        /// <param name="filename">The file to write.</param>
        /// <param name="contents">The contents to write. Cannot be null but can be the empty string.</param>
        void WriteAllText(string filename, string contents);

        /// <summary>
        /// Writes text to a file in the specified encoding.
        /// If the file exists it is overwritten.
        /// Any needed parent directories will be created.
        /// </summary>
        /// <param name="filename">The file to write.</param>
        /// <param name="contents">The contents to write. Cannot be null but can be the empty string.</param>
        /// <param name="encoding">The encoding.</param>
        void WriteAllText(string filename, string contents, Encoding encoding);

        // TODO Stream Open(string filename); reader or writer?  See File.Open(...);
    }
}

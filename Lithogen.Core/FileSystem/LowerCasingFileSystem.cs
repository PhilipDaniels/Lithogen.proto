using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// The <code>LowerCasingFileSystem</code> can be used to wrap an inner file system.
    /// It will force all file and directory names used as inputs and returned as results
    /// to be converted to lower case. It is an example of the Decorator pattern.
    /// The intention is to make it easy to deploy to Unix systems, which have case-sensitive
    /// file systems.
    /// </summary>
    public class LowerCasingFileSystem : IFileSystem
    {
        readonly IFileSystem WrappedFileSystem;

        public LowerCasingFileSystem(IFileSystem wrappedFileSystem)
        {
            WrappedFileSystem = wrappedFileSystem.ThrowIfNull("wrappedFileSystem");
        }

        /// <summary>
        /// Check for existence of a file.
        /// </summary>
        /// <param name="filename">The file to check.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        public bool FileExists(string filename)
        {
            return WrappedFileSystem.FileExists(MakeLower(filename));
        }

        /// <summary>
        /// Check for existence of a directory.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns>True if the directory exists, false otherwise.</returns>
        public bool DirectoryExists(string directory)
        {
            return WrappedFileSystem.DirectoryExists(MakeLower(directory));
        }

        /// <summary>
        /// Creates the specified directory.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        public void CreateDirectory(string directory)
        {
            WrappedFileSystem.CreateDirectory(MakeLower(directory));
        }

        /// <summary>
        /// Creates the parent directory of the specified file.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="filename">The filename whose parent directory should be created.</param>
        public void CreateParentDirectory(string filename)
        {
            WrappedFileSystem.CreateParentDirectory(MakeLower(filename));
        }

        /// <summary>
        /// Writes bytes to the specified file.
        /// If the file exists it is overwritten.
        /// Any needed parent directories will be created.
        /// </summary>
        /// <param name="filename">The file to write to.</param>
        /// <param name="bytes">The bytes to write. Cannot be null, but can be empty.</param>
        public void WriteAllBytes(string filename, byte[] bytes)
        {
            WrappedFileSystem.WriteAllBytes(MakeLower(filename), bytes);
        }

        /// <summary>
        /// Reads a file as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>The file contents as a byte array.</returns>
        public byte[] ReadAllBytes(string filename)
        {
            return WrappedFileSystem.ReadAllBytes(MakeLower(filename));
        }

        /// <summary>
        /// Deletes the specified file.
        /// This method can be called on a file that does not exist.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public void DeleteFile(string filename)
        {
            WrappedFileSystem.DeleteFile(MakeLower(filename));
        }

        /// <summary>
        /// Deletes the specified directory and all subdirectories.
        /// This method can be called on a directory that does not exist.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public void DeleteDirectory(string directory)
        {
            WrappedFileSystem.DeleteDirectory(MakeLower(directory));
        }

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        public IEnumerable<string> EnumerateFiles(string directory)
        {
            return from f in WrappedFileSystem.EnumerateFiles(MakeLower(directory))
                   select f.ToLowerInvariant();
        }

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) that match a pattern in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <param name="searchPattern">Search pattern to match against files in the directory.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            return from f in WrappedFileSystem.EnumerateFiles(MakeLower(directory), searchPattern)
                   select f.ToLowerInvariant();
        }

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) that match a pattern in a specified path,
        /// and optionally searches subdirectories.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <param name="searchPattern">Search pattern to match against files in the directory.</param>
        /// <param name="searchOption">Whether to search the top directory or subdirectories.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
        {
            return from f in WrappedFileSystem.EnumerateFiles(MakeLower(directory), searchPattern, searchOption)
                   select f.ToLowerInvariant();
        }

        /// <summary>
        /// Reads a file and returns the context as a string. Attempts to guess
        /// the encoding.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>Contents of the file.</returns>
        public string ReadAllText(string filename)
        {
            return WrappedFileSystem.ReadAllText(MakeLower(filename));
        }

        /// <summary>
        /// Reads a file and returns the context as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="encoding">The assumed encoding of the file.</param>
        /// <returns>Contents of the file, converted to the specified encoding.</returns>
        public string ReadAllText(string filename, Encoding encoding)
        {
            return WrappedFileSystem.ReadAllText(MakeLower(filename), encoding);
        }

        /// <summary>
        /// Writes text to a file. This version writes UTF-8 text without a BOM.
        /// If the file exists it is overwritten.
        /// Any needed parent directories will be created.
        /// </summary>
        /// <param name="filename">The file to write.</param>
        /// <param name="contents">The contents to write. Cannot be null but can be the empty string.</param>
        public void WriteAllText(string filename, string contents)
        {
            WrappedFileSystem.WriteAllText(MakeLower(filename), contents);
        }

        /// <summary>
        /// Writes text to a file in the specified encoding.
        /// If the file exists it is overwritten.
        /// Any needed parent directories will be created.
        /// </summary>
        /// <param name="filename">The file to write.</param>
        /// <param name="contents">The contents to write. Cannot be null but can be the empty string.</param>
        /// <param name="encoding">The encoding.</param>
        public void WriteAllText(string filename, string contents, Encoding encoding)
        {
            WrappedFileSystem.WriteAllText(MakeLower(filename), contents, encoding);
        }

        string MakeLower(string filename)
        {
            return filename.ThrowIfNullOrWhiteSpace("filename").ToLowerInvariant();
        }
    }
}

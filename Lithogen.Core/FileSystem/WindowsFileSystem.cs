using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// An implementation of IFileSystem that is backed by Windows (like
    /// using the File and Directory classes directly).
    /// </summary>
    public class WindowsFileSystem : IFileSystem
    {
        /// <summary>
        /// Check for existence of a file.
        /// </summary>
        /// <param name="filename">The file to check.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        public bool FileExists(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            return File.Exists(filename);
        }

        /// <summary>
        /// Check for existence of a directory.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns>True if the directory exists, false otherwise.</returns>
        public bool DirectoryExists(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return Directory.Exists(directory);
        }

        /// <summary>
        /// Creates the specified directory.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        public void CreateDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            Directory.CreateDirectory(directory);
        }

        /// <summary>
        /// Creates the parent directory of the specified file.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="filename">The filename whose parent directory should be created.</param>
        public void CreateParentDirectory(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            string parent = Path.GetDirectoryName(filename);
            CreateDirectory(parent);
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
            filename.ThrowIfNullOrWhiteSpace("filename");
            CreateParentDirectory(filename);
            File.WriteAllBytes(filename, bytes);
        }

        /// <summary>
        /// Reads a file as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>The file contents as a byte array.</returns>
        public byte[] ReadAllBytes(string filename)
        {
            filename.ThrowIfFileDoesNotExist("filename");
            return File.ReadAllBytes(filename);
        }

        /// <summary>
        /// Deletes the specified file.
        /// This method can be called on a file that does not exist.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public void DeleteFile(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            if (File.Exists(filename))
                File.Delete(filename);
        }

        /// <summary>
        /// Deletes the specified directory and all subdirectories.
        /// This method can be called on a directory that does not exist.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public void DeleteDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        public IEnumerable<string> EnumerateFiles(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) that match a pattern in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <param name="searchPattern">Search pattern to match against files in the directory.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            searchPattern.ThrowIfNullOrWhiteSpace("searchPattern");
            return EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
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
            directory.ThrowIfNullOrWhiteSpace("directory");
            searchPattern.ThrowIfNullOrWhiteSpace("searchPattern");
            searchOption.ThrowIfInvalidEnumerand<SearchOption>("searchOption");
            directory.ThrowIfDirectoryDoesNotExist("directory");
            return Directory.EnumerateFiles(directory, searchPattern, searchOption);
        }

        /// <summary>
        /// Reads a file and returns the context as a string. Attempts to guess
        /// the encoding.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>Contents of the file.</returns>
        public string ReadAllText(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            filename.ThrowIfFileDoesNotExist("filename");
            return File.ReadAllText(filename);
        }

        /// <summary>
        /// Reads a file and returns the context as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="encoding">The assumed encoding of the file.</param>
        /// <returns>Contents of the file, converted to the specified encoding.</returns>
        public string ReadAllText(string filename, Encoding encoding)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            encoding.ThrowIfNull("encoding"); 
            filename.ThrowIfFileDoesNotExist("filename");
            return File.ReadAllText(filename, encoding);
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
            filename.ThrowIfNullOrWhiteSpace("filename");
            contents.ThrowIfNull("contents");
            CreateParentDirectory(filename);
            File.WriteAllText(filename, contents);
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
            filename.ThrowIfNullOrWhiteSpace("filename");
            contents.ThrowIfNull("contents");
            encoding.ThrowIfNull("encoding");
            CreateParentDirectory(filename);
            File.WriteAllText(filename, contents, encoding);
        }
    }
}

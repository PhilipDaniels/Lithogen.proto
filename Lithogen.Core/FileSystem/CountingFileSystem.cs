using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lithogen.Core.FileSystem
{
    public class CountingFileSystem : ICountingFileSystem
    {
        readonly IFileSystem WrappedFileSystem;

        /// <summary>
        /// A summary of all files/bytes that were read/written.
        /// </summary>
        public FileSystemStats TotalStats
        {
            get
            {
                var stats = new FileSystemStats();
                foreach (var s in StatsByExtension.Values)
                {
                    stats.FilesRead += s.FilesRead;
                    stats.FilesWritten += s.FilesWritten;
                    stats.BytesRead += s.BytesRead;
                    stats.BytesWritten += s.BytesWritten;
                }
                return stats;
            }
        }

        /// <summary>
        /// A breakdown by extension (which can be the empty string for files
        /// that do not have an extension) of files/bytes that were read/written.
        /// </summary>
        public IDictionary<string, FileSystemStats> StatsByExtension { get; private set; }

        public CountingFileSystem(IFileSystem wrappedFileSystem)
        {
            WrappedFileSystem = wrappedFileSystem.ThrowIfNull("wrappedFileSystem");
            StatsByExtension = new Dictionary<string, FileSystemStats>();
        }

        /// <summary>
        /// Check for existence of a file.
        /// </summary>
        /// <param name="filename">The file to check.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        public bool FileExists(string filename)
        {
            return WrappedFileSystem.FileExists(filename);
        }

        /// <summary>
        /// Check for existence of a directory.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns>True if the directory exists, false otherwise.</returns>
        public bool DirectoryExists(string directory)
        {
            return WrappedFileSystem.DirectoryExists(directory);
        }

        /// <summary>
        /// Creates the specified directory.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        public void CreateDirectory(string directory)
        {
            WrappedFileSystem.CreateDirectory(directory);
        }

        /// <summary>
        /// Creates the parent directory of the specified file.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="filename">The filename whose parent directory should be created.</param>
        public void CreateParentDirectory(string filename)
        {
            WrappedFileSystem.CreateParentDirectory(filename);
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
            WrappedFileSystem.WriteAllBytes(filename, bytes);
            UpdateWriteStats(filename, bytes);
        }

        /// <summary>
        /// Reads a file as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>The file contents as a byte array.</returns>
        public byte[] ReadAllBytes(string filename)
        {
            byte[] bytes = WrappedFileSystem.ReadAllBytes(filename);
            UpdateReadStats(filename, bytes);
            return bytes;
        }

        /// <summary>
        /// Deletes the specified file.
        /// This method can be called on a file that does not exist.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public void DeleteFile(string filename)
        {
            WrappedFileSystem.DeleteFile(filename);
        }

        /// <summary>
        /// Deletes the specified directory and all subdirectories.
        /// This method can be called on a directory that does not exist.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public void DeleteDirectory(string directory)
        {
            WrappedFileSystem.DeleteDirectory(directory);
        }

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        public IEnumerable<string> EnumerateFiles(string directory)
        {
            return WrappedFileSystem.EnumerateFiles(directory);
        }

        /// <summary>
        /// Returns an enumerable collection of files (including full paths) that match a pattern in a specified path.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <param name="searchPattern">Search pattern to match against files in the directory.</param>
        /// <returns>An enumerable of filenames including paths.</returns>
        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            return WrappedFileSystem.EnumerateFiles(directory, searchPattern);
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
            return WrappedFileSystem.EnumerateFiles(directory, searchPattern, searchOption);
        }

        /// <summary>
        /// Reads a file and returns the context as a string. Attempts to guess
        /// the encoding.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>Contents of the file.</returns>
        public string ReadAllText(string filename)
        {
            string result = WrappedFileSystem.ReadAllText(filename);
            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(result);
            UpdateReadStats(filename, byteCount);
            return result;
        }

        /// <summary>
        /// Reads a file and returns the context as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="encoding">The assumed encoding of the file.</param>
        /// <returns>Contents of the file, converted to the specified encoding.</returns>
        public string ReadAllText(string filename, Encoding encoding)
        {
            string result = WrappedFileSystem.ReadAllText(filename, encoding);
            int byteCount = encoding.GetByteCount(result);
            UpdateReadStats(filename, byteCount);
            return result;
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
            WrappedFileSystem.WriteAllText(filename, contents);
            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(contents);
            UpdateWriteStats(filename, byteCount);
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
            WrappedFileSystem.WriteAllText(filename, contents, encoding);
            int byteCount = encoding.GetByteCount(contents);
            UpdateWriteStats(filename, byteCount);
        }



        void UpdateWriteStats(string filename, byte[] bytes)
        {
            UpdateWriteStats(filename, bytes.Length);
        }

        void UpdateWriteStats(string filename, int byteCount)
        {
            FileSystemStats stats = GetStatsForExtension(filename);
            stats.FilesWritten += 1;
            stats.BytesWritten += byteCount;
        }

        void UpdateReadStats(string filename, byte[] bytes)
        {
            UpdateReadStats(filename, bytes.Length);
        }

        void UpdateReadStats(string filename, int byteCount)
        {
            FileSystemStats stats = GetStatsForExtension(filename);
            stats.FilesRead += 1;
            stats.BytesRead += byteCount;
        }

        FileSystemStats GetStatsForExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            FileSystemStats stats;
            if (!StatsByExtension.TryGetValue(extension, out stats))
            {
                stats = new FileSystemStats();
                StatsByExtension[extension] = stats;
            }
            return stats;
        }
    }
}

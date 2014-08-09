using Lithogen.Interfaces.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// An implementation of IFileSystem that is backed by an in-RAM store.
    /// n.b. This file system is case-preserving (like Windows) and case-sensitive (unlike Windows).
    /// </summary>
    public class MemoryFileSystem : IFileSystem
    {
        Dictionary<string, byte[]> Files;
        HashSet<string> Directories;

        public MemoryFileSystem()
        {
            Files = new Dictionary<string, byte[]>();
            Directories = new HashSet<string>();
        }

        /// <summary>
        /// Check for existence of a file.
        /// </summary>
        /// <param name="filename">The file to check.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        public bool FileExists(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            return Files.ContainsKey(filename);
        }

        /// <summary>
        /// Check for existence of a directory.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <returns>True if the directory exists, false otherwise.</returns>
        public bool DirectoryExists(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return Directories.Contains(directory) || Files.Keys.Any(k => k.StartsWith(directory));
        }

        /// <summary>
        /// Creates the specified directory.
        /// This method can be called with a directory that already exists.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        public void CreateDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            Directories.Add(directory);
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
            bytes.ThrowIfNull("bytes");
            CreateParentDirectory(filename);
            Files[filename] = bytes;
        }

        /// <summary>
        /// Reads a file as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <returns>The file contents as a byte array.</returns>
        public byte[] ReadAllBytes(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            if (!FileExists(filename))
                throw new FileNotFoundException("filename");
            return Files[filename];
        }

        /// <summary>
        /// Deletes the specified file.
        /// This method can be called on a file that does not exist.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public void DeleteFile(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            if (Files.ContainsKey(filename))
            {
                Files.Remove(filename);
            }
        }

        /// <summary>
        /// Deletes the specified directory and all subdirectories.
        /// This method can be called on a directory that does not exist.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public void DeleteDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            IEnumerable<string> keys = Files.Keys.Where(k => k.StartsWith(directory)).ToArray();
            foreach (var key in keys)
            {
                Files.Remove(key);
            }
            Directories.RemoveWhere(d => d.StartsWith(directory));
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
            if (!DirectoryExists(directory))
                throw new DirectoryNotFoundException("The directory " + directory + " does not exist.");
            Regex regex = MakeRegexFromSearchPattern(searchPattern);

            var files = from filename in Files.Keys
                        where filename.StartsWith(directory) &&
                              filename.Length > directory.Length &&
                              (searchOption == SearchOption.AllDirectories || FileIsDirectlyInDir(filename, directory)) &&
                              (searchPattern == "*" || (regex != null && regex.IsMatch(filename)))
                        select filename;
            
            return files;
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
            if (!FileExists(filename))
                throw new FileNotFoundException("The file " + filename + " does not exist.");
            return DefaultEncoding.UTF8NoBOM.GetString(Files[filename]);
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
            if (!FileExists(filename))
                throw new FileNotFoundException("The file " + filename + " does not exist.");
            return encoding.GetString(Files[filename]);
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
            // Copy semantics of File.WriteAllText, UTF-8 with no BOM.
            // This is from dotPeek inspection.
            Files[filename] = DefaultEncoding.UTF8NoBOM.GetBytes(contents);
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
            Files[filename] = encoding.GetBytes(contents);
        }

        bool FileIsDirectlyInDir(string filename, string directory)
        {
            return filename.StartsWith(directory) &&
                   filename.IndexOf(Path.DirectorySeparatorChar, directory.Length + 1) == -1;
        }

        Regex MakeRegexFromSearchPattern(string pattern)
        {
            // http://stackoverflow.com/questions/6907720/need-to-perform-wildcard-etc-search-on-a-string-using-regex/16488364#16488364
            var dotdot = pattern.IndexOf("..", StringComparison.Ordinal);
            if (dotdot >= 0)
            {
                for (var i = dotdot; i < pattern.Length; i++)
                    if (pattern[i] != '.')
                        return null;
            }

            var normalized = Regex.Replace(pattern, @"\.+$", "");
            var endsWithDot = normalized.Length != pattern.Length;
            var endWeight = 0;
            if (endsWithDot)
            {
                var lastNonWildcard = normalized.Length - 1;
                for (; lastNonWildcard >= 0; lastNonWildcard--)
                {
                    var c = normalized[lastNonWildcard];
                    if (c == '*')
                        endWeight += short.MaxValue;
                    else if (c == '?')
                        endWeight += 1;
                    else
                        break;
                }

                if (endWeight > 0)
                    normalized = normalized.Substring(0, lastNonWildcard + 1);
            }

            var endsWithWildcardDot = endWeight > 0;
            var endsWithDotWildcardDot = endsWithWildcardDot && normalized.EndsWith(".");
            if (endsWithDotWildcardDot)
                normalized = normalized.Substring(0, normalized.Length - 1);

            normalized = Regex.Replace(normalized, @"(?!^)(\.\*)+$", @".*");

            var escaped = Regex.Escape(normalized);
            string head, tail;

            if (endsWithDotWildcardDot)
            {
                head = "^" + escaped;
                tail = @"(\.[^.]{0," + endWeight + "})?$";
            }
            else if (endsWithWildcardDot)
            {
                head = "^" + escaped;
                tail = "[^.]{0," + endWeight + "}$";
            }
            else
            {
                head = "^" + escaped;
                tail = "$";
            }

            if (head.EndsWith(@"\.\*") && head.Length > 5)
            {
                head = head.Substring(0, head.Length - 4);
                tail = @"(\..*)?" + tail;
            }

            string regexString = head.Replace(@"\*", ".*").Replace(@"\?", "[^.]?") + tail;
            return new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}

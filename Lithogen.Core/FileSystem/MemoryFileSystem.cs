using Lithogen.Interfaces.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

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

        public bool DirectoryExists(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return Directories.Contains(directory) || Files.Keys.Any(k => k.StartsWith(directory));
        }

        public void CreateDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            Directories.Add(directory);
        }

        public void CreateParentDirectory(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            string parent = Path.GetDirectoryName(filename);
            CreateDirectory(parent);
        }

        public void WriteAllBytes(string filename, byte[] bytes)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            bytes.ThrowIfNull("bytes");
            CreateParentDirectory(filename);
            Files[filename] = bytes;
        }

        public byte[] ReadAllBytes(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            if (!FileExists(filename))
                throw new FileNotFoundException("filename");
            return Files[filename];
        }

        public bool FileExists(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            return Files.ContainsKey(filename);
        }

        public void DeleteFile(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            if (Files.ContainsKey(filename))
            {
                Files.Remove(filename);
            }
        }

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

        public IEnumerable<string> EnumerateFiles(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            searchPattern.ThrowIfNullOrWhiteSpace("searchPattern");
            return EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
        }

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

        public string ReadAllText(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            if (!FileExists(filename))
                throw new FileNotFoundException("The file " + filename + " does not exist.");
            return DefaultEncoding.UTF8NoBOM.GetString(Files[filename]);
        }

        public string ReadAllText(string filename, Encoding encoding)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            encoding.ThrowIfNull("encoding");
            if (!FileExists(filename))
                throw new FileNotFoundException("The file " + filename + " does not exist.");
            return encoding.GetString(Files[filename]);
        }

        public void WriteAllText(string filename, string contents)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            contents.ThrowIfNull("contents");
            CreateParentDirectory(filename);
            // Copy semantics of File.WriteAllText, UTF-8 with no BOM.
            // This is from dotPeek inspection.
            Files[filename] = DefaultEncoding.UTF8NoBOM.GetBytes(contents);
        }

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

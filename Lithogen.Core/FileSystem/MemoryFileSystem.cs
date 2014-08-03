using Lithogen.Interfaces.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// An implementation of IFileSystem that is backed by an in-RAM store.
    /// </summary>
    public class MemoryFileSystem : IFileSystem
    {
        // We only hold files. A directory is deemed to exist if there is a
        // key which begins with it.
        Dictionary<string, byte[]> Files;

        public MemoryFileSystem()
        {
            Files = new Dictionary<string, byte[]>();
        }

        public bool DirectoryExists(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return Files.Keys.Any(k => k.StartsWith(directory));
        }

        public void CreateDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            Files[directory] = null;
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
        }

        public IEnumerable<string> EnumerateFiles(string directory)
        {
            return EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            return EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            searchPattern.ThrowIfNullOrWhiteSpace("searchPattern");
            searchOption.ThrowIfInvalidEnumerand<SearchOption>("searchOption");
            if (!DirectoryExists(directory))
                throw new DirectoryNotFoundException("The directory " + directory + " does not exist.");


            // Problem cannot tell that
            //    c:\foo\subdir
            // is supposed to be a subdirectory and not a file!

            var files = from filename in Files.Keys
                        where filename.StartsWith(directory) &&
                              filename.Length > directory.Length &&
                              (searchOption == SearchOption.AllDirectories || FileIsInDir(filename, directory))
                        select filename;

            return files;
            //return Directory.EnumerateFiles(directory, searchPattern, searchOption);
        }

        bool FileIsInDir(string filename, string directory)
        {
            return filename.StartsWith(directory) &&
                   filename.IndexOf(Path.DirectorySeparatorChar, directory.Length + 1) == -1;
        }

        string InternalDirectoryNameRepresentation(string directory)
        {
            if (directory[directory.Length -1 ] == Path.DirectorySeparatorChar)
                return directory;
            else
                return directory + Path.DirectorySeparatorChar;
        }
    }
}

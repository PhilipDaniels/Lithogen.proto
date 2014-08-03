using Lithogen.Interfaces.FileSystem;
using System.Collections.Generic;
using System.IO;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// An implementation of IFileSystem that is backed by Windows (like
    /// using the File and Directory classes directly).
    /// </summary>
    public class WindowsFileSystem : IFileSystem
    {
        public bool DirectoryExists(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return Directory.Exists(directory);
        }

        public void CreateDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            Directory.CreateDirectory(directory);
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
            CreateParentDirectory(filename);
            File.WriteAllBytes(filename, bytes);
        }

        public byte[] ReadAllBytes(string filename)
        {
            filename.ThrowIfFileDoesNotExist("filename");
            return File.ReadAllBytes(filename);
        }

        public bool FileExists(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            return File.Exists(filename);
        }

        public void DeleteFile(string filename)
        {
            filename.ThrowIfNullOrWhiteSpace("filename");
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        public void DeleteDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        public IEnumerable<string> EnumerateFiles(string directory)
        {
            return Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            return Directory.EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            searchPattern.ThrowIfNullOrWhiteSpace("searchPattern");
            searchOption.ThrowIfInvalidEnumerand<SearchOption>("searchOption");
            directory.ThrowIfDirectoryDoesNotExist("directory");
            return Directory.EnumerateFiles(directory, searchPattern, searchOption);
        }
    }
}

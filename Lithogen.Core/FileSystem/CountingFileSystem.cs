using Lithogen.Interfaces.FileSystem;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lithogen.Core.FileSystem
{
    public class CountingFileSystem : ICountingFileSystem
    {
        readonly IFileSystem WrappedFileSystem;
        public FileSystemStats TotalStats { get; private set; }
        public IDictionary<string, FileSystemStats> StatsByExtension { get; private set; }

        public CountingFileSystem(IFileSystem wrappedFileSystem)
        {
            WrappedFileSystem = wrappedFileSystem.ThrowIfNull("wrappedFileSystem");
            TotalStats = new FileSystemStats();
            StatsByExtension = new Dictionary<string, FileSystemStats>();
        }

        public bool FileExists(string filename)
        {
            return WrappedFileSystem.FileExists(filename);
        }

        public bool DirectoryExists(string directory)
        {
            return WrappedFileSystem.DirectoryExists(directory);
        }

        public void CreateDirectory(string directory)
        {
            WrappedFileSystem.CreateDirectory(directory);
        }

        public void CreateParentDirectory(string filename)
        {
            WrappedFileSystem.CreateParentDirectory(filename);
        }

        public void WriteAllBytes(string filename, byte[] bytes)
        {
            WrappedFileSystem.WriteAllBytes(filename, bytes);
            UpdateWriteStats(filename, bytes);
        }

        public byte[] ReadAllBytes(string filename)
        {
            byte[] bytes = WrappedFileSystem.ReadAllBytes(filename);
            UpdateReadStats(filename, bytes);
            return bytes;
        }

        public void DeleteFile(string filename)
        {
            WrappedFileSystem.DeleteFile(filename);
        }

        public void DeleteDirectory(string directory)
        {
            WrappedFileSystem.DeleteDirectory(directory);
        }

        public IEnumerable<string> EnumerateFiles(string directory)
        {
            return WrappedFileSystem.EnumerateFiles(directory);
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            return WrappedFileSystem.EnumerateFiles(directory, searchPattern);
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
        {
            return WrappedFileSystem.EnumerateFiles(directory, searchPattern, searchOption);
        }

        public string ReadAllText(string filename)
        {
            string result = WrappedFileSystem.ReadAllText(filename);
            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(result);
            UpdateReadStats(filename, byteCount);
            return result;
        }

        public string ReadAllText(string filename, Encoding encoding)
        {
            string result = WrappedFileSystem.ReadAllText(filename, encoding);
            int byteCount = encoding.GetByteCount(result);
            UpdateReadStats(filename, byteCount);
            return result;
        }

        public void WriteAllText(string filename, string contents)
        {
            WrappedFileSystem.WriteAllText(filename, contents);
            int byteCount = DefaultEncoding.UTF8NoBOM.GetByteCount(contents);
            UpdateWriteStats(filename, byteCount);
        }

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
            TotalStats.FilesWritten += 1;
            TotalStats.BytesWritten += byteCount;
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
            TotalStats.FilesRead += 1;
            TotalStats.BytesRead += byteCount;
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

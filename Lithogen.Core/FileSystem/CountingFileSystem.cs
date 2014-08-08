using Lithogen.Interfaces.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public void CreateDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public void CreateParentDirectory(string filename)
        {
            throw new NotImplementedException();
        }

        public void WriteAllBytes(string filename, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadAllBytes(string filename)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string filename)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string directory)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern, System.IO.SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public string ReadAllText(string filename)
        {
            throw new NotImplementedException();
        }

        public string ReadAllText(string filename, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public void WriteAllText(string filename, string contents)
        {
            throw new NotImplementedException();
        }

        public void WriteAllText(string filename, string contents, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}

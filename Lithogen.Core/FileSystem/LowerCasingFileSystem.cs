using Lithogen.Interfaces.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// The <code>LowerCasingFileSystem</code> can be used to wrap an inner file system.
    /// It will force all file and directory names used to be converted to lower case.
    /// It is an example of the Decorator pattern.
    /// </summary>
    public class LowerCasingFileSystem : IFileSystem
    {
        readonly IFileSystem WrappedFileSystem;

        public LowerCasingFileSystem(IFileSystem wrappedFileSystem)
        {
            WrappedFileSystem = wrappedFileSystem.ThrowIfNull("wrappedFileSystem");
        }

        public bool FileExists(string filename)
        {
            return WrappedFileSystem.FileExists(MakeLower(filename));
        }

        public bool DirectoryExists(string directory)
        {
            return WrappedFileSystem.DirectoryExists(MakeLower(directory));
        }

        public void CreateDirectory(string directory)
        {
            WrappedFileSystem.CreateDirectory(MakeLower(directory));
        }

        public void CreateParentDirectory(string filename)
        {
            WrappedFileSystem.CreateParentDirectory(MakeLower(filename));
        }

        public void WriteAllBytes(string filename, byte[] bytes)
        {
            WrappedFileSystem.WriteAllBytes(MakeLower(filename), bytes);
        }

        public byte[] ReadAllBytes(string filename)
        {
            return WrappedFileSystem.ReadAllBytes(MakeLower(filename));
        }

        public void DeleteFile(string filename)
        {
            WrappedFileSystem.DeleteFile(MakeLower(filename));
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

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
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

        string MakeLower(string filename)
        {
            return filename.ThrowIfNullOrWhiteSpace("filename").ToLowerInvariant();
        }
    }
}

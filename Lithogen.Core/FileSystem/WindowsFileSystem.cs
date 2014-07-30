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

        // byte[] ReadAllBytes(string filename);

    }
}

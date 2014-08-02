using Lithogen.Interfaces.FileSystem;
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
        Dictionary<string, bool> Files;

        public MemoryFileSystem()
        {
            Files = new Dictionary<string, bool>();
        }

        public bool DirectoryExists(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            return Files.Keys.Any(k => k.StartsWith(directory));
        }

        public void CreateDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");
            Files[directory] = true;
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
        }
    }
}

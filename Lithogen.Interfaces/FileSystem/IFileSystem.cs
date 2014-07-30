using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Interfaces.FileSystem
{
    /// <summary>
    /// Defines the basic operations that a Lithogen file system must support.
    /// Designed to be as minimal as possible, it maps closely to the Windows
    /// file system, yet allows a completely in-memory implementation to be
    /// created fairly easily.
    /// </summary>
    public interface IFileSystem
    {
        bool DirectoryExists(string directory); 
        void CreateDirectory(string directory);
        void CreateParentDirectory(string filename);

        void WriteAllBytes(string filename, byte[] bytes);
    }
}

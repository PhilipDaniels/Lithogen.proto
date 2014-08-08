using Lithogen.Interfaces.FileSystem;
using System.Collections.Generic;

namespace Lithogen.Core.FileSystem
{
    public interface ICountingFileSystem : IFileSystem
    {
        FileSystemStats TotalStats { get; }
        IDictionary<string, FileSystemStats> StatsByExtension { get; }
    }
}

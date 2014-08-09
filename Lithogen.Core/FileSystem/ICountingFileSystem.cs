using Lithogen.Interfaces.FileSystem;
using System.Collections.Generic;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// An ICountingFileSystem can be used to wrap an IFileSystem.
    /// It keeps a count of how many files/bytes were read/written.
    /// </summary>
    public interface ICountingFileSystem : IFileSystem
    {
        /// <summary>
        /// A summary of all files/bytes that were read/written.
        /// </summary>
        FileSystemStats TotalStats { get; }

        /// <summary>
        /// A breakdown by extension (which can be the empty string for files
        /// that do not have an extension) of files/bytes that were read/written.
        /// </summary>
        IDictionary<string, FileSystemStats> StatsByExtension { get; }
    }
}

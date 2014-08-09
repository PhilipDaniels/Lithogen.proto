namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// Represents counts of files and bytes read and written.
    /// </summary>
    public class FileSystemStats
    {
        /// <summary>
        /// The number of files read.
        /// </summary>
        public int FilesRead { get; set; }

        /// <summary>
        /// The number of files written.
        /// </summary>
        public int FilesWritten { get; set; }

        /// <summary>
        /// The number of bytes read.
        /// </summary>
        public int BytesRead { get; set; }

        /// <summary>
        /// The number of bytes written.
        /// </summary>
        public int BytesWritten { get; set; }
    }
}

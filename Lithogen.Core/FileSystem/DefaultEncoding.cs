using System.Text;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// The WriteAllText and ReadAllText methods in the File class that do not take an encoding
    /// use a "default encoding" that corresponds to UTF8 without a BOM (verified using dotPeek).
    /// In various places we want to emulate those semantics.
    /// </summary>
    public static class DefaultEncoding
    {
        /// <summary>
        /// The default encoding to use for text file operations when the
        /// caller does not specify an encoding.
        /// </summary>
        public static readonly Encoding UTF8NoBOM = new UTF8Encoding(false, true);

        /*
        // Taken from dotPeek of StreamWriter.cs, which does it like this.
        internal static Encoding UTF8NoBOM
        {
            get
            {
                if (_UTF8NoBOM == null)
                {
                    UTF8Encoding utf8Encoding = new UTF8Encoding(false, true);
                    Thread.MemoryBarrier();
                    _UTF8NoBOM = (Encoding)utf8Encoding;
                }
                return _UTF8NoBOM;
            }
        }
        static volatile Encoding _UTF8NoBOM;
        */
    }
}

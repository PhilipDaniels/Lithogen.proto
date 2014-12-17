using System.Collections.Generic;
using System.IO;

namespace Lithogen.Core
{
    public class FileFilter : IFileFilter
    {
        public FileFilter()
        {
            ExtensionsToIgnore = new HashSet<string>();
            ExtensionsToIgnore.Add(".gitignore");
        }

        public bool ShouldProcessFile(string file)
        {
            file.ThrowIfNullOrWhiteSpace("file");

            string extension = Path.GetExtension(file).ToLowerInvariant();
            return !ExtensionsToIgnore.Contains(extension);
        }

        public ICollection<string> ExtensionsToIgnore { get; private set; }
    }
}

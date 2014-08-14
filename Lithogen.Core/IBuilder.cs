using Lithogen.Core.FileSystem;
using System.Collections.Generic;

namespace Lithogen.Core
{
    public interface IBuilder
    {
        ICountingFileSystem FileSystem { get; }
        IList<IBuildStep> Steps { get; }
        bool Build();
    }
}

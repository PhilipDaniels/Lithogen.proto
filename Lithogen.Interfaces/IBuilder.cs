using Lithogen.Interfaces.FileSystem;
using System.Collections.Generic;

namespace Lithogen.Interfaces
{
    public interface IBuilder
    {
        IBuildContext BuildContext { get; }
        IFileSystem FileSystem { get; }
        IList<IBuildStep> Steps { get; }
        bool Build();
    }
}

using Lithogen.Core.FileSystem;

namespace Lithogen.Core.BuildSteps
{
    public interface IBuildStep
    {
        IFileSystem FileSystem { get; }
        Settings Settings { get; }
        ILogger Logger { get; }
        string Name { get; set; }
        bool Execute();
    }
}

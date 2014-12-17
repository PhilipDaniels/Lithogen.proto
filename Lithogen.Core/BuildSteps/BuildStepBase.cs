using Lithogen.Core.FileSystem;

namespace Lithogen.Core.BuildSteps
{
    public abstract class BuildStepBase : IBuildStep
    {
        public IFileSystem FileSystem { get; private set; }
        public Settings Settings { get; private set; }
        public ILogger Logger { get; private set; }
        public string Name { get; set; }

        public BuildStepBase(IFileSystem fileSystem, Settings settings, ILogger logger)
        {
            FileSystem = fileSystem.ThrowIfNull("fileSystem");
            Settings = settings.ThrowIfNull("settings");
            Logger = logger.ThrowIfNull("logger");
        }

        public abstract bool Execute();
    }
}

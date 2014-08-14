using Lithogen.Core.FileSystem;
using System.Threading;

namespace Lithogen.Core.BuildSteps
{
    public class AssetBuildStep : IBuildStep
    {
        public IFileSystem FileSystem { get; private set; }
        public Settings Settings { get; private set; }

        public string Name { get; set; }

        public AssetBuildStep(IFileSystem fileSystem, Settings settings)
        {
            FileSystem = fileSystem.ThrowIfNull("fileSystem");
            Settings = settings.ThrowIfNull("settings");
            Name = "AssetBuildStep";
        }

        public bool Execute()
        {
            Thread.Sleep(500);
            return true;
        }
    }
}

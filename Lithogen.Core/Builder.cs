using Lithogen.Core.BuildSteps;
using Lithogen.Core.FileSystem;

namespace Lithogen.Core
{
    public class Builder : BuilderBase
    {
        public Builder(ICountingFileSystem fileSystem, ILogger logger, Settings settings)
            : base(fileSystem, logger, settings)
        {
        }

        public override void InitialiseBuildSteps()
        {
            Steps.Add(new AssetBuildStep(FileSystem, Settings, Logger));
            Steps.Add(new CopyImagesBuildStep(FileSystem, Settings, Logger, new FileFilter()));
            Steps.Add(new RazorBuildStep(FileSystem, Settings, Logger));
        }
    }
}

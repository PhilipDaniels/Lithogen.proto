using Lithogen.Core.FileSystem;

namespace Lithogen.Core.BuildSteps
{
    public class AssetBuildStep : BuildStepBase
    {
        public AssetBuildStep(IFileSystem fileSystem, Settings settings, ILogger logger)
            : base(fileSystem, settings, logger)
        {
            Name = "AssetBuildStep";
        }

        public override bool Execute()
        {
            //Thread.Sleep(500);
            return true;
        }
    }
}

using Lithogen.Core.BuildSteps;
using Lithogen.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core
{
    public class Builder : BuilderBase
    {
        public Builder(ICountingFileSystem fileSystem, ILogger logger, Settings settings)
            :base(fileSystem, logger, settings)
        {
            Steps.Add(new AssetBuildStep(FileSystem, settings));
            Steps.Add(new RazorBuildStep(FileSystem, settings));
        }
    }
}

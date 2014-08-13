using Lithogen.Core.FileSystem;
using Lithogen.Interfaces;
using Lithogen.Interfaces.FileSystem;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core
{
    public class Builder : IBuilder
    {
        public IFileSystem FileSystem { get { return _FileSystem; } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly IFileSystem _FileSystem;

        public IList<IBuildStep> Steps { get; private set; }

        public Builder(IFileSystem fileSystem)
        {
            _FileSystem = fileSystem.ThrowIfNull("fileSystem");
            Steps = new List<IBuildStep>();
        }

        public bool Build()
        {
            //Logger.Msg("Builder.Build() starting.");
            //Logger.Indentation += 1;

            // Process css and js assets
            // Copy images
            // Copy various files from the root of the website
            // Find the layouts and load them into a RazorBuilder
            // Proces the views

            //Logger.Msg("Builder.Build() completed.");





            //int stepNum = 1;
            //foreach (var step in Steps)
            //{
            //    DateTime start = DateTime.Now;
            //    logger.Msg("Starting {0} ({1}/{2})", step.Name, stepNum, Steps.Count);
            //    bool ok = step.Execute(BuildContext);
            //    double timeTaken = (DateTime.Now - start).TotalSeconds;
            //    if (ok)
            //    {
            //        logger.Msg("Completed {0} ({1:0.00} seconds).", step.Name, timeTaken);
            //    }
            //    else
            //    {
            //        logger.Msg("BuildStep {0} returned false, aborting build ({1:0.00} seconds).", step.Name, timeTaken);
            //        return false;
            //    }

            //    stepNum++;
            //}

            return true;
        }
    }
}

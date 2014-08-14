using Lithogen.Core.BuildSteps;
using Lithogen.Core.FileSystem;
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
    public abstract class BuilderBase : IBuilder
    {
        public ICountingFileSystem FileSystem { get; private set; }
        public ILogger Logger { get; private set; }
        public Settings Settings { get; private set; }
        public IList<IBuildStep> Steps { get; private set; }

        public BuilderBase(ICountingFileSystem fileSystem, ILogger logger, Settings settings)
        {
            FileSystem = fileSystem.ThrowIfNull("fileSystem");
            Logger = logger.ThrowIfNull("logger");
            Settings = settings.ThrowIfNull("settings");
            Steps = new List<IBuildStep>();
        }

        public virtual bool Build()
        {
            Logger.Prefix = "Builder.Build() ";
            Logger.Msg("Starting.");

            // Process css and js assets
            // Copy images
            // Copy various files from the root of the website
            // Find the layouts and load them into a RazorBuilder
            // Proces the views

            int stepNum = 1;
            foreach (var step in Steps)
            {
                Logger.Msg("Starting {0} ({1} of {2}).", step.Name, stepNum, Steps.Count);
                bool ok = step.Execute();
                if (ok)
                {
                    Logger.Msg("Completed {0}.", step.Name);
                }
                else
                {
                    Logger.Msg("BuildStep {0} returned false, aborting build.", step.Name);
                    return false;
                }

                stepNum++;
            }

            WriteFileSystemReport(FileSystem, Logger);
            Logger.Msg("Completed.");

            return true;
        }

        public static void WriteFileSystemReport(ICountingFileSystem fileSystem, ILogger logger)
        {
            fileSystem.ThrowIfNull("fileSystem");
            logger.ThrowIfNull("logger");

            logger.Msg("File I/O Report");
            logger.Msg("===========================================================");
            logger.Msg("Extension  FilesRead  FilesWritten  BytesRead  BytesWritten");
            logger.Msg("---------  ---------  ------------  ---------  ------------");
            foreach (var stat in fileSystem.StatsByExtension.OrderBy(kvp => kvp.Key))
            {
                logger.Msg(stat.Key.PadLeft(9) + GetStatsReportLine(stat.Value));
            }
            logger.Msg("---------  ---------  ------------  ---------  ------------");
            logger.Msg("  Totals:  " + GetStatsReportLine(fileSystem.TotalStats));
            logger.Msg("===========================================================");
        }

        public static string GetStatsReportLine(FileSystemStats stats)
        {
            return stats.FilesRead.ToString().PadLeft(9) +
                   "  " +
                   stats.FilesWritten.ToString().PadLeft(12) +
                   "  " +
                   stats.BytesRead.ToString().PadLeft(9) +
                   "  " +
                   stats.BytesWritten.ToString().PadLeft(12);
        }
    }
}

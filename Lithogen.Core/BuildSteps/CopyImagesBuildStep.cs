using Lithogen.Core.FileSystem;
using System;
using System.IO;

namespace Lithogen.Core.BuildSteps
{
    public class CopyImagesBuildStep : BuildStepBase
    {
        readonly IFileFilter FileFilter;

        public CopyImagesBuildStep(IFileSystem fileSystem, Settings settings, ILogger logger, IFileFilter fileFilter)
            : base(fileSystem, settings, logger)
        {
            FileFilter = fileFilter.ThrowIfNull("fileFilter");
            Name = "CopyImagesBuildStep";
        }

        public override bool Execute()
        {
            Logger.PushPrefix("CopyImagesBuildStep.Execute() ");

            if (String.IsNullOrWhiteSpace(Settings.ImagesDirectory))
            {
                Logger.Msg("ImagesDirectory is not set. Exiting.");
                return true;
            }

            int numFilesCopied = 0;
            int numFilesIgnored = 0;
            foreach (var filename in FileSystem.EnumerateFiles(Settings.ImagesDirectory, "*", SearchOption.AllDirectories))
            {
                if (!FileFilter.ShouldProcessFile(filename))
                {
                    numFilesIgnored++;
                    continue;
                }

                string child = PathUtils.GetSubPath(Settings.ImagesDirectory, filename);
                string destination = Path.Combine(Settings.OutputDirectory, child);
                Logger.Msg("Copying {0} to {1}", filename, destination);
                byte[] data = FileSystem.ReadAllBytes(filename);
                FileSystem.WriteAllBytes(destination, data);
                numFilesCopied++;
            }
            Logger.Msg("Copied {0} files, ignored {1}.", numFilesCopied, numFilesIgnored);
            Logger.PopPrefix();
            return true;
        }
    }
}

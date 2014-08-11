using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace Lithogen.TaskShim
{
    /// <summary>
    /// This Shim is loaded by Visual Studio when a build is started and then never unloaded until you
    /// close Visual Studio. Therefore, it has to be a simple DLL that defers all the work onto the
    /// Lithogen subprocess.
    /// </summary>
    [Serializable]
    public class LithogenShim : AppDomainIsolatedTask
    {
        /// <summary>
        /// The full path of the solution file. This will not be set if you run MSBuild
        /// from the command line, it is only set within Visual Studio. So it is best to
        /// avoid creating tasks that rely on it.
        /// </summary>
        public string SolutionPath { get; set; }

        /// <summary>
        /// The full path of the project file (.csproj) that you want Lithogen to build.
        /// </summary>
        [Required]
        public string ProjectPath { get; set; }

        /// <summary>
        /// The build configuration.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// The full path of the Lithogen.Core.dll file.
        /// </summary>
        [Required]
        public string LithogenExeFile { get; set; }

        /// <summary>
        /// The importance of messages logged by the build. Defaults to HIGH (chatty).
        /// </summary>
        public string MessageImportance { get; set; }
        MessageImportance _MessageImportance;

        const string MsgPrefix = "Lithogen.TaskShim.LithogenShim.Execute() ";

        public override bool Execute()
        {
            DateTime start = DateTime.Now;
            _MessageImportance = ValidateMessageImportance();
            ValidateLithogenExeFile();

            // Run Lithogen.exe as a separate process. All messages, including error messages,
            // will be received on standard output. It is the format of the message that
            // defines it as an error.
            using (var p = new Process())
            {
                p.StartInfo.FileName = LithogenExeFile;
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(LithogenExeFile);
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += p_OutputDataReceived;

                Log.LogMessage(_MessageImportance, MsgPrefix + "Attempting to start {0}.", LithogenExeFile);

                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
            }

            return true;
        }

        /// <summary>
        /// This is executed when the child process writes a line to standard output.
        /// </summary>
        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.Data))
                return;
            Log.LogMessage(_MessageImportance, e.Data);
            Log.
        }

        MessageImportance ValidateMessageImportance()
        {
            if (String.IsNullOrWhiteSpace(MessageImportance) || MessageImportance.Equals("High", StringComparison.InvariantCultureIgnoreCase))
            {
                return Microsoft.Build.Framework.MessageImportance.High;
            }
            else if (MessageImportance.Equals("Normal", StringComparison.InvariantCultureIgnoreCase))
            {
                return Microsoft.Build.Framework.MessageImportance.Normal;
            }
            else if (MessageImportance.Equals("Low", StringComparison.InvariantCultureIgnoreCase))
            {
                return Microsoft.Build.Framework.MessageImportance.Low;
            }
            else
            {
                string msg = "Invalid MessageImportance of '" + MessageImportance + "'. Valid values are High, Normal and Low. High is the default.";
                throw new ArgumentOutOfRangeException(msg);
            }
        }

        void ValidateLithogenExeFile()
        {
            if (String.IsNullOrWhiteSpace(LithogenExeFile))
                throw new ArgumentNullException("LithogenExeFile must be set. Without it we cannot run the build.");
            LithogenExeFile = LithogenExeFile.Trim();
            if (!File.Exists(LithogenExeFile))
                throw new FileNotFoundException("The LithogenExeFile '" + LithogenExeFile + "' does not exist. Without it we cannot run the build.");
        }
    }
}

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lithogen.TaskShim
{
    /// <summary>
    /// This Shim is loaded by Visual Studio when a build is started and then never unloaded until you
    /// close Visual Studio. Therefore, it has to be a simple DLL that defers all the work onto the
    /// Lithogen subprocess.
    /// </summary>
    public class LithogenShim : Task
    {
        #region Things needed by the shim
        /// <summary>
        /// The full path of the Lithogen.exe file.
        /// </summary>
        [Required]
        public string LithogenExeFile { get; set; }

        /// <summary>
        /// The importance of messages logged by the build. Defaults to HIGH (chatty).
        /// </summary>
        public string MessageImportance { get; set; }
        MessageImportance _MessageImportance;

        const string MsgPrefix = "Lithogen.TaskShim.LithogenShim.Execute() ";
        #endregion

        #region Things passed on to the Lithogen.exe process
        /// <summary>
        /// Path to the (optional) Lithogen.exe.config configuration file.
        /// </summary>
        public string ConfigurationFile { get; set; }

        /// <summary>
        /// The full path of the solution file. This will not be set if you run MSBuild
        /// from the command line, it is only set within Visual Studio. So it is best to
        /// avoid creating tasks that rely on it.
        /// </summary>
        public string SolutionFile { get; set; }

        /// <summary>
        /// The full path of the project file (.csproj) that you want Lithogen to build.
        /// This can be used to derive various other folders.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// The build configuration - Debug, Release etc.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// The directory that contains CSS, Less, SASS etc.
        /// Optional, a default will be  assumed if this is not set.
        /// </summary>
        public string CssDirectory { get; set; }

        /// <summary>
        /// The directory that contains images.
        /// Optional, a default will be  assumed if this is not set.
        /// </summary>
        public string ImagesDirectory { get; set; }

        /// <summary>
        /// The directory that contains JavaScript.
        /// Optional, a default will be  assumed if this is not set.
        /// </summary>
        public string ScriptsDirectory { get; set; }

        /// <summary>
        /// The directory that contains the Models.
        /// Optional, a default will be  assumed if this is not set.
        /// </summary>
        public string ModelsDirectory { get; set; }

        /// <summary>
        /// The directory that contains the Views.
        /// Optional, a default will be  assumed if this is not set.
        /// </summary>
        public string ViewsDirectory { get; set; }
        #endregion


        public override bool Execute()
        {
            DateTime start = DateTime.Now;
            _MessageImportance = ValidateMessageImportance();
            ValidateLithogenExeFile();

            // Run Lithogen.exe as a separate process. All messages, including error messages,
            // will be received on standard output (it is the format of the message that
            // defines it as an error). 
            // n.b. There is a known bug in Visual Studio that causes it to buffer all output
            // from a task until the task is complete, so nothing will appear in the output
            // window until everything is complete.
            using (var p = new Process())
            {
                p.StartInfo.FileName = LithogenExeFile;
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(LithogenExeFile);
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.Arguments = GetCommandLineArguments();
                p.OutputDataReceived += p_OutputDataReceived;

                Log.LogMessage(_MessageImportance, MsgPrefix + "Attempting to start {0} {1}", LithogenExeFile, p.StartInfo.Arguments);

                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
            }

            return true;
        }

        string GetCommandLineArguments()
        {
            var sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(ConfigurationFile))
                sb.AppendFormat(" /cf=\"{0}\"", ConfigurationFile);

            if (!String.IsNullOrWhiteSpace(SolutionFile))
                sb.AppendFormat(" /sf=\"{0}\"", SolutionFile);
            if (!String.IsNullOrWhiteSpace(ProjectFile))
                sb.AppendFormat(" /pf=\"{0}\"", ProjectFile);
            if (!String.IsNullOrWhiteSpace(Configuration))
                sb.AppendFormat(" /co=\"{0}\"", Configuration);
            if (!String.IsNullOrWhiteSpace(CssDirectory))
                sb.AppendFormat(" /css=\"{0}\"", CssDirectory);
            if (!String.IsNullOrWhiteSpace(ImagesDirectory))
                sb.AppendFormat(" /img=\"{0}\"", ImagesDirectory);
            if (!String.IsNullOrWhiteSpace(ScriptsDirectory))
                sb.AppendFormat(" /js=\"{0}\"", ScriptsDirectory);
            if (!String.IsNullOrWhiteSpace(ModelsDirectory))
                sb.AppendFormat(" /mo=\"{0}\"", ModelsDirectory);
            if (!String.IsNullOrWhiteSpace(ViewsDirectory))
                sb.AppendFormat(" /vw=\"{0}\"", ViewsDirectory);

            return sb.ToString();
        }

        /// <summary>
        /// This is executed when the child process writes a line to standard output.
        /// </summary>
        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.Data))
                return;
            Log.LogMessage(_MessageImportance, e.Data);
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

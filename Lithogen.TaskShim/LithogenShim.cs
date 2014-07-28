using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Lithogen.TaskShim
{
    /// <summary>
    /// This Shim is loaded by Visual Studio when a build is started and then never unloaded until you
    /// close Visual Studio. Therefore, it has to be a simple DLL that defers all the work onto the
    /// Lithogen.Core.dll (which we can replace during compile time). That DLL in turn, is called
    /// to Bootstrap the build - the Bootstrap will dynamically load custom builders, etc.
    /// before it begins executing the build.
    /// <remarks>
    /// In order the really ensure things stay uncoupled from the core we invoke the Bootstrap()
    /// method by reflection rather than taking a dependency on an IBootstrap interface. This is
    /// slightly clunky but this code is never going to change.
    /// </remarks>
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
        public string LithogenCoreFile { get; set; }

        /// <summary>
        /// The path for Lithogen plugings. This is optional, Lithogen.Core can do
        /// a build without any extra help.
        /// </summary>
        public string LithogenPluginDir { get; set; }

        /// <summary>
        /// The importance of messages logged by the build. Defaults to HIGH (chatty).
        /// </summary>
        public string MessageImportance { get; set; }

        const string MsgPrefix = "Lithogen.TaskShim.LithogenShim.Execute() ";

        public override bool Execute()
        {
            DateTime start = DateTime.Now;
            MessageImportance msgImp = ValidateMessageImportance();
            Log.LogMessage(msgImp, MsgPrefix + "Starting. This AppDomain.Id is {0} and FriendlyName is '{1}'.", AppDomain.CurrentDomain.Id, AppDomain.CurrentDomain.FriendlyName);
            Log.LogMessage(msgImp, MsgPrefix + "The AppDomain.CurrentDomain.BaseDirectory is {0}.", AppDomain.CurrentDomain.BaseDirectory);
            Log.LogMessage(msgImp, MsgPrefix + "The Thread.CurrentThread.ManagedThreadId is {0}.", Thread.CurrentThread.ManagedThreadId);
            ValidateLithogenCoreFile();

            Log.LogMessage(msgImp, MsgPrefix + "Attempting to load LithogenCoreFile from {0}.", LithogenCoreFile);
            Assembly coreAssembly = Assembly.LoadFrom(LithogenCoreFile);
            if (coreAssembly == null)
                throw new Exception("Could not load LithogenCoreFile from " + LithogenCoreFile);
            Log.LogMessage(msgImp, MsgPrefix + "LithogenCoreFile loaded successfully.");

            object bootstrapper = coreAssembly.CreateInstance("Lithogen.Core.Bootstrapper", true);
            if (bootstrapper == null)
                throw new Exception("Could not create Lithogen.Core.Bootstrapper object.");
            Log.LogMessage(msgImp, MsgPrefix + "Lithogen.Core.Bootstrapper object created successfully.");

            Type bootstrapperType = bootstrapper.GetType();
            SetProperty(msgImp, bootstrapper, bootstrapperType, "BuildEngine", BuildEngine);
            SetProperty(msgImp, bootstrapper, bootstrapperType, "HostObject", HostObject);
            SetProperty(msgImp, bootstrapper, bootstrapperType, "SolutionPath", SolutionPath);
            SetProperty(msgImp, bootstrapper, bootstrapperType, "ProjectPath", ProjectPath);
            SetProperty(msgImp, bootstrapper, bootstrapperType, "Configuration", Configuration);
            SetProperty(msgImp, bootstrapper, bootstrapperType, "LithogenPluginDir", LithogenPluginDir);
            SetProperty(msgImp, bootstrapper, bootstrapperType, "MessageImportance", msgImp);     // Note, this is a enum now.

            MethodInfo exec = bootstrapperType.GetMethod("Bootstrap");
            if (exec == null)
                throw new Exception("Could not get hold of the Bootstrap() method from the Bootstrapper object.");
            Log.LogMessage(msgImp, MsgPrefix + "About to invoke the Bootstrap() method.");
            bool result = (bool)exec.Invoke(bootstrapper, null);
            double timeTaken = (DateTime.Now - start).TotalSeconds;
            if (!result)
            {
                Log.LogError(MsgPrefix + "failed ({0:0.00} seconds).", timeTaken);
            }
            else
            {
                Log.LogMessage(msgImp, MsgPrefix + "completed OK ({0:0.00} seconds).", timeTaken);
            }

            return result;
        }

        void SetProperty(MessageImportance imp, object bootstrapper, Type bootstrapperType, string property, object value)
        {
            PropertyInfo prop = bootstrapperType.GetProperty(property);
            MethodInfo setter = prop.GetSetMethod();
            setter.Invoke(bootstrapper, new object[] { value });
            Log.LogMessage(imp, MsgPrefix + "Set Bootstrapper.{0} to {1}.", property, value);
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

        void ValidateLithogenCoreFile()
        {
            if (String.IsNullOrWhiteSpace(LithogenCoreFile))
                throw new ArgumentOutOfRangeException("LithogenCoreFile must be set. Without it we cannot Bootstrap the build.");
            LithogenCoreFile = LithogenCoreFile.Trim();
            if (!File.Exists(LithogenCoreFile))
                throw new FileNotFoundException("The LithogenCoreFile '" + LithogenCoreFile + "' does not exist. Without it we cannot Bootstrap the build.");
        }
    }
}

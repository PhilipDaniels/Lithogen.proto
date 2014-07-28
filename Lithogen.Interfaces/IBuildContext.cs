using Microsoft.Build.Framework;
using System.Collections.Generic;

namespace Lithogen.Interfaces
{
    /// <summary>
    /// The IBuildContext represents a sort of global context that the build is operating in.
    /// As such, it contains important information such as the ProjectPath that is passed all the way
    /// in from the .csproj file. The BuildContext is associated with each step, so it
    /// can be used to pass data from one step to the next.
    /// </summary>
    public interface IBuildContext
    {
        /// <summary>
        /// The full path of the solution file. This will not be set if you run MSBuild
        /// from the command line, it is only set within Visual Studio. So it is best to
        /// avoid creating tasks that rely on it.
        /// </summary>
        string SolutionPath { get; set; }

        /// <summary>
        /// The full path of the project file (.csproj) that you want Lithogen to build.
        /// </summary>
        string ProjectPath { get; set; }

        /// <summary>
        /// The build configuration.
        /// </summary>
        string Configuration { get; set; }

        /// <summary>
        /// The importance of messages.
        /// </summary>
        MessageImportance MessageImportance { get; set; }

        /// <summary>
        /// The MSBuild BuildEngine that we are operating under.
        /// </summary>
        IBuildEngine BuildEngine { get; set; }

        /// <summary>
        /// The task host that we are operating under.
        /// </summary>
        ITaskHost HostObject { get; set; }

        /// <summary>
        /// A dictionary of data that can be used for any purpose. Empty by default.
        /// </summary>
        IDictionary<string, object> TagData { get; }
    }
}

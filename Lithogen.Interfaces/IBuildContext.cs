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

        /// <summary>
        /// If not explicitly set the ProjectDirectory is derived from the ProjectPath
        /// according to Visual Studio defaults (it's just the parent directory).
        /// It can also be explicitly set, in which case it is returned as-is.
        /// </summary>
        string ProjectDirectory { get; set; }

        /// <summary>
        /// If not explicitly set the CssDirectory is derived from the ProjectDirectory
        /// and the directory name "css".
        /// It can also be explicitly set, in which case it is returned as-is.
        /// </summary>
        string CssDirectory { get; set; }

        /// <summary>
        /// If not explicitly set the ImagesDirectory is derived from the ProjectDirectory
        /// and the directory name "img".
        /// It can also be explicitly set, in which case it is returned as-is.
        /// </summary>
        string ImagesDirectory { get; set; }

        /// <summary>
        /// If not explicitly set the ScriptDirectory is derived from the ProjectDirectory
        /// and the directory name "js".
        /// It can also be explicitly set, in which case it is returned as-is.
        /// </summary>
        string ScriptsDirectory { get; set; }

        /// <summary>
        /// If not explicitly set the ModelsDirectory is derived from the ProjectDirectory
        /// and the directory name "models".
        /// It can also be explicitly set, in which case it is returned as-is.
        /// </summary>
        string ModelsDirectory { get; set; }

        /// <summary>
        /// If not explicitly set the ViewsDirectory is derived from the ProjectDirectory
        /// and the directory name "views".
        /// It can also be explicitly set, in which case it is returned as-is.
        /// </summary>
        string ViewsDirectory { get; set; }
    }
}

using Lithogen.Core.FileSystem;
using System.Threading;

namespace Lithogen.Core.BuildSteps
{
    public class RazorBuildStep : IBuildStep
    {
        public IFileSystem FileSystem { get; private set; }
        public Settings Settings { get; private set; }
        public string Name { get; set; }

        public RazorBuildStep(IFileSystem fileSystem, Settings settings)
        {
            FileSystem = fileSystem.ThrowIfNull("fileSystem");
            Settings = settings.ThrowIfNull("settings");
            Name = "RazorBuildStep";
        }

        public bool Execute()
        {
            Thread.Sleep(500);
            return true;
        }

        //void ProveRazorMachineWorks(Logger logger)
        //{
        //    // Creating a machine is quick (about 0.1 seconds), rendering a simple
        //    // template is comparatively slow (about 0.6 seconds).
        //    var rm = CreateRazorMachineWithoutContentProviders();
        //    ITemplate template = rm.ExecuteContent("Razor says: Hello @Model.FirstName @Model.LastName", new { FirstName = "John", LastName = "Smith" });
        //    logger.Msg(template.Result);
        //}

        //RazorMachine CreateRazorMachineWithoutContentProviders(bool includeGeneratedSourceCode = false, string rootOperatorPath = null, bool htmlEncode = true)
        //{
        //    var rm = new RazorMachine(includeGeneratedSourceCode: includeGeneratedSourceCode, htmlEncode: htmlEncode, rootOperatorPath: rootOperatorPath);
        //    rm.Context.TemplateFactory.ContentManager.ClearAllContentProviders();
        //    return rm;
        //}
    }
}

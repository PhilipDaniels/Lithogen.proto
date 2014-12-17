using Lithogen.Core.FileSystem;
using Xipton.Razor;

namespace Lithogen.Core.BuildSteps
{
    public class RazorBuildStep : BuildStepBase
    {
        public IRazorMachine RazorMachine { get; set; }

        public RazorBuildStep(IFileSystem fileSystem, Settings settings, ILogger logger)
            : base(fileSystem, settings, logger)
        {
            Name = "RazorBuildStep";
            RazorMachine = CreateRazorMachineWithoutContentProviders();
        }

        public override bool Execute()
        {
            Logger.PushPrefix("RazorBuildStep.Execute() ");
            ITemplate template = RazorMachine.ExecuteContent("Razor says: Hello @Model.FirstName @Model.LastName", new { FirstName = "John", LastName = "Smith" });
            Logger.Msg(template.Result);
            Logger.PopPrefix();
            return true;
        }

        public static RazorMachine CreateRazorMachineWithoutContentProviders
            (
            bool includeGeneratedSourceCode = false,
            string rootOperatorPath = null,
            bool htmlEncode = true
            )
        {
            var rm = new RazorMachine(includeGeneratedSourceCode: includeGeneratedSourceCode, htmlEncode: htmlEncode, rootOperatorPath: rootOperatorPath);
            rm.Context.TemplateFactory.ContentManager.ClearAllContentProviders();
            return rm;
        }
    }
}

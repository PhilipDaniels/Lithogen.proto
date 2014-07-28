using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core.BuildSteps
{
    public class RazorSiteBuildStep
    {
        public string Name { get; set; }
        Logger Logger { get; set; }

        public RazorSiteBuildStep()
        {
            Name = "RazorSiteBuildStep";
        }

        //public bool Execute(IBuildContext context)
        //{
        //    bool result = true;

        //    Logger = context.MakeLogger(Name);

        //    //var domain = AppDomain.CreateDomain("Lithogen");
        //    ////domain.AssemblyResolve += rm_AssemblyResolve;
        //    //object machine = domain.CreateInstanceAndUnwrap("Xipton.Razor.dll", "RazorMachine");
        //    //RazorMachine rm = machine as RazorMachine;

        //    ////var rm = CreateRazorMachine();
        //    //ITemplate template = rm.ExecuteContent("Razor says: Hello @Model.FirstName @Model.LastName", new { FirstName = "John", LastName = "Smith" });
        //    //Logger.Msg(template.Result);

        //    return result;
        //}
    }
}

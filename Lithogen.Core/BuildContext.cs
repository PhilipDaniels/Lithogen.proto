using Lithogen.Interfaces;
using Microsoft.Build.Framework;
using System.Collections.Generic;

namespace Lithogen.Core
{
    public class BuildContext : IBuildContext
    {
        public string SolutionPath { get; set; }
        public string ProjectPath { get; set; }
        public string Configuration { get; set; }
        public string LithogenPluginDir { get; set; }
        public MessageImportance MessageImportance { get; set; }
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
        public IDictionary<string, object> TagData { get; private set; }

        public BuildContext()
        {
            TagData = new Dictionary<string, object>();
        }
    }
}

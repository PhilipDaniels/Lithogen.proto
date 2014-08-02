using Lithogen.Interfaces;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContextBase
    {
        public readonly static string T_ProjectPath = @"C:\temp\somewhere.csproj";
        public readonly static string T_ProjectDirectory = @"C:\temp";
        public readonly static string T_ViewsDirectory = @"C:\temp\views";
        public readonly static string T_ScriptsDirectory = @"C:\temp\scripts";
        public readonly static string T_ModelsDirectory = @"C:\temp\models";

        [SetUp]
        public virtual void Setup()
        {
            TheContext = new BuildContext();
        }
        public IBuildContext TheContext;
    }
}

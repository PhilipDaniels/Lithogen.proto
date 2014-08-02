using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContext_ScriptsDirectory : BuildContextBase
    {
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            TheContext.ProjectPath = null;
            string scriptsDirectory = TheContext.ScriptsDirectory;
        }

        [Test]
        public void WhenProjectPathIsSetAndScriptsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ScriptsDirectory, TheContext.ScriptsDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ScriptsDirectory = T_ScriptsDirectory;
            Assert.AreEqual(T_ScriptsDirectory, TheContext.ScriptsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            TheContext.ScriptsDirectory = @"";
        }

        [Test]
        public void WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ScriptsDirectory = @"anywhere";
            TheContext.ScriptsDirectory = null;
            Assert.AreEqual(T_ScriptsDirectory, TheContext.ScriptsDirectory);
        }

        [Test]
        public void WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            TheContext.ProjectPath = T_ProjectDirectory;
            TheContext.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\scripts", TheContext.ScriptsDirectory);
        }
    }
}

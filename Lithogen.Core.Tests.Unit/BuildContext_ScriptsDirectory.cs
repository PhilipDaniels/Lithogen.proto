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
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string scriptsDirectory = bc.ScriptsDirectory;
        }

        [Test]
        public void WhenProjectPathIsSetAndScriptsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ScriptsDirectory, bc.ScriptsDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ScriptsDirectory = T_ScriptsDirectory;
            Assert.AreEqual(T_ScriptsDirectory, bc.ScriptsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ScriptsDirectory = @"";
        }

        [Test]
        public void WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ScriptsDirectory = @"anywhere";
            bc.ScriptsDirectory = null;
            Assert.AreEqual(T_ScriptsDirectory, bc.ScriptsDirectory);
        }

        [Test]
        public void WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectDirectory;
            bc.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\scripts", bc.ScriptsDirectory);
        }
    }
}

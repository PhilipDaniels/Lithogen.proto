using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContext_ViewsDirectory : BuildContextBase
    {
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string viewsDirectory = bc.ViewsDirectory;
        }

        [Test]
        public void WhenProjectPathIsSetAndViewsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ViewsDirectory, bc.ViewsDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ViewsDirectory = T_ViewsDirectory;
            Assert.AreEqual(T_ViewsDirectory, bc.ViewsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ViewsDirectory = @"";
        }

        [Test]
        public void WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ViewsDirectory = @"anywhere";
            bc.ViewsDirectory = null;
            Assert.AreEqual(T_ViewsDirectory, bc.ViewsDirectory);
        }

        [Test]
        public void WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectDirectory;
            bc.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\views", bc.ViewsDirectory);
        }
    }
}

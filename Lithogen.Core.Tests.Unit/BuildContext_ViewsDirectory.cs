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
            TheContext.ProjectPath = null;
            string viewsDirectory = TheContext.ViewsDirectory;
        }

        [Test]
        public void WhenProjectPathIsSetAndViewsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ViewsDirectory, TheContext.ViewsDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ViewsDirectory = T_ViewsDirectory;
            Assert.AreEqual(T_ViewsDirectory, TheContext.ViewsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            TheContext.ViewsDirectory = @"";
        }

        [Test]
        public void WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ViewsDirectory = @"anywhere";
            TheContext.ViewsDirectory = null;
            Assert.AreEqual(T_ViewsDirectory, TheContext.ViewsDirectory);
        }

        [Test]
        public void WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            TheContext.ProjectPath = T_ProjectDirectory;
            TheContext.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\views", TheContext.ViewsDirectory);
        }
    }
}

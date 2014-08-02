using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContext_ProjectDirectory : BuildContextBase
    {
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            TheContext.ProjectPath = null;
            string projectDirectory = TheContext.ProjectDirectory;
        }

        [Test]
        public void WhenProjectPathIsValidAndNotExplicitlySet_IsTheParentDirectory()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ProjectDirectory, TheContext.ProjectDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ProjectDirectory = T_ProjectDirectory;
            Assert.AreEqual(T_ProjectDirectory, TheContext.ProjectDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            TheContext.ProjectDirectory = @"";
        }

        [Test]
        public void WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ProjectDirectory = @"anywhere";
            TheContext.ProjectDirectory = null;
            Assert.AreEqual(T_ProjectDirectory, TheContext.ProjectDirectory);
        }
    }
}

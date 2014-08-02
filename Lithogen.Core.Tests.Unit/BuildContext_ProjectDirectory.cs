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
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string projectDirectory = bc.ProjectDirectory;
        }

        [Test]
        public void WhenProjectPathIsValidAndNotExplicitlySet_IsTheParentDirectory()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ProjectDirectory, bc.ProjectDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ProjectDirectory = T_ProjectDirectory;
            Assert.AreEqual(T_ProjectDirectory, bc.ProjectDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ProjectDirectory = @"";
        }

        [Test]
        public void WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ProjectDirectory = @"anywhere";
            bc.ProjectDirectory = null;
            Assert.AreEqual(T_ProjectDirectory, bc.ProjectDirectory);
        }
    }
}

using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContext_ModelsDirectory : BuildContextBase
    {
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string modelsDirectory = bc.ModelsDirectory;
        }

        [Test]
        public void WhenProjectPathIsSetAndModelsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ModelsDirectory, bc.ModelsDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ModelsDirectory = T_ModelsDirectory;
            Assert.AreEqual(T_ModelsDirectory, bc.ModelsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ModelsDirectory = @"";
        }

        [Test]
        public void WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ModelsDirectory = @"anywhere";
            bc.ModelsDirectory = null;
            Assert.AreEqual(T_ModelsDirectory, bc.ModelsDirectory);
        }

        [Test]
        public void WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectDirectory;
            bc.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\models", bc.ModelsDirectory);
        }
    }
}

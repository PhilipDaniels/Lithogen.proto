using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContext_ModelsDirectory : BuildContextBase
    {
        [Test]
        public void WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            TheContext.ProjectPath = null;
            Assert.That(() => TheContext.ModelsDirectory, Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void WhenProjectPathIsSetAndModelsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ModelsDirectory, TheContext.ModelsDirectory);
        }

        [Test]
        public void WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ModelsDirectory = T_ModelsDirectory;
            Assert.AreEqual(T_ModelsDirectory, TheContext.ModelsDirectory);
        }

        [Test]
        public void WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            Assert.That(() => TheContext.ModelsDirectory = @"", Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ModelsDirectory = @"anywhere";
            TheContext.ModelsDirectory = null;
            Assert.AreEqual(T_ModelsDirectory, TheContext.ModelsDirectory);
        }

        [Test]
        public void WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            TheContext.ProjectPath = T_ProjectDirectory;
            TheContext.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\models", TheContext.ModelsDirectory);
        }
    }
}

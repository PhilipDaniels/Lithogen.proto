using NUnit.Framework;
using System;

// Method_StateUnderTest_ExpectedBehavior.
// It is nice to Assert.NotNull before checking more specific criteria as it results in a more meaningful error message.

namespace Lithogen.Core.Tests
{
    [TestFixture]
    public class BuildContextTests
    {
        const string T_ProjectPath = @"C:\temp\somewhere.csproj";
        const string T_ProjectDirectory = @"C:\temp";
        const string T_ViewsDirectory = @"C:\temp\views";
        const string T_ScriptsDirectory = @"C:\temp\scripts";
        const string T_ModelsDirectory = @"C:\temp\models";

        [Test]
        public void Ctor_Executed_CreatesEmptyTagDataDictionary()
        {
            var bc = new BuildContext();
            Assert.NotNull(bc.TagData);
            Assert.IsEmpty(bc.TagData);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ProjectDirectory_ForNullProjectPath_ThrowsNullReferenceException()
        {
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string projectDirectory = bc.ProjectDirectory;
        }

        [Test]
        public void ProjectDirectory_ForValidProjectPathAndNotExplicitlySet_IsTheParentDirectory()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ProjectDirectory, bc.ProjectDirectory);
        }

        [Test]
        public void ProjectDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ProjectDirectory = T_ProjectDirectory;
            Assert.AreEqual(T_ProjectDirectory, bc.ProjectDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ProjectDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ProjectDirectory = @"";
        }

        [Test]
        public void ProjectDirectory_WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ProjectDirectory = @"anywhere";
            bc.ProjectDirectory = null;
            Assert.AreEqual(T_ProjectDirectory, bc.ProjectDirectory);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ModelsDirectory_ForNullProjectPath_ThrowsNullReferenceException()
        {
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string modelsDirectory = bc.ModelsDirectory;
        }

        [Test]
        public void ModelsDirectory_ForValidProjectPathAndNotExplicitlySet_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ModelsDirectory, bc.ModelsDirectory);
        }

        [Test]
        public void ModelsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ModelsDirectory = T_ModelsDirectory;
            Assert.AreEqual(T_ModelsDirectory, bc.ModelsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ModelsDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ModelsDirectory = @"";
        }

        [Test]
        public void ModelsDirectory_WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ModelsDirectory = @"anywhere";
            bc.ModelsDirectory = null;
            Assert.AreEqual(T_ModelsDirectory, bc.ModelsDirectory);
        }

        [Test]
        public void ModelsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectDirectory;
            bc.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\models", bc.ModelsDirectory);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ScriptsDirectory_ForNullProjectPath_ThrowsNullReferenceException()
        {
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string scriptsDirectory = bc.ScriptsDirectory;
        }

        [Test]
        public void ScriptsDirectory_ForValidProjectPathAndNotExplicitlySet_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ScriptsDirectory, bc.ScriptsDirectory);
        }

        [Test]
        public void ScriptsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ScriptsDirectory = T_ScriptsDirectory;
            Assert.AreEqual(T_ScriptsDirectory, bc.ScriptsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ScriptsDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ScriptsDirectory = @"";
        }

        [Test]
        public void ScriptsDirectory_WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ScriptsDirectory = @"anywhere";
            bc.ScriptsDirectory = null;
            Assert.AreEqual(T_ScriptsDirectory, bc.ScriptsDirectory);
        }

        [Test]
        public void ScriptsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectDirectory;
            bc.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\scripts", bc.ScriptsDirectory);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ViewsDirectory_ForNullProjectPath_ThrowsNullReferenceException()
        {
            var bc = new BuildContext();
            bc.ProjectPath = null;
            string viewsDirectory = bc.ViewsDirectory;
        }

        [Test]
        public void ViewsDirectory_ForValidProjectPathAndNotExplicitlySet_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ViewsDirectory, bc.ViewsDirectory);
        }

        [Test]
        public void ViewsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            var bc = new BuildContext();
            bc.ViewsDirectory = T_ViewsDirectory;
            Assert.AreEqual(T_ViewsDirectory, bc.ViewsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ViewsDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            var bc = new BuildContext();
            bc.ViewsDirectory = @"";
        }

        [Test]
        public void ViewsDirectory_WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectPath;
            bc.ViewsDirectory = @"anywhere";
            bc.ViewsDirectory = null;
            Assert.AreEqual(T_ViewsDirectory, bc.ViewsDirectory);
        }

        [Test]
        public void ViewsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            var bc = new BuildContext();
            bc.ProjectPath = T_ProjectDirectory;
            bc.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\views", bc.ViewsDirectory);
        }
    }
}

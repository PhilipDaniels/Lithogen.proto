using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests
{
    class BuildContextTests
    {
        const string T_ProjectPath = @"C:\temp\somewhere.csproj";
        const string T_ProjectDirectory = @"C:\temp";
        const string T_ViewsDirectory = @"C:\temp\views";
        const string T_ScriptsDirectory = @"C:\temp\scripts";
        const string T_ModelsDirectory = @"C:\temp\models";

        class Ctor
        {
            [Test]
            public void WhenCompleted_CreatesEmptyTagDataDictionary()
            {
                var bc = new BuildContext();
                Assert.NotNull(bc.TagData);
                Assert.IsEmpty(bc.TagData);
            }
        }

        class ProjectDirectory
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

        class ModelsDirectory
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

        class ScriptsDirectory
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

        class ViewsDirectory
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
}

using NUnit.Framework;
using System;

namespace Lithogen.Tests.Unit
{
    public class SettingsTests
    {
        public readonly static string T_ProjectFile = @"C:\temp\somewhere.csproj";
        public readonly static string T_ProjectDirectory = @"C:\temp";
        public readonly static string T_CssDirectory = @"C:\temp\css";
        public readonly static string T_ImagesDirectory = @"C:\temp\img";
        public readonly static string T_ScriptsDirectory = @"C:\temp\js";
        public readonly static string T_ViewsDirectory = @"C:\temp\views";
        public readonly static string T_ModelsDirectory = @"C:\temp\models";

        [SetUp]
        public virtual void Setup()
        {
            TheSettings = new Settings();
        }
        public Settings TheSettings;

        [Test]
        public virtual void Ctor_WhenCompleted_CreatesEmptyTagDataDictionary()
        {
            Assert.NotNull(TheSettings.TagData);
            Assert.IsEmpty(TheSettings.TagData);
        }

        #region ProjectDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ProjectDirectory_WhenProjectFileIsNull_ThrowsNullReferenceException()
        {
            TheSettings.ProjectFile = null;
            string projectDirectory = TheSettings.ProjectDirectory;
        }

        [Test]
        public virtual void ProjectDirectory_WhenProjectFileIsValidAndNotExplicitlySet_IsTheParentDirectory()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            Assert.AreEqual(T_ProjectDirectory, TheSettings.ProjectDirectory);
        }

        [Test]
        public virtual void ProjectDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheSettings.ProjectDirectory = T_ProjectDirectory;
            Assert.AreEqual(T_ProjectDirectory, TheSettings.ProjectDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public virtual void ProjectDirectory_WhenSettingToWhitespace_ThrowsArgumentException()
        {
            TheSettings.ProjectDirectory = "";
        }

        [Test]
        public virtual void ProjectDirectory_WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            TheSettings.ProjectDirectory = "anywhere";
            TheSettings.ProjectDirectory = null;
            Assert.AreEqual(T_ProjectDirectory, TheSettings.ProjectDirectory);
        }
        #endregion

        #region CssDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void CssDirectory_WhenProjectFileIsNull_ThrowsNullReferenceException()
        {
            TheSettings.ProjectFile = null;
            string cssDirectory = TheSettings.CssDirectory;
        }

        [Test]
        public virtual void CssDirectory_WhenProjectFileIsSetAndCssDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            Assert.AreEqual(T_CssDirectory, TheSettings.CssDirectory);
        }

        [Test]
        public virtual void CssDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheSettings.CssDirectory = T_CssDirectory;
            Assert.AreEqual(T_CssDirectory, TheSettings.CssDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public virtual void CssDirectory_WhenSettingToWhitespace_ThrowsArgumentException()
        {
            TheSettings.CssDirectory = "";
        }

        [Test]
        public virtual void CssDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            TheSettings.CssDirectory = "anywhere";
            TheSettings.CssDirectory = null;
            Assert.AreEqual(T_CssDirectory, TheSettings.CssDirectory);
        }

        [Test]
        public virtual void CssDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectFile()
        {
            TheSettings.ProjectFile = T_ProjectDirectory;
            TheSettings.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\css", TheSettings.CssDirectory);
        }
        #endregion

        #region ImagesDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ImagesDirectory_WhenProjectFileIsNull_ThrowsNullReferenceException()
        {
            TheSettings.ProjectFile = null;
            string imagesDirectory = TheSettings.ImagesDirectory;
        }

        [Test]
        public virtual void ImagesDirectory_WhenProjectFileIsSetAndImagesDirectoryIsNull_MatchesLithogenDefaultsAndIsLowerCase()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            Assert.AreEqual(T_ImagesDirectory, TheSettings.ImagesDirectory);
        }

        [Test]
        public virtual void ImagesDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheSettings.ImagesDirectory = T_ImagesDirectory;
            Assert.AreEqual(T_ImagesDirectory, TheSettings.ImagesDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public virtual void ImagesDirectory_WhenSettingToWhitespace_ThrowsArgumentException()
        {
            TheSettings.ImagesDirectory = "";
        }

        [Test]
        public virtual void ImagesDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            TheSettings.ImagesDirectory = "anywhere";
            TheSettings.ImagesDirectory = null;
            Assert.AreEqual(T_ImagesDirectory, TheSettings.ImagesDirectory);
        }

        [Test]
        public virtual void ImagesDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectFile()
        {
            TheSettings.ProjectFile = T_ProjectDirectory;
            TheSettings.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\img", TheSettings.ImagesDirectory);
        }
        #endregion

        #region ScriptsDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ScriptsDirectory_WhenProjectFileIsNull_ThrowsNullReferenceException()
        {
            TheSettings.ProjectFile = null;
            string scriptsDirectory = TheSettings.ScriptsDirectory;
        }

        [Test]
        public virtual void ScriptsDirectory_WhenProjectFileIsSetAndScriptsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            Assert.AreEqual(T_ScriptsDirectory, TheSettings.ScriptsDirectory);
        }

        [Test]
        public virtual void ScriptsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheSettings.ScriptsDirectory = T_ScriptsDirectory;
            Assert.AreEqual(T_ScriptsDirectory, TheSettings.ScriptsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public virtual void ScriptsDirectory_WhenSettingToWhitespace_ThrowsArgumentException()
        {
            TheSettings.ScriptsDirectory = "";
        }

        [Test]
        public virtual void ScriptsDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            TheSettings.ScriptsDirectory = "anywhere";
            TheSettings.ScriptsDirectory = null;
            Assert.AreEqual(T_ScriptsDirectory, TheSettings.ScriptsDirectory);
        }

        [Test]
        public virtual void ScriptsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectFile()
        {
            TheSettings.ProjectFile = T_ProjectDirectory;
            TheSettings.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\js", TheSettings.ScriptsDirectory);
        }
        #endregion

        #region ModelsDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ModelsDirectory_WhenProjectFileIsNull_ThrowsNullReferenceException()
        {
            TheSettings.ProjectFile = null;
            string s = TheSettings.ModelsDirectory;
        }

        [Test]
        public virtual void ModelsDirectory_WhenProjectFileIsSetAndModelsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            Assert.AreEqual(T_ModelsDirectory, TheSettings.ModelsDirectory);
        }

        [Test]
        public virtual void ModelsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheSettings.ModelsDirectory = T_ModelsDirectory;
            Assert.AreEqual(T_ModelsDirectory, TheSettings.ModelsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public virtual void ModelsDirectory_WhenSettingToWhitespace_ThrowsArgumentException()
        {
            TheSettings.ModelsDirectory = "";
        }

        [Test]
        public virtual void ModelsDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            TheSettings.ModelsDirectory = "anywhere";
            TheSettings.ModelsDirectory = null;
            Assert.AreEqual(T_ModelsDirectory, TheSettings.ModelsDirectory);
        }

        [Test]
        public virtual void ModelsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectFile()
        {
            TheSettings.ProjectFile = T_ProjectDirectory;
            TheSettings.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\models", TheSettings.ModelsDirectory);
        }
        #endregion

        #region ViewsDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ViewsDirectory_WhenProjectFileIsNull_ThrowsNullReferenceException()
        {
            TheSettings.ProjectFile = null;
            string viewsDirectory = TheSettings.ViewsDirectory;
        }

        [Test]
        public virtual void ViewsDirectory_WhenProjectFileIsSetAndViewsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            Assert.AreEqual(T_ViewsDirectory, TheSettings.ViewsDirectory);
        }

        [Test]
        public virtual void ViewsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheSettings.ViewsDirectory = T_ViewsDirectory;
            Assert.AreEqual(T_ViewsDirectory, TheSettings.ViewsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public virtual void ViewsDirectory_WhenSettingToWhitespace_ThrowsArgumentException()
        {
            TheSettings.ViewsDirectory = "";
        }

        [Test]
        public virtual void ViewsDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheSettings.ProjectFile = T_ProjectFile;
            TheSettings.ViewsDirectory = "anywhere";
            TheSettings.ViewsDirectory = null;
            Assert.AreEqual(T_ViewsDirectory, TheSettings.ViewsDirectory);
        }

        [Test]
        public virtual void ViewsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectFile()
        {
            TheSettings.ProjectFile = T_ProjectDirectory;
            TheSettings.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\views", TheSettings.ViewsDirectory);
        }
        #endregion
    }
}

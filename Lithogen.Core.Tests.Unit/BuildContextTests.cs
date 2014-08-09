using Lithogen.Interfaces;
using NUnit.Framework;
using System;

namespace Lithogen.Core.Tests.Unit
{
    public class BuildContextTests
    {
        public readonly static string T_ProjectPath = @"C:\temp\somewhere.csproj";
        public readonly static string T_ProjectDirectory = @"C:\temp";
        public readonly static string T_ViewsDirectory = @"C:\temp\views";
        public readonly static string T_ScriptsDirectory = @"C:\temp\scripts";
        public readonly static string T_ModelsDirectory = @"C:\temp\models";

        [SetUp]
        public virtual void Setup()
        {
            TheContext = new BuildContext();
        }
        public IBuildContext TheContext;

        [Test]
        public virtual void Ctor_WhenCompleted_CreatesEmptyTagDataDictionary()
        {
            Assert.NotNull(TheContext.TagData);
            Assert.IsEmpty(TheContext.TagData);
        }

        #region ModelsDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ModelsDirectory_WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            TheContext.ProjectPath = null;
            string s = TheContext.ModelsDirectory;
        }

        [Test]
        public virtual void ModelsDirectory_WhenProjectPathIsSetAndModelsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ModelsDirectory, TheContext.ModelsDirectory);
        }

        [Test]
        public virtual void ModelsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ModelsDirectory = T_ModelsDirectory;
            Assert.AreEqual(T_ModelsDirectory, TheContext.ModelsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public virtual void ModelsDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            TheContext.ModelsDirectory = @"";
        }

        [Test]
        public virtual void ModelsDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ModelsDirectory = @"anywhere";
            TheContext.ModelsDirectory = null;
            Assert.AreEqual(T_ModelsDirectory, TheContext.ModelsDirectory);
        }

        [Test]
        public virtual void ModelsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            TheContext.ProjectPath = T_ProjectDirectory;
            TheContext.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\models", TheContext.ModelsDirectory);
        }
        #endregion

        #region ProjectDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ProjectDirectory_WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            TheContext.ProjectPath = null;
            string projectDirectory = TheContext.ProjectDirectory;
        }

        [Test]
        public virtual void ProjectDirectory_WhenProjectPathIsValidAndNotExplicitlySet_IsTheParentDirectory()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ProjectDirectory, TheContext.ProjectDirectory);
        }

        [Test]
        public virtual void ProjectDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ProjectDirectory = T_ProjectDirectory;
            Assert.AreEqual(T_ProjectDirectory, TheContext.ProjectDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public virtual void ProjectDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            TheContext.ProjectDirectory = "";
        }

        [Test]
        public virtual void ProjectDirectory_WhenReSettingToNull_ResetsToDerivedBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ProjectDirectory = "anywhere";
            TheContext.ProjectDirectory = null;
            Assert.AreEqual(T_ProjectDirectory, TheContext.ProjectDirectory);
        }
        #endregion

        #region ScriptsDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ScriptsDirectory_WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            TheContext.ProjectPath = null;
            string scriptsDirectory = TheContext.ScriptsDirectory;
        }

        [Test]
        public virtual void ScriptsDirectory_WhenProjectPathIsSetAndScriptsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ScriptsDirectory, TheContext.ScriptsDirectory);
        }

        [Test]
        public virtual void ScriptsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ScriptsDirectory = T_ScriptsDirectory;
            Assert.AreEqual(T_ScriptsDirectory, TheContext.ScriptsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public virtual void ScriptsDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            TheContext.ScriptsDirectory = "";
        }

        [Test]
        public virtual void ScriptsDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ScriptsDirectory = "anywhere";
            TheContext.ScriptsDirectory = null;
            Assert.AreEqual(T_ScriptsDirectory, TheContext.ScriptsDirectory);
        }

        [Test]
        public virtual void ScriptsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            TheContext.ProjectPath = T_ProjectDirectory;
            TheContext.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\scripts", TheContext.ScriptsDirectory);
        }
        #endregion

        #region ViewsDirectory
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public virtual void ViewsDirectory_WhenProjectPathIsNull_ThrowsNullReferenceException()
        {
            TheContext.ProjectPath = null;
            string viewsDirectory = TheContext.ViewsDirectory;
        }

        [Test]
        public virtual void ViewsDirectory_WhenProjectPathIsSetAndViewsDirectoryIsNull_MatchesVisualStudioDefaultsAndIsLowerCase()
        {
            TheContext.ProjectPath = T_ProjectPath;
            Assert.AreEqual(T_ViewsDirectory, TheContext.ViewsDirectory);
        }

        [Test]
        public virtual void ViewsDirectory_WhenExplicitlySet_IsReturnedUnchanged()
        {
            TheContext.ViewsDirectory = T_ViewsDirectory;
            Assert.AreEqual(T_ViewsDirectory, TheContext.ViewsDirectory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public virtual void ViewsDirectory_WhenSettingToWhitespace_ThrowsArgumentNullException()
        {
            TheContext.ViewsDirectory = @"";
        }

        [Test]
        public virtual void ViewsDirectory_WhenReSettingToNull_RevertsToDefaultBehaviour()
        {
            TheContext.ProjectPath = T_ProjectPath;
            TheContext.ViewsDirectory = @"anywhere";
            TheContext.ViewsDirectory = null;
            Assert.AreEqual(T_ViewsDirectory, TheContext.ViewsDirectory);
        }

        [Test]
        public virtual void ViewsDirectory_WhenProjectDirectoryIsExplicitlySet_DerivesFromProjectDirectoryNotProjectPath()
        {
            TheContext.ProjectPath = T_ProjectDirectory;
            TheContext.ProjectDirectory = @"C:\somewhere\else\";
            Assert.AreEqual(@"C:\somewhere\else\views", TheContext.ViewsDirectory);
        }
        #endregion
    }
}

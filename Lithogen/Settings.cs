using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lithogen
{
    /// <summary>
    /// Represents the settings that this invocation of Lithogen is working with.
    /// This is worked out when the program starts, and is a merge of the config file,
    /// if any, and command line parameters. It also contains a TagData dictionary
    /// which can be used for anything.
    /// </summary>
    public class Settings
    {
        public string SolutionFile { get; set; }
        public string ProjectFile { get; set; }
        public string Configuration { get; set; }
        public string PluginsDirectory { get; set; }
        public IDictionary<string, object> TagData { get; private set; }

        public Settings()
        {
            TagData = new Dictionary<string, object>();
            _CssDirectory = new ProjectDirectoryDerivedSubfolder(this, "css", "CssDirectory");
            _ImagesDirectory = new ProjectDirectoryDerivedSubfolder(this, "img", "ImagesDirectory");
            _ScriptsDirectory = new ProjectDirectoryDerivedSubfolder(this, "js", "ScriptsDirectory");
            _ModelsDirectory = new ProjectDirectoryDerivedSubfolder(this, "models", "ModelsDirectory");
            _ViewsDirectory = new ProjectDirectoryDerivedSubfolder(this, "views", "ViewsDirectory");
        }

        public string ProjectDirectory
        {
            get
            {
                if (_ProjectDirectory != null)
                    return _ProjectDirectory;
                if (ProjectFile == null)
                    throw new NullReferenceException("The ProjectDirectory cannot be derived because the ProjectFile is not set.");
                return Path.GetDirectoryName(ProjectFile);
            }
            set
            {
                if (value == null || value.Trim().Length > 0)
                    _ProjectDirectory = value;
                else
                    throw new ArgumentException("ProjectDirectory cannot be set to an empty or whitespace string.");
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _ProjectDirectory;

        public string CssDirectory
        {
            get { return _CssDirectory.Value; }
            set { _CssDirectory.Value = value; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ProjectDirectoryDerivedSubfolder _CssDirectory;

        public string ImagesDirectory
        {
            get { return _ImagesDirectory.Value; }
            set { _ImagesDirectory.Value = value; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ProjectDirectoryDerivedSubfolder _ImagesDirectory;

        public string ScriptsDirectory
        {
            get { return _ScriptsDirectory.Value; }
            set { _ScriptsDirectory.Value = value; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ProjectDirectoryDerivedSubfolder _ScriptsDirectory;

        public string ModelsDirectory
        {
            get { return _ModelsDirectory.Value; }
            set { _ModelsDirectory.Value = value; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ProjectDirectoryDerivedSubfolder _ModelsDirectory;

        public string ViewsDirectory
        {
            get { return _ViewsDirectory.Value; }
            set { _ViewsDirectory.Value = value; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ProjectDirectoryDerivedSubfolder _ViewsDirectory;

        class ProjectDirectoryDerivedSubfolder
        {
            readonly Settings OwnerContext;
            readonly string SubFolderName;
            readonly string SubFolderDescription;

            public ProjectDirectoryDerivedSubfolder(Settings ownerContext, string subFolderName, string subFolderDescription)
            {
                OwnerContext = ownerContext.ThrowIfNull("ownerContext");
                SubFolderName = subFolderName.ThrowIfNullOrWhiteSpace("subFolderName");
                SubFolderDescription = subFolderDescription.ThrowIfNullOrWhiteSpace("subFolderDescription");
            }

            public string Value
            {
                get
                {
                    if (_Value != null)
                        return _Value;
                    return Path.Combine(OwnerContext.ProjectDirectory, SubFolderName);
                }
                set
                {
                    if (value == null || value.Trim().Length > 0)
                        _Value = value;
                    else
                        throw new ArgumentException(SubFolderDescription + " cannot be set to an empty or whitespace string.");
                }
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            string _Value;
        }
    }
}

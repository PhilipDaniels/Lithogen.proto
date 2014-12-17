using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lithogen.Core
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
            _OutputDirectory = new ProjectDirectoryDerivedSubfolder(this, "bin\\" + Configuration, "OutputDirectory");
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

        public string OutputDirectory
        {
            get { return _OutputDirectory.Value; }
            set { _OutputDirectory.Value = value; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ProjectDirectoryDerivedSubfolder _OutputDirectory;

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

        public void Validate(ILogger logger)
        {
            if (SolutionFile != null)
            {
                SolutionFile = SolutionFile.Trim();
                if (SolutionFile.Length == 0)
                {
                    SolutionFile = null;
                }
                if (SolutionFile != null && !File.Exists(SolutionFile))
                    throw new FileNotFoundException("The SolutionFile '" + SolutionFile + "' does not exist.");
            }
            else
            {
                logger.Msg("SolutionFile is not specified (which is probably OK, you should base things off the project anyway).");
            }

            if (String.IsNullOrWhiteSpace(ProjectFile))
                throw new ArgumentException("ProjectFile must be set.");
            ProjectFile = ProjectFile.Trim();
            if (!File.Exists(ProjectFile))
                throw new FileNotFoundException("The ProjectFile '" + ProjectFile + "' does not exist.");

            if (String.IsNullOrWhiteSpace(OutputDirectory))
                throw new ArgumentException("The OutputDirectory is not set.");

            if (Configuration != null)
                Configuration = Configuration.Trim();

            if (PluginsDirectory != null)
            {
                PluginsDirectory = PluginsDirectory.Trim();
                if (PluginsDirectory.Length == 0)
                {
                    PluginsDirectory = null;
                }
                if (PluginsDirectory != null)
                {
                    if (!Directory.Exists(PluginsDirectory))
                    {
                        logger.Msg("The PluginsDirectory '{0}' that you specified does not exist - ignoring.", PluginsDirectory);
                        PluginsDirectory = null;
                    }
                    else
                    {
                        string[] files = Directory.GetFiles(PluginsDirectory, "*.dll", SearchOption.TopDirectoryOnly);
                        if (files == null || files.Length == 0)
                        {
                            logger.Msg("The PluginsDirectory '{0}' exists, but contains no DLLs. Only built-in Lithogen types will be used.", PluginsDirectory);
                        }
                        else
                        {
                            logger.Msg("The PluginsDirectory '{0}' exists and contains {1} file(s) matching the filter *.dll. They will be searched for plugin types later.", PluginsDirectory, files.Length);
                        }
                    }
                }
            }
            else
            {
                logger.Msg("The PluginsDirectory is not specified, only built-in Lithogen types will be used.");
            }
        }
    }
}

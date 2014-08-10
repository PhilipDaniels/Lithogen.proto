﻿using Lithogen.Interfaces;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lithogen.Core
{
    public class BuildContext : IBuildContext
    {
        public string SolutionPath { get; set; }
        public string ProjectPath { get; set; }
        public string Configuration { get; set; }
        public string LithogenPluginDir { get; set; }
        public MessageImportance MessageImportance { get; set; }
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
        public IDictionary<string, object> TagData { get; private set; }

        public BuildContext()
        {
            TagData = new Dictionary<string, object>();
        }

        public string ProjectDirectory
        {
            get
            {
                if (_ProjectDirectory != null)
                    return _ProjectDirectory;
                if (ProjectPath == null)
                    throw new NullReferenceException("The ProjectDirectory cannot be derived because the ProjectPath is not set.");
                return Path.GetDirectoryName(ProjectPath);
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
            get
            {
                if (_CssDirectory != null)
                    return _CssDirectory;
                return Path.Combine(ProjectDirectory, "css");
            }
            set
            {
                if (value == null || value.Trim().Length > 0)
                    _CssDirectory = value;
                else
                    throw new ArgumentException("CssDirectory cannot be set to an empty or whitespace string.");
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _CssDirectory;

        public string ImagesDirectory
        {
            get
            {
                if (_ImagesDirectory != null)
                    return _ImagesDirectory;
                return Path.Combine(ProjectDirectory, "img");
            }
            set
            {
                if (value == null || value.Trim().Length > 0)
                    _ImagesDirectory = value;
                else
                    throw new ArgumentException("ImagesDirectory cannot be set to an empty or whitespace string.");
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _ImagesDirectory;

        public string ScriptsDirectory
        {
            get
            {
                if (_ScriptsDirectory != null)
                    return _ScriptsDirectory;
                return Path.Combine(ProjectDirectory, "js");
            }
            set
            {
                if (value == null || value.Trim().Length > 0)
                    _ScriptsDirectory = value;
                else
                    throw new ArgumentException("ScriptsDirectory cannot be set to an empty or whitespace string.");
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _ScriptsDirectory;

        public string ModelsDirectory
        {
            get
            {
                if (_ModelsDirectory != null)
                    return _ModelsDirectory;
                return Path.Combine(ProjectDirectory, "models");
            }
            set
            {
                if (value == null || value.Trim().Length > 0)
                    _ModelsDirectory = value;
                else
                    throw new ArgumentException("ModelsDirectory cannot be set to an empty or whitespace string.");
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _ModelsDirectory;

        public string ViewsDirectory
        {
            get
            {
                if (_ViewsDirectory != null)
                    return _ViewsDirectory;
                return Path.Combine(ProjectDirectory, "views");
            }
            set
            {
                if (value == null || value.Trim().Length > 0)
                    _ViewsDirectory = value;
                else
                    throw new ArgumentException("ViewsDirectory cannot be set to an empty or whitespace string.");
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _ViewsDirectory;
    }
}

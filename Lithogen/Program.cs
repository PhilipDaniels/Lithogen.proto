﻿using Lithogen.Core;
using Lithogen.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lithogen
{
    class Program
    {
        public static readonly string NAME = "Lithogen";
        public static Settings Settings; 
        static ILogger Logger;

        static void Main(string[] args)
        {
            try
            {
                Logger = new ConsoleLogger();
                Logger.PushPrefix("Main() ");
                Logger.Msg("Starting. Parsing settings.");
                var argsDict = ProcessArgs(args);
                Settings = LoadSettings(argsDict);
                Settings.Validate(Logger);
                DumpConfig(Settings);
                if (Settings == null)
                    return;
                var container = ConfigureIoC();
                var builder = container.GetInstance<IBuilder>();
                Logger.Msg("IBuilder of type {0} created.", builder.GetType());
                builder.InitialiseBuildSteps(); 
                builder.Build();
                Logger.Msg("Done.");
            }
            catch (Exception ex)
            {
                Logger.Error(Program.NAME, "E000", "An exception occurred: " + ex.ToString());
            }
        }

        static SimpleInjector.Container ConfigureIoC()
        {
            var container = new SimpleInjector.Container();
            container.Options.AllowOverridingRegistrations = true;
            Logger.Msg("IoC container created.");

            container.RegisterSingle<ILogger>(Logger);
            container.Register<IBuilder, Builder>();
            container.Register<IFileSystem, WindowsFileSystem>();
            container.Register<ICountingFileSystem, CountingFileSystem>();
            container.Register<IFileFilter, FileFilter>();
            container.RegisterSingle<Settings>(Settings);
            Logger.Msg("Default Lithogen types registered.");

            string[] plugins = Directory.GetFiles(Settings.PluginsDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            if (plugins.Length > 0)
            {
                foreach (string plugin in plugins)
                {
                    Logger.Msg("Registering plugins from " + plugin);
                    var pluginTypes = from type in Assembly.LoadFile(plugin).GetExportedTypes()
                                      where !type.IsAbstract && !type.IsGenericTypeDefinition
                                      select type;
                    foreach (Type pluginType in pluginTypes)
                    {
                        foreach (Type t in OverridableTypes)
                        {
                            if (t.IsAssignableFrom(pluginType))
                            {
                                container.Register(t, pluginType);
                                Logger.Msg("Registered type {0} against interface {1}.", pluginType.FullName, t.Name);
                            }
                        }
                    }
                }
                Logger.Msg("All plugin types registered.");
            }

            container.Verify();
            Logger.Msg("IoC container verified.");
            return container;
        }

        static IEnumerable<Type> OverridableTypes
        {
            get
            {
                yield return typeof(IBuilder);
                yield return typeof(ILogger);
                yield return typeof(IFileSystem);
                yield return typeof(ICountingFileSystem);
            }
        }

        /// <summary>
        /// Loads the settings for this invocation, from the config files
        /// and/or the command line.
        /// </summary>
        static Settings LoadSettings(IDictionary<string, string> args)
        {
            Settings settings = null;

            // If there is a config file specified on the command line use that
            // otherwise use the default. Load all options from the file, treating
            // blank strings as null.
            string configFile = null;
            if (args.TryGetValue("cf", out configFile))
            {
                if (!File.Exists(configFile))
                {
                    Logger.Error(Program.NAME, "E005", "The config file '" + configFile + "' specified on the command line does not exist.");
                    throw new ArgumentException("Bad config file specified on command line.");
                }
                settings = LoadConfigFile(configFile);
            }
            else
            {
                settings = LoadConfigFile(null);
            }

            // Apply any command line parameters on top.
            foreach (var kvp in args)
            {
                if (kvp.Key.Equals("cf", StringComparison.OrdinalIgnoreCase))
                    continue;
                string k = kvp.Key;
                if (k == "sf")
                    settings.SolutionFile = kvp.Value;
                else if (k == "pf")
                    settings.ProjectFile = kvp.Value;
                else if (k == "co")
                    settings.Configuration = kvp.Value;
                else if (k == "css")
                    settings.CssDirectory = kvp.Value;
                else if (k == "img")
                    settings.ImagesDirectory = kvp.Value;
                else if (k == "js")
                    settings.ScriptsDirectory = kvp.Value;
                else if (k == "mo")
                    settings.ModelsDirectory = kvp.Value;
                else if (k == "vw")
                    settings.ViewsDirectory = kvp.Value;
                else if (k == "od")
                    settings.OutputDirectory = kvp.Value;
            }

            if (settings.PluginsDirectory == null)
            {
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                settings.PluginsDirectory = Path.Combine(dir, "Plugins");
                Logger.Msg("The PluginsDirectory was not explicitly set (or set to 'none'), so assuming the default of " + settings.PluginsDirectory);
            }

            return settings;
        }

        static Settings LoadConfigFile(string configFile)
        {
            var configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = configFile == null ? 
                                            Assembly.GetExecutingAssembly().Location + ".config" :
                                            configMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var settings = new Settings();
            settings.SolutionFile = GetNullOrValue(config, "SolutionFile");
            settings.ProjectFile = GetNullOrValue(config, "ProjectFile");
            settings.Configuration = GetNullOrValue(config, "Configuration");
            settings.PluginsDirectory = GetNullOrValue(config, "PluginsDirectory");
            settings.CssDirectory = GetNullOrValue(config, "CssDirectory");
            settings.ImagesDirectory = GetNullOrValue(config, "ImagesDirectory");
            settings.ScriptsDirectory = GetNullOrValue(config, "ScriptsDirectory");
            settings.ModelsDirectory = GetNullOrValue(config, "ModelsDirectory");
            settings.ViewsDirectory = GetNullOrValue(config, "ViewsDirectory");
            settings.OutputDirectory = GetNullOrValue(config, "OutputDirectory");
            return settings;
        }

        static string GetNullOrValue(Configuration config, string appSetting)
        {
            try
            {
                string setting = config.AppSettings.Settings[appSetting].Value;
                if (String.IsNullOrWhiteSpace(setting))
                    return null;
                else
                    return setting.Trim();
            }
            catch
            {
                return null;
            }
        }

        static Dictionary<string, string> ProcessArgs(string[] args)
        {
            var result = new Dictionary<string, string>();

            foreach (string arg in args)
            {
                if (String.IsNullOrWhiteSpace(arg))
                    continue;
                string[] parts = arg.Substring(1).Split('=');
                if (parts == null || parts.Length != 2)
                {
                    Logger.Error(Program.NAME, "E002", "Do not understand argument '" + arg + "', ignoring.");
                    continue;
                }

                string key = parts[0].ToLowerInvariant();
                if (result.ContainsKey(key))
                {
                    Logger.Error(Program.NAME, "E003", "Argument '" + key + "' specified more than once, ignoring.");
                    continue;
                }
                else
                {
                    string value = parts[1];
                    // Have to be careful, if we get an argument of the form "C:\somewhere\" the last double
                    // quote gets included in the argument.
                    if (value.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                        value = value.Substring(0, value.Length - 1);
                    result[key] = value;
                }
            }

            return result;
        }

        static void DumpConfig(Settings config)
        {
            if (config == null)
            {
                Logger.Error(Program.NAME, "E001", "Configuration object is null. Cannot continue.");
                return;
            }

            Logger.Msg("Lithogen is working with the following settings:");
            Logger.Msg("  SolutionFile = {0}", config.SolutionFile);
            Logger.Msg("  ProjectFile = {0}", config.ProjectFile);
            Logger.Msg("  ProjectDirectory = {0}", config.ProjectDirectory);
            Logger.Msg("  Configuration = {0}", config.Configuration);
            Logger.Msg("  PluginsDirectory = {0}", config.PluginsDirectory);
            Logger.Msg("  CssDirectory = {0}", config.CssDirectory);
            Logger.Msg("  ImagesDirectory = {0}", config.ImagesDirectory);
            Logger.Msg("  ScriptsDirectory = {0}", config.ScriptsDirectory);
            Logger.Msg("  ModelsDirectory = {0}", config.ModelsDirectory);
            Logger.Msg("  ViewsDirectory = {0}", config.ViewsDirectory);
            Logger.Msg("  OutputDirectory = {0}", config.OutputDirectory);
        }
    }
}

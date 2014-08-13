using Lithogen.Core;
using Lithogen.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Lithogen
{
    class Program
    {
        public static readonly string NAME = "Lithogen";
        public static Settings Settings; 
        static Logger Logger;

        static void Main(string[] args)
        {
            try
            {
                Logger = new Logger("Main() ");
                Logger.Msg("Starting. Parsing configuration.");
                var argsDict = ProcessArgs(args);
                Settings = LoadSettings(argsDict);
                Settings.Validate(Logger);
                DumpConfig(Settings);
                if (Settings == null)
                    return;
                var container = ConfigureIoC();
                var builder = container.GetInstance<IBuilder>();
                Logger.Msg("IBuilder of type {0} created.", builder.GetType());
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

            container.Register<IBuilder, Builder>();
            container.Register<IFileSystem, WindowsFileSystem>();
            container.Register<ICountingFileSystem, CountingFileSystem>();
            Logger.Msg("Default Lithogen types registered.");

            container.Verify();
            Logger.Msg("IoC container verified.");
            return container;
        }

        //void ProveRazorMachineWorks(Logger logger)
        //{
        //    // Creating a machine is quick (about 0.1 seconds), rendering a simple
        //    // template is comparatively slow (about 0.6 seconds).
        //    var rm = CreateRazorMachineWithoutContentProviders();
        //    ITemplate template = rm.ExecuteContent("Razor says: Hello @Model.FirstName @Model.LastName", new { FirstName = "John", LastName = "Smith" });
        //    logger.Msg(template.Result);
        //}

        //RazorMachine CreateRazorMachineWithoutContentProviders(bool includeGeneratedSourceCode = false, string rootOperatorPath = null, bool htmlEncode = true)
        //{
        //    var rm = new RazorMachine(includeGeneratedSourceCode: includeGeneratedSourceCode, htmlEncode: htmlEncode, rootOperatorPath: rootOperatorPath);
        //    rm.Context.TemplateFactory.ContentManager.ClearAllContentProviders();
        //    return rm;
        //}


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
                    result[key] = parts[1];
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
        }
    }
}

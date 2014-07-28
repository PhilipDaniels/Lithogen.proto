using AppDomainToolkit;
using Lithogen.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Xipton.Razor;

namespace Lithogen.Core
{
    public class Bootstrapper : MarshalByRefObject
    {
        public string SolutionPath { get; set; }
        public string ProjectPath { get; set; }
        public string Configuration { get; set; }
        public string LithogenPluginDir { get; set; }
        public MessageImportance MessageImportance { get; set; }
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
        Logger Logger { get; set; }

        public bool Bootstrap()
        {
            if (BuildEngine == null)
                throw new NullReferenceException("The BuildEngine cannot be null.");
            // Nope, it can be.
            //if (HostObject == null)
            //    throw new NullReferenceException("The HostObject cannot be null.");
            if (!Enum.IsDefined(typeof(MessageImportance), MessageImportance))
                throw new ArgumentOutOfRangeException("The MessageImportance '" + ((int)MessageImportance) + "' is not valid.");

            Logger = new Logger(new TaskLoggingHelper(BuildEngine, "Lithogen.Core.Bootstrapper"));
            Logger.Prefix = "Lithogen.Core.Bootstrapper.Bootstrap() ";
            Logger.Importance = MessageImportance;
            Logger.Msg("Starting. This AppDomain.Id is {0} and FriendlyName is '{1}'.", AppDomain.CurrentDomain.Id, AppDomain.CurrentDomain.FriendlyName);
            Logger.Msg("The AppDomain.CurrentDomain.BaseDirectory is {0}.", AppDomain.CurrentDomain.BaseDirectory);
            Logger.Msg("The Thread.CurrentThread.ManagedThreadId is {0}.", Thread.CurrentThread.ManagedThreadId);

            ValidateRemainingInputParameters(Logger);

            using (var context = AppDomainContext.Create())
            {
                string thisFile = Assembly.GetExecutingAssembly().Location;
                IAssemblyTarget tgt = context.LoadAssembly(LoadMethod.LoadFrom, thisFile);

                var child = Remote<Bootstrapper>.CreateProxy(context.Domain);
                child.RemoteObject.SolutionPath = SolutionPath;
                child.RemoteObject.ProjectPath = ProjectPath;
                child.RemoteObject.Configuration = Configuration;
                child.RemoteObject.LithogenPluginDir = LithogenPluginDir;
                child.RemoteObject.MessageImportance = MessageImportance;
                child.RemoteObject.BuildEngine = BuildEngine;
                child.RemoteObject.HostObject = HostObject;
                bool result = child.RemoteObject.InnerBootstrap(AppDomain.CurrentDomain.Id);
            }

            Logger.Msg("completed.");
            return true;
        }

        public bool InnerBootstrap(int outerDomainId)
        {
            Logger = new Logger(new TaskLoggingHelper(BuildEngine, "Lithogen.Core.Bootstrapper"));
            Logger.Prefix = "Lithogen.Core.Bootstrapper.InnerBootstrap() ";
            Logger.Importance = MessageImportance;
            
            Logger.Msg("Starting. This AppDomain.Id is {0} and FriendlyName is '{1}'.", AppDomain.CurrentDomain.Id, AppDomain.CurrentDomain.FriendlyName);
            if (outerDomainId != AppDomain.CurrentDomain.Id)
            {
                Logger.Msg("We have successfully transitioned to a new AppDomain and the BaseDirectory is {0}.", AppDomain.CurrentDomain.BaseDirectory);
                Logger.Msg("The Thread.CurrentThread.ManagedThreadId is {0}.", Thread.CurrentThread.ManagedThreadId);
            }
            else
            {
                throw new Exception("FAILED TO TRANSITION TO NEW APP DOMAIN. SOMETHING IS VERY WRONG.");
            }

            //ProveRazorMachineWorks(Logger);

            var container = new SimpleInjector.Container();
            container.Options.AllowOverridingRegistrations = true; 
            Logger.Msg("IoC container created.");

            container.Register<IBuildContext, BuildContext>();
            Logger.Msg("Default Lithogen types registered.");

            container.Verify();
            Logger.Msg("IoC container verified.");

            var bc = container.GetInstance<IBuildContext>();
            bc.SolutionPath = SolutionPath;
            bc.ProjectPath = ProjectPath;
            bc.Configuration = Configuration;
            bc.MessageImportance = MessageImportance;
            bc.BuildEngine = BuildEngine;
            bc.HostObject = HostObject;
            Logger.Msg("IBuildContext created and configured.");

            var builder = container.GetInstance<IBuilder>();
            Logger.Msg("IBuilder created.");
            builder.Build();

            Logger.Msg("Completed.");
            return true;
        }

        void ProveRazorMachineWorks(Logger logger)
        {
            var rm = CreateRazorMachineWithoutContentProviders();
            ITemplate template = rm.ExecuteContent("Razor says: Hello @Model.FirstName @Model.LastName", new { FirstName = "John", LastName = "Smith" });
            logger.Msg(template.Result);
        }

        RazorMachine CreateRazorMachineWithoutContentProviders(bool includeGeneratedSourceCode = false, string rootOperatorPath = null, bool htmlEncode = true)
        {
            var rm = new RazorMachine(includeGeneratedSourceCode: includeGeneratedSourceCode, htmlEncode: htmlEncode, rootOperatorPath: rootOperatorPath);
            rm.Context.TemplateFactory.ContentManager.ClearAllContentProviders();
            //rm.AssemblyLoadPath = ThisLocation;
            return rm;
        }

        void ValidateRemainingInputParameters(Logger logger)
        {
            if (SolutionPath != null)
            {
                SolutionPath = SolutionPath.Trim();
                if (SolutionPath.Length == 0)
                {
                    SolutionPath = null;
                }
                if (SolutionPath != null && !File.Exists(SolutionPath))
                    throw new FileNotFoundException("The SolutionPath '" + SolutionPath + "' does not exist.");
            }

            if (String.IsNullOrWhiteSpace(ProjectPath))
                throw new ArgumentOutOfRangeException("ProjectPath must be set.");
            ProjectPath = ProjectPath.Trim();
            if (!File.Exists(ProjectPath))
                throw new FileNotFoundException("The ProjectPath '" + ProjectPath + "' does not exist.");

            if (Configuration != null)
                Configuration = Configuration.Trim();

            if (LithogenPluginDir != null)
            {
                LithogenPluginDir = LithogenPluginDir.Trim();
                if (LithogenPluginDir.Length == 0)
                {
                    LithogenPluginDir = null;
                }
                if (LithogenPluginDir != null)
                {
                    if (!Directory.Exists(LithogenPluginDir))
                    {
                        logger.Msg("The LithogenPluginDir '{0}' that you specified does not exist - ignoring.", LithogenPluginDir);
                        LithogenPluginDir = null;
                    }
                    else
                    {
                        string[] files = Directory.GetFiles(LithogenPluginDir, "*.dll", SearchOption.TopDirectoryOnly);
                        if (files == null || files.Length == 0)
                        {
                            logger.Msg("The LithogenPluginDir '{0}' exists, but contains no DLLs. Only built-in Lithogen types will be used.", LithogenPluginDir);
                        }
                        else
                        {
                            logger.Msg("The LithogenPluginDir '{0}' exists and contains {1} file(s) matching the filter *.dll. They will be searched for plugin types later.", LithogenPluginDir, files.Length);
                        }
                    }
                }
            }
        }
    }
}

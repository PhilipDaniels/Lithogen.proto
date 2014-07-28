using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;

namespace Lithogen.Core
{
    /// <summary>
    /// A facade around the MSBuild logger that supports indentation.
    /// </summary>
    public class Logger : MarshalByRefObject
    {
        public TaskLoggingHelper WrappedLog { get; set; }
        public MessageImportance Importance { get; set; }

        public Logger(TaskLoggingHelper wrappedLog)
        {
            WrappedLog = wrappedLog.ThrowIfNull("wrappedLog");
            Prefix = "";
        }

        public void Msg(string message)
        {
            Msg(message, null);
        }

        public void Msg(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
                message = String.Format(message, args);
            message = (new String(' ', Indentation * 2)) + Prefix + message;
            WrappedLog.LogMessage(Importance, message);
        }

        public int Indentation
        {
            get
            {
                return _Indentation;
            }
            set
            {
                _Indentation = value.ThrowIfLessThan(0, "Indentation cannot be negative.");
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int _Indentation;

        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            set
            {
                _Prefix = value ?? "";
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string _Prefix;
    }
}

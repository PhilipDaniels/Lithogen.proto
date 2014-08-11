using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen
{
    /// <summary>
    /// Writes messages to standard output (from where they will be picked up by
    /// MSBuild). Also logs errors to the standard output in a format that will be understood
    /// by MSBuild and routed to its "Errors" tab. See "Inside the Microsoft Build Engine"
    /// by Sayed Ibrahim Hashimi for the formats of errors and warnings.
    /// </summary>
    public class Logger
    {
        public string Prefix { get; set; }
        DateTime StartTime;
        DateTime LastTime;

        public Logger()
        {
            StartTime = DateTime.Now;
            LastTime = StartTime;
        }

        /// <summary>
        /// Logs a standard message, including time formatting.
        /// This is completely free-format.
        /// </summary>
        /// <param name="msg">Format text of the message.</param>
        /// <param name="args">Any parameters.</param>
        public void Msg(string msg, params object[] args)
        {
            msg = FormatMessage(msg, args);
            WriteLine(msg);
        }

        public void Error(string origin, string code, string msg, params object[] args)
        {
            Console.Write(origin);
            Console.Write(" : error ");
            Console.Write(code);
            Console.Write(" ");
            msg = FormatMessage(msg);
            WriteLine(msg);
        }

        string FormatMessage(string msg, params object[] args)
        {
            if (args != null && args.Length > 0)
                msg = String.Format(msg, args);

            DateTime now = DateTime.Now;
            double thisSecs = (now - LastTime).TotalSeconds;
            double totalSecs = (now - StartTime).TotalSeconds;
            msg = String.Format("[{0,6:##0.00},{1,6:##0.00}] {2}{3}",
                totalSecs,
                thisSecs,
                Prefix ?? "",
                msg ?? ""
                );
            LastTime = now;
            return msg;
        }

        void WriteLine(string msg)
        {
            Console.WriteLine(msg);
            Console.Out.Flush();
        }
    }
}

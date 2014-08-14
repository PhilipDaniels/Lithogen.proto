using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core
{
    public abstract class LoggerBase : ILogger
    {
        public string Prefix { get; set; }
        DateTime StartTime;
        DateTime LastTime;

        public LoggerBase()
            : this(null)
        {
        }

        public LoggerBase(string prefix)
        {
            StartTime = DateTime.Now;
            LastTime = StartTime;
            Prefix = prefix;
        }

        /// <summary>
        /// Logs a standard message, including time formatting.
        /// This is completely free-format.
        /// </summary>
        /// <param name="msg">Format text of the message.</param>
        /// <param name="args">Any parameters.</param>
        public virtual void Msg(string msg, params object[] args)
        {
            msg = FormatMessage(msg, args);
            LastTime = DateTime.Now;
            WriteLine(msg);
        }

        public virtual void Error(string origin, string code, string msg, params object[] args)
        {
            Write(origin);
            Write(" : error ");
            Write(code);
            Write(" ");
            msg = String.Format(msg, args);
            WriteLine(msg);
            LastTime = DateTime.Now;
        }

        public abstract void Write(string msg);
        public abstract void WriteLine(string msg);

        protected virtual string FormatMessage(string msg, params object[] args)
        {
            if (args != null && args.Length > 0)
                msg = String.Format(msg, args);

            var sb = new StringBuilder();
            DateTime now = DateTime.Now;
            double thisSecs = (now - LastTime).TotalSeconds;
            double totalSecs = (now - StartTime).TotalSeconds;
            string thisSecsFormatted = String.Format("{0,6:##0.00}", thisSecs);
            if (thisSecsFormatted.Equals("  0.00", StringComparison.OrdinalIgnoreCase)) {
                sb.Append("[       ");
            }
            else
            {
                sb.Append("[");
                sb.Append(thisSecsFormatted);
                sb.Append(",");
            }

            sb.AppendFormat("{0,6:##0.00}] {1}{2}", totalSecs, Prefix ?? "", msg ?? "" );

            return sb.ToString();
        }
    }
}

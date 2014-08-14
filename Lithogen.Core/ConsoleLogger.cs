using System;

namespace Lithogen.Core
{
    /// <summary>
    /// Writes messages to standard output (from where they will be picked up by
    /// MSBuild). Also logs errors to the standard output in a format that will be understood
    /// by MSBuild and routed to its "Errors" tab. See "Inside the Microsoft Build Engine"
    /// by Sayed Ibrahim Hashimi for the formats of errors and warnings.
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {
        public ConsoleLogger()
        {
        }

        public override void Write(string msg)
        {
            Console.Write(msg);
        }

        public override void WriteLine(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}

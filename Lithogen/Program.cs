using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lithogen
{
    class Program
    {
        static Logger Logger;

        static void Main(string[] args)
        {
            Logger = new Logger();
            Logger.Prefix = "Main() ";

            Logger.Msg("Starting.");
            Thread.Sleep(3000);
            Console.Out.Flush();
            Logger.Msg("Starting.");
            Thread.Sleep(3000);
            Console.Out.Flush();
            Logger.Msg("Starting.");
            Thread.Sleep(3000);
            Console.Out.Flush();
            Logger.Msg("Done.");
        }
    }
}

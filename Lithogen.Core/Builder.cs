using Lithogen.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core
{
    public class Builder : IBuilder
    {
        public IBuildContext BuildContext { get { return _BuildContext; } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly IBuildContext _BuildContext;

        public IList<IBuildStep> Steps { get; private set; }

        public Builder(IBuildContext buildContext)
        {
            _BuildContext = buildContext.ThrowIfNull("buildContext");
            Steps = new List<IBuildStep>();
        }

        public bool Build()
        {
            //var logger = BuildContext.MakeLogger("Builder");
            //logger.Msg("The Builder is beginning its execution.");
            //logger.LogIndent++;

            //int stepNum = 1;
            //foreach (var step in Steps)
            //{
            //    DateTime start = DateTime.Now;
            //    logger.Msg("Starting {0} ({1}/{2})", step.Name, stepNum, Steps.Count);
            //    bool ok = step.Execute(BuildContext);
            //    double timeTaken = (DateTime.Now - start).TotalSeconds;
            //    if (ok)
            //    {
            //        logger.Msg("Completed {0} ({1:0.00} seconds).", step.Name, timeTaken);
            //    }
            //    else
            //    {
            //        logger.Msg("BuildStep {0} returned false, aborting build ({1:0.00} seconds).", step.Name, timeTaken);
            //        return false;
            //    }

            //    stepNum++;
            //}

            return true;
        }
    }
}

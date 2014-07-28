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

        public Builder(IBuildContext buildContext)
        {
            _BuildContext = buildContext.ThrowIfNull("buildContext");
        }

        public bool Build()
        {
            throw new NotImplementedException();
        }
    }
}

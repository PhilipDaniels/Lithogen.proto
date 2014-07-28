using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Interfaces
{
    public interface IBuilder
    {
        IBuildContext BuildContext { get; }
        //IList<IBuildStep> Steps { get; }
        bool Build();
    }
}

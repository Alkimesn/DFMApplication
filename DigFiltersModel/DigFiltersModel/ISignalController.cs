using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigFiltersModel
{
    interface ISignalController
    {
        List<DFMSignal> savedSignals { get; }
        void LoadSignal(string id, Controller controller);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigFiltersModel
{
    interface IFilterController
    {
        List<DFMFilter> savedFilters { get; }
        void LoadFilter(string id, Controller controller);
    }
}

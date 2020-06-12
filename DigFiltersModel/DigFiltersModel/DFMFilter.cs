using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigFiltersModel
{
    public class DFMFilter
    {
        DFMCoefList upper;
        DFMCoefList lower;
        public string Name { get; private set; }
        public string UpperCoefsString { get { return upper.ToString(); } }
        public string LowerCoefsString { get { return lower.ToString(); } }
        public int Order { get { return Math.Max(upper.order, lower.order); } }
        public static DFMFilter identityFilter { get; private set; }
        public static DFMFilter doublingFilter { get; private set; }
        public static DFMFilter delayFilter { get; private set; }
        public static DFMFilter fadeFilter { get; private set; }
        public static DFMFilter avg2Filter { get; private set; }
        static DFMFilter()
        {
            DFMCoefList upper1 = new DFMCoefList(1, 0);
            DFMCoefList upper2 = new DFMCoefList(2);
            DFMCoefList upper3 = new DFMCoefList(0, 1);
            DFMCoefList upper4 = new DFMCoefList(0.5, 0.5);
            DFMCoefList lower1 = new DFMCoefList(1, 0);
            DFMCoefList lower2 = new DFMCoefList(1, -0.5);
            identityFilter = new DFMFilter(upper1, lower1);
            doublingFilter = new DFMFilter(upper2, lower1);
            delayFilter = new DFMFilter(upper3, lower1);
            fadeFilter = new DFMFilter(upper1, lower2);
            avg2Filter = new DFMFilter(upper4, lower1, "Average2");
        }
        public DFMFilter(DFMCoefList upper, DFMCoefList lower, string name=null)
        {
            Name = name == null ? "Unnamed filter" : name;
            this.upper = upper;
            this.lower = lower;
        }
        void OutputStep(DFMSignal input, DFMSignal output, int stepnum)
        {
            double res = upper.MultiplyBySignal(input, stepnum) - lower.MultiplyBySignal(output, stepnum, true);
            output[stepnum] = (int)(res / lower[0]);
        }
        public DFMSignal GenerateOutput(DFMSignal input, int stepsNumber)
        {
            DFMSignal output = new DFMSignal();
            for(int i=0;i<stepsNumber;i++)
            {
                OutputStep(input, output, i);
            }
            return output;
        }
    }
}

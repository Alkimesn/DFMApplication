using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DigFiltersModel
{
    class Controller
    {
        DFMFilter curFilter;
        public DFMFilter CurFilter
        {
            get { return curFilter; }
            set { curFilter = value; Reload(); }
        }
        DFMSignal curSignal;
        public DFMSignal CurSignal
        {
            get { return curSignal; }
            set { curSignal = value; Reload(); }
        }
        int maxLength;
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; Reload(); }
        }
        public DFMSignal OutSignal { get; private set; }
        public double[] InputSignalAsArray { get; private set; }
        public double[] OutputSignalAsArray { get; private set; }
        public double[] InputSignalFTAsArray { get; private set; }
        public double[] OutputSignalFTAsArray { get; private set; }
        public event EventHandler OnReload;
        bool IsPowerOf2(int value)
        {
            while(value!=1)
            {
                if (value % 2 == 1) return false;
                value = value / 2;
            }
            return true;
        }
        void Reload()
        {
            OutSignal = curFilter.GenerateOutput(curSignal, maxLength);
            InputSignalAsArray = curSignal.ToDoubleArray(maxLength);
            OutputSignalAsArray = OutSignal.ToDoubleArray(maxLength);
            if (IsPowerOf2(maxLength))
            {
                InputSignalFTAsArray = FastFourierTransform.Transform(InputSignalAsArray).Select(x => x.R).ToArray();
                OutputSignalFTAsArray = FastFourierTransform.Transform(OutputSignalAsArray).Select(x => x.R).ToArray();
            }
            else
            {
                InputSignalFTAsArray = new double[] { 0 };
                OutputSignalFTAsArray = new double[] { 0 };
            }
            OnReload?.Invoke(this,null);
        }
        public Controller()
        {
            curFilter = DFMFilter.delayFilter;
            curSignal = 2*DFMSignal.PeriodicSignal2+DFMSignal.PeriodicSignal3;
            maxLength = 8;
            Reload();
        }
    }
}

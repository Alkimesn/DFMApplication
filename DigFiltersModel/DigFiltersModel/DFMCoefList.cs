using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigFiltersModel
{
    public class DFMCoefList
    {
        double[] coefs;
        public int order { get { return coefs.Length - 1; } }
        public double this[int x] { get { if (x > coefs.Length) return 0; return coefs[x]; } }
        public DFMCoefList(params double[] coefs)
        {
            this.coefs = new double[coefs.Length];
            coefs.CopyTo(this.coefs, 0);
        }
        public double MultiplyBySignal(DFMSignal signal, int signalpos, bool IgnoreFirst=false)
        {
            double res = 0;
            int startInd = IgnoreFirst ? 1 : 0;
            for (int i = startInd; i < coefs.Length; i++)
                res += signal[signalpos - i] * coefs[i];
            return res;
        }
        public override string ToString()
        {
            return coefs.Aggregate("", (res, val) => res + " " + val);
        }
    }
}

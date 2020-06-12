using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigFiltersModel
{
    public class DFMSignal
    {
        string name;
        public string Name { get { return name == null ? "Unnamed signal" : name; } }
        List<int> values=new List<int>();
        public int this[int x]
        {
            get { if (x < 0 || x >= values.Count) return 0; return values[x]; }
            set { if (values.Count != x) throw new InvalidOperationException(); else values.Add(value); }
        }
        public int Length() { return values.Count; }
        public string ValuesString { get
            {
                return values.Aggregate("", (res, val) => res + " " + val);
            } }
        public DFMSignal(string name=null)
        {
            this.name= name;
        }
        public DFMSignal(string name=null, params int[] vals):this(name)
        {
            for (int i = 0; i < vals.Length; i++)
                this[i] = vals[i];
        }
        public static DFMSignal operator *(double x, DFMSignal signal)
        {
            DFMSignal res = new DFMSignal();
            for (int i = 0; i < signal.Length(); i++)
                res[i] = (int)(signal[i] * x);
            return res;
        }
        public static DFMSignal operator +(DFMSignal sig1, DFMSignal sig2)
        {
            DFMSignal res = new DFMSignal();
            int maxlength = sig1.Length() > sig2.Length() ? sig1.Length() : sig2.Length();
            for (int i = 0; i < maxlength; i++)
                res[i] = sig1[i] + sig2[i];
            return res;
        }
        public bool IsEqual(DFMSignal signal, int maxCompareLength=-1)
        {
            int maxlength = maxCompareLength == -1
                ? signal.Length() > Length() ? signal.Length() : Length()
                : maxCompareLength;
            for (int i = 0; i < maxlength; i++)
                if (this[i] != signal[i]) return false;
            return true;
        }
        public DFMSignal RightShift(int steps)
        {
            DFMSignal res = new DFMSignal();
            for (int i = 0; i < Length() + steps; i++)
                res[i] = this[i - steps];
            return res;
        }
        public double[] ToDoubleArray(int maxlength)
        {
            double[] res = new double[maxlength];
            for (int i = 0; i < maxlength; i++)
                res[i] = this[i];
            return res;
        }
        public static DFMSignal ImpulseSignal;
        public static DFMSignal ConstantSignal;
        public static DFMSignal VariableSignal;
        public static DFMSignal PeriodicSignal2;
        public static DFMSignal PeriodicSignal3;
        static DFMSignal()
        {
            ImpulseSignal = new DFMSignal("Impulse", 5);
            ConstantSignal = new DFMSignal("Constant", 5,5,5,5,5);
            VariableSignal = new DFMSignal("Variable", 4, 0, -4, 4, 4, 0, -4, -4);
            PeriodicSignal2 = new DFMSignal("Periodic2", 4, -4, 4, -4, 4, -4, 4, -4, 4, -4, 4, -4);
            PeriodicSignal3 = new DFMSignal("Periodic3", 4,0, -4, 4,0, -4, 4,0, -4, 4,0, -4, 4,0, -4, 4,0, -4);
        }
    }
}

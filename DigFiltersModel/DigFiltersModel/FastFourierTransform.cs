using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigFiltersModel
{
    static class FastFourierTransform
    {
        public static ComplexDouble[] Transform(ComplexDouble[] input)
        {
            int length = input.Length;
            if (length == 1) return new ComplexDouble[] { input[0] };
            ComplexDouble[] evens = new ComplexDouble[length / 2];
            ComplexDouble[] odds = new ComplexDouble[length / 2];
            for (int i = 0; i < length / 2; i++)
            {
                evens[i] = input[2 * i];
                odds[i] = input[2 * i + 1];
            }
            ComplexDouble[] evensRes = Transform(evens);
            ComplexDouble[] oddsRes = Transform(odds);
            for (int i = 0; i < length / 2; i++)
                oddsRes[i] *= ComplexDouble.FromRF(1, -2 * Math.PI * i / length);
            ComplexDouble[] res = new ComplexDouble[length];
            for (int i = 0; i < length / 2; i++)
            {
                res[i] = evensRes[i] + oddsRes[i];
                res[i+length/2] = evensRes[i] - oddsRes[i];
            }
            return res;
        }
        public static ComplexDouble[] Transform(double[] input)
        {
            return Transform(input.Select(x => ComplexDouble.FromAB(x, 0)).ToArray());
        }
    }
}

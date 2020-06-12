using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigFiltersModel
{
    class ComplexDouble
    {
        double a, b;
        public static ComplexDouble FromAB(double a, double b)
        {
            ComplexDouble res = new ComplexDouble();
            res.a = a;
            res.b = b;
            return res;
        }
        public static ComplexDouble FromRF(double r, double f)
        {
            ComplexDouble res = new ComplexDouble();
            res.a = r*Math.Cos(f);
            res.b = r*Math.Sin(f);
            return res;
        }
        public double A => a;
        public double B => b;
        public double R => Math.Sqrt(a * a + b * b);
        public static ComplexDouble I => ComplexDouble.FromAB(0, 1);
        public static ComplexDouble operator +(ComplexDouble z1, ComplexDouble z2)
        {
            return ComplexDouble.FromAB(z1.a + z2.a, z1.b + z2.b);
        }
        public static ComplexDouble operator *(ComplexDouble z, double x)
        {
            return ComplexDouble.FromAB(z.a*x, z.b*x);
        }
        public static ComplexDouble operator -(ComplexDouble z1, ComplexDouble z2)
        {
            return z1+(z2*-1);
        }
        public static ComplexDouble operator *(ComplexDouble z1, ComplexDouble z2)
        {
            return ComplexDouble.FromAB(z1.a*z2.a-z1.b*z2.b, z1.a*z2.b+z2.a*z1.b);
        }
        static ComplexDouble ImgExponent(double power)
        {
            return ComplexDouble.FromRF(1,power);
        }
    }
}

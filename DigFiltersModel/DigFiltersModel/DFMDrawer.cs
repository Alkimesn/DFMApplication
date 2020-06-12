using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DigFiltersModel
{
    static class DFMDrawer
    {
        //actual graph should fit in central 80%, both height and width
        static Func<double,double> GetLinear(double arg1, double res1, double arg2, double res2)
        {
            return x => (x - arg1) * (res2 - res1) / (arg2 - arg1) + res1;
        }
        static void DrawLine(Canvas canvas, double x1, double y1, double x2, double y2, Brush brush, double thickness)
        {
            Line line = new Line();
            (line.X1, line.X2, line.Y1, line.Y2) = (x1, x2, y1, y2);
            line.Stroke = brush;
            line.StrokeThickness = thickness;
            canvas.Children.Add(line);
        }
        static void DrawCoordAxes(Canvas canvas, double x, double y)
        {
            DrawLine(canvas, 0, y, ((Border)canvas.Parent).ActualWidth, y, Brushes.Black,3);
            DrawLine(canvas, x, 0, x, ((Border)canvas.Parent).ActualHeight, Brushes.Black, 3);
        }
        public static void Draw(Canvas canvas, double[] values1, double[] values2)
        {
            canvas.Children.Clear();
            double ymax = Math.Max(values1.Max(), values2.Max());
            if (ymax < 0) ymax = 0;
            double ymin = Math.Min(values1.Min(), values2.Min());
            if (ymin > 0) ymin = 0;
            ymin--;
            ymax++;
            double totalHeight = ((Border)canvas.Parent).ActualHeight;
            Func<double, double> yTransf = GetLinear(ymin, 0.95 * totalHeight, ymax, 0.05 * totalHeight);

            double xmax = Math.Max(values1.Length, values2.Length);
            double xmin = 0;
            xmin--;
            xmax++;
            double totalWidth = ((Border)canvas.Parent).ActualWidth;
            Func<double, double> xTransf = GetLinear(xmin, 0.05 * totalWidth, xmax, 0.95 * totalWidth);

            DrawCoordAxes(canvas, xTransf(0), yTransf(0));

            Brush br1 = Brushes.Blue;
            Brush br2 = Brushes.Red;

            for (int i = 0; i < values1.Length; i++)
            {
                double x0 = xTransf(i==0?0:i - 0.5);
                double x1 = xTransf(i + 0.5);
                double y = yTransf(values1[i]);
                DrawLine(canvas, x0, y, x1, y, br1, 1);
                if(i<values1.Length-1)
                {
                    double y2 = yTransf(values1[i + 1]);
                    DrawLine(canvas, x1, y, x1, y2, br1, 1);
                }
                else
                {
                    double y2 = yTransf(0);
                    DrawLine(canvas, x1, y, x1, y2, br1, 1);
                }
            }
            for (int i = 0; i < values2.Length; i++)
            {
                double x0 = xTransf(i == 0 ? 0 : i - 0.5);
                double x1 = xTransf(i + 0.5);
                double y = yTransf(values2[i]);
                DrawLine(canvas, x0, y, x1, y, br2, 1);
                if (i < values1.Length - 1)
                {
                    double y2 = yTransf(values2[i + 1]);
                    DrawLine(canvas, x1, y, x1, y2, br2, 1);
                }
                else
                {
                    double y2 = yTransf(0);
                    DrawLine(canvas, x1, y, x1, y2, br2, 1);
                }
            }
        }
    }
}

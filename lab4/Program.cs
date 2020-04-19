using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
    static class Program
    {
        static public double Func(double x) => Math.Pow(Math.Pow(x + 2, 2), 1 / 3f) - Math.Pow(Math.Pow(x - 2, 2), 1 / 3f);
        static public double X(double t) => 1 / Math.Pow(Math.Cos(t), 3);
        static public double Y(double t) => Math.Pow(Math.Tan(t), 3);
        static public double FuncY(double x, double a)
        {
            double b = (double)Math.Pow((double)(a / x), (1.0 / 3.0));
            double c = Math.Acos(b);
            double d = Math.Tan(c);
            double f = Math.Pow(d, 3);
            return f;
        }
        static public double FuncX(double y, double a) => a / Math.Pow(Math.Cos(Math.Atan(Math.Pow(y / a, 1 / 3f))), 3);
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

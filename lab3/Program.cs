using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    static class Program
    {
        static public double Func(double x) =>(Math.Pow(x + 2, 2 / 3f) - Math.Pow(Math.Pow(x - 2, 2), 1 / 3f));
          
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

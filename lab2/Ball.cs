using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace lab2
{
    class Ball
    {
        public Vector Pos { get; set; }
        public double Diameter { get; set; }
        public double Mass { get; set; }
        public Vector Velocity { get; set; }
        public Vector Direction { get; set; }
        public Color color { get; set; }
        public void DirectionVelocity() // вираховує напрям вектора, за законами лін алгебри
        {
            double x1 = (Direction.X - Pos.X);
            double y1 = (Direction.Y - Pos.Y);
            double k = Math.Sqrt(x1 * x1 + y1 * y1);
            Velocity.X = x1 * Velocity.X / k;
            Velocity.Y = y1 * Velocity.Y / k;

        }
        public void Move() // Функція Movе, яка вираховує наступні координати з врахуванням масси мяча
        {
            if (Velocity.X > 0 && Velocity.Y > 0)
            {
                Pos.X += Velocity.X - Velocity.X * Mass * 0.0001;
                Pos.Y += Velocity.Y - Velocity.Y * Mass * 0.0001;
            }
            else if (Velocity.X < 0 && Velocity.Y > 0)
            {
                Pos.X += Velocity.X + Velocity.X * Mass * 0.0001;
                Pos.Y += Velocity.Y - Velocity.Y * Mass * 0.0001;
            }
            else if (Velocity.X > 0 && Velocity.Y < 0)
            {
                Pos.X += Velocity.X - Velocity.X * Mass * 0.0001;
                Pos.Y += Velocity.Y + Velocity.Y * Mass * 0.0001;
            }
            else
            {
                Pos.X += Velocity.X + Velocity.X * Mass * 0.0001;
                Pos.Y += Velocity.Y + Velocity.Y * Mass * 0.0001;
            }
            Velocity.X = Velocity.X - Velocity.X * Mass * 0.0001;
            Velocity.Y = Velocity.Y - Velocity.Y * Mass * 0.0001;
            if (Math.Abs(Velocity.X) < 1E-3 || Math.Abs(Velocity.Y) < 1E-3)
            {
                Velocity.X = 0;
                Velocity.Y = 0;
            }

        }
        public void CollideBall(Ball b) // викликається при зіткненні з мячем, вираховує новий напрям вектору
        {
            double[] tempVelocity = { Velocity.X, Velocity.Y };
            Velocity.X = (Velocity.X * (Mass - b.Mass) + 2 * b.Mass * b.Velocity.X) / (Mass + b.Mass);
            Velocity.Y = (Velocity.Y * (Mass - b.Mass) + 2 * b.Mass * b.Velocity.Y) / (Mass + b.Mass);
            b.Velocity.X = (b.Velocity.X * (b.Mass - Mass) + 2 * Mass * tempVelocity[0]) / (Mass + b.Mass);
            b.Velocity.Y = (b.Velocity.Y * (b.Mass - Mass) + 2 * Mass * tempVelocity[1]) / (Mass + b.Mass);
        }
        public void CollideWall(Wall w) // викликається при зіткненні зі стіною, виразовує новий напрям вектору
        {

            switch (w.WallNumber)
            {
                case 1:
                    Velocity.X *= Math.Cos(Math.PI / 4) * (1 - Mass * 0.01);
                    Velocity.Y *= -(1 - Mass * 0.01);
                    break;
                case 2:
                    Velocity.X *= Math.Sin(Math.PI / 4) * (1 - Mass * 0.01);
                    Velocity.Y *= -(1 - Mass * 0.01);
                    break;
                case 3:
                    Velocity.X *= -(1 - Mass * 0.01);
                    Velocity.Y *= Math.Sin(Math.PI / 4) * (1 - Mass * 0.01);
                    break;
                case 4:
                    Velocity.X *= -(1 - Mass * 0.01);
                    Velocity.Y *= Math.Sin(Math.PI / 4) * (1 - Mass * 0.01);
                    break;
                default:
                    break;
            }


        }
    }
}

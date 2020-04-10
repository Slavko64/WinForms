using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace lab2
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        //Vector(double x, double y)
        //{
        //    X = x;
        //    Y = y;
        //}
    }
    class Wall
    {
        public int WallNumber { get; set; }
    }
    class Ball
        {
        public Vector Pos { get; set; }
        //public double PosY { get; set; }
        public double Diameter { get; set; }
        public double Mass { get; set; }
        public Vector Velocity { get; set; }
        public Vector Direction { get; set; }
        public Color color { get; set; }
        public void CollideBall(Ball b)
        {

            double alpha = (Velocity.X * b.Velocity.X + Velocity.Y * b.Velocity.Y) / (Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2)) * Math.Sqrt(Math.Pow(b.Velocity.X, 2) + Math.Pow(b.Velocity.Y, 2)));
            Velocity.X *= alpha * 0.9;
            Velocity.Y *= alpha * 0.9;
            b.Velocity.X = b.Velocity.X / alpha * 0.9;
            b.Velocity.Y = b.Velocity.Y / alpha * 0.9;
        }
        public void CollideWall(Wall w)
        {
            //Velocity.X *= -0.9;
            switch (w.WallNumber) {
                case 1: 
                    Velocity.X *= Math.Cos(Math.PI / 4) *0.9;
                    Velocity.Y *= -0.9;
                    break;
                case 2:
                    Velocity.X *= Math.Sin(Math.PI / 4) * 0.9;
                    Velocity.Y *= -0.9;
                    break;
                case 3:
                    Velocity.X *= -0.9;
                    Velocity.Y *= Math.Sin(Math.PI / 4) * 0.9;
                    break;
                case 4:
                    Velocity.X *= -0.9;
                    Velocity.Y *= Math.Sin(Math.PI / 4) * 0.9;
                    break;
                default:
                    break;
        }
            

        }
    }
    class MyForm : Form
    {
        private readonly SolidBrush[] brush = new SolidBrush[2];
        private readonly RectangleF[] rect = new RectangleF[2];
        Panel panel;
        Graphics graphics;
        BufferedGraphicsContext bufferedGraphicsContext;
        BufferedGraphics bufferedGraphics;

        //
        static Ball[] Balls = new Ball[2];
        int down = 245;
        //
        TextBox Balltxt;
        private readonly Button[] _getXY = new Button[2];
        string[] colors = new string[]{"Red", "Orange", "Yellow", "Green",
            "Cyan", "Blue", "Violet", "Purple", "Pink", "Brown",
            "White", "Gray", "Black"};
        private readonly NumericUpDown[] xBox = new NumericUpDown[2];
        private readonly NumericUpDown[] yBox = new NumericUpDown[2];
        Label Velocity;
        private readonly NumericUpDown[] VelocityBox = new NumericUpDown[2];
        Label Diameter;
        private readonly NumericUpDown[] DiameterBox = new NumericUpDown[2];
        Label Mass;
        private readonly NumericUpDown[] MassBox = new NumericUpDown[2];
        Label Colorl;
        private readonly ComboBox[] ColorsBox = new ComboBox[2];
        Button[] GetDirection = new Button[2];
        private readonly NumericUpDown[] DirectionX = new NumericUpDown[2];
        private readonly NumericUpDown[] DirectionY = new NumericUpDown[2];
        private readonly Button[] drawBall = new Button[2];
        private readonly Button[] HideBall = new Button[2];
        Timer t1 = new Timer();
        int Ballcount = 0;
        //static List<Ball> Balls = new List<Ball>(2);
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyForm());
        }

        private void InitializeGraphics()
        {
            graphics = panel.CreateGraphics();
            bufferedGraphicsContext = new BufferedGraphicsContext();
            bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, new Rectangle(0, 0, panel.Width, panel.Height));
        }
        private void DrawToBuffer()
        {
            bufferedGraphics.Graphics.Clear(BackColor);

            bufferedGraphics.Graphics.FillEllipse(brush[0], rect[0]);
            bufferedGraphics.Graphics.FillEllipse(brush[1], rect[1]);

            bufferedGraphics.Render();
        }
        MyForm()
        {
            panel = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(760, 540)
            };
            Controls.Add(panel);

            InitializeGraphics();

            Text = "Ball";
            MaximizeBox = false;
            ClientSize = new Size(1000, 600);
            CenterToScreen();
            MenuItem miNewCalc = new MenuItem("Reset",
                new EventHandler(OnMenuStart), Shortcut.F2);
            MenuItem miSeparator = new MenuItem("-");
            MenuItem miAdd = new MenuItem("Add", new EventHandler(OnMenuAdd));
            MenuItem miExit = new MenuItem("Exit",
                new EventHandler(OnMenuExit), Shortcut.CtrlX);
            MenuItem miCalc = new MenuItem("&Menu",
                new MenuItem[] { miNewCalc, miAdd, miSeparator, miExit });
            Menu = new MainMenu(new MenuItem[] { miCalc });
            this.ActiveControl = null;
            Paint += new PaintEventHandler(PaintWall);
            Button Go = new Button
            {
                Top = 490,
                Left = 800,
                Width = 200,
                Height = 60,
                Text = "Go",
                Font = new Font("Arial", 30, FontStyle.Bold)
            };
            Controls.Add(Go);
            Label Name = new Label
            {
                Top = 560,
                Left = 800,
                Width = 200,
                Text = "Виконав: Ярослав Рибак ПМ-31",
                Font = new Font("Arial",8, FontStyle.Italic)
            };
            Controls.Add(Name);

            t1.Enabled = true;
            t1.Interval = (int)20D;
            Balls[0] = new Ball();
            Balls[1] = new Ball();
            Go.Click += new EventHandler(OnClickGo);
            xBox[0] = new NumericUpDown();
            xBox[1] = new NumericUpDown();
            yBox[0] = new NumericUpDown();
            yBox[1] = new NumericUpDown();
            _getXY[0] = new Button();
            _getXY[1] = new Button();
            VelocityBox[0] = new NumericUpDown();
            VelocityBox[1] = new NumericUpDown();
            DiameterBox[0] = new NumericUpDown();
            DiameterBox[1] = new NumericUpDown();
            MassBox[0] = new NumericUpDown();
            MassBox[1] = new NumericUpDown();
            ColorsBox[0] = new ComboBox();
            ColorsBox[1] = new ComboBox();
            DirectionX[0] = new NumericUpDown();
            DirectionY[0] = new NumericUpDown();
            DirectionX[1] = new NumericUpDown();
            DirectionY[1] = new NumericUpDown();
            brush[0] = new SolidBrush(Color.FromArgb(240,240,240));
            brush[1] = new SolidBrush(Color.FromArgb(240, 240, 240));
            rect[0] = new RectangleF(20, 20, 0, 0);
            rect[1] = new RectangleF(20, 20, 0, 0);


        }
        private void OnClickGo(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Go")
            {
                (sender as Button).Text = "Stop";

                t1.Start();
                t1.Tick += new EventHandler(OnTimer1);
            }
            else
            {
                (sender as Button).Text = "Go";
                t1.Stop();
            }
        }
        private void OnTimer1(object sender, EventArgs e)
        {

            if (brush[0].Color != Color.FromArgb(240, 240, 240))
            {
                if (rect[0].IntersectsWith(rect[1]) == true)
                {
                    Balls[0].CollideBall(Balls[1]);
                }
            }
            if (rect[0].Bottom >= panel.Bottom-20 && Balls[0].Velocity.Y > 0)
            {
                Balls[0].CollideWall(new Wall { WallNumber = 1 });
            }
            else if(rect[0].Top<= panel.Top-20 && Balls[0].Velocity.Y < 0)
            {
                Balls[0].CollideWall(new Wall { WallNumber = 2 });
            }
            else if(rect[0].Right >= panel.Right-20 && Balls[0].Velocity.X > 0)
            {
                Balls[0].CollideWall(new Wall { WallNumber = 3 });
            }
            else if(rect[0].Left  <= panel.Left-20 && Balls[0].Velocity.X < 0)
            {
                Balls[0].CollideWall(new Wall { WallNumber = 4 });
            }
           
            Balls[0].Pos.X += Balls[0].Velocity.X;
            Balls[0].Pos.Y += Balls[0].Velocity.Y;
            rect[0].Offset((float)Balls[0].Velocity.X,(float)Balls[0].Velocity.Y);
            
            if(brush[0].Color != Color.FromArgb(240,240,240))
            {
                
                    if (rect[1].Bottom >= panel.Bottom - 20 && Balls[1].Velocity.Y > 0)
                {
                    Balls[1].CollideWall(new Wall { WallNumber = 1 });
                }
                else if (rect[1].Top <= panel.Top - 20 && Balls[1].Velocity.Y < 0)
                {
                    Balls[1].CollideWall(new Wall { WallNumber = 2 });
                }
                else if (rect[1].Right >= panel.Right - 20 && Balls[1].Velocity.X > 0)
                {
                    Balls[1].CollideWall(new Wall { WallNumber = 3 });
                }
                else if (rect[1].Left <= panel.Left - 20 && Balls[1].Velocity.X < 0)
                {
                    Balls[1].CollideWall(new Wall { WallNumber = 4 });
                }

                Balls[1].Pos.X += Balls[1].Velocity.X;
                Balls[1].Pos.Y += Balls[1].Velocity.Y;
                rect[1].Offset((float)Balls[1].Velocity.X, (float)Balls[1].Velocity.Y);
            }
            DrawToBuffer();

        }

        private void OnMenuAdd(object sender, EventArgs e)
        {

            #region Ball Properties
            Balltxt = new TextBox
            {
                Left = 805,
                Width = 190,
                Top =  0,
                TextAlign = HorizontalAlignment.Center,
                ReadOnly = true,
                BackColor = System.Drawing.Color.White,
                Text = "Ball №" + (Ballcount + 1),
                Font = new Font("Cambria", 12, FontStyle.Regular)
            };
            _getXY[0].Top = 30;
            _getXY[0].Left = 805;
            _getXY[0].Text = "X   Y";
            _getXY[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            _getXY[0].Width = 90;
            Velocity = new Label
            {
                Left = 805,
                Top = 72,
                Text = "Velocity",
                Font = new Font("Cambria", 12, FontStyle.Regular),
                Width = 90
            };
            Diameter = new Label
            {
                Left = 805,
                Top = 102,
                Text = "Diameter",
                Font = new Font("Cambria", 12, FontStyle.Regular),
                Width = 90
            };
            Mass = new Label
            {
                Left = 805,
                Top = 132,
                Text = "Mass",
                Font = new Font("Cambria", 12, FontStyle.Regular),
                Width = 90
            };
            Colorl = new Label
            {
                Top =  160,
                Left = 805,
                Text = "Color",
                Font = new Font("Cambria", 12, FontStyle.Regular),
                Width = 80
            };
            GetDirection[0] = new Button
            {
                Top =  190,
                Left = 805,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Direction",
                Name = "" + Ballcount,
                Font = new Font("Cambria", 12, FontStyle.Regular),
                Width = 90
            };
            drawBall[0] = new Button
            {
                Top =  217,
                TextAlign = ContentAlignment.TopCenter,
                Left = 805,
                Text = "Draw",
                Name = "" + Ballcount,
                Font = new Font("Cambria", 12, FontStyle.Regular)
            };

            HideBall[0] = new Button
            {
                Top =  217,
                Left = 900,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Hide",
                Font = new Font("Cambria", 12, FontStyle.Regular)
            };
            GetDirection[0] = new Button
            {
                Top = 190,
                Left = 805,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Direction",
                Name = "" + Ballcount,
                Font = new Font("Cambria", 12, FontStyle.Regular),
                Width = 90
            };



            xBox[0].Left = 900;
            xBox[0].Top =  30;
            xBox[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            xBox[0].Minimum = 100;
            xBox[0].Maximum = 780;
            xBox[0].Width = 50;

            yBox[0].Left = 950;
            yBox[0].Top =  30;
            yBox[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            yBox[0].Width = 50;
            yBox[0].Minimum = 100;
            yBox[0].Maximum = 780;


            VelocityBox[0].Left = 900;
            VelocityBox[0].Top = 70;
            VelocityBox[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            VelocityBox[0].Minimum = 5;
            VelocityBox[0].Width = 80;


            DiameterBox[0].Left = 900;
            DiameterBox[0].Top = 100;
            DiameterBox[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            DiameterBox[0].Minimum = 50;
            DiameterBox[0].Width = 80;

            MassBox[0].Left = 900;
            MassBox[0].Top =  130;
            MassBox[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            MassBox[0].Minimum = 5;
            MassBox[0].Width = 80;




            ColorsBox[0].Top =  160;
            ColorsBox[0].Left = 900;
            ColorsBox[0].IntegralHeight = false;
            ColorsBox[0].DropDownStyle = ComboBoxStyle.DropDownList;
            ColorsBox[0].Size = new System.Drawing.Size(80, 81);
            ColorsBox[0].Items.AddRange(colors);

            DirectionX[0].Left = 900;
            DirectionX[0].Top =  190;
            DirectionX[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            DirectionX[0].Minimum = 20;
            DirectionX[0].Maximum = 780;
            DirectionX[0].Width = 50;


            DirectionY[0].Left = 950;
            DirectionY[0].Top = 190;
            DirectionY[0].Font = new Font("Cambria", 12, FontStyle.Regular);
            DirectionY[0].Minimum = 20;
            DirectionY[0].Maximum = 780;
            DirectionY[0].Width = 50;


            Controls.Add(_getXY[0]);
            Controls.Add(Balltxt);
            Controls.Add(xBox[0]);
            Controls.Add(yBox[0]);
            Controls.Add(Velocity);
            Controls.Add(VelocityBox[0]);
            Controls.Add(Diameter);
            Controls.Add(DiameterBox[0]);
            Controls.Add(Mass);
            Controls.Add(MassBox[0]);
            Controls.Add(Colorl);
            Controls.Add(ColorsBox[0]);
            Controls.Add(GetDirection[0]);
            Controls.Add(DirectionX[0]);
            Controls.Add(DirectionY[0]);
            Controls.Add(drawBall[0]);
            Controls.Add(HideBall[0]);
            drawBall[0].Click += new EventHandler(OnDrawClick0);
            if(Ballcount == 1)
            {
                Balltxt = new TextBox
                {
                    Left = 805,
                    Width = 190,
                    Top = Ballcount * down,
                    TextAlign = HorizontalAlignment.Center,
                    ReadOnly = true,
                    BackColor = System.Drawing.Color.White,
                    Text = "Ball №" + (Ballcount + 1),
                    Font = new Font("Cambria", 12, FontStyle.Regular)
                };
                _getXY[1].Top = 30 + Ballcount * down;
                _getXY[1].Left = 805;
                _getXY[1].Text = "X  Y";
                _getXY[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                _getXY[1].Width = 90;
                Velocity = new Label
                {
                    Left = 805,
                    Top = Ballcount * down + 72,
                    Text = "Velocity",
                    Font = new Font("Cambria", 12, FontStyle.Regular),
                    Width = 90
                };
                Diameter = new Label
                {
                    Left = 805,
                    Top = Ballcount * down + 102,
                    Text = "Diameter",
                    Font = new Font("Cambria", 12, FontStyle.Regular),
                    Width = 90
                };
                Mass = new Label
                {
                    Left = 805,
                    Top = Ballcount * down + 132,
                    Text = "Mass",
                    Font = new Font("Cambria", 12, FontStyle.Regular),
                    Width = 90
                };
                Colorl = new Label
                {
                    Top = Ballcount * down + 160,
                    Left = 805,
                    Text = "Color",
                    Font = new Font("Cambria", 12, FontStyle.Regular),
                    Width = 80
                };
                GetDirection[1] = new Button
                {
                    Top = Ballcount * down + 190,
                    Left = 805,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "Direction",
                    Name = "" + Ballcount,
                    Font = new Font("Cambria", 12, FontStyle.Regular),
                    Width = 90
                };
                drawBall[1] = new Button
                {
                    Top = Ballcount * down + 217,
                    TextAlign = ContentAlignment.TopCenter,
                    Left = 805,
                    Text = "Draw",
                    Name = "" + Ballcount,
                    Font = new Font("Cambria", 12, FontStyle.Regular)
                };

                HideBall[1] = new Button
                {
                    Top = Ballcount * down + 217,
                    Left = 900,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "Hide",
                    Font = new Font("Cambria", 12, FontStyle.Regular)
                };
                GetDirection[1] = new Button
                {
                    Top = Ballcount * down + 190,
                    Left = 805,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "Direction",
                    Name = "" + Ballcount,
                    Font = new Font("Cambria", 12, FontStyle.Regular),
                    Width = 90
                };

                xBox[1].Left = 900;
                xBox[1].Top = Ballcount * down + 30;
                xBox[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                xBox[1].Width = 50;
                xBox[1].Minimum = 100;
                xBox[1].Maximum = 780;

                yBox[1].Left = 950;
                yBox[1].Top = Ballcount * down + 30;
                yBox[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                yBox[1].Width = 50;
                yBox[1].Minimum = 100;
                yBox[1].Maximum = 780;

                DirectionX[1].Left = 900;
                DirectionX[1].Top = Ballcount * down + 190;
                DirectionX[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                DirectionX[1].Minimum = 20;
                DirectionX[1].Maximum = 780;
                DirectionX[1].Name = "" + Ballcount;
                DirectionX[1].Width = 50;

                DirectionY[1].Left = 950;
                DirectionY[1].Top = Ballcount * down + 190;
                DirectionY[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                DirectionY[1].Minimum = 20;
                DirectionY[1].Maximum = 780;
                DirectionY[1].Name = "" + Ballcount;
                DirectionY[1].Width = 50;

                VelocityBox[1].Left = 900;
                VelocityBox[1].Top = Ballcount * down + 70;
                VelocityBox[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                VelocityBox[1].Minimum = 5;
                VelocityBox[1].Width = 80;


                DiameterBox[1].Left = 900;
                DiameterBox[1].Top = Ballcount * down + 100;
                DiameterBox[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                DiameterBox[1].Minimum = 50;
                DiameterBox[1].Width = 80;

                MassBox[1].Left = 900;
                MassBox[1].Top = Ballcount * down + 130;
                MassBox[1].Font = new Font("Cambria", 12, FontStyle.Regular);
                MassBox[1].Minimum = 5;
                MassBox[1].Width = 80;




                ColorsBox[1].Top = Ballcount * down + 160;
                ColorsBox[1].Left = 900;
                ColorsBox[1].IntegralHeight = false;
                ColorsBox[1].DropDownStyle = ComboBoxStyle.DropDownList;
                ColorsBox[1].Size = new System.Drawing.Size(80, 81);
                ColorsBox[1].Items.AddRange(colors);

                //Controls.Add(x);
                //Controls.Add(y);
                Controls.Add(Balltxt);
                Controls.Add(_getXY[1]);
                Controls.Add(xBox[1]);
                Controls.Add(yBox[1]);
                Controls.Add(Velocity);
                Controls.Add(VelocityBox[1]);
                Controls.Add(Diameter);
                Controls.Add(DiameterBox[1]);
                Controls.Add(Mass);
                Controls.Add(MassBox[1]);
                Controls.Add(Colorl);
                Controls.Add(ColorsBox[1]);
                Controls.Add(GetDirection[1]);
                Controls.Add(DirectionX[1]);
                Controls.Add(DirectionY[1]);
                Controls.Add(drawBall[1]);
                Controls.Add(HideBall[1]);
                drawBall[1].Click += new EventHandler(OnDrawClick1);
            }
           
            ColorsBox[0].SelectedIndexChanged += (s, ea) =>
            {
                
                
                switch ((s as ComboBox).SelectedItem.ToString()) {
                    case "Red":
                        Balls[0].color = Color.Red;
                        break;
                    case "Orange":
                        Balls[0].color = Color.Orange;
                        break;
                    case "Yellow":
                        Balls[0].color = Color.Yellow;
                        break;
                    case "Green":
                        Balls[0].color = Color.Green;
                        break;
                    case "Cyan":
                        Balls[0].color = Color.Cyan;
                        break;
                    case "Blue":
                        Balls[0].color = Color.Blue;
                        break;
                    case "Violet":
                        Balls[0].color = Color.Violet;
                        break;
                    case "Purple":
                        Balls[0].color = Color.Purple;
                        break;
                    case "Pink":
                        Balls[0].color = Color.Pink;
                        break;
                    case "Brown":
                        Balls[0].color = Color.Brown;
                        break;
                    case "White":
                        Balls[0].color = Color.White;
                        break;
                    case "Gray":
                        Balls[0].color = Color.Gray;
                        break;
                    case "Black":
                        Balls[0].color = Color.Black;
                        break;
                    default:
                        Balls[0].color = Color.Red;
                        break;
                }
            };
            ColorsBox[1].SelectedIndexChanged += (s, ea) =>
            {


                switch ((s as ComboBox).SelectedItem.ToString())
                {
                    case "Red":
                        Balls[1].color = Color.Red;
                        break;
                    case "Orange":
                        Balls[1].color = Color.Orange;
                        break;
                    case "Yellow":
                        Balls[1].color = Color.Yellow;
                        break;
                    case "Green":
                        Balls[1].color = Color.Green;
                        break;
                    case "Cyan":
                        Balls[1].color = Color.Cyan;
                        break;
                    case "Blue":
                        Balls[1].color = Color.Blue;
                        break;
                    case "Violet":
                        Balls[1].color = Color.Violet;
                        break;
                    case "Purple":
                        Balls[1].color = Color.Purple;
                        break;
                    case "Pink":
                        Balls[1].color = Color.Pink;
                        break;
                    case "Brown":
                        Balls[1].color = Color.Brown;
                        break;
                    case "White":
                        Balls[1].color = Color.White;
                        break;
                    case "Gray":
                        Balls[1].color = Color.Gray;
                        break;
                    case "Black":
                        Balls[1].color = Color.Black;
                        break;
                    default:
                        Balls[1].color = Color.Red;
                        break;
                }
            };
            GetDirection[0].Click += (s, ea) =>
            {
                Balls[0].Direction = new Vector();
                Timer t = new Timer();
                t.Interval = 10;
                t.Start();
                t.Tick += (s2,ea2) => {
                    Point p = new Point();
                    p = panel.PointToClient(Cursor.Position);
                    if (p.X > DirectionX[0].Maximum) p.X = (int)DirectionX[0].Maximum;
                    else if (p.X < DirectionX[0].Minimum) p.X = (int)DirectionX[0].Minimum;
                    DirectionX[0].Value = p.X;
                    if (p.Y > DirectionY[0].Maximum) p.Y = (int)DirectionY[0].Maximum;
                    else if (p.Y < DirectionX[0].Minimum) p.Y = (int)DirectionY[0].Minimum;
                    DirectionY[0].Value = p.Y;
                };
                panel.Click += (s1, ea1) =>
                {
                    t.Stop();
                    t.Dispose();
                    Balls[0].Direction.X = (double)DirectionX[0].Value;
                    Balls[0].Direction.Y = (double)DirectionY[0].Value;
                };
                Click += (s1, ea1) =>
                 {
                     t.Stop();
                     t.Dispose();
                     Balls[0].Direction.X = (double)DirectionX[0].Value;
                     Balls[0].Direction.Y = (double)DirectionY[0].Value;
                 };
                
            };
            _getXY[0].Click += (s, ea) =>
            {
                Timer t = new Timer();
                t.Interval = 10;
                t.Start();
                t.Tick += (s2, ea2) => {
                    Point p = new Point();
                    p = panel.PointToClient(Cursor.Position);
                    if (p.X > xBox[0].Maximum) p.X = (int)xBox[0].Maximum;
                    else if (p.X < xBox[0].Minimum) p.X = (int)xBox[0].Minimum;
                    xBox[0].Value = p.X;
                    if (p.Y > yBox[0].Maximum) p.Y = (int)yBox[0].Maximum;
                    else if (p.Y < yBox[0].Minimum) p.Y = (int)yBox[0].Minimum;
                    yBox[0].Value = p.Y;
                };
                panel.Click += (s1, ea1) =>
                {
                    t.Stop();
                    t.Dispose();
                };
            };
            #endregion
            Ballcount++;

        }

        private void OnDrawClick1(object sender, EventArgs e)
        {
            Balls[1].Pos = new Vector
            {
                X = Convert.ToDouble(xBox[1].Text),
                Y = Convert.ToDouble(yBox[1].Text)
            };
            Balls[1].Diameter = Convert.ToDouble(DiameterBox[1].Text);
            Balls[1].Mass = Convert.ToDouble(MassBox[1].Text);
            Balls[1].Velocity = new Vector
            {
                X = Convert.ToDouble(VelocityBox[1].Text),
                Y = Convert.ToDouble(VelocityBox[1].Text)
            };
            Balls[1].Direction = new Vector
            {
                X = Convert.ToDouble(DirectionX[1].Text),
                Y = Convert.ToDouble(DirectionY[1].Text)
            };
            rect[1].X = (float)Balls[1].Pos.X;
            rect[1].Y = (float)Balls[1].Pos.Y;
            rect[1].Height = (float)Balls[1].Diameter;
            rect[1].Width = (float)Balls[1].Diameter;
            brush[1].Color = Balls[1].color;
            DrawToBuffer();

            double x1 = (Balls[1].Direction.X - rect[1].X);
            double y1 = (Balls[1].Direction.Y - rect[1].Y);
            double k = Math.Sqrt(x1 * x1 + y1 * y1);
            if (Balls[1].Direction.X < rect[1].X && Balls[1].Direction.Y < rect[1].Y)
            {
                Balls[1].Velocity.X = x1 * Balls[1].Velocity.X / k;
                Balls[1].Velocity.Y = y1 * Balls[1].Velocity.Y / k;
            }
            else
            {
                Balls[1].Velocity.X = x1 * Balls[1].Velocity.X / k - (Balls[1].Diameter / 2) * Balls[1].Velocity.X / k;
                Balls[1].Velocity.Y = y1 * Balls[1].Velocity.Y / k - (Balls[1].Diameter / 2) * Balls[1].Velocity.X / k;
            }
        }

        private void OnDrawClick0(object sender, EventArgs e)
        {
            
            Balls[0].Pos = new Vector
            {
                X = Convert.ToDouble(xBox[0].Text),
                Y = Convert.ToDouble(yBox[0].Text)
            };
            Balls[0].Diameter = Convert.ToDouble(DiameterBox[0].Text);
            Balls[0].Mass = Convert.ToDouble(MassBox[0].Text);
            Balls[0].Velocity = new Vector
            {
                X = Convert.ToDouble(VelocityBox[0].Text),
                Y = Convert.ToDouble(VelocityBox[0].Text)
            };
            Balls[0].Direction = new Vector
            {
                X = Convert.ToDouble(DirectionX[0].Text),
                Y = Convert.ToDouble(DirectionY[0].Text)
            };
            rect[0].X = (float)Balls[0].Pos.X;
            rect[0].Y = (float)Balls[0].Pos.Y;
            rect[0].Height = (float)Balls[0].Diameter;
            rect[0].Width = (float)Balls[0].Diameter;
            brush[0].Color = Balls[0].color;
            DrawToBuffer();

            double x1 = (Balls[0].Direction.X - rect[0].X);
            double y1 = (Balls[0].Direction.Y - rect[0].Y);
            double k = Math.Sqrt(x1 * x1 + y1 * y1);
            if (Balls[0].Direction.X < rect[0].X && Balls[0].Direction.Y < rect[0].Y)
            {
                Balls[0].Velocity.X = x1 * Balls[0].Velocity.X / k;
                Balls[0].Velocity.Y = y1 * Balls[0].Velocity.Y / k;
            }
            else
            {
                Balls[0].Velocity.X = x1 * Balls[0].Velocity.X / k - (Balls[0].Diameter / 2) * Balls[0].Velocity.X / k;
                Balls[0].Velocity.Y = y1 * Balls[0].Velocity.Y / k - (Balls[0].Diameter / 2) * Balls[0].Velocity.X / k;
            }
        }
        public void PaintWall(object obj, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, 20, 600);
            e.Graphics.FillRectangle(Brushes.Black, 780, 0, 20, 600);
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, 780, 20);
            e.Graphics.FillRectangle(Brushes.Black, 0, 560, 780, 20);
        }

        void OnMenuStart(object obj, EventArgs ea)
        {

        }
        
        void OnMenuExit(object obj, EventArgs ea)
        {
            Close();
        }
    }


}

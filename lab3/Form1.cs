using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
   
    public partial class Form1 : Form
    {
        double a;
        double b;
        double n;
        List<Pt> graph_pts = new List<Pt>();
        int SizeX = 800;
        int SizeY = 600;
        //a - start, b - end, n - count of points tabul 
        TextBox A;
        TextBox B = new TextBox();
        TextBox N = new TextBox();
        TextBox Min = new TextBox();
        TextBox Max = new TextBox();
        Chart chart1 = new Chart();
        PictureBox pictureBox1 = new PictureBox();
        Button Ok = new Button();
        Label x = new Label();
        Label y = new Label();

        DataGridView _data = new DataGridView();
        public Form1()
        {
            InitializeComponent();
            Text = "lab3";
            MaximizeBox = true;
            ClientSize = new Size(SizeX, SizeY);
            MenuItem miNewCalc = new MenuItem("Reset",
                new EventHandler(OnMenuStart), Shortcut.F2);
            MenuItem miSeparator = new MenuItem("-");
            MenuItem miExit = new MenuItem("Exit",
                new EventHandler(OnMenuExit), Shortcut.CtrlX);
            MenuItem miCalc = new MenuItem("&Menu",
                new MenuItem[] { miNewCalc, miSeparator, miExit });
            Menu = new MainMenu(new MenuItem[] { miCalc });
            CenterToScreen();
            _data.Width = ClientSize.Width / 4;
            _data.Height = ClientSize.Height /2;
            _data.Location = new Point(ClientSize.Width - SizeX / 4, ClientSize.Height / 10);
            _data.BackgroundColor = Color.Azure;
            _data.Columns.Add("", "");
            _data.ColumnCount = 2;
            //_data.Columns[-1].Width = 0;
            _data.RowHeadersVisible = false;
            _data.Columns[0].Name = "X";
            //_data.Columns[0]. = 0;
            _data.Columns[0].HeaderText = "X";
            _data.Columns[0].Width = _data.Width / 2 - 10;
            _data.Columns[0].ReadOnly = true;
            _data.Columns[1].Name = "Y";
            _data.Columns[1].HeaderText = "Y";
            _data.Columns[1].Width = _data.Width / 2 - 10;
            _data.Columns[1].ReadOnly = true;
            


            A  = new TextBox();
            A.Location = new Point(ClientSize.Width - ClientSize.Width*4/5,ClientSize.Height);
            A.AutoSize = false;
            B.AutoSize = false;
            N.AutoSize = false;
            A.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            A.Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12, ClientSize.Height - A.Height- ClientSize.Height/15);
            A.Font = new Font("Arial", A.Height/2, FontStyle.Bold);
           
            B.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            B.Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + B.Width + ClientSize.Width / 25, ClientSize.Height - A.Height - ClientSize.Height / 15);
            B.Font = new Font("Arial", B.Height / 2, FontStyle.Bold);
            
            N.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            N.Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + 2 * N.Width + 2*ClientSize.Width / 25, ClientSize.Height - A.Height - ClientSize.Height / 15);
            N.Font = new Font("Arial", N.Height / 2, FontStyle.Bold);

            Max.AutoSize = false;
            Max.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Max.ReadOnly = true;
            Max.Location = new Point(_data.Left, _data.Top + _data.Height + ClientSize.Height / 15);
            Max.Font = new Font("Arial", Max.Height / 2, FontStyle.Bold);
            Max.BackColor = Color.White;
            Max.Text = "MAX";
            Max.TextAlign = HorizontalAlignment.Center;

            Min.AutoSize = false;
            Min.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min.ReadOnly = true;
            Min.Location = new Point(_data.Left + ClientSize.Width / 6, _data.Top + _data.Height + ClientSize.Height / 15);
            Min.Font = new Font("Arial", Min.Height / 2, FontStyle.Bold);
            Min.BackColor = Color.White;
            Min.Text = "MIN";
            Min.TextAlign = HorizontalAlignment.Center;

            chart1.Size = new Size(ClientSize.Width - (ClientSize.Width - _data.Location.X), ClientSize.Height - (ClientSize.Height - A.Location.Y) - ClientSize.Height / 12);
            chart1.ChartAreas.Add("1");
            chart1.Series.Add("X");
            chart1.BackColor = Color.White;
            
            Ok.Location = new Point(chart1.Width, chart1.Height);

            Ok.Size = new Size(ClientSize.Width -Ok.Location.X, ClientSize.Height - Ok.Location.Y);
            Ok.Font = new Font("Arial", (int)Math.Sqrt(Ok.Height * Ok.Width) * 3 / 11, FontStyle.Bold);
            Ok.Text = "Ok";
            Ok.Click += new EventHandler(OnOkClick);
            Controls.Add(A);
            Controls.Add(B);
            Controls.Add(N);
            Controls.Add(Max);
            Controls.Add(Min);
            Controls.Add(_data);
            Controls.Add(Ok);
            ClientSizeChanged += new EventHandler(OnSizeChanged);
            pictureBox1.Image = Image.FromFile("C:\\WinForms/lab3/f1.png");
            pictureBox1.Top = 0;
            pictureBox1.Size = new Size(ClientSize.Width - ClientSize.Width*3 / 4, ClientSize.Height / 10);
            pictureBox1.Left = ClientSize.Width - pictureBox1.Width;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;




            x.Width = 23;
            x.Text = "X";
            x.Location = new Point(chart1.Width - x.Width, chart1.Height - x.Height);
            x.BackColor = Color.White;
            x.ForeColor = Color.White;

            y.Width = 23;
            y.Text = "Y";
            y.Location = new Point(y.Width/2, y.Height/2);
            y.BackColor = Color.White;
            y.ForeColor = Color.White;
            Controls.Add(y);
            Controls.Add(x);


            // chart1.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            Controls.Add(chart1);

            Controls.Add(pictureBox1);
            chart1.Invalidate();
            
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            try
            {
                a = Convert.ToDouble(A.Text);
                b = Convert.ToDouble(B.Text);
                n = Convert.ToDouble(N.Text);
            }
            catch
            {
                MessageBox.Show("Невірні дані!");
                return;
            }
            if(a>b)
            {
                MessageBox.Show("a<b !");
                return;
            }
            x.ForeColor = Color.Black;
            y.ForeColor = Color.Black;
            chart1.Series["X"].Points.Clear();
            Calculating(a,b,n);
            chart1.Series["X"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["X"].XValueType = ChartValueType.Double;
            chart1.Series["X"].YValueType = ChartValueType.Double;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            
            for (int i = 0; i < graph_pts.Count; i++)
            {
                chart1.Series["X"].Points.AddXY(graph_pts[i].X, graph_pts[i].Y);
            }
            chart1.Invalidate();
            graph_pts.Clear();
            
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            SizeX = Width;
            SizeY = Height;
            pictureBox1.Top = 0;
            pictureBox1.Size = new Size(ClientSize.Width - ClientSize.Width * 3 / 4, ClientSize.Height / 10);
            pictureBox1.Left = ClientSize.Width - pictureBox1.Width;


            _data.Location = new Point(ClientSize.Width - SizeX / 4, ClientSize.Height / 10);
            _data.Width = ClientSize.Width / 4;
            _data.Height = ClientSize.Height / 2;
            _data.Columns[0].Width = _data.Width / 2 - 10;
            _data.Columns[1].Width = _data.Width / 2 - 10;

            A.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            A.Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12, ClientSize.Height - A.Height - ClientSize.Height / 15);
            A.Font = new Font("Arial", A.Height / 2, FontStyle.Bold);

            B.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            B.Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + B.Width + ClientSize.Width / 25, ClientSize.Height - A.Height - ClientSize.Height / 15);
            B.Font = new Font("Arial", B.Height / 2, FontStyle.Bold);

            N.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            N.Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + 2 * N.Width + 2 * ClientSize.Width / 25, ClientSize.Height - A.Height - ClientSize.Height / 15);
            N.Font = new Font("Arial", N.Height / 2, FontStyle.Bold);
         
            Max.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Max.Location = new Point(_data.Left, _data.Top + _data.Height + ClientSize.Height / 15);
            Max.Font = new Font("Arial", Max.Height / 2, FontStyle.Bold);

            Min.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min.Font = new Font("Arial", Min.Height / 2, FontStyle.Bold);
            Min.Location = new Point(_data.Left + ClientSize.Width / 6, _data.Top + _data.Height + ClientSize.Height / 15);

            


            chart1.Size = new Size(ClientSize.Width - (ClientSize.Width - _data.Location.X), ClientSize.Height-( ClientSize.Height - A.Location.Y) - ClientSize.Height / 12);
            chart1.Update();

            x.Location = new Point(chart1.Width - x.Width, chart1.Height - x.Height);
            y.Location = new Point(y.Width / 2, y.Height / 2);
            Ok.Location = new Point(chart1.Width, chart1.Height);
            Ok.Size = new Size(ClientSize.Width - Ok.Location.X, ClientSize.Height - Ok.Location.Y);
            Ok.Font = new Font("Arial", (int)Math.Sqrt(Ok.Height * Ok.Width) * 3 / 11, FontStyle.Bold);

        }
        void Calculating(double a, double b, double n)
        {
            
            double MaxV = Program.Func(b), MinV = Program.Func(a);
            double temp;
            double step = (b - a )/ n;
            for (double i = a; i <= b; i += step)
            {
                temp = Program.Func(i);
                if (temp > MaxV) MaxV = temp;
                else if (temp < MinV) MinV = temp;
                graph_pts.Add(new Pt(i, temp));
                _data.Rows.Add(i, temp);
            }
            Max.Text = "" + MaxV;
            Min.Text = "" + MinV;
            chart1.ChartAreas["1"].AxisX.Minimum = a;
            chart1.ChartAreas["1"].AxisX.Interval = step;
            chart1.ChartAreas["1"].AxisX.Maximum = b;
            chart1.ChartAreas["1"].AxisY.Minimum = (int)MinV-1;
            chart1.ChartAreas["1"].AxisY.Interval = 1;
            chart1.ChartAreas["1"].AxisY.Maximum = (int)MaxV+1;

        }
            private void OnMenuStart(object obj, EventArgs ea)
        {

        }
        private void OnMenuExit(object obj, EventArgs ea)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    class Pt
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Pt(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}

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
    public partial class Form1 : Form
    {

        List<Pt> graph_pts = new List<Pt>();
        Series series1;
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

        DataGridView _data = new DataGridView();
        public Form1()
        {
            InitializeComponent();
            Text = "lab3";
            MaximizeBox = false;
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
            _data.Height = 0;
            _data.Location = new Point(ClientSize.Width - ClientSize.Width / 4, ClientSize.Height / 8);
            _data.BackgroundColor = Color.Azure;
            _data.Columns.Add("", "");
            _data.ColumnCount = 2;
            //_data.Columns[-1].Width = 0;
            _data.RowHeadersVisible = false;
            _data.Columns[0].Name = "X";
            //_data.Columns[0]. = 0;
            _data.Columns[0].HeaderText = "X";
            _data.Columns[0].Width = _data.Width / 2;
            _data.Columns[0].ReadOnly = true;
            _data.Columns[1].Name = "Y";
            _data.Columns[1].HeaderText = "Y";
            _data.Columns[1].Width = _data.Width / 2;
            _data.Columns[1].ReadOnly = true;
            _data.MaximumSize = new Size(400, 400);
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
            Max.Location = new Point(_data.Left, _data.Top + _data.Height + ClientSize.Height/13);
            Max.Font = new Font("Arial", Max.Height*5 / 11, FontStyle.Bold);
            Max.BackColor = Color.White;
            Max.Text = "MAX";

            Min.AutoSize = false;
            Min.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min.ReadOnly = true;
            Min.Location = new Point(_data.Left + ClientSize.Width / 6, _data.Top + _data.Height + ClientSize.Height / 13);
            Min.Font = new Font("Arial", Min.Height * 5 / 11, FontStyle.Bold);
            Min.BackColor = Color.White;
            Min.Text = "MIN";


            Controls.Add(A);
            Controls.Add(B);
            Controls.Add(N);
            Controls.Add(Max);
            Controls.Add(Min);
            Controls.Add(_data);
            ClientSizeChanged += new EventHandler(OnSizeChanged);
            pictureBox1.Image = Image.FromFile("C:\\WinForms/lab3/f1.png");
            pictureBox1.Top = 0;
            pictureBox1.Size = new Size(ClientSize.Width - ClientSize.Width*3 / 4, ClientSize.Height / 10);
            pictureBox1.Left = ClientSize.Width - pictureBox1.Width;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            Calculating();

            chart1.ChartAreas.Add("1");
            chart1.Series.Add("X");
            chart1.ChartAreas["1"].AxisX.Minimum = 0;
            chart1.ChartAreas["1"].AxisX.Interval = 1;
            chart1.ChartAreas["1"].AxisX.Maximum = 2 * Math.PI;
            chart1.ChartAreas["1"].AxisY.Minimum = -10;
            chart1.ChartAreas["1"].AxisY.Interval = 1;
            chart1.ChartAreas["1"].AxisY.Maximum = 10;
           // chart1.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            chart1.Series["X"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["X"].XValueType = ChartValueType.Double;
            chart1.Series["X"].YValueType = ChartValueType.Double;
            for (int i = 0; i < graph_pts.Count; i++)
            {
                chart1.Series["X"].Points.AddXY(graph_pts[i].X, graph_pts[i].Y);
            }
            Controls.Add(chart1);
            
            Controls.Add(pictureBox1);
            chart1.Invalidate();
            
        }
        private void OnSizeChanged(object sender, EventArgs e)
        {
            SizeX = Width;
            SizeY = Height;
            pictureBox1.Top = 0;
            pictureBox1.Size = new Size(ClientSize.Width - ClientSize.Width * 3 / 4, ClientSize.Height / 10);
            pictureBox1.Left = ClientSize.Width - pictureBox1.Width;

            
            _data.Columns[0].Width = _data.Width / 2;
            _data.Columns[1].Width = _data.Width / 2;
            _data.Location = new Point(ClientSize.Width - SizeX / 4, ClientSize.Height / 10);

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
            Max.Location = new Point(_data.Left, _data.Top + _data.Height + ClientSize.Height / 13);
            Max.Font = new Font("Arial", Max.Height *5 / 11, FontStyle.Bold);

            Min.Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min.Font = new Font("Arial", Min.Height * 5 / 11, FontStyle.Bold);
            Min.Location = new Point(_data.Left + ClientSize.Width / 6, _data.Top + _data.Height + ClientSize.Height / 13);

            

        }
        void Calculating()
        {
            //double a = Convert.ToDouble(A.Text);
            //double b = Convert.ToDouble(B.Text);
            //double n = Convert.ToDouble(N.Text);
            double a = 0;
            double b = 3;
            double n = 6;
            double step = (b - a )/ n;
            for (double i = a; i <= b; i += step)
            {
                graph_pts.Add(new Pt(i, Program.Func(i)));
                _data.Rows.Add(i, Program.Func(i));
               // series1.Points.AddXY(i, Program.Func(i));
            }
            _data.Height = ClientSize.Height / 12 * _data.Rows.Count;
            Max.Location = new Point(_data.Left, _data.Top + _data.Height + ClientSize.Height / 13);
            Min.Location = new Point(_data.Left + ClientSize.Width / 6, _data.Top + _data.Height + ClientSize.Height / 13);
           // chart1.Series.Add(series1);
        }
            private void OnMenuStart(object obj, EventArgs ea)
        {

        }
        private void OnMenuExit(object obj, EventArgs ea)
        {
            Close();
        }
    }
}

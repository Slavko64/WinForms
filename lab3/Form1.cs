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
        double a1;
        double b1;
        double n1;
        List<Pt> [] graph_pts = new List<Pt>[2];
        int SizeX = 800;
        int SizeY = 600;
        //a - start, b - end, n - count of points tabul 
        TextBox[] A = new TextBox[2];
        TextBox[] B = new TextBox[2];
        TextBox[] N = new TextBox[2];
        TextBox[] Min = new TextBox[2];
        TextBox[] Max = new TextBox[2];
        Chart[] chart = new Chart[2];
        PictureBox[] pictureBox = new PictureBox[2];
        Button Ok = new Button();
        Label x = new Label();
        Label y = new Label();
        Label _a = new Label();
        Label _b = new Label();
        Label _n = new Label();
        Label _Max = new Label();
        Label _Min = new Label();
        Button[] _grid = new Button[2];
        bool[] grid_bool = new bool[2];
        Label _scaling = new Label();
        Button[] _scalingplus = new Button[2];
        Button[] _scalingminus = new Button[2];
        DataGridView [] _data = new DataGridView[2];
        RadioButton[] RadioButtons = new RadioButton[4];
        int index;
        int current;
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
            MenuItem SaveChart = new MenuItem("SaveChart", new EventHandler(OnSaveChart));
            MenuItem SaveData = new MenuItem("SaveData", new EventHandler(OnSaveData));
            MenuItem GetData = new MenuItem("GetData", new EventHandler(OnGetData));
            MenuItem WorkWithFiles = new MenuItem("&WorkWithFiles", new MenuItem[] { SaveChart, SaveData, GetData });
            MenuItem Linear = new MenuItem("Linear", new EventHandler(OnLinear));
            MenuItem Parametric = new MenuItem("Parametric", new EventHandler(OnParametric));
            MenuItem Function = new MenuItem("&Function", new MenuItem[] { Linear, Parametric });
            Menu = new MainMenu(new MenuItem[] { miCalc, WorkWithFiles, Function });
            CenterToScreen();
            for (int i = 0; i < 2; i++)
            {
                    A[i] = new TextBox();
                    B[i] = new TextBox();
                    N[i] = new TextBox();
                    Min[i] = new TextBox();
                    Max[i] = new TextBox();
                    chart[i] = new Chart();
                    pictureBox[i] = new PictureBox();
                    _grid[i] = new Button();
                    grid_bool[i] = false;
                    _scalingplus[i] = new Button();
                    _scalingminus[i] = new Button();
                    _data[i] = new DataGridView();
                    graph_pts[i] = new List<Pt>();
            }
            for (int i = 0; i < 4; i++)
            {
                RadioButtons[i] = new RadioButton();
            }
            _data[0].Width = ClientSize.Width / 4;
            _data[0].Height = ClientSize.Height /2;
            _data[0].Location = new Point(ClientSize.Width - SizeX / 4, ClientSize.Height / 10);
            _data[0].BackgroundColor = Color.Azure;
            _data[0].Columns.Add("", "");
            _data[0].ColumnCount = 2;
            //_data.Columns[-1].Width = 0;
            _data[0].RowHeadersVisible = false;
            _data[0].Columns[0].Name = "X";
            //_data.Columns[0]. = 0;
            _data[0].Columns[0].HeaderText = "X";
            _data[0].Columns[0].Width = _data[0].Width / 2;
            _data[0].Columns[1].Width = _data[0].Width / 2;
            _data[0].Columns[0].ReadOnly = true;
            _data[0].Columns[1].Name = "Y";
            _data[0].Columns[1].HeaderText = "Y";
            _data[0].Columns[1].ReadOnly = true;

            _data[1].Width = ClientSize.Width / 4;
            _data[1].Height = ClientSize.Height / 2 - ClientSize.Height / 20;
            _data[1].Location = new Point(ClientSize.Width - SizeX / 4, ClientSize.Height / 10 + ClientSize.Height / 20);
            _data[1].BackgroundColor = Color.Azure;
            _data[1].Columns.Add("", "");
            _data[1].ColumnCount = 2;
            _data[1].RowHeadersVisible = false;
            _data[1].Columns[0].Name = "X";
            _data[1].Columns[0].HeaderText = "X";
            _data[1].Columns[0].ReadOnly = true;
            _data[1].Columns[1].Name = "Y";
            _data[1].Columns[1].HeaderText = "Y";
            _data[1].Columns[0].Width = _data[1].Width / 2;
            _data[1].Columns[1].Width = _data[1].Width / 2;
            _data[1].Columns[1].ReadOnly = true;


            //A1  = new TextBox();
            A[0].Location = new Point(ClientSize.Width - ClientSize.Width*4/5,ClientSize.Height);
            A[0].AutoSize = false;
            A[1].Location = new Point(ClientSize.Width - ClientSize.Width * 4 / 5, ClientSize.Height);
            A[1].AutoSize = false;

            B[0].AutoSize = false;
            N[0].AutoSize = false;
            A[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            A[0].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12, ClientSize.Height - A[0].Height- ClientSize.Height/15);
            A[0].Font = new Font("Arial", A[0].Height/2, FontStyle.Bold);
            B[1].AutoSize = false;
            N[1].AutoSize = false;
            A[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            A[1].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12, ClientSize.Height - A[1].Height - ClientSize.Height / 15);
            A[1].Font = new Font("Arial", A[1].Height / 2, FontStyle.Bold);

            _a.Width = 10;
            _a.Location = new Point(A[0].Left + A[0].Width / 2 - _a.Width/2, A[0].Top - A[0].Height / 2);
            _a.Text = "A";
            
            B[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            B[0].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + B[0].Width + ClientSize.Width / 25, ClientSize.Height - B[0].Height - ClientSize.Height / 15);
            B[0].Font = new Font("Arial", B[0].Height / 2, FontStyle.Bold);
            B[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            B[1].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + B[1].Width + ClientSize.Width / 25, ClientSize.Height - B[1].Height - ClientSize.Height / 15);
            B[1].Font = new Font("Arial", B[1].Height / 2, FontStyle.Bold);

            _b.Width = 10;
            _b.Location = new Point(B[0].Left + B[0].Width / 2 - _b.Width / 2, B[0].Top - B[0].Height / 2);
            _b.Text = "B";

            N[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            N[0].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + 2 * N[0].Width + 2*ClientSize.Width / 25, ClientSize.Height - N[0].Height - ClientSize.Height / 15);
            N[0].Font = new Font("Arial", N[0].Height / 2, FontStyle.Bold);
            N[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            N[1].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + 2 * N[1].Width + 2 * ClientSize.Width / 25, ClientSize.Height - N[1].Height - ClientSize.Height / 15);
            N[1].Font = new Font("Arial", N[1].Height / 2, FontStyle.Bold);

            _n.Width = 10;
            _n.Location = new Point(N[0].Left + N[0].Width / 2 - _n.Width / 2, N[0].Top - N[0].Height / 2);
            _n.Text = "N";

            Max[0].AutoSize = false;
            Max[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Max[0].ReadOnly = true;
            Max[0].Location = new Point(_data[0].Left, _data[0].Top + _data[0].Height + ClientSize.Height / 15);
            Max[0].Font = new Font("Arial", Max[0].Height / 2, FontStyle.Bold);
            Max[0].BackColor = Color.White;
            Max[0].TextAlign = HorizontalAlignment.Center;

            Max[1].AutoSize = false;
            Max[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Max[1].ReadOnly = true;
            Max[1].Location = new Point(_data[1].Left, _data[1].Top + _data[1].Height + ClientSize.Height / 15);
            Max[1].Font = new Font("Arial", Max[1].Height / 2, FontStyle.Bold);
            Max[1].BackColor = Color.White;
            Max[1].TextAlign = HorizontalAlignment.Center;

            _Max.Width = 30;
            _Max.Location = new Point(Max[0].Left + Max[0].Width / 2 - _Max.Width / 2, Max[0].Top - Max[0].Height / 2);
            _Max.Text = "Max";

            Min[0].AutoSize = false;
            Min[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min[0].ReadOnly = true;
            Min[0].Location = new Point(_data[0].Left + ClientSize.Width / 6, _data[0].Top + _data[0].Height + ClientSize.Height / 15);
            Min[0].Font = new Font("Arial", Min[0].Height / 2, FontStyle.Bold);
            Min[0].BackColor = Color.White;
            Min[0].TextAlign = HorizontalAlignment.Center;
            Min[1].AutoSize = false;
            Min[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min[1].ReadOnly = true;
            Min[1].Location = new Point(_data[1].Left + ClientSize.Width / 6, _data[1].Top + _data[1].Height + ClientSize.Height / 15);
            Min[1].Font = new Font("Arial", Min[1].Height / 2, FontStyle.Bold);
            Min[1].BackColor = Color.White;
            Min[1].TextAlign = HorizontalAlignment.Center;

            _Min.Width = 30;
            _Min.Location = new Point(Min[0].Left + Min[0].Width / 2 - _Min.Width / 2, Min[0].Top - Min[0].Height / 2);
            _Min.Text = "Min";


            chart[0].Size = new Size(ClientSize.Width - (ClientSize.Width - _data[0].Location.X), ClientSize.Height - (ClientSize.Height - A[0].Location.Y) - ClientSize.Height / 12);
            chart[0].ChartAreas.Add("1");
            chart[0].Series.Add("X");
            chart[0].BackColor = Color.White;
            chart[0].ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart[0].ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart[1].Size = new Size(ClientSize.Width - (ClientSize.Width - _data[1].Location.X), ClientSize.Height - (ClientSize.Height - A[1].Location.Y) - ClientSize.Height / 12);
            chart[1].ChartAreas.Add("1");
            chart[1].Series.Add("X");
            chart[1].BackColor = Color.White;
            chart[1].ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart[1].ChartAreas[0].AxisY.MajorGrid.Enabled = false;


            Ok.Location = new Point(chart[0].Width, chart[0].Height);

            Ok.Size = new Size(ClientSize.Width -Ok.Location.X, ClientSize.Height - Ok.Location.Y);
            Ok.Font = new Font("Arial", (int)Math.Sqrt(Ok.Height * Ok.Width) * 3 / 11, FontStyle.Bold);
            Ok.Text = "Ok";
            Ok.Click += new EventHandler(OnOkClick);

            _grid[0].Location = new Point(chart[0].Width - ClientSize.Width / 5, chart[0].Height + ClientSize.Height / 40);
            _grid[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 20);
            _grid[0].Text = "Grid: OFF";
            _grid[0].Click += new EventHandler(OnGridClick);
            _grid[1].Location = new Point(chart[1].Width - ClientSize.Width / 5, chart[1].Height + ClientSize.Height / 41);
            _grid[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 20);
            _grid[1].Text = "Grid: OFF";
            _grid[1].Click += new EventHandler(OnGridClick);


            _scaling.Width = 70;
            _scaling.Height = ClientSize.Height * 23 / 600;
            _scaling.Location = new Point(_grid[0].Location.X - _scaling.Width / 2, _grid[0].Location.Y + _grid[0].Height + ClientSize.Height / 40);
            _scaling.Text = "scaling 10%";
            _scalingplus[0].Location = new Point(_scaling.Right + ClientSize.Width / 80, _scaling.Location.Y- ClientSize.Width / 150);
            _scalingplus[0].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingplus[0].Text = "+";
            _scalingplus[0].Click += new EventHandler(OnPlusClick);
            _scalingminus[0].Location = new Point(_scalingplus[0].Right + ClientSize.Width / 80, _scaling.Location.Y - ClientSize.Width / 150);
            _scalingminus[0].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingminus[0].Text = "-";
            _scalingminus[0].Click += new EventHandler(OnMinusClick);
            _scalingplus[1].Location = new Point(_scaling.Right + ClientSize.Width / 80, _scaling.Location.Y - ClientSize.Width / 150);
            _scalingplus[1].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingplus[1].Text = "+";
            _scalingplus[1].Click += new EventHandler(OnPlusClick);
            _scalingminus[1].Location = new Point(_scalingplus[1].Right + ClientSize.Width / 80, _scaling.Location.Y - ClientSize.Width / 150);
            _scalingminus[1].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingminus[1].Text = "-";
            _scalingminus[1].Click += new EventHandler(OnMinusClick);



            ClientSizeChanged += new EventHandler(OnSizeChanged);
            pictureBox[0].Image = Image.FromFile("C:\\WinForms/lab3/f1.png");
            pictureBox[0].Top = 0;
            pictureBox[0].Size = new Size(ClientSize.Width - ClientSize.Width*3 / 4, ClientSize.Height / 10);
            pictureBox[0].Left = ClientSize.Width - pictureBox[0].Width;
            pictureBox[0].SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox[1].Image = Image.FromFile("C:\\WinForms/lab3/f2.png");
            pictureBox[1].Top = 3;
            pictureBox[1].Size = new Size(ClientSize.Width - ClientSize.Width * 3 / 4, ClientSize.Height / 13);
            pictureBox[1].Left = ClientSize.Width - pictureBox[1].Width;
            pictureBox[1].SizeMode = PictureBoxSizeMode.StretchImage;


            current = 0;

            x.Width = 23;
            x.Text = "X";
            x.Location = new Point(chart[0].Width - x.Width, chart[0].Height - x.Height);
            x.BackColor = Color.White;
            x.ForeColor = Color.White;

            y.Width = 23;
            y.Text = "Y";
            y.Location = new Point(y.Width/2, y.Height/2);
            y.BackColor = Color.White;
            y.ForeColor = Color.White;


            Controls.Add(A[0]);
            Controls.Add(_a);
            Controls.Add(B[0]);
            Controls.Add(_b);
            Controls.Add(N[0]);
            Controls.Add(_n);
            Controls.Add(Max[0]);
            Controls.Add(_Max);
            Controls.Add(Min[0]);
            Controls.Add(_Min);
            Controls.Add(_data[0]);
            Controls.Add(y);
            Controls.Add(x);
            Controls.Add(Ok);

            // chart1.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            Controls.Add(chart[0]);
            Controls.Add(_grid[0]);
            Controls.Add(pictureBox[0]);
            chart[0].Invalidate();
            
        }

        private void OnLinear(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnParametric(object sender, EventArgs e)
        {
            Controls.Clear();
            Controls.Add(A[1]);
            Controls.Add(_a);
            Controls.Add(B[1]);
            Controls.Add(_b);
            Controls.Add(N[1]);
            Controls.Add(_n);
            Controls.Add(Max[1]);
            Controls.Add(_Max);
            Controls.Add(Min[1]);
            Controls.Add(_Min);
            Controls.Add(_data[1]);
            Controls.Add(y);
            Controls.Add(x);
            Controls.Add(Ok);

            // chart1.Legends[1].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            Controls.Add(chart[1]);
            Controls.Add(_grid[1]);
            Controls.Add(pictureBox[1]);
            current = 1;


            RadioButtons[0].Location = new Point(pictureBox[1].Left, pictureBox[1].Bottom + 10);
            RadioButtons[1].Location = new Point(pictureBox[1].Left + 100, pictureBox[1].Bottom + 10);
            //RadioButton[0]
            RadioButtons[0].Text = "x(t)";
            RadioButtons[1].Text = "y(t)";
            RadioButtons[2].Text = "y(x)";
            RadioButtons[3].Text = "x(y)";
            for (int i = 0; i < 4; i++)
            {
                RadioButtons[i].Width = ClientSize.Width / 18;
                RadioButtons[i].Location = new Point(pictureBox[1].Left + i * RadioButtons[i].Width + ClientSize.Width / 30, pictureBox[1].Bottom + ClientSize.Height/60);
                Controls.Add(RadioButtons[i]);
            }
            Controls.Add(RadioButtons[0]);
            Controls.Add(RadioButtons[1]);
        }

        private void OnSaveChart(object sender, EventArgs e)
        {
            chart[0].SaveImage("C:\\WinForms/lab3/chart.png", ChartImageFormat.Png);
        }
        private void OnSaveData(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void OnGetData(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMinusClick(object sender, EventArgs e)
        {
            chart[current].ChartAreas[current].InnerPlotPosition.Width -= (int)chart[current].ChartAreas[current].InnerPlotPosition.Width * ((float)0.1);
            chart[current].Invalidate();
        }

        private void OnPlusClick(object sender, EventArgs e)
        {
            if (chart[current].ChartAreas[current].InnerPlotPosition.Width * 1.1 > 100) return;
            chart[current].ChartAreas[current].InnerPlotPosition.Width *= (float)1.1;
            chart[current].Invalidate();
        }

        private void OnGridClick(object sender, EventArgs e)
        {
            if(grid_bool[current] == false)
            {
                grid_bool[current] = true;
                _grid[current].Text = "Grid: ON";
                chart[current].ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                chart[current].ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            }
            else
            {
                grid_bool[current] = false;
                _grid[current].Text = "Grid: OFF";
                chart[current].ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart[current].ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            }
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            Controls.Add(_scaling);
            Controls.Add(_scalingplus[current]);
            Controls.Add(_scalingminus[current]);
            try
            {
                a1 = Convert.ToDouble(A[current].Text.Replace('.', ','));
                b1 = Convert.ToDouble(B[current].Text.Replace('.', ','));
                n1 = Convert.ToDouble(N[current].Text.Replace('.', ','));
            }
            catch
            {
                MessageBox.Show("Невірні дані!");
                return;
            }
            if(a1>b1)
            {
                MessageBox.Show("a<b !");
                return;
            }
            if (n1 < 1)
            {
                MessageBox.Show("n<1 !");
                return;
            }
            x.ForeColor = Color.Black;
            y.ForeColor = Color.Black;
            chart[current].Series["X"].Points.Clear();
            Int32 selectedCellCount = _data[current].GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                for (int i = 0; i < selectedCellCount; i++) {
                    index = int.Parse(_data[current].SelectedCells[i].RowIndex.ToString());
                    graph_pts[current].Add(new Pt(double.Parse(_data[current].Rows[index].Cells[0].Value.ToString()), double.Parse(_data[current].Rows[index].Cells[1].Value.ToString())));
                }
            }
            else
            {
                _data[current].Rows.Clear();
                Calculating(a1, b1, n1);
            }
            chart[current].Series["X"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart[current].Series["X"].XValueType = ChartValueType.Double;
            chart[current].Series["X"].YValueType = ChartValueType.Double;
            _data[current].ClearSelection();
            if ((_data[current].Rows.Count +1) * _data[current].Rows[0].Height > _data[current].Height)
            {
                _data[current].Columns[0].Width = _data[current].Width / 2 - 10;
                _data[current].Columns[1].Width = _data[current].Width / 2 - 10;
            }
            else
            {
                _data[current].Columns[0].Width = _data[current].Width / 2-2;
                _data[current].Columns[1].Width = _data[current].Width / 2-2;
            }
            for (int i = 0; i < graph_pts[current].Count; i++)
            {
                chart[current].Series["X"].Points.AddXY(graph_pts[current][i].X, graph_pts[current][i].Y);
            }
            chart[current].Invalidate();
            graph_pts[current].Clear();
            
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            SizeX = Width;
            SizeY = Height;
            pictureBox[current].Top = 0;
            pictureBox[current].Size = new Size(ClientSize.Width - ClientSize.Width * 3 / 4, ClientSize.Height / 10);
            pictureBox[current].Left = ClientSize.Width - pictureBox[current].Width;


            _data[0].Location = new Point(ClientSize.Width - SizeX / 4, ClientSize.Height / 10);
            _data[current].Width = ClientSize.Width / 4;
            _data[0].Height = ClientSize.Height / 2;
            _data[1].Height = ClientSize.Height / 2 - ClientSize.Height / 20;
            _data[1].Location = new Point(ClientSize.Width - SizeX / 4, ClientSize.Height / 10 + ClientSize.Height / 20);
            _data[current].Columns[0].Width = _data[current].Width / 2;
            _data[current].Columns[1].Width = _data[current].Width / 2;
            if ((_data[current].Rows.Count + 1) * _data[current].Rows[0].Height > _data[current].Height)
            {
                _data[current].Columns[0].Width = _data[current].Width / 2 - 10;
                _data[current].Columns[1].Width = _data[current].Width / 2 - 10;
            }
            else
            {
                _data[current].Columns[0].Width = _data[current].Width / 2 - 2;
                _data[current].Columns[1].Width = _data[current].Width / 2 - 2;
            }


            A[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            A[current].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12, ClientSize.Height - A[current].Height - ClientSize.Height / 15);
            A[current].Font = new Font("Arial", A[current].Height / 2+1, FontStyle.Bold);
            _a.Location = new Point(A[current].Left + A[current].Width / 2 - _a.Width / 2, A[current].Top - A[current].Height / 2);

            B[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            B[current].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + B[current].Width + ClientSize.Width / 25, ClientSize.Height - B[current].Height - ClientSize.Height / 15);
            B[current].Font = new Font("Arial", B[current].Height / 2+1, FontStyle.Bold);
            _b.Location = new Point(B[current].Left + B[current].Width / 2 - _b.Width / 2, B[current].Top - B[current].Height / 2);

            N[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            N[current].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + 2 * N[current].Width + 2 * ClientSize.Width / 25, ClientSize.Height - N[current].Height - ClientSize.Height / 15);
            N[current].Font = new Font("Arial", N[current].Height / 2+1, FontStyle.Bold);
            _n.Location = new Point(N[current].Left + N[current].Width / 2 - _n.Width / 2, N[current].Top - N[current].Height / 2);

            Max[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Max[current].Location = new Point(_data[current].Left, _data[current].Top + _data[current].Height + ClientSize.Height / 15);
            Max[current].Font = new Font("Arial", Max[current].Height / 2+1, FontStyle.Bold);
            _Max.Location = new Point(Max[current].Left + Max[current].Width / 2 - _Max.Width / 2, Max[current].Top - Max[current].Height / 2);

            Min[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min[current].Font = new Font("Arial", Min[current].Height / 2+1, FontStyle.Bold);
            Min[current].Location = new Point(_data[current].Left + ClientSize.Width / 6, _data[current].Top + _data[current].Height + ClientSize.Height / 15);
            _Min.Location = new Point(Min[current].Left + Min[current].Width / 2 - _Min.Width / 2, Min[current].Top - Min[current].Height / 2);


            if (ClientSize.Width == 0)
            {
                chart[current].Size = new Size(ClientSize.Width - (ClientSize.Width - _data[current].Location.X) + 40, ClientSize.Height - (ClientSize.Height - A[current].Location.Y) - ClientSize.Height / 12 + 1);
            }
            else
            {
                chart[current].Size = new Size(ClientSize.Width - (ClientSize.Width - _data[current].Location.X), ClientSize.Height - (ClientSize.Height - A[current].Location.Y) - ClientSize.Height / 12 + 1);
            }
            chart[current].Update();

            x.Location = new Point(chart[current].Width - x.Width, chart[current].Height - x.Height);
            y.Location = new Point(y.Width / 2, y.Height / 2);
            Ok.Location = new Point(chart[current].Width, chart[current].Height);
            Ok.Size = new Size(ClientSize.Width - Ok.Location.X, ClientSize.Height - Ok.Location.Y);
            Ok.Font = new Font("Arial", (int)Math.Sqrt(Ok.Height * Ok.Width) * 3 / 11+1, FontStyle.Bold);

            _grid[current].Location = new Point(chart[current].Width - ClientSize.Width / 5, chart[current].Height + ClientSize.Height / 40);
            _grid[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 20);

            _scaling.Location = new Point(_grid[current].Location.X - _scaling.Width / 2, _grid[current].Location.Y + _grid[current].Height + ClientSize.Height / 40);
            _scaling.Height = ClientSize.Height * 23 / 600;
            _scalingplus[current].Location = new Point(_scaling.Right + ClientSize.Width / 80, _scaling.Location.Y - ClientSize.Width / 150);
            _scalingplus[current].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingplus[current].TextAlign = ContentAlignment.MiddleCenter;
            _scalingminus[current].Location = new Point(_scalingplus[current].Right + ClientSize.Width / 80, _scaling.Location.Y - ClientSize.Width / 150);
            _scalingminus[current].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingminus[current].TextAlign = ContentAlignment.MiddleCenter;
        }
        void Calculating(double a, double b, double n)
        {
            
            double MaxV = Program.Func(b), MinV = Program.Func(a);
            double temp;
            double step = (b - a )/ (n-1);
            for (double i = a; i <= b; i += step)
            {
                temp = Program.Func(i);
                if (temp > MaxV) MaxV = temp;
                else if (temp < MinV) MinV = temp;
                graph_pts[current].Add(new Pt(i, temp));
                _data[current].Rows.Add(i, temp);
            }
            Max[current].Text = "" + MaxV;
            Min[current].Text = "" + MinV;
            chart[current].ChartAreas["1"].AxisX.Minimum = a;
            chart[current].ChartAreas["1"].AxisX.Interval = step;
            chart[current].ChartAreas["1"].AxisX.Maximum = b;
            chart[current].ChartAreas["1"].AxisY.Minimum = (int)MinV-1;
            chart[current].ChartAreas["1"].AxisY.Interval = 1;
            chart[current].ChartAreas["1"].AxisY.Maximum = (int)MaxV+1;

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

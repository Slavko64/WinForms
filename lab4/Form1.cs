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
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing.Imaging;

namespace lab4
{

    public partial class Form1 : Form
    {//Ініціалізація всіх змінних
        Font drawFont = new Font("Arial", 8);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        PointF[] curvePoints;
        List<int> digitsY = new List<int>();
        // For Drawing
        Pen pen = new Pen(Color.Black,1);
        Panel panel;
        Graphics graphics;
        BufferedGraphicsContext bufferedGraphicsContext;
        BufferedGraphics bufferedGraphics;
        List<PointF[]> linesX = new List<PointF[]>();
        List<PointF[]> linesY = new List<PointF[]>();
        List<Point[]> lines = new List<Point[]>();
        //
        bool bool_ok = false;
        double a1;
        double b1;
        int n1;
        double alpha;
        List<Pt>[] graph_pts = new List<Pt>[2];
        int SizeX = 800;
        int SizeY = 600;
        //a - start, b - end, n - count of points tabul 
        TextBox[] A = new TextBox[2];
        TextBox[] B = new TextBox[2];
        TextBox[] N = new TextBox[2];
        TextBox[] Min = new TextBox[2];
        TextBox[] Max = new TextBox[2];
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
        DataGridView[] _data = new DataGridView[2];
        RadioButton[] RadioButtons = new RadioButton[4];
        Label parameter = new Label();
        TextBox _parameter = new TextBox();

        //int index;
        int current;
        private void InitializeGraphics() // ініціалізація графіки для буферу
        {
            graphics = panel.CreateGraphics();
            bufferedGraphicsContext = new BufferedGraphicsContext();
            bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, new Rectangle(0, 0, panel.Width, panel.Height));
        }
        private void DrawToBuffer() // функція яка рисує
        {
            bufferedGraphics.Graphics.Clear(Color.White);
            foreach(var line in lines)
                bufferedGraphics.Graphics.DrawLine(pen, line[0].X, line[0].Y, line[1].X, line[1].Y);
            for (int i = 0; i < linesY.Count; i++)
            {
                bufferedGraphics.Graphics.DrawLine(pen, linesY[i][0].X, linesY[i][0].Y, linesY[i][1].X, linesY[i][1].Y);
                bufferedGraphics.Graphics.DrawString("" + digitsY[i], drawFont, drawBrush, linesY[i][0].X - panel.Width / 30, linesY[i][1].Y-5);
            }
            for (int i = 0; i < linesX.Count; i++)
            {
                bufferedGraphics.Graphics.DrawLine(pen, linesX[i][0].X, linesX[i][0].Y, linesX[i][1].X, linesX[i][1].Y);
            }
            if(curvePoints.Length > 2)
            bufferedGraphics.Graphics.DrawLines(new Pen(Color.Red, 1), curvePoints);
            bufferedGraphics.Render();
        }
        public Form1()
        {
            #region 1
            InitializeComponent();
            Text = "lab4";
            MaximizeBox = true;
            ClientSize = new Size(SizeX, SizeY);
            MenuItem miExit = new MenuItem("Exit",
                new EventHandler(OnMenuExit), Shortcut.CtrlX);
            MenuItem miCalc = new MenuItem("&Menu",
                new MenuItem[] {  miExit });
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
            _data[0].Height = ClientSize.Height / 2;
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


            Controls.Add(panel);
            //A1  = new TextBox();
            A[0].Location = new Point(ClientSize.Width - ClientSize.Width * 4 / 5, ClientSize.Height);
            A[0].AutoSize = false;
            A[1].Location = new Point(ClientSize.Width - ClientSize.Width * 4 / 5, ClientSize.Height);
            A[1].AutoSize = false;

            B[0].AutoSize = false;
            N[0].AutoSize = false;
            A[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            A[0].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12, ClientSize.Height - A[0].Height - ClientSize.Height / 15);
            A[0].Font = new Font("Arial", A[0].Height / 2, FontStyle.Bold);
            B[1].AutoSize = false;
            N[1].AutoSize = false;
            A[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            A[1].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12, ClientSize.Height - A[1].Height - ClientSize.Height / 15);
            A[1].Font = new Font("Arial", A[1].Height / 2, FontStyle.Bold);

            _a.Width = 10;
            _a.Location = new Point(A[0].Left + A[0].Width / 2 - _a.Width / 2, A[0].Top - A[0].Height / 2);
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
            N[0].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + 2 * N[0].Width + 2 * ClientSize.Width / 25, ClientSize.Height - N[0].Height - ClientSize.Height / 15);
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
            panel = new Panel()
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width - (ClientSize.Width - _data[0].Location.X), ClientSize.Height - (ClientSize.Height - A[0].Location.Y) - ClientSize.Height / 12),
                BackColor = Color.White
            };
            x.Width = 23;
            x.Text = "X";
            x.Location = new Point(panel.Width - x.Width, panel.Height - x.Height);
            x.BackColor = Color.White;

            y.Width = 23;
            y.Text = "Y";
            y.Location = new Point(y.Width / 2, y.Height / 2);
            y.BackColor = Color.White;
            Controls.Add(y);
            Controls.Add(x);
            Controls.Add(panel);

            Ok.Location = new Point(panel.Width, panel.Height);

            Ok.Size = new Size(ClientSize.Width - Ok.Location.X, ClientSize.Height - Ok.Location.Y);
            Ok.Font = new Font("Arial", (int)Math.Sqrt(Ok.Height * Ok.Width) * 3 / 11, FontStyle.Bold);
            Ok.Text = "Ok";
            Ok.Click += new EventHandler(OnOkClick);

            _grid[0].Location = new Point(panel.Width - ClientSize.Width / 5, panel.Height + ClientSize.Height / 40);
            _grid[0].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 20);
            _grid[0].Text = "Grid: OFF";
            _grid[0].Click += new EventHandler(OnGridClick);
            _grid[1].Location = new Point(panel.Width - ClientSize.Width / 5, panel.Height + ClientSize.Height / 41);
            _grid[1].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 20);
            _grid[1].Text = "Grid: OFF";
            _grid[1].Click += new EventHandler(OnGridClick);




            ClientSizeChanged += new EventHandler(OnSizeChanged);


            current = 0;



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
            Controls.Add(Ok);

            Controls.Add(_grid[0]);
            Controls.Add(pictureBox[0]);




            InitializeGraphics();
            #endregion
            
         } // конструктор, у якому оголошуються усі змінні та додаються на форму

        private void OnLinear(object sender, EventArgs e)
        {
            bool_ok = false;
            Controls.Clear();
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
            Controls.Add(panel);
            Controls.Add(Ok);
            Controls.Add(_grid[0]);
            Controls.Add(pictureBox[0]);
            current = 0;
        }  // функція, яка стирає усі Controls і до додає потрібні для лінійного графіка елементи Controls
        private void OnParametric(object sender, EventArgs e) // функція, яка стирає усі Controls і до додає потрібні для параметричного графіка елементи Controls
        {
            bool_ok = false;
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
            Controls.Add(panel);
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
                RadioButtons[i].Location = new Point(pictureBox[1].Left + i * RadioButtons[i].Width + ClientSize.Width / 30, pictureBox[1].Bottom + ClientSize.Height / 60);
                RadioButtons[i].CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                RadioButtons[i].Name = "" + i;
                Controls.Add(RadioButtons[i]);
            }
            RadioButtons[0].Checked = true;
            parameter.Text = "a";
            parameter.Width = 10;
            parameter.Location = new Point(panel.Width - ClientSize.Width / 5, _scaling.Bottom + ClientSize.Height / 50);
            _parameter.Location = new Point(parameter.Right + ClientSize.Width / 50, _scaling.Bottom + ClientSize.Height / 50 - parameter.Height / 6);
            _parameter.Width = 40;
            Controls.Add(parameter);
            Controls.Add(_parameter);

        } 

        private void radioButton_CheckedChanged(object sender, EventArgs e) // ловить зміну радіобатона і змінює віповідні надписи координат
        {
            _data[1].Rows.Clear();
            //chart[1].Series["X"].Points.Clear();
            switch ((sender as RadioButton).Name)
            {
                case "0":
                    _data[1].Columns[0].HeaderText = "t";
                    _data[1].Columns[1].HeaderText = "X";
                    x.Text = "t";
                    y.Text = "X";
                    break;
                case "1":
                    _data[1].Columns[0].HeaderText = "t";
                    _data[1].Columns[1].HeaderText = "Y";
                    x.Text = "t";
                    y.Text = "Y";
                    break;
                case "2":
                    _data[1].Columns[0].HeaderText = "X";
                    _data[1].Columns[1].HeaderText = "Y";
                    x.Text = "X";
                    y.Text = "Y";
                    break;
                case "3":
                    _data[1].Columns[0].HeaderText = "Y";
                    _data[1].Columns[1].HeaderText = "X";
                    x.Text = "Y";
                    y.Text = "X";
                    break;
            }
        }

        private void OnSaveChart(object sender, EventArgs e) // збереження графіку у файл
        {
            Image bmp = new Bitmap(panel.Width, panel.Height);
            var gg = Graphics.FromImage(bmp);
            var rect = RectangleToScreen(new Rectangle(0, 0, panel.Width, panel.Height));
            gg.CopyFromScreen(rect.Location, Point.Empty, panel.Size);
            bmp.Save("C:\\WinForms/lab4/chart.png", ImageFormat.Png);

        }

        private void OnSaveData(object sender, EventArgs e)
        {
            // creating Excel Application  
            Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = workbook.Sheets["Лист1"];
            worksheet = workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";
            // storing header part in Excel  
            for (int i = 1; i < _data[current].Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = _data[current].Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < _data[current].Rows.Count - 1; i++)
            {
                for (int j = 0; j < _data[current].Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = _data[current].Rows[i].Cells[j].Value;
                }
            }
            // save the application  
            workbook.SaveAs("C:\\WinForms/lab4/SaveData.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application  
            app.Quit();
        } // збереження данних у файл ексель
        private void OnGetData(object sender, EventArgs e) //зчитування данних з файлу ексель
        {
            Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open("C:\\WinForms/lab4/GetData.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
            A[current].Text = ObjWorkSheet.Cells[1, 2].Value.ToString();
            B[current].Text = ObjWorkSheet.Cells[2, 2].Value.ToString();
            N[current].Text = ObjWorkSheet.Cells[3, 2].Value.ToString();
            if (current == 1)
                _parameter.Text = ObjWorkSheet.Cells[4, 2].Value.ToString();
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из экселя
            GC.Collect(); // убрать за собой

        }
        private void OnGridClick(object sender, EventArgs e)
        {
            if (grid_bool[current] == false)
            {
                grid_bool[current] = true;
                _grid[current].Text = "Grid: ON";
            }
            else
            {
                  grid_bool[current] = false;
                  _grid[current].Text = "Grid: OFF";
            }
            if (bool_ok == true)
                DrawGraph();
        } // для виводу/забирання сітки

        private void OnOkClick(object sender, EventArgs e) // Ок - запуск обчислення і побудова графіка
        {
            #region
            if (current == 0)
            {
                try
                {
                    a1 = Convert.ToDouble(A[current].Text.Replace('.', ','));
                    b1 = Convert.ToDouble(B[current].Text.Replace('.', ','));
                    n1 = Convert.ToInt32(N[current].Text.Replace('.', ','));
                }
                catch
                {
                    MessageBox.Show("Невірні dані!");
                    return;
                }
                if (a1 > b1)
                {
                    MessageBox.Show("a<b !");
                    return;
                }
                if (n1 < 1)
                {
                    MessageBox.Show("n<1 !");
                    return;
                }
                bool_ok = true;
                x.ForeColor = Color.Black;
                y.ForeColor = Color.Black;
                _data[current].Rows.Clear();
                graph_pts[current].Clear();
                Calculating(a1, b1, n1);
                _data[current].ClearSelection();
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

            }
            else
            {
                try
                {
                    a1 = Convert.ToDouble(A[current].Text.Replace('.', ','));
                    b1 = Convert.ToDouble(B[current].Text.Replace('.', ','));
                    n1 = Convert.ToInt32(N[current].Text.Replace('.', ','));
                    alpha = Convert.ToDouble(_parameter.Text.Replace('.', ','));
                }
                catch
                {
                    MessageBox.Show("Невірні dані!");
                    return;
                }
                if (a1 > b1)
                {
                    MessageBox.Show("a<b !");
                    return;
                }
                if (n1 < 1)
                {
                    MessageBox.Show("n<1 !");
                    return;
                }
                if (alpha < 0)
                {
                    MessageBox.Show("alpha < 0 !");
                    return;
                }
                bool_ok = true;
                x.ForeColor = Color.Black;
                y.ForeColor = Color.Black;
                _data[current].Rows.Clear();
                graph_pts[current].Clear();
                Calculating(a1, b1, n1, alpha);
                _data[current].ClearSelection();
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
            }
            #endregion
        }

        private void OnSizeChanged(object sender, EventArgs e) // для адаптипної сторінки
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
            A[current].Font = new Font("Arial", A[current].Height / 2 + 1, FontStyle.Bold);
            _a.Location = new Point(A[current].Left + A[current].Width / 2 - _a.Width / 2, A[current].Top - A[current].Height / 2);

            B[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            B[current].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + B[current].Width + ClientSize.Width / 25, ClientSize.Height - B[current].Height - ClientSize.Height / 15);
            B[current].Font = new Font("Arial", B[current].Height / 2 + 1, FontStyle.Bold);
            _b.Location = new Point(B[current].Left + B[current].Width / 2 - _b.Width / 2, B[current].Top - B[current].Height / 2);

            N[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            N[current].Location = new Point(ClientSize.Width - ClientSize.Width * 11 / 12 + 2 * N[current].Width + 2 * ClientSize.Width / 25, ClientSize.Height - N[current].Height - ClientSize.Height / 15);
            N[current].Font = new Font("Arial", N[current].Height / 2 + 1, FontStyle.Bold);
            _n.Location = new Point(N[current].Left + N[current].Width / 2 - _n.Width / 2, N[current].Top - N[current].Height / 2);

            Max[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Max[current].Location = new Point(_data[current].Left, _data[current].Top + _data[current].Height + ClientSize.Height / 15);
            Max[current].Font = new Font("Arial", Max[current].Height / 2 + 1, FontStyle.Bold);
            _Max.Location = new Point(Max[current].Left + Max[current].Width / 2 - _Max.Width / 2, Max[current].Top - Max[current].Height / 2);

            Min[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 12);
            Min[current].Font = new Font("Arial", Min[current].Height / 2 + 1, FontStyle.Bold);
            Min[current].Location = new Point(_data[current].Left + ClientSize.Width / 6, _data[current].Top + _data[current].Height + ClientSize.Height / 15);
            _Min.Location = new Point(Min[current].Left + Min[current].Width / 2 - _Min.Width / 2, Min[current].Top - Min[current].Height / 2);



            



            _scaling.Location = new Point(_grid[current].Location.X - _scaling.Width / 2, _grid[current].Location.Y + _grid[current].Height + ClientSize.Height / 40);
            _scaling.Height = ClientSize.Height * 23 / 600;
            _scalingplus[current].Location = new Point(_scaling.Right + ClientSize.Width / 80, _scaling.Location.Y - ClientSize.Width / 150);
            _scalingplus[current].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingplus[current].TextAlign = ContentAlignment.MiddleCenter;
            _scalingminus[current].Location = new Point(_scalingplus[current].Right + ClientSize.Width / 80, _scaling.Location.Y - ClientSize.Width / 150);
            _scalingminus[current].Size = new Size(_scaling.Height, _scaling.Height);
            _scalingminus[current].TextAlign = ContentAlignment.MiddleCenter;

            parameter.Location = new Point(_grid[1].Left, _scaling.Bottom + ClientSize.Height / 50);
            _parameter.Location = new Point(parameter.Right + ClientSize.Width / 50, _scaling.Bottom + ClientSize.Height / 50 - parameter.Height / 6);

            for (int i = 0; i < 4; i++)
            {
                RadioButtons[i].Width = ClientSize.Width / 18;
                RadioButtons[i].Location = new Point(pictureBox[1].Left + i * RadioButtons[i].Width + ClientSize.Width / 30, pictureBox[1].Bottom + ClientSize.Height / 60);

            }
            panel.Size = new Size(ClientSize.Width - (ClientSize.Width - _data[0].Location.X), ClientSize.Height - (ClientSize.Height - A[0].Location.Y) - ClientSize.Height / 12);
            Ok.Location = new Point(panel.Width, panel.Height);
            Ok.Size = new Size(ClientSize.Width - Ok.Location.X, ClientSize.Height - Ok.Location.Y);
            Ok.Font = new Font("Arial", (int)Math.Sqrt(Ok.Height * Ok.Width) * 3 / 11 + 1, FontStyle.Bold);
            x.Location = new Point(panel.Width - x.Width, panel.Height - x.Height);
            y.Location = new Point(y.Width / 2, y.Height / 2);
            _grid[current].Location = new Point(panel.Width - ClientSize.Width / 5, panel.Height + ClientSize.Height / 40);
            _grid[current].Size = new Size(ClientSize.Width / 12, ClientSize.Height / 20);
            if(bool_ok == true)
            DrawGraph();
        }
        void Calculating(double a, double b, int n)
        {
            double MaxV = Program.Func(b), MinV = Program.Func(a);
            double temp;
            double step = (b - a) / (n - 1);
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
            DrawGraph();
        } // функція для обчислень значень функцій
        private void DrawGraph() // обчислює точки координат для побудови графіка і викликає функцію Draw()
        {
            lines.Clear();
            linesX.Clear();
            linesY.Clear();
            bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, new Rectangle(0, 0, panel.Width, panel.Height));
            lines.Add(new Point[] { new Point(panel.Left + panel.Width / 12, panel.Top + panel.Height / 20), new Point(panel.Left + panel.Width / 12, panel.Bottom - panel.Height / 24) });
            lines.Add(new Point[] { new Point(panel.Left + panel.Width / 14, panel.Bottom - panel.Height / 20), new Point(panel.Right - panel.Width / 24, panel.Bottom - panel.Height / 20) });

            double MaxV = double.Parse(Max[current].Text);
            double MinV = double.Parse(Min[current].Text);
            float zero = 0;
            int n = graph_pts[current].Count;
            int countY = Math.Abs((int)MaxV + 1 - ((int)MinV - 1)) + 1;
            MinV = (int)MinV - 1;
            int lengthY = lines[0][1].Y - lines[0][0].Y;
            int LengthX = lines[1][1].X - lines[0][0].X;
            curvePoints = new PointF[n];
            for (int i = 0; i < countY; i++)
            {
                linesY.Add(new PointF[] { new PointF(lines[0][0].X - (float)panel.Width / 30, lines[1][0].Y - (float)(lengthY / (countY - 1)) * i), new PointF(lines[0][0].X, lines[1][0].Y - (float)(lengthY / (countY - 1)) * i) });
                if (MinV == 0)
                    zero = linesY[i][0].Y;
                digitsY.Add((int)MinV++);
            }
            if (n > 1)
            {
                for (int i = 0; i < n; i++)
                {
                    linesX.Add(new PointF[] { new PointF(lines[0][0].X + ((float)LengthX / (n - 1) * i), lines[1][0].Y), new PointF(lines[0][0].X + ((float)LengthX / (n - 1) * i), lines[1][0].Y + panel.Width / 70) });
                    curvePoints[i] = new PointF(linesX[i][0].X, (float)(zero + graph_pts[current][i].Y * (linesY[1][0].Y - linesY[0][0].Y)));
                }
            }
            if(grid_bool[current] == true)
            {
                foreach(PointF[] i in linesY)
                {
                    i[1].X = lines[1][1].X;
                }
                foreach (PointF[] i in linesX)
                {
                    i[0].Y = lines[0][0].Y;
                }
            }
            DrawToBuffer();
        }
        void Calculating(double a, double b, double n, double alpha)
        {

            double MaxV = Program.Func(b), MinV = Program.Func(a);
            double temp;
            double step = (b - a) / (n - 1);
            for (double i = a; i <= b; i += step)
            {
                if (x.Text == "t" && y.Text == "X")
                    temp = Program.X(i);
                else if (x.Text == "t" && y.Text == "Y")
                    temp = Program.Y(i);
                else if (x.Text == "X" && y.Text == "Y")
                    temp = Program.FuncY(i, alpha);
                else temp = Program.FuncX(i, alpha);
                if (temp > MaxV) MaxV = temp;
                else if (temp < MinV) MinV = temp;
                graph_pts[current].Add(new Pt(i, temp));
                _data[current].Rows.Add(i, temp);
            }
            Max[current].Text = "" + MaxV;
            Min[current].Text = "" + MinV;
            DrawGraph();
        }

        private void OnMenuExit(object obj, EventArgs ea)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
   
}

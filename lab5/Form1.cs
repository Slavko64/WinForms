using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace lab5
{
    public partial class Form1 : Form
    {
        
        public XmlDocument xDoc;
        int SideX = 3;
        int SideY = 4;

        TextBox[] TextFields;
        TextBox Per;

        Button[,] Field;
        Button Enters;

        Label[] letters;
        Label lb;

        ToolTip[] toolTips;
        ToolTip toolTip1 = new ToolTip();

        PictureBox pictureBox1 = new PictureBox();

        int[] dotcount = new int[4];

        int FocusedTextField = -1;

        public Form1()
        {
            xDoc = new XmlDocument();
            Text = "Calc";
            MaximizeBox = false;
            ClientSize = new Size(SideX * 150, SideY * 110);
            MenuItem miNewCalc = new MenuItem("Reset",
                new EventHandler(OnMenuStart), Shortcut.F2);
            MenuItem miGetData = new MenuItem("Get data", new EventHandler(OnGetData));
            MenuItem miSeparator = new MenuItem("-");
            MenuItem miExit = new MenuItem("Exit",
                new EventHandler(OnMenuExit), Shortcut.CtrlX);
            MenuItem miCalc = new MenuItem("&Menu",
                new MenuItem[] { miNewCalc, miGetData, miSeparator, miExit });
            Menu = new MainMenu(new MenuItem[] { miCalc });
            this.ActiveControl = null;
            FormClosing += Form1_Closing;
            int i, j;

            TextFields = new TextBox[4];
            for (i = 0; i < 4; i++)
            {
                TextFields[i] = new TextBox();
                TextFields[i].Size = new System.Drawing.Size(100, 200);
                TextFields[i].Font = new Font("Times New Roman", 20, FontStyle.Bold);
                TextFields[i].Top = 30;
                TextFields[i].Left = i * 105 + 10;
                TextFields[i].Click += new EventHandler(OnTextClick);
                TextFields[i].GotFocus += new EventHandler(OnTextClick);
                TextFields[i].KeyDown += txtAdd_KeyDown;
                Controls.Add(TextFields[i]);
            }
            TextFields[0].Name = "TextField A";
            TextFields[1].Name = "TextField B";
            TextFields[2].Name = "TextField C";
            Per = new TextBox();
            Per.Left = 325;
            Per.Top = 100;
            Controls.Add(Per);

            //вивід кнопок цифр
            Field = new Button[SideY, SideX];
            int k = 9;
            int[] Numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (i = 0; i < SideY; i++)
            {
                for (j = 0; j < SideX; j++)
                {
                    Field[i, j] = new Button();
                    Field[i, j].Parent = this;
                    Field[i, j].Left = 10 + 160 - j * 80;
                    Field[i, j].Top = 100 + i * 80;
                    Field[i, j].Width = 80;
                    Field[i, j].Height = 80;
                    Field[i, j].GotFocus += new EventHandler(CellGotFocus);
                    // Шрифт кнопки
                    Field[i, j].Font = new Font("Times New Roman", 20, FontStyle.Bold);
                    Field[i, j].Click += new EventHandler(OnCellClick);
                    if (k > 0)
                        Field[i, j].Text = "" + k--;
                    if (i == 3 && j == 0) Field[3, 0].Text = "C";
                    if (i == 3 && j == 1) Field[3, 1].Text = "0";
                    if (i == 3 && j == 2) Field[3, 2].Text = ",";
                }
            }

            Enters = new Button();
            Enters.Left = 300;
            Enters.Top = 300;
            Enters.Text = "=";
            Enters.Width = 100;
            Enters.Height = 100;
            Enters.Font = new Font("Century", 12, FontStyle.Bold);
            Enters.Click += new EventHandler(OnCellClick);
            Controls.Add(Enters);

            //вивід букв сторін трикутника
            letters = new Label[4];
            for (i = 0; i < 4; i++)
            {
                letters[i] = new Label();
                letters[i].Width = 50;
                if (i != 3)
                    letters[i].Left = i * 104 + 50;
                else
                    letters[i].Left = 350;
                letters[i].Top = 3;
                letters[i].Font = new Font("Arial", 16, FontStyle.Bold);
                switch (i)
                {
                    case 0:
                        letters[i].Text = "a";
                        break;
                    case 1:
                        letters[i].Text = "b";
                        break;
                    case 2:
                        letters[i].Text = "c";
                        break;
                    default:
                        letters[i].Text = "ma";
                        break;
                }
                Controls.Add(letters[i]);
            }

            lb = new Label();
            lb.Left = 270;
            lb.Top = 100;
            lb.Text = "Per = ";
            lb.Font = new Font("Century", 12, FontStyle.Bold);
            Controls.Add(lb);

            //додаю тултіпс
            toolTips = new ToolTip[4];
            for (i = 0; i < 4; i++)
            {
                toolTips[i] = new ToolTip();
                toolTips[i].InitialDelay = 0;
                toolTips[i].IsBalloon = true;
                switch (i)
                {
                    case 0:
                        toolTips[i].SetToolTip(TextFields[i], "side \"a\" of the triangle \"abc\"");
                        break;
                    case 1:
                        toolTips[i].SetToolTip(TextFields[i], "side \"b\" of the triangle \"abc\"");
                        break;
                    case 2:
                        toolTips[i].SetToolTip(TextFields[i], "side \"c\" of the triangle \"abc\"");
                        break;
                    default:
                        toolTips[i].SetToolTip(TextFields[i], "median of the triangle \"abc\"");
                        break;
                }
            }

            toolTip1.InitialDelay = 0;
            toolTip1.IsBalloon = true;
            toolTip1.SetToolTip(Per, "Perimeter of the triangle \"abc\"");
            CenterToScreen();


            pictureBox1.Image = Image.FromFile("../../Screenshot_2.png");
            pictureBox1.Left = 262;
            pictureBox1.Top = 155;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            Controls.Add(pictureBox1);
            GetDataFromXML();
            InitializeComponent();
        }

        private void OnGetData(object sender, EventArgs e)
        {
            GetDataFromXML();
        }

        void OnMenuStart(object obj, EventArgs ea) // вивід на екран після нажаття кнопки Reset, повністю очищає всі поля
        {

            for (int i = 0; i < 4; i++)
            {
                if (i == 0) TextFields[i].Text = "";
                else if (i == 1) TextFields[i].Text = "";
                else if (i == 2) TextFields[i].Text = "";
                else TextFields[i].Text = "";
                Controls.Add(TextFields[i]);
            }
            lb.Text = "Per = ";
            Per.Text = "";
            pictureBox1.BackColor = Color.Blue;
            pictureBox1.Image = Image.FromFile("../../Screenshot_2.png");
            pictureBox1.Left = 262;
            pictureBox1.Top = 155;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            Controls.Add(pictureBox1);
        }

        void OnMenuExit(object obj, EventArgs ea)
        {
            SetDataInXML();
            Close();
        }


        private void CellGotFocus(object sender, EventArgs e) // щоб фокус не пропадав при нажатті на кнопку цифри
        {

            if (FocusedTextField != -1)
            {
                TextFields[FocusedTextField].Focus();
            }
        }

        void OnTextClick(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                if (TextFields[i] == sender as TextBox)
                    FocusedTextField = i;
            }

        }

        public void txtAdd_KeyDown(object sender, KeyEventArgs e) // цей Event-Handler перехоплює нажаття клавіатури і дозволяє 
        {                                                         // надпис тільки цифр або розділових знаків
            e.SuppressKeyPress = true;
            if (TextFields[FocusedTextField].Text.Length == 1 && TextFields[FocusedTextField].Text[0] == '0' && (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0))
                e.SuppressKeyPress = true;
            else if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9))
                e.SuppressKeyPress = false;
            else if ((e.KeyCode >= Keys.NumPad0) && (e.KeyCode <= Keys.NumPad9))
                e.SuppressKeyPress = false;

            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                e.SuppressKeyPress = false;
            }
            else if (e.KeyValue == 110 || e.KeyValue == 190 || e.KeyValue == 191 || e.KeyValue == 188)
            {
                int count = 0;
                for (int i = 0; i < TextFields[FocusedTextField].TextLength; i++)
                {
                    if (TextFields[FocusedTextField].Text[i] == ',')
                        count++;
                }
                dotcount[FocusedTextField] = count;
                dotcount[FocusedTextField]++;
                if (dotcount[FocusedTextField] == 1 && TextFields[FocusedTextField].Text.Length != 0)
                    TextFields[FocusedTextField].Paste(",");
                else dotcount[FocusedTextField]--;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Calculating(double.Parse(TextFields[0].Text), double.Parse(TextFields[1].Text), double.Parse(TextFields[2].Text));
                    return;
                }
                catch
                {
                    MessageBox.Show("Невірний ввід");
                    return;
                }
            }
            TextFields[3].Text = "";
            Per.Text = "";
        }

        public void OnCellClick(object obj, EventArgs ea)  // зчитує нажаття кнопки на екрані, і передає її в текстове поле 
        {
            Button Cell = obj as Button;
            try
            {
                if (obj == this.Enters)
                {
                    Calculating(double.Parse(TextFields[0].Text), double.Parse(TextFields[1].Text), double.Parse(TextFields[2].Text));
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Невірний ввід");
                return;
            }
            if (FocusedTextField != -1)
            {
                if (Cell.Text == "C")
                {
                    TextFields[FocusedTextField].Text = "";
                    dotcount[FocusedTextField] = 0;
                }
                else if (Cell.Text == ",") // ком не може бути більше ніж 1
                {
                    int count = 0;
                    for (int i = 0; i < TextFields[FocusedTextField].TextLength; i++)
                    {
                        if (TextFields[FocusedTextField].Text[i] == ',')
                            count++;
                    }
                    dotcount[FocusedTextField] = count;
                    dotcount[FocusedTextField]++;
                    if (dotcount[FocusedTextField] == 1 && TextFields[FocusedTextField].Text.Length != 0)
                        TextFields[FocusedTextField].Paste(",");
                    else dotcount[FocusedTextField]--;

                }
                else if (Cell.Text == "0")
                {
                    if (TextFields[FocusedTextField].Text.Length == 1 && TextFields[FocusedTextField].Text[0] == '0') return;
                    else TextFields[FocusedTextField].Paste(Cell.Text);
                }

                else TextFields[FocusedTextField].Paste(Cell.Text);
                TextFields[3].Text = "";
                Per.Text = "";
            }
        }

        private void Calculating(double a, double b, double c) // основна функція, в якій рахується результат
        {

            if (a + b > c && a + c > b && c + b > a)
            {
                TextFields[3].Text = ("" + Math.Round(Math.Sqrt((2 * b * b + 2 * c * c - a * a)) / 2, 3));
                Per.Text = ("" + (a + b + c));
                Label Name = new Label();
                Name.Top = 425;
                Name.Left = 270;
                Name.Width = 170;
                Name.Text = "Виконав: Ярослав Рибак ПМ-31";
                Controls.Add(Name);
            }
            else
            {
                MessageBox.Show("не трикутник");
            }
        }
        private void GetDataFromXML()
        {

            xDoc.Load("../../config_me.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");

                    string name = "TextField " + attr.Value;
                    if (name == TextFields[0].Name)
                    {
                        TextFields[0].Text = xnode.Attributes.GetNamedItem("value").InnerText;

                    }
                    else if (name == TextFields[1].Name)
                    {
                        TextFields[1].Text = xnode.Attributes.GetNamedItem("value").InnerText;

                    }
                    else if (name == TextFields[2].Name)
                    {
                        TextFields[2].Text = xnode.Attributes.GetNamedItem("value").InnerText;

                    }

                }

            }
        }

        private void SetDataInXML()
        {
            XDocument docX = XDocument.Load("../../config_me.xml");

            XElement root = docX.Element("labels");

            foreach (XElement xe in root.Elements("label").ToList())
            {

                if (xe.Attribute("name").Value == "A")
                {
                    xe.Attribute("value").Value = TextFields[0].Text;
                }
                else if (xe.Attribute("name").Value == "B")
                {
                    xe.Attribute("value").Value = TextFields[1].Text;
                }
                else if (xe.Attribute("name").Value == "C")
                {
                    xe.Attribute("value").Value = TextFields[2].Text;
                }


            }
            //зберігання файлу
            docX.Save("../../config_me.xml");
        }
        private void Form1_Closing(object sender, EventArgs e)
        {
           SetDataInXML();
        }
    }
}

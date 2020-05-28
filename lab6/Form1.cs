using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace lab6
{
    
    public partial class Form1 : Form
    {
        private DataGridView CompanyDataGridView;
        private Button Add;
        private Button Delete;
        public Form1()
        {
            FormClosing += Form1_Closing;
            InitializeComponent();
            AutoSize = false;
            ClientSize = new Size(500, 400);
            CompanyDataGridView = new DataGridView
            {
                ColumnCount = 5,
                Name = "Company",
                Location = new Point(8, 8),
                Size = new Size(ClientSize.Width, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                CellBorderStyle = DataGridViewCellBorderStyle.Single,
                GridColor = Color.Black,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Dock = DockStyle.Fill
            };
            CompanyDataGridView.Rows[0].Height = CompanyDataGridView.Height - CompanyDataGridView.ColumnHeadersHeight;
            CompanyDataGridView.Columns[0].Name = "Name";
            CompanyDataGridView.Columns[1].Name = "Surname";
            CompanyDataGridView.Columns[2].Name = "Post";
            CompanyDataGridView.Columns[3].Name = "Salary";
            CompanyDataGridView.Columns[4].Name = "Address";

            Add = new Button();
            Add.Click += new EventHandler(OnAddClick);
            Add.AutoSize = false;
            Add.Size = new Size(ClientSize.Width / 8, ClientSize.Height / 8);
            Add.Location = new Point(ClientSize.Width - ClientSize.Width / 3, ClientSize.Height - ClientSize.Height / 6);
            Add.Text = "Add";

            Delete = new Button();
            Delete.Click += new EventHandler(OnDeleteClick);
            Delete.Size = new Size(ClientSize.Width / 8, ClientSize.Height / 8);
            Delete.Location = new Point(ClientSize.Width - Delete.Size.Width - ClientSize.Width / 16, ClientSize.Height - ClientSize.Height /6);
            Delete.Text = "Delete";
            

            ClientSizeChanged += new EventHandler(OnClientSizeChanged);
            Controls.Add(Add);
            Controls.Add(Delete);
            Controls.Add(CompanyDataGridView);
            InitializationFromXml();


        }
        private void InitializationFromXml()
        {
            XDocument docX = XDocument.Load("../../Company.xml");

            int row = 0, column = 0;

            int newrows = docX.Root.Elements("company").Count();
            CompanyDataGridView.Rows.Add(newrows);

            foreach (XElement el in docX.Root.Elements("company"))
            {
                XAttribute attr_id = el.Attribute("id");
                XElement id = el.Element("id");
                XElement name = el.Element("name");
                XElement surname = el.Element("surname");
                XElement post = el.Element("post");
                XElement salary = el.Element("salary");
                XElement address = el.Element("address");


                if (attr_id != null && name != null && surname != null && salary != null && address != null)
                {
                    CompanyDataGridView.Rows[row].Cells[column++].Value = name.Value;
                    CompanyDataGridView.Rows[row].Cells[column++].Value = surname.Value;
                    CompanyDataGridView.Rows[row].Cells[column++].Value = post.Value;
                    CompanyDataGridView.Rows[row].Cells[column++].Value = salary.Value;
                    CompanyDataGridView.Rows[row].Cells[column++].Value = address.Value;
                }
                column = 0;
                row++;
            }

        }
        private void WriteInXml()
        {
            int id = 0, row = 0, column = 0;
            XDocument docX = XDocument.Load("../../Company.xml");

            docX.Root.RemoveAll();
            foreach (DataGridViewRow Row in CompanyDataGridView.Rows)
            {
                try
                {
                    XElement track = new XElement("company",
                        new XAttribute("id", id++),
                        new XElement("name", CompanyDataGridView.Rows[row].Cells[column++].Value.ToString()),
                        new XElement("surname", CompanyDataGridView.Rows[row].Cells[column++].Value.ToString()),
                        new XElement("post", CompanyDataGridView.Rows[row].Cells[column++].Value.ToString()),
                        new XElement("salary", CompanyDataGridView.Rows[row].Cells[column++].Value.ToString()),
                        new XElement("address", CompanyDataGridView.Rows[row].Cells[column++].Value.ToString())
                    );

                    docX.Root.Add(track);
                    column = 0;
                    row++;
                }
                catch (Exception)
                {

                    break;
                }
            }

            docX.Save("../../Company.xml");
        }
        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            Add.Size = new Size(ClientSize.Width / 8, ClientSize.Height / 8);
            Add.Location = new Point(ClientSize.Width - ClientSize.Width / 3, ClientSize.Height - ClientSize.Height / 6);
            Delete.Size = new Size(ClientSize.Width / 8, ClientSize.Height / 8);
            Delete.Location = new Point(ClientSize.Width - Delete.Size.Width - ClientSize.Width / 16, ClientSize.Height - ClientSize.Height / 6);
        }
        private void OnAddClick(object sender, EventArgs e)
        {
            CompanyDataGridView.Rows.Add();
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            if (CompanyDataGridView.SelectedRows.Count > 0 && CompanyDataGridView.SelectedRows[0].Index != CompanyDataGridView.Rows.Count - 1)
                CompanyDataGridView.Rows.RemoveAt(CompanyDataGridView.SelectedRows[0].Index);
            
        }
        private void Form1_Closing(object sender, EventArgs e)
        {
            WriteInXml();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

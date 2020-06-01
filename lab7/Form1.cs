using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Svg;
using System.Xml.Linq;




namespace lab7
{
    public partial class Form1 : Form
    {
        string fileNameSvg;
        OpenFileDialog openSvgFile = new OpenFileDialog();
        SaveFileDialog saveSvgFile = new SaveFileDialog();
        PictureBox svgImage;
        TextBox Code;
        public Form1()
        {
            InitializeComponent();
            Text = "lab7";
            MenuItem miOpenFile = new MenuItem("Open File",
                new EventHandler(OnMenuOpenFile));
            MenuItem miSaveFile = new MenuItem("Save File",
                new EventHandler(OnMenuSaveFile));
            MenuItem miMenu = new MenuItem("&Menu",
                new MenuItem[] { miOpenFile, miSaveFile });
            Menu = new MainMenu(new MenuItem[] { miMenu });
            ClientSizeChanged += new EventHandler(OnClientSizeChanged1);
            svgImage = new PictureBox();
            svgImage.Size = new Size(ClientSize.Width * 3 / 4, ClientSize.Height);
            Code = new TextBox();
            Code.AutoSize = false;
            Code.Location = new Point(svgImage.Right, svgImage.Top);
            Code.Size = new Size(ClientSize.Width/ 4, ClientSize.Height);
            Code.Multiline = true;
            openSvgFile.Filter = "Text files(*.SVG)|*.svg|All files(*.*)|*.*";
            saveSvgFile.Filter = "Text files(*.SVG)|*.svg|All files(*.*)|*.*";
            Controls.Add(svgImage);
            Controls.Add(Code);
        }

        private void OnClientSizeChanged1(object sender, EventArgs e)
        {
            svgImage.Size = new Size(ClientSize.Width * 3 / 4, ClientSize.Height);
            Code.Location = new Point(svgImage.Right, svgImage.Top);
            Code.Size = new Size(ClientSize.Width / 4, ClientSize.Height);
        }

        private void OnMenuSaveFile(object sender, EventArgs e)
        {
            var svgDoc = SvgDocument.FromSvg<SvgDocument>(Code.Text);
            if (saveSvgFile.ShowDialog() == DialogResult.OK)
            {
                svgDoc.Write(saveSvgFile.FileName);
            }
        }

        private void OnMenuOpenFile(object sender, EventArgs e)
        {
            try
            {
                if (openSvgFile.ShowDialog() == DialogResult.OK)
                {
                    var svgDoc = SvgDocument.Open(openSvgFile.FileName);

                    RenderSvg(svgDoc);

                    Code.TextChanged -= Code_TextChanged;
                    try
                    {
                        var xmlDoc = new XmlDocument
                        {
                            XmlResolver = null,
                            PreserveWhitespace = true
                            
                        };
                        xmlDoc.Load(openSvgFile.FileName);
                        Code.Text = xmlDoc.InnerXml;
                    }
                    finally
                    {
                        Code.TextChanged += Code_TextChanged;
                    }
                }
            }
            catch
            {
                return;
            }
            try
            {
                fileNameSvg = openSvgFile.FileName;
                XDocument svgText = XDocument.Load(fileNameSvg);
                XElement root = svgText.Element("svg");
            }
            catch
            {
                return;
            }
        }
        private void Code_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var svgDoc = SvgDocument.FromSvg<SvgDocument>(Code.Text);
                RenderSvg(svgDoc);
            }
            catch
            {
            }
        }
        private void RenderSvg(SvgDocument svgDoc)
        {
            if (svgImage.Image != null)
                svgImage.Image.Dispose();
            
            svgImage.Image = svgDoc.Draw();

            var baseUri = svgDoc.BaseUri;
            var outputDir = Path.GetDirectoryName(baseUri != null && baseUri.IsFile ? baseUri.LocalPath : Application.ExecutablePath);
            svgImage.Image.Save(Path.Combine(outputDir, "output.png"));
        }
    }
}

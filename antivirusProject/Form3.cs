using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace antivirusProject
{
    public partial class Form3 : Form
    {
        public Point mouseLocation;
        public NotifyIcon notifyIcon = new NotifyIcon();
        public Form3()
        {

            InitializeComponent();
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.Visible = false;
        }


        private void button4_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;

            }
        }

        private void button5_Click_1(object sender, EventArgs e)  // hızlı tarama
        {
            Antivirus tarama = new Antivirus();
            tarama.Main(Antivirus.scanTypes.FAST);

            ScanComplete();
           
        }

        private void button6_Click(object sender, EventArgs e) // tam tarama
        {
            Antivirus tarama = new Antivirus();
            tarama.Main(Antivirus.scanTypes.FULL);

            ScanComplete();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Antivirus tarama = new Antivirus();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Filter = "Tüm Dosyalar|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                tarama.Main(selectedFileName);
            }
            ScanComplete();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Antivirus tarama = new Antivirus();
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolderPath = folderBrowserDialog.SelectedPath;
                tarama.Main(selectedFolderPath);
            }
            ScanComplete();
        }

        private void ScanComplete()
        {
            string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
            richTextBox1.Text = textFromFile;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000, "Tarama Tamamlandı", "Tarama işlemi başarıyla tamamlandı", ToolTipIcon.Info);
            notifyIcon.Visible = false;

        }
    }
}

using System.ComponentModel;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace antivirusProject
{
    public partial class Form3 : Form
    {

        public Point mouseLocation;
        signatureAntivirus scan = new signatureAntivirus();
        heuristicAntivirus heuristic = new heuristicAntivirus();
        List<string> quarantine = new List<string>();
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000; // USB cihazı eklendiğinde
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // USB cihazı çıkarıldığında
        public List<string> filesForHeuristicScan = new List<string>();
        public Form3()
        {

            InitializeComponent();
            groupBox2.Hide();
            groupBox3.Hide();
            groupBox1.Show();
            string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
            richTextBox1.Text = textFromFile;

        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_DEVICECHANGE)
            {
                int eventType = m.WParam.ToInt32();
                if (eventType == DBT_DEVICEARRIVAL)
                {

                    ChangeSceneWhenScanStarts();
                    string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
                    richTextBox1.Text = textFromFile;
                    Application.DoEvents();
                    richTextBox1.ScrollToCaret();
                    this.scan = new signatureAntivirus("F:");
                    scan.RunScan();
                    for (int i = 0; i < scan.results.MalwareFiles.Count; i++)
                    { //Todo : burayı malware files a göre yap dostum sonra da delay ekle ki anlaşılmasın
                        richTextBox1.Text += scan.results.MalwareFiles[i].FName.ToString() + "\n";
                        Application.DoEvents();
                        CheckAndHighlight("F", Color.Red);
                        Application.DoEvents();
                        Thread.Sleep(100);

                    }

                    ScanComplete(scan.results);
                }
                else if (eventType == DBT_DEVICEREMOVECOMPLETE)
                {

                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {

            Application.Exit();
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
            ChangeSceneWhenScanStarts();
            string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
            richTextBox1.Text = textFromFile;
            Application.DoEvents();
            richTextBox1.ScrollToCaret();

            this.scan = new signatureAntivirus(scanTypes.FAST);
            scan.RunScan();

            for (int i = 0; i < scan.results.MalwareFiles.Count; i++)
            { //Todo : burayı malware files a göre yap dostum sonra da delay ekle ki anlaşılmasın
                richTextBox1.Text += scan.results.MalwareFiles[i].FName.ToString() + "\n";
                Application.DoEvents();
                CheckAndHighlight("C", Color.Red);
                Application.DoEvents();
                Thread.Sleep(100);

            }
            ScanComplete(scan.results);
            prepareForHeuristic(scan.results.allSystemFiles, scan.results.MalwareFiles);

            if (filesForHeuristicScan.Count != 0)
            {
                for (int i = 0; i < filesForHeuristicScan.Count; i++)
                {
                    heuristicAntivirus.getPEfile(filesForHeuristicScan[i]);
                    heuristicAntivirus.extract();
                    heuristicAntivirus.csvTransfer();
                }
                quarantine = heuristicAntivirus.predict(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\pythonpredict.py", "", filesForHeuristicScan);
                if (quarantine.Count != 0)
                {
                    NotifyIcon icon = new NotifyIcon();
                    icon.Visible = true;
                    icon.Icon = SystemIcons.Warning;
                    icon.BalloonTipIcon = ToolTipIcon.Warning;
                    icon.ShowBalloonTip(1000, "Tarama Tamamlandı", "Tarama sonucu bilgisayarda " + (quarantine.Count - 1).ToString() + " adet yazılım karantina altına alındı!", ToolTipIcon.Warning);
                    Thread.Sleep(100);
                    icon.Visible = false;
                }
            }
            textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
            richTextBox1.Text = textFromFile;
            heuristicResults();
            Application.DoEvents();
            ScanLoadingEvents();

        }

        private void button6_Click(object sender, EventArgs e) // tam tarama
        {
            ChangeSceneWhenScanStarts();
            string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
            richTextBox1.Text = textFromFile;
            Application.DoEvents();
            richTextBox1.ScrollToCaret();

            this.scan = new signatureAntivirus(scanTypes.FULL);
            scan.RunScan();
            for (int i = 0; i < scan.results.MalwareFiles.Count; i++)
            { //Todo : burayı malware files a göre yap dostum sonra da delay ekle ki anlaşılmasın
                richTextBox1.Text += scan.results.MalwareFiles[i].FName.ToString() + "\n";
                Application.DoEvents();
                CheckAndHighlight("C", Color.Red);
                Application.DoEvents();
                Thread.Sleep(100);

            }

            ScanComplete(scan.results);
            prepareForHeuristic(scan.results.allSystemFiles, scan.results.MalwareFiles);

            if (filesForHeuristicScan.Count != 0)
            {
                for (int i = 0; i < filesForHeuristicScan.Count; i++)
                {
                    heuristicAntivirus.getPEfile(filesForHeuristicScan[i]);
                    heuristicAntivirus.extract();
                    heuristicAntivirus.csvTransfer();
                }
                quarantine = heuristicAntivirus.predict(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\pythonpredict.py", "", filesForHeuristicScan);
                if (quarantine.Count != 0)
                {
                    NotifyIcon icon = new NotifyIcon();
                    icon.Visible = true;
                    icon.Icon = SystemIcons.Warning;
                    icon.BalloonTipIcon = ToolTipIcon.Warning;
                    icon.ShowBalloonTip(1000, "Tarama Tamamlandı", "Tarama sonucu bilgisayarda " + (quarantine.Count - 1).ToString() + " adet yazılım karantina altına alındı!", ToolTipIcon.Warning);
                    Thread.Sleep(100);
                    icon.Visible = false;
                }
            }
            textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
            richTextBox1.Text = textFromFile;
            heuristicResults();
            Application.DoEvents();
            ScanLoadingEvents();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Filter = "Tüm Dosyalar|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ChangeSceneWhenScanStarts();
                string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
                richTextBox1.Text = textFromFile;
                Application.DoEvents();
                richTextBox1.ScrollToCaret();

                this.scan = new signatureAntivirus(openFileDialog1.FileName);
                for (int i = 0; i < scan.results.MalwareFiles.Count; i++)
                { //Todo : burayı malware files a göre yap dostum sonra da delay ekle ki anlaşılmasın
                    richTextBox1.Text += scan.results.MalwareFiles[i].FName.ToString() + "\n";
                    Application.DoEvents();
                    CheckAndHighlight("C", Color.Red);
                    Application.DoEvents();
                    Thread.Sleep(100);

                }

                ScanComplete(scan.results);
                prepareForHeuristic(scan.results.allSystemFiles, scan.results.MalwareFiles);

                if (filesForHeuristicScan.Count != 0)
                {
                    for (int i = 0; i < filesForHeuristicScan.Count; i++)
                    {
                        heuristicAntivirus.getPEfile(filesForHeuristicScan[i]);
                        heuristicAntivirus.extract();
                        heuristicAntivirus.csvTransfer();
                    }
                    quarantine = heuristicAntivirus.predict(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\pythonpredict.py", "", filesForHeuristicScan);
                    if (quarantine.Count != 0)
                    {
                        NotifyIcon icon = new NotifyIcon();
                        icon.Visible = true;
                        icon.Icon = SystemIcons.Warning;
                        icon.BalloonTipIcon = ToolTipIcon.Warning;
                        icon.ShowBalloonTip(1000, "Tarama Tamamlandı", "Tarama sonucu bilgisayarda " + (quarantine.Count - 1).ToString() + " adet yazılım karantina altına alındı!", ToolTipIcon.Warning);
                        Thread.Sleep(100);
                        icon.Visible = false;
                    }
                }
                heuristicResults();
                textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
                richTextBox1.Text = textFromFile;
                Application.DoEvents();
                ScanLoadingEvents();
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                ChangeSceneWhenScanStarts();
                string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
                richTextBox1.Text = textFromFile;
                Application.DoEvents();
                richTextBox1.ScrollToCaret();
                this.scan = new signatureAntivirus(folderBrowserDialog.SelectedPath);
                scan.RunScan();
                for (int i = 0; i < scan.results.MalwareFiles.Count; i++)
                { //Todo : burayı malware files a göre yap dostum sonra da delay ekle ki anlaşılmasın
                    richTextBox1.Text += scan.results.MalwareFiles[i].FName.ToString() + "\n";
                    Application.DoEvents();
                    CheckAndHighlight("C", Color.Red);
                    Application.DoEvents();
                    Thread.Sleep(100);

                }

                ScanComplete(scan.results);
                prepareForHeuristic(scan.results.allSystemFiles, scan.results.MalwareFiles);

                if (filesForHeuristicScan.Count != 0)
                {
                    for (int i = 0; i < filesForHeuristicScan.Count; i++)
                    {
                        heuristicAntivirus.getPEfile(filesForHeuristicScan[i]);
                        heuristicAntivirus.extract();
                        heuristicAntivirus.csvTransfer();
                    }
                    quarantine = heuristicAntivirus.predict(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\pythonpredict.py", "", filesForHeuristicScan);
                    if (quarantine.Count != 0)
                    {
                        NotifyIcon icon = new NotifyIcon();
                        icon.Visible = true;
                        icon.Icon = SystemIcons.Warning;
                        icon.BalloonTipIcon = ToolTipIcon.Warning;
                        icon.ShowBalloonTip(1000, "Tarama Tamamlandı", "Tarama sonucu bilgisayarda " + (quarantine.Count-1).ToString() + " adet yazılım karantina altına alındı!", ToolTipIcon.Warning);
                        Thread.Sleep(100);
                        icon.Visible = false;
                    }
                }
                textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
                richTextBox1.Text = textFromFile;


                heuristicResults();
                Application.DoEvents();
                ScanLoadingEvents();
            }

        }
        private void ChangeSceneWhenScanStarts()
        {
            groupBox3.Hide();
            groupBox1.Hide();
            groupBox2.Show();
        }

        private void heuristicResults()
        {
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "Karantinaya Alınan Uygulamalar";

            // Liste verilerini DataGridView'e ekleme
            foreach (var name in quarantine)
            {
                if (!string.IsNullOrEmpty(name)) // Boş olmayanları kontrol et
                {
                    string[] row = new string[] { name };
                    dataGridView1.Rows.Add(row);
                }
            }
            if (quarantine.Count == 0)
            {
                dataGridView1.Rows.Add("");
            }
            dataGridView1.Columns[0].Width = 700;
            Application.DoEvents();
        }

        private void ScanComplete(Results result)
        {
            string textFromFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt");
            richTextBox1.Text = textFromFile;
            ScanLoadingEvents();
            if (result.MalwareFiles.Count > 0)
            {
                NotifyIcon icon = new NotifyIcon();
                icon.Visible = true;
                icon.Icon = SystemIcons.Error;
                icon.BalloonTipIcon = ToolTipIcon.Error;
                icon.ShowBalloonTip(1000, "Tarama Tamamlandı", "Tarama sonucu bilgisayarda " + result.MalwareFiles.Count.ToString() + " adet zararlı yazılım bulundu!", ToolTipIcon.Error);
                Thread.Sleep(100);
                icon.Visible = false;
            }
            else
            {
                NotifyIcon icon = new NotifyIcon();
                icon.Visible = true;
                icon.Icon = SystemIcons.Information;
                icon.BalloonTipIcon = ToolTipIcon.Info;
                icon.ShowBalloonTip(1000, "Tarama Tamamlandı", "Tarama işlemi başarıyla tamamlandı. Zararlı yazılım bulunamadı.", ToolTipIcon.Info);
                Thread.Sleep(100);
                icon.Visible = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Hide();
            groupBox3.Hide();
            groupBox2.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox2.Hide();
            groupBox1.Show();
            groupBox3.Hide();

        }

        private void CheckAndHighlight(string keyword, Color highlightColor)
        {
            int startIndex = 0;
            while (startIndex < richTextBox1.TextLength)
            {
                int wordStartIndex = richTextBox1.Find(keyword, startIndex, RichTextBoxFinds.None);
                if (wordStartIndex != -1)
                {
                    int lineIndex = richTextBox1.GetLineFromCharIndex(wordStartIndex);
                    int lineStartIndex = richTextBox1.GetFirstCharIndexFromLine(lineIndex);
                    int lineEndIndex = richTextBox1.GetFirstCharIndexFromLine(lineIndex + 1);
                    if (lineEndIndex == -1)
                        lineEndIndex = richTextBox1.TextLength;
                    richTextBox1.Select(lineStartIndex, lineEndIndex - lineStartIndex);
                    richTextBox1.SelectionColor = highlightColor;
                    startIndex = lineEndIndex + 1;
                }
                else
                {
                    break;
                }
            }
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            // Her metin değiştiğinde, belirli bir kelimeyi kontrol edip renklendiriyoruz.
            CheckAndHighlight("GÜVENLİ", Color.Green);
            CheckAndHighlight("SİLİNDİ", Color.Red);
            CheckAndHighlight("BULUNAN", Color.Red);
            CheckAndHighlight("KARANTİNA", Color.DarkGoldenrod);

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadingEvents();

        }
        private void LoadingEvents()
        {
            label2.Text = "Son Tarama Bilgileri\n";
            string text = "";
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length > 0)
                {
                    string lastLine = lines[lines.Length - 1];
                    string[] words = lastLine.Split(';');
                    foreach (string word in words)
                    {
                        label2.Text += "\n" + word;
                    }

                    foreach (string line in lines)
                    {

                        text = text + "\n" + line;
                    }
                    richTextBox1.Text = text;
                }
                else
                {
                    richTextBox1.Text = "Dosya boş.";
                }
            }
            else
            {
                richTextBox1.Text = "Dosya bulunamadı.";
            }
            CheckAndHighlight("GÜVENLİ", Color.Green);
            CheckAndHighlight("SİLİNDİ", Color.Red);
            CheckAndHighlight("BULUNAN", Color.Red);
            CheckAndHighlight("KARANTİNA", Color.DarkGoldenrod);
            if (label2.Text.Contains("KARANTİNA")) { Status(true); } else { Status(false); }
        }

        public void ScanLoadingEvents()
        {

            label2.Text = "Son Tarama Bilgileri\n";
            string text = "";
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length > 0)
                {
                    string lastLine = lines[lines.Length - 1];
                    string[] words = lastLine.Split(';');
                    foreach (string word in words)
                    {
                        label2.Text += "\n" + word;

                    }
                    if (label2.Text.Contains("KARANTİNA"))
                    {
                        Status(true);
                        /*  string[] a = label2.Text.Split(";");
                          label2.Text += a[0] + "\n" + a[1];*/

                    }
                    else
                    {
                        Status(false);
                    }
                }
            }

        }

        public void Status(bool stat)
        {

            if (stat)
            {
                pictureBox1.Image = Image.FromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\error.png");
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                label3.Text = "Sistem güvenli değil,\ntehditler karantinaya alındı.";
            }
            else
            {
                pictureBox1.Image = Image.FromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\9827073.png");
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                label3.Text = "Sistem şu anda güvenli,\nherhangi bir tehdit bulunmamakta.";
            }
        }

        public void prepareForHeuristic(List<FileFormat> allfiles, List<FileFormat> malwarefiles)
        {
            if (malwarefiles.Count > 0)
            {
                for (int i = 0; i < allfiles.Count; i++)
                {
                    for (int j = 0; j < malwarefiles.Count; j++)
                    {
                        if (allfiles[i].FName == malwarefiles[j].FName)
                        {
                            break;
                        }
                        else
                        {

                            string a = allfiles[i].FName;
                            if (a.Contains(".exe"))
                            {
                                filesForHeuristicScan.Add(a);
                            }
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < allfiles.Count; i++)
                {
                    string a = allfiles[i].FName;
                    if (a.Contains(".exe"))
                    {
                        this.filesForHeuristicScan.Add(a);
                    }

                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox2.Hide();
            groupBox1.Hide();
            groupBox3.Show();
            dataGridView1.Rows.Clear();
            heuristicResults();
        }

        private void button9_Click(object sender, EventArgs e) //karantina sil
        {
            if (dataGridView1.SelectedCells[0].Value != "")
            {
                string a = dataGridView1.SelectedCells[0].Value.ToString();
                scan.DeleteFile(a);
                quarantine.Remove(a);
            }
            dataGridView1.Rows.Clear();
            heuristicResults();
            Application.DoEvents();
        }

        private void button10_Click(object sender, EventArgs e) //görmezden gel
        {
            if (dataGridView1.SelectedCells[0].Value != "")
            {
                string a = dataGridView1.SelectedCells[0].Value.ToString();
                quarantine.Remove(a);
            }
            dataGridView1.Rows.Clear();
            heuristicResults();
            Application.DoEvents();

        }
    }
}

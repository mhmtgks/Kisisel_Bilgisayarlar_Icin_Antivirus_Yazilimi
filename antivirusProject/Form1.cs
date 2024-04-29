using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Xml;

namespace antivirusProject
{
	public partial class Form1 : Form
	{
		public Point mouseLocation2;
        static NotifyIcon notifyIcon;
        static Form mainForm;
        public Form1()
		{
            string belgelerYolu = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami"; // Belgeler klas�r�n�n yolu
            string projeKlasorYolu = Directory.GetCurrentDirectory();
            // Belgeler klas�r�n�n varl���n� kontrol et
            if (Directory.Exists(belgelerYolu))
            {
                Console.WriteLine("Belgeler klas�r�ne eri�im ba�ar�l�.");

            }
            else
            {
                KlasorKopyala(projeKlasorYolu, belgelerYolu);
                StartupCopy();
            }

            InitializeComponent();
            mainForm = new Form3();
            mainForm.Text = "Ki�isel Bilgisayarlar ��in Antivir�s Program�";

            // NotifyIcon olu�turma
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.Visible = true;
            mainForm.FormClosing += Form1_FormClosing;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // NotifyIcon'un ba�l� oldu�u ContextMenuStrip olu�turma
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += (sender, e) => { Application.Exit(); };
            contextMenuStrip.Items.Add(exitMenuItem);
            notifyIcon.ContextMenuStrip = contextMenuStrip;


            // Ana formu g�ster
            Application.Run(mainForm);

            
        }
        private static void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // Bildirim simgesine �ift t�klan�rsa ana formu g�ster
            mainForm.Show();
            mainForm.WindowState = FormWindowState.Normal;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ana form kapat�l�rken uygulamay� gizle
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Kapatmay� iptal et
                mainForm.Hide(); // Ana formu gizle
                                 // notifyIcon.ShowBalloonTip(1000, "Uygulama Gizlendi", "Uygulama gizlendi, bildirim alan�nda devam ediyor.", ToolTipIcon.Info);
                notifyIcon.ShowBalloonTip(100, "Uygulama �al���yor", "Antivir�s yaz�l�m� arka planda �al��maya devam ediyor.", ToolTipIcon.None);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void label4_Click(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{

			Application.Exit();
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			mouseLocation2 = new Point(-e.X, -e.Y);
			
		}

		private void panel1_MouseMove(object sender, MouseEventArgs e)
		{
			if(e.Button== MouseButtons.Left)
			{
				Point mousePose = Control.MousePosition;
				mousePose.Offset(mouseLocation2.X, mouseLocation2.Y);
				Location = mousePose;
					
			}
		}

        static void KlasorKopyala(string kaynak, string hedef)
        {
            DirectoryInfo kaynakKlasor = new DirectoryInfo(kaynak);
            DirectoryInfo hedefKlasor = new DirectoryInfo(hedef);

            // Klas�r� olu�tur
            if (!hedefKlasor.Exists)
            {
                hedefKlasor.Create();
            }

            // Klas�rdeki dosyalar� kopyala
            foreach (FileInfo dosya in kaynakKlasor.GetFiles())
            {
                dosya.CopyTo(Path.Combine(hedefKlasor.FullName, dosya.Name), true);
            }

            // Alt klas�rler varsa, kopyalama i�lemini onlar i�in tekrarla
            foreach (DirectoryInfo altKlasor in kaynakKlasor.GetDirectories())
            {
                KlasorKopyala(altKlasor.FullName, Path.Combine(hedefKlasor.FullName, altKlasor.Name));
            }
        }

        static void StartupCopy()
        {
            // Kullan�c�n�n y�netici olup olmad���n� kontrol et
            if (IsAdministrator())
            {

                try
                {
                    string uygulama_dizin = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +@"\AntivirusProgrami\antivirusProject.exe"; // k�sayolu olu�turulacak dizin
                    string kisayol_adi = "AntivirusProgram�"; // k�sayolu olu�turulacak uygulaman�n masa�zerindeki ad�
                    string masaustu_dizin = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup) + "\\" + kisayol_adi + ".lnk"; // masa�st� dizini ve birle�tirildi.
                    Shortcut.Create(masaustu_dizin, uygulama_dizin, "", null, kisayol_adi, "", null); // k�sayol olu�turma fonksiyonu kullan�ld�.
                    changeAdminintratorRequests.change(uygulama_dizin + ".manifest");
                }
                catch (Exception hata) { MessageBox.Show("Uygulama Kurulamad�. Hata: " + hata.Message); }
            }

        }

        static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }

    public class Shortcut
    {

        private static Type m_type = Type.GetTypeFromProgID("WScript.Shell");
        private static object m_shell = Activator.CreateInstance(m_type);

        [ComImport, TypeLibType((short)0x1040), Guid("F935DC23-1CF0-11D0-ADB9-00C04FD58A0B")]
        private interface IWshShortcut
        {
            [DispId(0)]
            string FullName { [return: MarshalAs(UnmanagedType.BStr)][DispId(0)] get; }
            [DispId(0x3e8)]
            string Arguments { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3e8)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3e8)] set; }
            [DispId(0x3e9)]
            string Description { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3e9)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3e9)] set; }
            [DispId(0x3ea)]
            string Hotkey { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3ea)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ea)] set; }
            [DispId(0x3eb)]
            string IconLocation { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3eb)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3eb)] set; }
            [DispId(0x3ec)]
            string RelativePath { [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ec)] set; }
            [DispId(0x3ed)]
            string TargetPath { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3ed)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ed)] set; }
            [DispId(0x3ee)]
            int WindowStyle { [DispId(0x3ee)] get; [param: In][DispId(0x3ee)] set; }
            [DispId(0x3ef)]
            string WorkingDirectory { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3ef)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ef)] set; }
            [TypeLibFunc((short)0x40), DispId(0x7d0)]
            void Load([In, MarshalAs(UnmanagedType.BStr)] string PathLink);
            [DispId(0x7d1)]
            void Save();
        }

        public static void Create(string fileName, string targetPath, string arguments, string workingDirectory, string description, string hotkey, string iconPath)
        {
            IWshShortcut shortcut = (IWshShortcut)m_type.InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, m_shell, new object[] { fileName });
            shortcut.Description = description;
            shortcut.Hotkey = hotkey;
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = workingDirectory;
            shortcut.Arguments = arguments;
            if (!string.IsNullOrEmpty(iconPath))
                shortcut.IconLocation = iconPath;
            shortcut.Save();
        }
    }

    public static class changeAdminintratorRequests
    {
        public static void change(string manifestYolu)
        {
           

            // Manifest dosyas�n� kontrol et
            if (File.Exists(manifestYolu))
            {
                // Manifest dosyas�n� y�kle
                XmlDocument doc = new XmlDocument();
                doc.Load(manifestYolu);

                // UAC (User Account Control) eklemek veya g�ncellemek i�in bir XML ��esi olu�tur
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
                nsMgr.AddNamespace("asmv3", "urn:schemas-microsoft-com:asm.v3");
                XmlNode requestedExecutionLevelNode = doc.SelectSingleNode("//asmv3:requestedExecutionLevel", nsMgr);

                // UAC (User Account Control) ��esi varsa, kald�r
                if (requestedExecutionLevelNode != null)
                {
                    requestedExecutionLevelNode.Attributes["level"].Value = "asInvoker"; // "asInvoker" olarak de�i�tir

                    // Manifest dosyas�n� kaydet
                    doc.Save(manifestYolu);

                    Console.WriteLine("Manifest dosyas� ba�ar�yla g�ncellendi. Uygulama art�k y�netici izni gerektirmiyor.");
                }
                else
                {
                    Console.WriteLine("Uygulama zaten y�netici izni gerektirmiyor.");
                }
            }
            else
            {
                Console.WriteLine("Manifest dosyas� bulunamad�.");
            }
        }


    }
}
namespace antivirusProject
{
	public partial class Form1 : Form
	{
		public Point mouseLocation2;
        static NotifyIcon notifyIcon;
        static Form mainForm;
        public Form1()
		{

			
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
	}
}
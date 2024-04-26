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
            mainForm.Text = "Kiþisel Bilgisayarlar Ýçin Antivirüs Programý";

            // NotifyIcon oluþturma
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.Visible = true;
            mainForm.FormClosing += Form1_FormClosing;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // NotifyIcon'un baðlý olduðu ContextMenuStrip oluþturma
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += (sender, e) => { Application.Exit(); };
            contextMenuStrip.Items.Add(exitMenuItem);
            notifyIcon.ContextMenuStrip = contextMenuStrip;


            // Ana formu göster
            Application.Run(mainForm);
        }
        private static void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // Bildirim simgesine çift týklanýrsa ana formu göster
            mainForm.Show();
            mainForm.WindowState = FormWindowState.Normal;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ana form kapatýlýrken uygulamayý gizle
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Kapatmayý iptal et
                mainForm.Hide(); // Ana formu gizle
                                 // notifyIcon.ShowBalloonTip(1000, "Uygulama Gizlendi", "Uygulama gizlendi, bildirim alanýnda devam ediyor.", ToolTipIcon.Info);
                notifyIcon.ShowBalloonTip(100, "Uygulama Çalýþýyor", "Antivirüs yazýlýmý arka planda çalýþmaya devam ediyor.", ToolTipIcon.None);
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
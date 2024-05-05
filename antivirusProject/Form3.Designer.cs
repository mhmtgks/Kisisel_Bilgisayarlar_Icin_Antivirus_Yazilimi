namespace antivirusProject
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panelMenu = new Panel();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            panel1 = new Panel();
            label1 = new Label();
            richTextBox1 = new RichTextBox();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            panelMenu.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Dock = DockStyle.Top;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.White;
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(219, 68);
            button1.TabIndex = 0;
            button1.Text = "Anasayfa";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(32, 56, 90);
            panelMenu.Controls.Add(button4);
            panelMenu.Controls.Add(button3);
            panelMenu.Controls.Add(button2);
            panelMenu.Controls.Add(button1);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 45);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(219, 681);
            panelMenu.TabIndex = 4;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Bottom;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.ForeColor = Color.White;
            button4.Location = new Point(0, 592);
            button4.Name = "button4";
            button4.Size = new Size(219, 89);
            button4.TabIndex = 3;
            button4.Text = "Mehmet Göksu - Özgür Çetinkaya     v1.0";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Top;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.ForeColor = Color.White;
            button3.Location = new Point(0, 136);
            button3.Name = "button3";
            button3.Size = new Size(219, 68);
            button3.TabIndex = 2;
            button3.Text = "Karantina";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Top;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.White;
            button2.Location = new Point(0, 68);
            button2.Name = "button2";
            button2.Size = new Size(219, 68);
            button2.TabIndex = 1;
            button2.Text = "Durum";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(32, 56, 90);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1207, 45);
            panel1.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(6, 10);
            label1.Name = "label1";
            label1.Size = new Size(128, 17);
            label1.TabIndex = 0;
            label1.Text = "OxMSEC Technology";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.Control;
            richTextBox1.Location = new Point(53, 215);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(903, 356);
            richTextBox1.TabIndex = 7;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged_1;
            // 
            // button5
            // 
            button5.Location = new Point(53, 19);
            button5.Name = "button5";
            button5.Size = new Size(442, 80);
            button5.TabIndex = 6;
            button5.Text = "Hızlı Tarama";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click_1;
            // 
            // button6
            // 
            button6.Location = new Point(53, 125);
            button6.Name = "button6";
            button6.Size = new Size(442, 91);
            button6.TabIndex = 8;
            button6.Text = "Tam Tarama";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(528, 19);
            button7.Name = "button7";
            button7.Size = new Size(428, 80);
            button7.TabIndex = 9;
            button7.Text = "Dosya Seç";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(528, 125);
            button8.Name = "button8";
            button8.Size = new Size(428, 91);
            button8.TabIndex = 10;
            button8.Text = "Klasör Seç";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button8);
            groupBox1.Controls.Add(button5);
            groupBox1.Controls.Add(button7);
            groupBox1.Controls.Add(button6);
            groupBox1.Location = new Point(219, 45);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(985, 623);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(richTextBox1);
            groupBox2.Location = new Point(219, 45);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(985, 617);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1207, 726);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Controls.Add(panelMenu);
            Controls.Add(panel1);
            Name = "Form3";
            Text = "Form3";
            Load += Form3_Load;
            panelMenu.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panelMenu;
        private Button button4;
        private Button button3;
        private Button button2;
        private Panel panel1;
        private Label label1;
        private RichTextBox richTextBox1;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
    }
}
namespace VoiceMeeterControlSuite
{
    partial class VoiceMeeterControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VoiceMeeterControl));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 100);
            this.button1.TabIndex = 0;
            this.button1.Text = "Wired Headphones";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.WiredHeadphoneButton);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(218, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(200, 100);
            this.button2.TabIndex = 1;
            this.button2.Text = "Soundbar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SoundbarButton);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(424, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(200, 100);
            this.button3.TabIndex = 2;
            this.button3.Text = "Wireless Headset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.WirelessHeadsetButton);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(141, 118);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(333, 139);
            this.button4.TabIndex = 3;
            this.button4.Text = "Mute Mic";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.MuteMicButton);
            // 
            // TrayIcon
            // 
            this.TrayIcon.Text = "Voice Meeter Control";
            this.TrayIcon.Visible = true;
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Icon_MouseDoubleClick);
            // 
            // VoiceMeeterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(640, 266);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VoiceMeeterControl";
            this.Text = "Voice Meeter Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VoiceMeeterControl_FormClosing);
            this.Resize += new System.EventHandler(this.ResizeForm);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.NotifyIcon TrayIcon;
    }
}


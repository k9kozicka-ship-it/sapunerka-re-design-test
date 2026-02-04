namespace testtest
{
    partial class NormalUser
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
            this.UserNameLbl = new Krypton.Toolkit.KryptonLabel();
            this.Floor_label = new Krypton.Toolkit.KryptonLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mainPanel = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // UserNameLbl
            // 
            this.UserNameLbl.Location = new System.Drawing.Point(485, 11);
            this.UserNameLbl.Name = "UserNameLbl";
            this.UserNameLbl.Size = new System.Drawing.Size(133, 28);
            this.UserNameLbl.StateCommon.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameLbl.TabIndex = 1;
            this.UserNameLbl.Values.Text = "Username";
            // 
            // Floor_label
            // 
            this.Floor_label.Location = new System.Drawing.Point(0, 176);
            this.Floor_label.Name = "Floor_label";
            this.Floor_label.Size = new System.Drawing.Size(99, 31);
            this.Floor_label.StateCommon.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Floor_label.TabIndex = 2;
            this.Floor_label.Values.Text = "Floor 1:";
            this.Floor_label.Click += new System.EventHandler(this.kryptonLabel2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1067, 554);
            this.mainPanel.TabIndex = 4;
            // 
            // NormalUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.Floor_label);
            this.Controls.Add(this.UserNameLbl);
            this.Controls.Add(this.mainPanel);
            this.Name = "NormalUser";
            this.Text = "NormalUser";
            this.Load += new System.EventHandler(this.NormalUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonLabel UserNameLbl;
        private Krypton.Toolkit.KryptonLabel Floor_label;
        private System.Windows.Forms.Timer timer1;
        private Krypton.Toolkit.KryptonPanel mainPanel;
    }
}
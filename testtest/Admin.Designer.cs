namespace testtest
{
    partial class Admin
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
            this.btnManageDatabase = new Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // btnManageDatabase
            // 
            this.btnManageDatabase.Location = new System.Drawing.Point(440, 447);
            this.btnManageDatabase.Name = "btnManageDatabase";
            this.btnManageDatabase.Size = new System.Drawing.Size(161, 92);
            this.btnManageDatabase.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageDatabase.TabIndex = 1;
            this.btnManageDatabase.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnManageDatabase.Values.Text = "Manage";
            this.btnManageDatabase.Click += new System.EventHandler(this.btnManageDatabase_Click_1);
            // 
            // Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btnManageDatabase);
            this.Name = "Admin";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnManageDatabase;
    }
}
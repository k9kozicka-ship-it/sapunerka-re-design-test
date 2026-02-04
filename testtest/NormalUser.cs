using Krypton.Toolkit;
using MySqlConnector;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace testtest
{
    public partial class NormalUser : KryptonForm
    {
        private MySqlConnection conn;
        private string connectionString = "Server=127.0.0.1;Port=3307;User=root;Password=1234;Database=sapunerkdb;SslMode=None;";
        private int corX = 80;
        private int corY = 150;

        public NormalUser()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void NormalUser_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
                timer1.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var cmd = new MySqlCommand("SELECT * FROM sapunerki WHERE User = @u", conn))
                {
                    cmd.Parameters.AddWithValue("@u", UserNameLbl.Text);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("Id");
                            float dist = reader.GetFloat("Distance");
                            int val = (int)Math.Max(0, Math.Min(100, ((9.5 - dist) / 8) * 100));

                            string pName = "p" + id;
                            string lName = "d" + id;

                            KryptonProgressBar p = mainPanel.Controls.Find(pName, true).FirstOrDefault() as KryptonProgressBar;
                            KryptonLabel l = mainPanel.Controls.Find(lName, true).FirstOrDefault() as KryptonLabel;

                            if (p == null)
                            {
                                p = new KryptonProgressBar { Name = pName, Size = new Size(100, 20), Location = new Point(corX, corY) };
                                l = new KryptonLabel { Name = lName, Location = new Point(corX, corY - 30), LabelStyle = LabelStyle.NormalPanel };
                                l.StateCommon.ShortText.Color1 = Color.White;
                                mainPanel.Controls.Add(p);
                                mainPanel.Controls.Add(l);
                                corX += 120;
                            }
                            p.Value = val;
                            l.Values.Text = $"№{id} {val}%";
                        }
                    }
                }
            }
            catch { /* Handle closed reader */ }
        }

        public void SetLBLName(string n) { UserNameLbl.Text = n; }
        public void SetFloorLBL(int p) { Floor_label.Text = "Floor " + p + ":"; }



        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}

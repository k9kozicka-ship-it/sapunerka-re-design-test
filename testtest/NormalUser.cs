using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using MySqlConnector;

namespace testtest
{
    public partial class NormalUser : KryptonForm
    {
        double distance = 0.0;
        int id;
        MySqlCommand myCommand;
        MySqlConnection conn;
        string connectionString;
        bool isOn = false;
        string user;
        string pass;
        int corX = 80;
        int corY = 150;
        int corYlbl = 110;
        public NormalUser()
        {
            InitializeComponent();
        }

        private void NormalUser_Load(object sender, EventArgs e)
        {
            try
            {
                // set the correct values for your server, user, password and database name
                connectionString = "Server=127.0.0.1;Port=3307;User=root;Password=1234;Database=sapunerkdb;SslMode=None;";



                // create the connection
                conn = new MySqlConnection(connectionString);

                // open a connection
                conn.Open();

                // create a MySQL command and set the SQL statement
                myCommand = new MySqlCommand();
                myCommand.Connection = conn;
                myCommand.CommandText = @"SELECT * FROM sapunerki where User = @user;";
                myCommand.Parameters.AddWithValue("@user", UserNameLbl.Text);



                // close the connection when done
                //conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            timer1.Enabled = true;
        }

        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isOn)
            {
                isOn = true;
            }

            // execute the command and read the results
            using (var myReader = myCommand.ExecuteReader())
            {
                while (myReader.Read())
                {
                    distance = myReader.GetFloat("Distance");
                    id = myReader.GetInt32("Id");

                    double currentAmmount = 9.5 - distance;
                    double precentage = (currentAmmount / 8) * 100;
                    int value = (int)Math.Max(0, Math.Min(100, precentage));

                    // Create control names
                    string pName = "p" + id;
                    string lName = "d" + id;

                    // Try to find existing controls
                    ProgressBar p2 = this.Controls.Find(pName, true).FirstOrDefault() as ProgressBar;
                    Label lbl2 = this.Controls.Find(lName, true).FirstOrDefault() as Label;

                    if (p2 == null)
                    {
                        // Create new progress bar
                        p2 = new ProgressBar
                        {
                            Minimum = 0,
                            Maximum = 100,
                            Value = value,
                            Location = new Point(corX, corY),
                            Name = pName,
                            Size = new Size(80, 20)
                        };
                        this.Controls.Add(p2);

                        // Create label
                        lbl2 = new Label
                        {
                            Location = new Point(corX, corYlbl),
                            Name = lName,
                            Text = "№" + id + "   " + value + "%"
                        };
                        this.Controls.Add(lbl2);

                        corX += 100; // only move X when adding new one
                    }
                    else
                    {
                        // Just update existing
                        p2.Value = value;
                        lbl2.Text = "№" + id + "   " + value + "%";
                    }
                }
            }
        }

        public void SetLBLName(string username)
        {
            UserNameLbl.Text = username;
        }
        public void SetFloorLBL(int priority)
        {
            Floor_label.Text = "Floor " + priority.ToString() + ":";
        }
        public int GetPriority(int priority)
        {
            return priority;
        }
    }
}

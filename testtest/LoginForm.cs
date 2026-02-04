using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using Krypton.Toolkit;

namespace testtest
{
    public partial class LoginForm : KryptonForm
    {
        public string username;
        public string password;
        public int priority;

        public LoginForm()
        {
            InitializeComponent();
            priority = -1;
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            username = kryptonTextBox1.Text;
            password = kryptonTextBox2.Text;
            MySqlCommand myCommand;
            MySqlConnection conn;
            string connectionString;
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
                myCommand.CommandText = @"SELECT * FROM users where Password = @password and Username = @username;";
                myCommand.Parameters.AddWithValue("@password", password);
                myCommand.Parameters.AddWithValue("@username", username);

                // execute the command and read the results
                using (var myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        priority = myReader.GetInt32("Priority");
                    }
                    else
                    {
                        MessageBox.Show("please enter valid username and password.");
                    }
                }
                //MessageBox.Show(priority.ToString());

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            if (priority == 0)
            {
                Admin admin = new Admin();
                this.Hide();
                admin.ShowDialog();
                admin.Activate();
                this.Close();
            }
            else if (priority < 0 || priority > 3)
            {
                kryptonTextBox1.Text = string.Empty;
                kryptonTextBox2.Text = string.Empty;
            }
            else
            {
                NormalUser form = new NormalUser();
                form.SetLBLName(username);
                form.SetFloorLBL(priority);
                this.Hide();
                form.ShowDialog();
                form.Activate();
                this.Close();
            }
        }

        private void kryptonLabel1_Click(object sender, EventArgs e)
        {

        }

        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }

        private void kryptonTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}

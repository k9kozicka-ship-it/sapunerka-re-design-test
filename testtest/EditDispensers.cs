using Krypton.Toolkit;
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

namespace testtest
{
    public partial class EditDispensers : KryptonForm
    {
        private MySqlConnection conn;
        private string connectionString = "Server=127.0.0.1;Port=3307;User=root;Password=1234;Database=sapunerkdb;SslMode=None;";

        private DataTable dispenserDataTable;
        private DataTable userDataTable;
        public EditDispensers()
        {
            InitializeComponent();
        }

        private void EditDispensers_Load(object sender, EventArgs e)
        {
            this.Text = "Database Management";

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // Configure Dispenser Controls
            dispensersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dispensersGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dispensersGridView.MultiSelect = false;
            dispensersGridView.ReadOnly = true;
            dispensersGridView.AllowUserToAddRows = false;
            dispensersGridView.CellClick += dispensersGridView_CellClick;

            // Configure User Controls
            usersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            usersGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usersGridView.MultiSelect = false;
            usersGridView.ReadOnly = true;
            usersGridView.AllowUserToAddRows = false;
            usersGridView.CellClick += usersGridView_CellClick;

            // Load all data
            LoadDispenserData();
            LoadUserData();
            PopulateComboBoxes();
        }
        private void PopulateComboBoxes()
        {
            try
            {
                // Populate the User ComboBox
                comboBoxUser.DataSource = null;
                string userQuery = "SELECT Username FROM users";
                MySqlDataAdapter userAdapter = new MySqlDataAdapter(userQuery, conn);
                DataTable userNames = new DataTable();
                userAdapter.Fill(userNames);
                comboBoxUser.DataSource = userNames;
                comboBoxUser.DisplayMember = "Username";
                comboBoxUser.ValueMember = "Username";

                // Populate the Floor ComboBox
                comboBoxFloor.DataSource = null;
                string floorQuery = "SELECT Priority FROM users";
                MySqlDataAdapter floorAdapter = new MySqlDataAdapter(floorQuery, conn);
                DataTable floorPriorities = new DataTable();
                floorAdapter.Fill(floorPriorities);
                comboBoxFloor.DataSource = floorPriorities;
                comboBoxFloor.DisplayMember = "Priority";
                comboBoxFloor.ValueMember = "Priority";

                comboBoxUser.SelectedIndex = -1;
                comboBoxFloor.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to populate dropdown menus: " + ex.Message);
            }
        }

        // ====================================================================
        // DISPENSER MANAGEMENT LOGIC
        // ====================================================================

        private void LoadDispenserData()
        {
            try
            {
                string query = "SELECT Id, User, Distance, Floor FROM sapunerki";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                dispenserDataTable = new DataTable();
                adapter.Fill(dispenserDataTable);
                dispensersGridView.DataSource = dispenserDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load dispenser data: " + ex.Message);
            }
        }

        private void dispensersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {   
                DataGridViewRow row = dispensersGridView.Rows[e.RowIndex];
                textBoxId.Text = row.Cells["Id"].Value.ToString();
                comboBoxUser.SelectedValue = row.Cells["User"].Value.ToString();
                comboBoxFloor.SelectedValue = row.Cells["Floor"].Value;
            }
        }
        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxId.Text))
            {
                MessageBox.Show("Please select a dispenser to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Are you sure you want to delete this dispenser?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM sapunerki WHERE Id = @Id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", textBoxId.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Dispenser deleted successfully!", "Success");
                    LoadDispenserData();
                    ClearDispenserFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete dispenser: " + ex.Message, "Error");
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxUser.SelectedIndex == -1 || comboBoxFloor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a User, a Floor, and enter a Distance.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "INSERT INTO sapunerki (User, Distance, Floor) VALUES (@User, @Distance, @Floor)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@User", comboBoxUser.SelectedValue);
                cmd.Parameters.AddWithValue("@Distance", 6);
                cmd.Parameters.AddWithValue("@Floor", comboBoxFloor.SelectedValue);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Dispenser added successfully!", "Success");
                LoadDispenserData();
                ClearDispenserFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add dispenser: " + ex.Message, "Error");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxId.Text))
            {
                MessageBox.Show("Please select a dispenser from the grid to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "UPDATE sapunerki SET User = @User, Distance = @Distance, Floor = @Floor WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@User", comboBoxUser.SelectedValue);
                cmd.Parameters.AddWithValue("@Distance", 6);
                cmd.Parameters.AddWithValue("@Floor", comboBoxFloor.SelectedValue);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(textBoxId.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Dispenser updated successfully!", "Success");
                LoadDispenserData();
                ClearDispenserFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update dispenser: " + ex.Message, "Error");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearDispenserFields();
        }
        private void ClearDispenserFields()
        {
            textBoxId.Clear();
            comboBoxUser.SelectedIndex = -1;
            comboBoxFloor.SelectedIndex = -1;
            dispensersGridView.ClearSelection();
        }

        // ====================================================================
        // USER MANAGEMENT LOGIC
        // ====================================================================

        private void LoadUserData()
        {
            try
            {
                string query = "SELECT Username, Password, Priority FROM users";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                userDataTable = new DataTable();
                adapter.Fill(userDataTable);
                usersGridView.DataSource = userDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load user data: " + ex.Message, "Error");
            }
        }
        
        private void usersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = usersGridView.Rows[e.RowIndex];

                textboxUsername.Text = row.Cells["Username"].Value.ToString();
                textBoxPassword.Text = row.Cells["Password"].Value.ToString();
                textBoxPriority.Text = row.Cells["Priority"].Value.ToString();
            }
        }
        private void kryptonLabel4_Click(object sender, EventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textboxUsername.Text) || string.IsNullOrWhiteSpace(textBoxPassword.Text) || string.IsNullOrWhiteSpace(textBoxPriority.Text))
            {
                MessageBox.Show("Please fill in Username, Password, and Priority.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "INSERT INTO users (Username, Password, Priority) VALUES (@Username, @Password, @Priority)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", textboxUsername.Text);
                cmd.Parameters.AddWithValue("@Password", textBoxPassword.Text);
                cmd.Parameters.AddWithValue("@Priority", Convert.ToInt32(textBoxPriority.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("User added successfully!", "Success");
                LoadUserData();
                PopulateComboBoxes();
                ClearUserFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add user. Ensure Username and Priority are unique.\n\n" + ex.Message, "Error");
            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textboxUsername.Text))
            {
                MessageBox.Show("Please select a user to update.", "Selection Error");
                return;
            }

            try
            {
                string query = "UPDATE users SET Username = @NewUsername, Password = @Password, Priority = @Priority WHERE Username = @OldUsername";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NewUsername", textboxUsername.Text);
                cmd.Parameters.AddWithValue("@Password", textBoxPassword.Text);
                cmd.Parameters.AddWithValue("@Priority", Convert.ToInt32(textBoxPriority.Text));
                cmd.Parameters.AddWithValue("@OldUsername", textboxUsername.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("User updated successfully!", "Success");
                LoadUserData();
                PopulateComboBoxes();
                ClearUserFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update user. Ensure Username and Priority remain unique.\n\n" + ex.Message, "Error");
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textboxUsername.Text))
            {
                MessageBox.Show("Please select a user to delete.", "Selection Error");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this user?\nThis may affect dispensers assigned to them.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM users WHERE Username = @Username";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", textboxUsername.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("User deleted successfully.", "Success");
                    LoadUserData();
                    PopulateComboBoxes();
                    ClearUserFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete user. Make sure no dispensers are assigned to this user before deleting.\n\n" + ex.Message, "Error");
                }
            }
        }

        private void btnClearUserField_Click(object sender, EventArgs e)
        {
            ClearUserFields();
        }
        private void ClearUserFields()
        {

            textboxUsername.Clear();
            textBoxPassword.Clear();
            textBoxPriority.Clear();
            usersGridView.ClearSelection();
        }
        private void EditDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn?.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}

using Krypton.Toolkit;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace testtest
{
    public partial class Admin : KryptonForm
    {
        private MySqlConnection conn;
        private string connectionString = "Server=127.0.0.1;Port=3307;User=root;Password=1234;Database=sapunerkdb;SslMode=None;";
        private Timer refreshTimer;

        public Admin()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Dispenser System - Admin";
            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
                RefreshDispenserDisplay();
                refreshTimer = new Timer { Interval = 4000 };
                refreshTimer.Tick += (s, ev) => RefreshDispenserDisplay();
                refreshTimer.Start();
            }
            catch (Exception ex) { MessageBox.Show("DB Error: " + ex.Message); }
        }

        private void RefreshDispenserDisplay()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cmd = new MySqlCommand("SELECT Id, Distance, Floor FROM sapunerki ORDER BY Floor, Id", conn))
                using (var adapter = new MySqlDataAdapter(cmd)) { adapter.Fill(dt); }

                // Get REAL sensor data from ID 1
                double realDistance = 6.0;
                var realRow = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("Id") == 1);
                if (realRow != null) realDistance = Convert.ToDouble(realRow["Distance"]);

                var existingIds = mainPanel.Controls.OfType<KryptonProgressBar>().Select(p => p.Name.Substring(1)).ToHashSet();
                int currentFloor = -1, startX = 160, currentX = startX, currentY = 20;

                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    int floor = Convert.ToInt32(row["Floor"]);
                    double distToUse;

                    // REALISTIC FAKING LOGIC
                    if (id == 1) distToUse = realDistance;
                    else
                    {
                        Random rng = new Random(id); // Seed with ID for consistent offset
                        distToUse = realDistance + (rng.NextDouble() * 3.0 - 1.5);
                    }

                    existingIds.Remove(id.ToString());
                    if (floor != currentFloor)
                    {
                        currentFloor = floor; currentY += 90; currentX = startX;
                        AddFloorHeader(floor, currentY);
                    }

                    int percent = (int)Math.Max(0, Math.Min(100, ((9.5 - distToUse) / 8) * 100));
                    UpdateDispenserUI(id, percent, new Point(currentX, currentY + 25));
                    currentX += 220;
                }
                foreach (string id in existingIds) { RemoveControl("p" + id); RemoveControl("l" + id); }
            }
            catch { }
        }

        private void AddFloorHeader(int floor, int y)
        {
            string name = "floorLabel" + floor;
            if (!mainPanel.Controls.ContainsKey(name))
            {
                var lbl = new KryptonLabel { Name = name, Text = $"FLOOR {floor}", Location = new Point(20, y + 15), LabelStyle = LabelStyle.TitleControl, AutoSize = true };
                lbl.StateCommon.ShortText.Color1 = Color.White;
                mainPanel.Controls.Add(lbl);
            }
        }

        private void UpdateDispenserUI(int id, int val, Point loc)
        {
            string pName = "p" + id, lName = "l" + id;
            KryptonProgressBar bar = mainPanel.Controls[pName] as KryptonProgressBar;
            if (bar == null) { bar = new KryptonProgressBar { Name = pName, Size = new Size(180, 26) }; mainPanel.Controls.Add(bar); }
            bar.Location = loc; bar.Value = val;

            KryptonLabel lbl = mainPanel.Controls[lName] as KryptonLabel;
            if (lbl == null)
            {
                lbl = new KryptonLabel { Name = lName, AutoSize = true, LabelStyle = LabelStyle.NormalPanel };
                lbl.StateCommon.ShortText.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lbl.StateCommon.ShortText.Color1 = Color.White;
                mainPanel.Controls.Add(lbl);
            }
            lbl.Location = new Point(loc.X, loc.Y - 28);
            lbl.Values.Text = $"Dispenser №{id} - {val}%";
        }

        private void RemoveControl(string name)
        {
            if (mainPanel.Controls.ContainsKey(name)) { var c = mainPanel.Controls[name]; mainPanel.Controls.Remove(c); c.Dispose(); }
        }

        private void btnManageDatabase_Click_1(object sender, EventArgs e)
        {
            refreshTimer.Stop();
            new EditDispensers().ShowDialog();
            refreshTimer.Start();
        }
    }
}
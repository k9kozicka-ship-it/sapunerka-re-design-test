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
        private string connectionString =
            "Server=127.0.0.1;Port=3307;User=root;Password=1234;Database=sapunerkdb;SslMode=None;";

        private Timer refreshTimer;

        private class Dispenser
        {
            public int Id { get; set; }
            public int Floor { get; set; }
            public double Distance { get; set; }
        }

        public Admin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Text = "Admin Dashboard";

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error:\n" + ex.Message);
                Close();
                return;
            }

            RefreshDispenserDisplay();

            refreshTimer = new Timer { Interval = 5000 };
            refreshTimer.Tick += (s, ev) => RefreshDispenserDisplay();
            refreshTimer.Start();
        }

        private void RefreshDispenserDisplay()
        {
            var dispensers = new List<Dispenser>();

            try
            {
                using (var cmd = new MySqlCommand(
                    "SELECT Id, Distance, Floor FROM sapunerki ORDER BY Floor, Id", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dispensers.Add(new Dispenser
                        {
                            Id = reader.GetInt32("Id"),
                            Distance = reader.GetDouble("Distance"),
                            Floor = reader.GetInt32("Floor")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                refreshTimer.Stop();
                MessageBox.Show(ex.Message);
                return;
            }

            var existingIds = Controls
                .OfType<KryptonProgressBar>()
                .Select(p => p.Name.Substring(1))
                .ToHashSet();

            int currentFloor = -1;
            int startX = 160;
            int currentX = startX;
            int currentY = 40;

            foreach (var d in dispensers)
            {
                existingIds.Remove(d.Id.ToString());

                if (d.Floor != currentFloor)
                {
                    currentFloor = d.Floor;
                    currentY += 90;
                    currentX = startX;

                    string floorLabelName = $"floorLabel{currentFloor}";
                    if (!Controls.ContainsKey(floorLabelName))
                    {
                        Controls.Add(new Label
                        {
                            Name = floorLabelName,
                            Text = $"Floor {currentFloor}",
                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                            AutoSize = true,
                            Location = new Point(20, currentY),
                            ForeColor = Color.White,
                            BackColor = Color.Transparent
                        });
                    }
                }

                double maxDistance = 9.5;
                double minDistance = 1.5;

                double ratio =
                    (maxDistance - d.Distance) / (maxDistance - minDistance);

                int percent = (int)Math.Round(ratio * 100);
                percent = Math.Max(0, Math.Min(100, percent));

                UpdateProgressBarAndLabel(
                    d.Id,
                    percent,
                    new Point(currentX, currentY + 25)
                );

                currentX += 220;
            }

            foreach (string id in existingIds)
            {
                RemoveControl("p" + id);
                RemoveControl("l" + id);
            }
        }

        private void UpdateProgressBarAndLabel(int id, int value, Point location)
        {
            string pName = "p" + id;
            KryptonProgressBar bar = Controls[pName] as KryptonProgressBar;

            if (bar == null)
            {
                bar = new KryptonProgressBar
                {
                    Name = pName,
                    Minimum = 0,
                    Maximum = 100,
                    Size = new Size(180, 24)
                };

                Controls.Add(bar);
                bar.BringToFront();
            }

            bar.Value = Math.Max(1, value);
            bar.Location = location;

            string lName = "l" + id;
            Label lbl = Controls[lName] as Label;

            if (lbl == null)
            {
                
                lbl = new Label
                {
                    Name = lName,
                    AutoSize = true,
                    BackColor = System.Drawing.Color.Transparent,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Location = new Point(location.X, location.Y - 22)
                };
                

                Controls.Add(lbl);
                lbl.BringToFront();
            }

            lbl.Text = $"№{id}   {value}%";
        }

        private void RemoveControl(string name)
        {
            if (Controls[name] != null)
            {
                Controls[name].Dispose();
                Controls.RemoveByKey(name);
            }
        }

        private void Admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            if (conn?.State == ConnectionState.Open)
                conn.Close();
        }

        private void btnManageDatabase_Click_1(object sender, EventArgs e)
        {
            refreshTimer.Stop();
            new EditDispensers().ShowDialog();
            RefreshDispenserDisplay();
            refreshTimer.Start();
        }

      
    }
}

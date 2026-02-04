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

        public Admin() { InitializeComponent(); this.DoubleBuffered = true; }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
                RefreshDispenserDisplay();
                refreshTimer = new Timer { Interval = 4000 };
                refreshTimer.Tick += (s, ev) => RefreshDispenserDisplay();
                refreshTimer.Start();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void RefreshDispenserDisplay()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cmd = new MySqlCommand("SELECT Id, Distance, Floor FROM sapunerki ORDER BY Floor, Id", conn))
                using (var adp = new MySqlDataAdapter(cmd)) { adp.Fill(dt); }

                double realDist = 6.0;
                var realRow = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("Id") == 1);
                if (realRow != null) realDist = Convert.ToDouble(realRow["Distance"]);

                var existingIds = mainPanel.Controls.OfType<KryptonProgressBar>().Select(p => p.Name.Substring(1)).ToHashSet();
                int currentFloor = -1, startX = 160, currentX = startX, currentY = 20;

                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    int floor = Convert.ToInt32(row["Floor"]);
                    double dist = (id == 1) ? realDist : realDist + (new Random(id).NextDouble() * 3.0 - 1.5);

                    existingIds.Remove(id.ToString());
                    if (floor != currentFloor)
                    {
                        currentFloor = floor; currentY += 90; currentX = startX;
                        AddFloorHeader(floor, currentY);
                    }

                    int percent = (int)Math.Max(0, Math.Min(100, ((9.5 - dist) / 8) * 100));
                    UpdateUI(id, percent, new Point(currentX, currentY + 25));
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
                var l = new KryptonLabel { Name = name, Text = $"FLOOR {floor}", Location = new Point(20, y + 15), LabelStyle = LabelStyle.TitleControl, AutoSize = true };
                l.StateCommon.ShortText.Color1 = Color.White;
                mainPanel.Controls.Add(l);
            }
        }

        private void UpdateUI(int id, int val, Point loc)
        {
            string pName = "p" + id, lName = "l" + id;
            KryptonProgressBar p = mainPanel.Controls[pName] as KryptonProgressBar;

            if (p == null)
            {
                p = new KryptonProgressBar
                {
                    Name = pName,
                    Size = new Size(180, 26),
                    Minimum = 0,
                    Maximum = 100
                };

                // --- SUBTLE ROUNDING (3) ---
                p.StateCommon.Border.DrawBorders = PaletteDrawBorders.All;
                p.StateCommon.Border.Rounding = 3;
                p.StateCommon.Border.Color1 = Color.White;

                mainPanel.Controls.Add(p);
            }
            p.Location = loc;

            // --- SMOOTH SLIDING ANIMATION ---
            // This loop moves the bar 1% at a time until it hits the target
            if (p.Value != val)
            {
                int startValue = p.Value;
                int endValue = val;

                // Move towards the target value
                if (startValue < endValue)
                {
                    for (int i = startValue; i <= endValue; i++)
                    {
                        p.Value = i;
                        p.Refresh(); // Forces the bar to redraw immediately
                        System.Threading.Thread.Sleep(5); // 5ms delay per 1% movement
                    }
                }
                else
                {
                    for (int i = startValue; i >= endValue; i--)
                    {
                        p.Value = i;
                        p.Refresh();
                        System.Threading.Thread.Sleep(5);
                    }
                }
            }

            KryptonLabel l = mainPanel.Controls[lName] as KryptonLabel;
            if (l == null)
            {
                l = new KryptonLabel { Name = lName, AutoSize = true, LabelStyle = LabelStyle.NormalPanel };
                l.StateCommon.ShortText.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                l.StateCommon.ShortText.Color1 = Color.White;
                mainPanel.Controls.Add(l);
            }
            l.Location = new Point(loc.X, loc.Y - 28);
            l.Values.Text = $"№{id} - {val}%";
        }

        private void RemoveControl(string name)
        {
            if (mainPanel.Controls.ContainsKey(name)) { var c = mainPanel.Controls[name]; mainPanel.Controls.Remove(c); c.Dispose(); }
        }

        private void btnManageDatabase_Click_1(object sender, EventArgs e)
        {
            refreshTimer.Stop(); new EditDispensers().ShowDialog(); refreshTimer.Start();
        }
    }
}
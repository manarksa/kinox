using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Movie_Management_System.Login;

namespace Movie_Management_System.Forms
{
    public partial class ProfileForm : Form
    {
        public ProfileForm()
        {
            InitializeComponent();
            BuildUI();
        }
        private void BuildUI()
        {
            Text = "My Profile";
            Size = new Size(520, 420);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(10, 20, 70);

            string name = "", email = "";
            int count = 0;

            string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Nada\OneDrive\Desktop\MovieApp\Movie Management System\Movie Management System\MovieDB.mdf"";Integrated Security=True;Connect Timeout=30";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                SqlCommand c1 = new SqlCommand("SELECT Name, Email FROM Users WHERE Id=@id", con);
                c1.Parameters.AddWithValue("@id", Session.UserId);
                using (SqlDataReader dr = c1.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        name = dr["Name"].ToString();
                        email = dr["Email"].ToString();
                    }
                }

                SqlCommand c2 = new SqlCommand("SELECT COUNT(*) FROM Watchlist WHERE UserId=@id", con);
                c2.Parameters.AddWithValue("@id", Session.UserId);
                count = (int)c2.ExecuteScalar();
            }

            AddLabel("My Profile", 28, 30, 20, true, Color.FromArgb(0, 200, 240));
            AddLabel("Name", 11, 40, 100, false, Color.FromArgb(140, 160, 190));
            AddLabel(name, 16, 40, 122, true, Color.White);
            AddLabel("Email", 11, 40, 180, false, Color.FromArgb(140, 160, 190));
            AddLabel(email, 16, 40, 202, true, Color.White);
            AddLabel("Watchlist", 11, 40, 260, false, Color.FromArgb(140, 160, 190));
            AddLabel(count + (count == 1 ? " movie" : " movies"), 16, 40, 282, true, Color.White);

            Button close = new Button();
            close.Text = "Close";
            close.Size = new Size(120, 40);
            close.Location = new Point(360, 320);
            close.FlatStyle = FlatStyle.Flat;
            close.FlatAppearance.BorderSize = 0;
            close.BackColor = Color.FromArgb(100, 220, 230);
            close.ForeColor = Color.Black;
            close.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            close.Cursor = Cursors.Hand;
            close.Click += (s, e) => Close();
            Controls.Add(close);
        }

        private void AddLabel(string text, float size, int x, int y, bool bold, Color color)
        {
            Label l = new Label();
            l.Text = text;
            l.AutoSize = true;
            l.ForeColor = color;
            l.BackColor = Color.Transparent;
            l.Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
            l.Location = new Point(x, y);
            Controls.Add(l);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush br = new LinearGradientBrush(
                ClientRectangle, Color.FromArgb(10, 20, 70), Color.FromArgb(60, 110, 200), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }
        }
    }
}
  

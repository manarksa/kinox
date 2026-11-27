using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static Movie_Management_System.Login;

namespace Movie_Management_System.Forms
{
    public partial class SettingsForm : Form
    {
        private TextBox txtOld, txtNew, txtConfirm;

        private static string ConStr {get {return Db.ConStr;} }
        // يرجع true لو اليوزر حذف حسابه (Form1 يستخدمها عشان يرجع لصفحة Login)
        public bool AccountDeleted = false;

        public SettingsForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Settings";
            Size = new Size(520, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(10, 20, 70);

            AddLabel("Settings", 28, 30, 20, true, Color.FromArgb(0, 200, 240));
            AddLabel("Change Password", 14, 40, 90, true, Color.White);

            AddLabel("Old password", 10, 40, 135, false, Color.FromArgb(140, 160, 190));
            txtOld = AddBox(40, 158);
            AddLabel("New password", 10, 40, 200, false, Color.FromArgb(140, 160, 190));
            txtNew = AddBox(40, 223);
            AddLabel("Confirm new password", 10, 40, 265, false, Color.FromArgb(140, 160, 190));
            txtConfirm = AddBox(40, 288);

            Button change = MakeButton("Change Password", 40, 340, 200, Color.FromArgb(100, 220, 230), Color.Black);
            change.Click += btnChange_Click;
            Controls.Add(change);

            Panel line = new Panel();
            line.BackColor = Color.FromArgb(25, 55, 100);
            line.SetBounds(40, 400, 420, 1);
            Controls.Add(line);

            AddLabel("Danger Zone", 14, 40, 415, true, Color.FromArgb(255, 110, 110));
            Button del = MakeButton("Delete Account", 40, 455, 200, Color.FromArgb(200, 50, 50), Color.White);
            del.Click += btnDelete_Click;
            Controls.Add(del);

            Button close = MakeButton("Close", 340, 455, 120, Color.FromArgb(14, 30, 65), Color.White);
            close.Click += (s, e) => Close();
            Controls.Add(close);
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (txtOld.Text == "" || txtNew.Text == "" || txtConfirm.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("New passwords do not match.");
                return;
            }

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();

                SqlCommand check = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE Id=@id AND Password=@p", con);
                check.Parameters.AddWithValue("@id", Session.UserId);
                check.Parameters.AddWithValue("@p", Login.Hash(txtOld.Text));
                if ((int)check.ExecuteScalar() == 0)
                {
                    MessageBox.Show("Old password is incorrect.");
                    return;
                }

                SqlCommand upd = new SqlCommand(
                    "UPDATE Users SET Password=@p WHERE Id=@id", con);
                upd.Parameters.AddWithValue("@p", Login.Hash(txtNew.Text));
                upd.Parameters.AddWithValue("@id", Session.UserId);
                upd.ExecuteNonQuery();
            }

            MessageBox.Show("Password changed successfully.");
            txtOld.Text = txtNew.Text = txtConfirm.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will permanently delete your account and watchlist. Continue?",
                "Delete Account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();

                SqlCommand c1 = new SqlCommand("DELETE FROM Watchlist WHERE UserId=@id", con);
                c1.Parameters.AddWithValue("@id", Session.UserId);
                c1.ExecuteNonQuery();

                SqlCommand c2 = new SqlCommand("DELETE FROM Users WHERE Id=@id", con);
                c2.Parameters.AddWithValue("@id", Session.UserId);
                c2.ExecuteNonQuery();
            }

            AccountDeleted = true;
            MessageBox.Show("Your account has been deleted.");
            Close();
        }

        // ===== أدوات مساعدة =====
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

        private TextBox AddBox(int x, int y)
        {
            TextBox t = new TextBox();
            t.Location = new Point(x, y);
            t.Width = 420;
            t.Font = new Font("Segoe UI", 12F);
            t.BackColor = Color.White;
            t.ForeColor = Color.Black;
            t.UseSystemPasswordChar = true;
            Controls.Add(t);
            return t;
        }

        private Button MakeButton(string text, int x, int y, int w, Color back, Color fore)
        {
            Button b = new Button();
            b.Text = text;
            b.Size = new Size(w, 40);
            b.Location = new Point(x, y);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = back;
            b.ForeColor = fore;
            b.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            return b;
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
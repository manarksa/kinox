using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using System.Security.Cryptography;
using System.Text;

namespace Movie_Management_System
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            SignUp signup = new SignUp();
            signup.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string conStr = Db.ConStr;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Id , Name FROM Users WHERE Email=@e AND Password=@p", con);
                cmd.Parameters.AddWithValue("@e", textBox1.Text);
                cmd.Parameters.AddWithValue("@p", textBox2.Text);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        Session.UserId = (int)dr["Id"];
                        Session.UserName = dr["Name"].ToString();

                        Form1 main = new Form1();
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password");
                    }
                }
            }
        }
        public static string Hash(string text)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] b = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(b).Replace("-", "");
            }
        }
                public static class Session
        {
            public static int UserId;
            public static string UserName = "";
        }
    }
}
    


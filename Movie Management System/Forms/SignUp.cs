using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Movie_Management_System
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
           
            if (textFullName.Text == "" || textEmail.Text == "" || textPassword.Text == "")
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }
            if (textPassword.Text != textConfirm.Text)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Nada\OneDrive\Desktop\MovieApp\Movie Management System\Movie Management System\MovieDB.mdf"";Integrated Security=True;Connect Timeout=30";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email=@e", con);
                check.Parameters.AddWithValue("@e", textEmail.Text.Trim());
                if ((int)check.ExecuteScalar() > 0)
                {
                    MessageBox.Show("This email is already registered");
                    return;
                }
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Users (Name, Email, Password) VALUES (@n, @e, @p)", con);
                cmd.Parameters.AddWithValue("@n", textFullName.Text);
                cmd.Parameters.AddWithValue("@e", textEmail.Text);
                cmd.Parameters.AddWithValue("@p", textPassword.Text);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Account created successfully");
            this.Close();
        }
    }
    }


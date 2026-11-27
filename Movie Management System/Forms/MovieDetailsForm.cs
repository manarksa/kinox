using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Movie_Management_System.Login;

namespace Movie_Management_System
{
    public partial class MovieDetailsForm : Form
    {
        private static string ConStr { get { return Db.ConStr; } }
        private Movie movie;

        public MovieDetailsForm(Movie m)
        {
            InitializeComponent();
            movie = m;

            lblCategory.Text = m.Category;
            lblTitle.Text = m.Title;
            lblYear.Text = "Released " + m.Year;
            lblCast.Text = m.Cast;
            lblStory.Text = m.Story;

            string path = Path.Combine(Application.StartupPath, "Posters", m.Poster);
            if (File.Exists(path))
                picPoster.Image = Image.FromFile(path);

            UpdateWatchlistButton();
        }

        // هل الفيلم موجود بواتش ليست اليوزر الحالي؟ (من الداتابيس)
        public static bool IsInWatchlist(int movieId)
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Watchlist WHERE UserId=@u AND MovieId=@m", con);
                cmd.Parameters.AddWithValue("@u", Session.UserId);
                cmd.Parameters.AddWithValue("@m", movieId);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private void UpdateWatchlistButton()
        {
            if (IsInWatchlist(movie.Id))
            {
                btnWatchlist.Text = "Remove from Watchlist";
                btnWatchlist.BackColor = Color.Gray;
            }
            else
            {
                btnWatchlist.Text = "Add to Watchlist";
                btnWatchlist.Enabled = true;
            }
        }

        private void ToggleWatchlist()
        {
            if (Session.UserId == 0)
            {
                MessageBox.Show("Please log in first.");
                return;
            }

            bool exists = IsInWatchlist(movie.Id);

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd;
                if (exists)
                    cmd = new SqlCommand("DELETE FROM Watchlist WHERE UserId=@u AND MovieId=@m", con);
                else
                    cmd = new SqlCommand("INSERT INTO Watchlist (UserId, MovieId) VALUES (@u, @m)", con);

                cmd.Parameters.AddWithValue("@u", Session.UserId);
                cmd.Parameters.AddWithValue("@m", movie.Id);
                cmd.ExecuteNonQuery();
            }
            UpdateWatchlistButton();
        }

        // الاثنين مربوطين بالـ Designer، فنخليهم يسوون نفس الشي
        private void btnWatchlist_Click(object sender, EventArgs e)
        {
            ToggleWatchlist();
        }

        private void btnWatchlist_Click_1(object sender, EventArgs e)
        {
            ToggleWatchlist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTrailer_Click(object sender, EventArgs e)
        {
            string url = !string.IsNullOrWhiteSpace(movie.TrailerUrl)
                ? movie.TrailerUrl
                : "https://www.youtube.com/results?search_query=" +
                  Uri.EscapeDataString(movie.Title + " " + movie.Year.Trim() + " official trailer");

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void MovieDetailsForm_Load(object sender, EventArgs e)
        {
        }
    }
}
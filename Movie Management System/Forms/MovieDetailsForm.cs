using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Movie_Management_System
{
    public partial class MovieDetailsForm : Form
    {
        public static List<Movie> Watchlist = new List<Movie>();
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
        private void UpdateWatchlistButton()
        {
            bool isExist = MovieDetailsForm.Watchlist.Any(m => m.Title == movie.Title);

            if (isExist)
            {
                btnWatchlist.Text = "Already in Watchlist";
                btnWatchlist.BackColor = Color.Gray;
            }
            else
            {
                btnWatchlist.Text = "Add to Watchlist";
                btnWatchlist.Enabled = true;
            }
        }

        private void btnWatchlist_Click(object sender, EventArgs e)
        {
            if (Watchlist.Contains(movie))
                Watchlist.Remove(movie);
            else
                Watchlist.Add(movie);

            UpdateWatchlistButton();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnWatchlist_Click_1(object sender, EventArgs e)
        {
            var existingMovie = MovieDetailsForm.Watchlist.FirstOrDefault(m => m.Title == movie.Title);

            if (existingMovie != null)
            {
                MovieDetailsForm.Watchlist.Remove(existingMovie);
            }
            else
            {
                MovieDetailsForm.Watchlist.Add(movie);
            }

            UpdateWatchlistButton();
        }
        }
}

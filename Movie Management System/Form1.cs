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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadMovies();
        }

        private void LoadMovies()
        {
            List<Movie> action = new List<Movie>();

            action.Add(new Movie
            {
                Title = "Mad Max: Fury Road",
                Year = "2015",
                Story = "A chase across the desert to escape a tyrant.",
                Cast = "Tom Hardy, Charlize Theron",
                Poster = "madmax.jpg"
            });

            action.Add(new Movie
            {
                Title = "John Wick",
                Year = "2014",
                Story = "A retired hitman returns for revenge.",
                Cast = "Keanu Reeves",
                Poster = "johnwick.jpg"
            });

            action.Add(new Movie
            {
                Title = "Die Hard",
                Year = "1988",
                Story = "A cop fights terrorists inside a skyscraper.",
                Cast = "Bruce Willis, Alan Rickman",
                Poster = "diehard.jpg"
            });

            action.Add(new Movie
            {
                Title = "The Dark Knight",
                Year = "2008",
                Story = "Batman faces the Joker in Gotham City.",
                Cast = "Christian Bale, Heath Ledger",
                Poster = "darkknight.jpg"
            });

            action.Add(new Movie
            {
                Title = "Gladiator",
                Year = "2000",
                Story = "A betrayed general becomes a gladiator.",
                Cast = "Russell Crowe, Joaquin Phoenix",
                Poster = "gladiator.jpg"
            });

            AddMovies(flowAction, action,"Action");
        }

        private void AddMovies(FlowLayoutPanel flow, List<Movie> movies, string category)
        {
            foreach (Movie m in movies)
            {
                m.Category = category;
                PictureBox pic = new PictureBox();
                pic.Size = new Size(180, 260);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.BackColor = Color.FromArgb(5, 12, 35);
                pic.Cursor = Cursors.Hand;
                pic.Tag = m;   // نخزن بيانات الفيلم داخل الصورة

                string path = Path.Combine(Application.StartupPath, "Posters", m.Poster);
                if (File.Exists(path))
                    pic.Image = Image.FromFile(path);

                pic.Click += Poster_Click;
                flow.Controls.Add(pic);
            }
        }

        private void Poster_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            Movie m = (Movie)pic.Tag;
            MovieDetailsForm details = new MovieDetailsForm(m);
            details.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            l.Show();
            this.Hide();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            SignUp s = new SignUp();
            s.Show();
            this.Hide();
        }

        private void btnWatchlistTab_Click(object sender, EventArgs e)
        {
            flowAction.Controls.Clear();
            AddMovies(flowAction, MovieDetailsForm.Watchlist, "Watchlist");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flowAction.Controls.Clear();
            LoadMovies();
        }
    }
}

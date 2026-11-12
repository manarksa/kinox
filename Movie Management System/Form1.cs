using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace Movie_Management_System
{
    public partial class Form1 : Form
    {
        // =====================================================
        //  الألوان والمقاسات
        // =====================================================
        private static readonly Color Accent = Color.FromArgb(0, 200, 240);
        private static readonly Color BgCard = Color.FromArgb(5, 12, 35);
        private static readonly Color BgSide = Color.FromArgb(6, 14, 34);
        private static readonly Color BgItem = Color.FromArgb(14, 30, 65);

        private const int PosterW = 180;
        private const int PosterH = 260;
        private const int CardPad = 3;                       // سماكة الإطار عند Hover
        private const int CardW = PosterW + CardPad * 2;
        private const int CardH = PosterH + 40 + CardPad * 2;
        private const int Gap = 20;
        private const int PerRow = 5;                        // عدد الأفلام بالصف

        // =====================================================
        //  البيانات
        // =====================================================
        private readonly List<KeyValuePair<string, List<Movie>>> allSections =
            new List<KeyValuePair<string, List<Movie>>>();

        // =====================================================
        //  عناصر الواجهة الجديدة
        // =====================================================
        private Panel scrollHost;           // يحتوي المحتوى ويسوي Scroll
        private Panel content;              // المحتوى الفعلي (يتوسط)
        private Button btnMenu;             // زر ☰
        private Panel sidePanel;            // القائمة الجانبية
        private FlowLayoutPanel flowMenu;
        private FlowLayoutPanel catPanel;   // التصنيفات (تظهر عند الضغط على Movies)
        private Timer slideTimer;
        private bool sideOpen = false;
        private const int SideWidth = 270;

        public Form1()
        {
            InitializeComponent();

            Directory.CreateDirectory(PostersFolder);

            BuildLayout();
            BuildMenuButton();      // يخفي أزرار الهيدر القديمة ويربط اللوقو بالرئيسية
            BuildSidePanel();
            LoadMovies();
        }

        private static string PostersFolder
        {
            get { return Path.Combine(Application.StartupPath, "Posters"); }
        }

        // =====================================================
        //  بناء منطقة العرض (الصور بالسنتر)
        // =====================================================
        private void BuildLayout()
        {
            // نخفي القسم الأصلي (نعرض كل شي ديناميكياً بدلاً منه)
            flowAction.Visible = false;
            lblAction.Visible = false;

            scrollHost = new Panel();
            scrollHost.Dock = DockStyle.Fill;
            scrollHost.AutoScroll = true;
            scrollHost.BackColor = panelBody.BackColor;
            scrollHost.MouseEnter += (s, e) => scrollHost.Focus();   // عشان عجلة الماوس تشتغل

            content = new Panel();
            content.BackColor = panelBody.BackColor;
            content.Width = PerRow * CardW + (PerRow - 1) * Gap;
            scrollHost.Controls.Add(content);

            panelBody.Controls.Add(scrollHost);
            scrollHost.BringToFront();

            scrollHost.Resize += (s, e) => CenterContent();
        }

        private void CenterContent()
        {
            int left = (scrollHost.ClientSize.Width - content.Width) / 2;
            if (left < 10) left = 10;
            content.Left = left;
        }

        // =====================================================
        //  زر الثلاث شرطات ☰ (يمين الهيدر)
        //  + إخفاء أزرار الهيدر القديمة (All Movies / Watchlist / Login / Sign Up)
        //  + اللوقو يرجّع للصفحة الرئيسية
        // =====================================================
        private void BuildMenuButton()
        {
            btnMenu = new Button();
            btnMenu.Text = "☰";
            btnMenu.Size = new Size(56, 46);
            btnMenu.FlatStyle = FlatStyle.Flat;
            btnMenu.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            btnMenu.ForeColor = Accent;
            btnMenu.BackColor = panelHeader.BackColor;
            btnMenu.Cursor = Cursors.Hand;
            btnMenu.FlatAppearance.BorderColor = Accent;
            btnMenu.FlatAppearance.BorderSize = 2;
            btnMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(15, 40, 80);
            btnMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMenu.Location = new Point(panelHeader.Width - btnMenu.Width - 25,
                                         (panelHeader.Height - btnMenu.Height) / 2);
            btnMenu.Click += (s, e) => ToggleSidePanel();

            // نخفي كل الأزرار القديمة بالهيدر (ما نحذفها عشان الـ Designer مربوط فيها)
            foreach (Control c in panelHeader.Controls)
            {
                if (c is Button)
                    c.Visible = false;
            }

            // اللوقو: أي PictureBox داخل الهيدر يوديك للرئيسية
            foreach (Control c in panelHeader.Controls)
            {
                PictureBox logo = c as PictureBox;
                if (logo != null)
                {
                    logo.Cursor = Cursors.Hand;
                    logo.Click += (s, e) => GoHome();
                }
            }

            panelHeader.Controls.Add(btnMenu);
            btnMenu.BringToFront();
        }

        // يرجع للصفحة الرئيسية: كل الأفلام + يقفل القائمة لو مفتوحة
        private void GoHome()
        {
            ShowAll();
            if (sideOpen) ToggleSidePanel();
        }

        // =====================================================
        //  القائمة الجانبية:
        //  Profile > Settings > Watchlist > Login > Movies
        // =====================================================
        private void BuildSidePanel()
        {
            sidePanel = new Panel();
            sidePanel.Width = SideWidth;
            sidePanel.Height = ClientSize.Height;
            sidePanel.Top = 0;
            sidePanel.Left = ClientSize.Width;          // مخفية خارج الشاشة
            sidePanel.BackColor = BgSide;
            sidePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            sidePanel.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Accent, 2))
                    e.Graphics.DrawLine(p, 0, 0, 0, sidePanel.Height);
            };

            Label title = new Label();
            title.Text = "MENU";
            title.ForeColor = Accent;
            title.AutoSize = true;
            title.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            title.Location = new Point(24, 22);
            sidePanel.Controls.Add(title);

            flowMenu = new FlowLayoutPanel();
            flowMenu.FlowDirection = FlowDirection.TopDown;
            flowMenu.WrapContents = false;
            flowMenu.AutoScroll = true;
            flowMenu.BackColor = BgSide;
            flowMenu.Location = new Point(0, 80);
            flowMenu.Size = new Size(SideWidth, sidePanel.Height - 80);
            flowMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            sidePanel.Controls.Add(flowMenu);

            // 1) الملف الشخصي
            flowMenu.Controls.Add(MakeMenuButton("Profile", 0, (s, e) =>
            {
                ToggleSidePanel();
                // TODO: افتح فورم الملف الشخصي عندك مثلاً: new ProfileForm().Show();
                MessageBox.Show("Profile page coming soon.", "Profile");
            }));

            // 2) الإعدادات
            flowMenu.Controls.Add(MakeMenuButton("Settings", 0, (s, e) =>
            {
                ToggleSidePanel();
                // TODO: افتح فورم الإعدادات عندك مثلاً: new SettingsForm().ShowDialog();
                MessageBox.Show("Settings page coming soon.", "Settings");
            }));

            // 3) Watchlist
            flowMenu.Controls.Add(MakeMenuButton("Watchlist", 0, (s, e) =>
            {
                ToggleSidePanel();
                btnWatchlistTab_Click(s, e);
            }));

            // 4) تسجيل الدخول
            flowMenu.Controls.Add(MakeMenuButton("Login", 0, (s, e) =>
            {
                ToggleSidePanel();
                btnLogin_Click(s, e);
            }));

            // 5) الأفلام (قائمة منسدلة بالتصنيفات)
            flowMenu.Controls.Add(MakeMenuButton("Movies  ▾", 0, (s, e) =>
            {
                catPanel.Visible = !catPanel.Visible;
            }));

            // لوحة التصنيفات (مخفية لين يضغط Movies)
            catPanel = new FlowLayoutPanel();
            catPanel.FlowDirection = FlowDirection.TopDown;
            catPanel.WrapContents = false;
            catPanel.AutoSize = true;
            catPanel.Visible = false;
            catPanel.BackColor = BgSide;
            catPanel.Margin = new Padding(0);
            flowMenu.Controls.Add(catPanel);

            Controls.Add(sidePanel);
            sidePanel.BringToFront();

            slideTimer = new Timer();
            slideTimer.Interval = 10;
            slideTimer.Tick += (s, e) =>
            {
                int target = sideOpen ? ClientSize.Width - SideWidth : ClientSize.Width;
                int step = 45;
                if (Math.Abs(sidePanel.Left - target) <= step)
                {
                    sidePanel.Left = target;
                    slideTimer.Stop();
                }
                else
                {
                    sidePanel.Left += sidePanel.Left < target ? step : -step;
                }
            };
        }

        private Button MakeMenuButton(string text, int indent, EventHandler onClick)
        {
            Button b = new Button();
            b.Text = text;
            b.Size = new Size(SideWidth - 40 - indent, 46);
            b.Margin = new Padding(20 + indent, 6, 20, 6);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 120, 160);
            b.BackColor = BgItem;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 12F);
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(14, 0, 0, 0);
            b.Cursor = Cursors.Hand;
            b.Click += onClick;
            return b;
        }

        private void ToggleSidePanel()
        {
            sideOpen = !sideOpen;
            sidePanel.BringToFront();
            slideTimer.Start();
        }

        // يبني أزرار التصنيفات داخل القائمة بعد تحميل البيانات
        private void BuildCategoryButtons()
        {
            catPanel.Controls.Clear();

            Button all = MakeMenuButton("All Movies", 16, (s, e) =>
            {
                ShowAll();
                ToggleSidePanel();
            });
            all.ForeColor = Accent;
            catPanel.Controls.Add(all);

            foreach (KeyValuePair<string, List<Movie>> sec in allSections)
            {
                string cat = sec.Key;
                Button b = MakeMenuButton(cat, 16, (s, e) =>
                {
                    ShowCategory(cat);
                    ToggleSidePanel();
                });
                b.ForeColor = Accent;
                catPanel.Controls.Add(b);
            }
        }

        // =====================================================
        //  تحميل الأقسام
        // =====================================================
        private void LoadMovies()
        {
            allSections.Clear();
            allSections.Add(new KeyValuePair<string, List<Movie>>("Action", GetActionMovies()));
            allSections.Add(new KeyValuePair<string, List<Movie>>("Comedy", GetComedyMovies()));
            allSections.Add(new KeyValuePair<string, List<Movie>>("Drama", GetDramaMovies()));
            allSections.Add(new KeyValuePair<string, List<Movie>>("Animation / Family", GetFamilyMovies()));

            foreach (KeyValuePair<string, List<Movie>> sec in allSections)
                foreach (Movie m in sec.Value)
                    m.Category = sec.Key;

            BuildCategoryButtons();
            ShowAll();
        }

        private void ShowAll()
        {
            Render(allSections, null);
        }

        private void ShowCategory(string category)
        {
            List<KeyValuePair<string, List<Movie>>> one = new List<KeyValuePair<string, List<Movie>>>();
            foreach (KeyValuePair<string, List<Movie>> sec in allSections)
                if (sec.Key == category) one.Add(sec);
            Render(one, null);
        }

        // =====================================================
        //  بيانات الأفلام
        //  لإضافة صورة: حط الملف في مجلد Posters واكتب اسمه مع الامتداد
        //  (jpg / jpeg / png / bmp / gif). لو كتبت الاسم بدون امتداد
        //  البرنامج يدور عليه تلقائياً.
        // =====================================================
        private static Movie CreateMovie(string title, string year, string story, string cast, string poster)
        {
            return new Movie
            {
                Title = title,
                Year = year,
                Story = story,
                Cast = cast,
                Poster = poster
            };
        }

        private List<Movie> GetActionMovies()
        {
            return new List<Movie>
            {
                CreateMovie("Mad Max: Fury Road", "2015", "A chase across the desert to escape a tyrant.", "Tom Hardy, Charlize Theron", "madmax.jpg"),
                CreateMovie("John Wick", "2014", "A retired hitman returns for revenge.", "Keanu Reeves", "johnwick.jpg"),
                CreateMovie("Mission: Impossible - Fallout", " 2018", "Ethan Hunt and his team race against time after a mission goes wrong.", "Tom Cruise, Henry Cavill", "mission.jpg"),
                CreateMovie("The Dark Knight", "2008", "Batman faces the Joker in Gotham City.", "Christian Bale, Heath Ledger", "darkknight.jpg"),
                CreateMovie("Gladiator", "2000", "A betrayed general becomes a gladiator.", "Russell Crowe, Joaquin Phoenix", "gladiator.jpg")
            };
        }

        private List<Movie> GetComedyMovies()
        {
            return new List<Movie>
            {
                CreateMovie("Home Alone", "1990", "An eight-year-old troublemaker must protect his house from a pair of burglars.", "Macaulay Culkin, Joe Pesci", "HomeAlone.jpg"),
                CreateMovie("Night at the Museum", "2006", "A night watchman at a museum discovers that an ancient curse brings the exhibits to life.", "Ben Stiller, Robin Williams", "NightattheMuseum.jpg"),
                CreateMovie("Paddington 2", "2017", "Paddington tries to get a gift for his aunt’s birthday, but things go hilariously wrong.", "Ben Whishaw, Hugh Grant", "Paddington.jpg"),
                CreateMovie("Free Guy", "2021", "A bank teller discovers that he is actually a background player in an open-world video game.", "Ryan Reynolds, Jodie Comer", "FreeGuy.jpg"),
                CreateMovie("Toy Story", "1995", "A cowboy doll is profoundly threatened and jealous when a new spaceman action figure supplants him.", "Tom Hanks, Tim Allen", "ToyStory.jpg")
            };
        }

        private List<Movie> GetDramaMovies()
        {
            return new List<Movie>
            {
                CreateMovie("The Pursuit of Happyness", "2006", "A struggling salesman takes custody of his son as he’s poised to begin a life-changing professional career.", "Will Smith, Jaden Smith", "ThePursuitofHappyness.jpg"),
                CreateMovie("The Green Mile", "1999", "The lives of guards on Death Row are affected by one of their charges: a black man accused of child murder and rape, yet who has a mysterious gift.", "Tom Hanks, Michael Clarke Duncan", "TheGreenMile.jpg"),
                CreateMovie("Wonder", "2017", "Based on the New York Times bestseller, this movie tells the incredibly inspiring and heartwarming story of August Pullman.", "Julia Roberts, Jacob Tremblay", "Wonder.jpg"),
                CreateMovie("Dead Poets Society", "1989", "An English teacher inspires his students to look at poetry with a different perspective of authentic knowledge and extrasolar life.", "Robin Williams, Robert Sean\r\nLeonard", "DeadPoetsSociety.jpg"),
                CreateMovie("Good Will Hunting", "1997", "Will Hunting, a janitor at M.I.T., has a gift for mathematics, but needs help from a psychologist to find direction in his life.", "Robin Williams, Matt Damon", "GoodWillHunting.jpg")
            };
        }

        private List<Movie> GetFamilyMovies()
        {
            return new List<Movie>
            {
                CreateMovie("Inside Out", "2015", "After young Riley is uprooted from her Midwest life and moved to San Francisco, her emotions conflict on how best to navigate a new city, house, and school.", "Amy Poehler, Bill Hader", "InsideOut.jpg"),
                CreateMovie("Finding Nemo", "2003", "After his son is captured in the Great Barrier Reef and taken to Sydney, a timid clownfish sets out on a journey to bring him home.", "Albert Brooks, Ellen DeGeneres", "FindingNemo.jpg"),
                CreateMovie("Ratatouille", "2007", "A rat who can cook makes an unusual alliance with a young kitchen worker at a famous restaurant in Paris.", "Patton Oswalt, lan Holm", "Ratatouille.jpg"),
                CreateMovie("Zootopia", "2016", "In a city of anthropomorphic animals, a rookie bunny cop and a cynical con artist fox must work together to uncover a conspiracy.", "Ginnifer Goodwin, Jason Bateman", "Zootopia.jpg"),
                CreateMovie("Moana", "2016", "In Ancient Polynesia, a young woman sets out on a daring mission to save her people with the help of a demigod.", "Auli'i Cravalho, Dwayne Johnson", "Moana.jpg")
            };
        }

        // =====================================================
        //  العرض: كل تصنيف عنوان + شبكة بطاقات بالسنتر
        // =====================================================
        private void Render(List<KeyValuePair<string, List<Movie>>> sections, string emptyText)
        {
            scrollHost.SuspendLayout();
            ClearContent();

            int y = 30;
            int totalMovies = 0;

            foreach (KeyValuePair<string, List<Movie>> sec in sections)
            {
                if (sec.Value == null || sec.Value.Count == 0) continue;
                totalMovies += sec.Value.Count;

                // خط الأكسنت الجانبي
                Panel bar = new Panel();
                bar.BackColor = Accent;
                bar.Size = new Size(5, 34);
                bar.Location = new Point(0, y);
                content.Controls.Add(bar);

                // اسم التصنيف
                Label lbl = new Label();
                lbl.Text = sec.Key;
                lbl.AutoSize = true;
                lbl.ForeColor = Accent;
                lbl.BackColor = content.BackColor;
                lbl.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
                lbl.Location = new Point(16, y - 4);
                content.Controls.Add(lbl);

                // عدد الأفلام
                Label cnt = new Label();
                cnt.Text = sec.Value.Count + (sec.Value.Count == 1 ? " movie" : " movies");
                cnt.AutoSize = true;
                cnt.ForeColor = Color.FromArgb(140, 160, 190);
                cnt.BackColor = content.BackColor;
                cnt.Font = new Font("Segoe UI", 10F);
                cnt.RightToLeft = RightToLeft.No;
                cnt.Location = new Point(lbl.Right + 14, y + 10);
                content.Controls.Add(cnt);

                // خط فاصل رفيع
                Panel line = new Panel();
                line.BackColor = Color.FromArgb(25, 55, 100);
                line.Size = new Size(content.Width, 1);
                line.Location = new Point(0, y + 42);
                content.Controls.Add(line);

                y += 60;

                int col = 0;
                int x = 0;
                foreach (Movie m in sec.Value)
                {
                    if (col == PerRow)
                    {
                        col = 0;
                        x = 0;
                        y += CardH + Gap;
                    }
                    content.Controls.Add(MakeCard(m, x, y));
                    x += CardW + Gap;
                    col++;
                }
                y += CardH + 40;   // مسافة بين التصنيفات
            }

            if (totalMovies == 0)
            {
                Label empty = new Label();
                empty.Text = string.IsNullOrEmpty(emptyText) ? "No movies to show." : emptyText;
                empty.AutoSize = true;
                empty.ForeColor = Color.FromArgb(140, 160, 190);
                empty.BackColor = content.BackColor;
                empty.Font = new Font("Segoe UI", 16F);
                empty.Location = new Point(0, 60);
                content.Controls.Add(empty);
                y = 160;
            }

            content.Height = y;
            scrollHost.AutoScrollMinSize = new Size(0, y);
            scrollHost.AutoScrollPosition = new Point(0, 0);
            CenterContent();
            scrollHost.ResumeLayout();
        }

        private void ClearContent()
        {
            while (content.Controls.Count > 0)
            {
                Control c = content.Controls[0];
                content.Controls.RemoveAt(0);

                foreach (Control inner in c.Controls)
                {
                    PictureBox pb = inner as PictureBox;
                    if (pb != null && pb.Image != null)
                    {
                        pb.Image.Dispose();
                        pb.Image = null;
                    }
                }
                c.Dispose();
            }
        }

        // =====================================================
        //  بطاقة الفيلم (صورة + اسم + Hover)
        // =====================================================
        private Panel MakeCard(Movie m, int x, int y)
        {
            Panel card = new Panel();
            card.Size = new Size(CardW, CardH);
            card.Location = new Point(x, y);
            card.BackColor = BgCard;
            card.Cursor = Cursors.Hand;
            card.Tag = m;

            PictureBox pic = new PictureBox();
            pic.Size = new Size(PosterW, PosterH);
            pic.Location = new Point(CardPad, CardPad);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.BackColor = BgCard;
            pic.Cursor = Cursors.Hand;
            pic.Tag = m;                       // نخزن بيانات الفيلم داخل الصورة
            pic.Image = LoadPoster(m);

            Label name = new Label();
            name.Text = m.Title;
            name.Size = new Size(PosterW, 40);
            name.Location = new Point(CardPad, CardPad + PosterH);
            name.TextAlign = ContentAlignment.MiddleCenter;
            name.ForeColor = Color.White;
            name.BackColor = BgCard;
            name.Font = new Font("Segoe UI Semibold", 10.5F);
            name.AutoEllipsis = true;
            name.Cursor = Cursors.Hand;
            name.Tag = m;

            // تأثير Hover
            EventHandler enter = (s, e) =>
            {
                card.BackColor = Accent;
                name.BackColor = Accent;
                name.ForeColor = Color.Black;
            };
            EventHandler leave = (s, e) =>
            {
                card.BackColor = BgCard;
                name.BackColor = BgCard;
                name.ForeColor = Color.White;
            };

            foreach (Control c in new Control[] { card, pic, name })
            {
                c.MouseEnter += enter;
                c.MouseLeave += leave;
                c.Click += Poster_Click;
            }

            card.Controls.Add(pic);
            card.Controls.Add(name);
            return card;
        }

        // يحمّل الصورة من مجلد Posters (بدون ما يقفل الملف)، ولو مو موجودة يسوي صورة بديلة
        private Image LoadPoster(Movie m)
        {
            string path = FindPosterFile(m.Poster);
            if (path != null)
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (Image img = Image.FromStream(fs))
                    {
                        return new Bitmap(img);   // نسخة مستقلة عن الملف
                    }
                }
                catch
                {
                    // ملف تالف: نكمل ونرجع الصورة البديلة
                }
            }
            return MakePlaceholder();
        }

        private string FindPosterFile(string poster)
        {
            if (string.IsNullOrWhiteSpace(poster)) return null;

            string full = Path.Combine(PostersFolder, poster);
            if (File.Exists(full)) return full;

            // لو الاسم بدون امتداد نجرب الامتدادات المعروفة
            if (string.IsNullOrEmpty(Path.GetExtension(poster)))
            {
                string[] exts = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                foreach (string ext in exts)
                {
                    string p = full + ext;
                    if (File.Exists(p)) return p;
                }
            }
            return null;
        }

        // صورة بديلة (أيقونة فقط، الاسم يظهر تحت البطاقة)
        private Image MakePlaceholder()
        {
            Bitmap bmp = new Bitmap(PosterW, PosterH);
            using (Graphics g = Graphics.FromImage(bmp))
            using (LinearGradientBrush br = new LinearGradientBrush(
                new Rectangle(0, 0, PosterW, PosterH),
                Color.FromArgb(18, 40, 85), Color.FromArgb(5, 12, 35), 90F))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.FillRectangle(br, 0, 0, PosterW, PosterH);

                using (Font icon = new Font("Segoe UI Emoji", 36F))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    using (Brush b = new SolidBrush(Color.FromArgb(120, Accent)))
                        g.DrawString("🎬", icon, b, new RectangleF(0, 0, PosterW, PosterH), sf);
                }
            }
            return bmp;
        }

        // =====================================================
        //  الأحداث
        // =====================================================
        private void Poster_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Movie m = (Movie)c.Tag;

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
            Hide();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            SignUp s = new SignUp();
            s.Show();
            Hide();
        }

        private void btnWatchlistTab_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, List<Movie>>> one = new List<KeyValuePair<string, List<Movie>>>();
            one.Add(new KeyValuePair<string, List<Movie>>("Watchlist", MovieDetailsForm.Watchlist));
            Render(one, "Your watchlist is empty.");
        }

        // زر All Movies (مخفي من الهيدر لكن الدالة مربوطة بالـ Designer)
        private void button1_Click(object sender, EventArgs e)
        {
            ShowAll();
        }

        // فاضيين بس مربوطين في Form1.Designer.cs فلازم يبقون
        private void flowAction_Paint(object sender, PaintEventArgs e)
        {
        }

        private void lblAction_Click(object sender, EventArgs e)
        {
        }
    }
}

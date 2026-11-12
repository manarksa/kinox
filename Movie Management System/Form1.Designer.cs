namespace Movie_Management_System
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.All = new System.Windows.Forms.Button();
            this.btnWatchlistTab = new System.Windows.Forms.Button();
            this.btnSignUp = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panelBody = new System.Windows.Forms.Panel();
            this.flowAction = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAction = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(12)))), ((int)(((byte)(35)))));
            this.panelHeader.Controls.Add(this.All);
            this.panelHeader.Controls.Add(this.btnWatchlistTab);
            this.panelHeader.Controls.Add(this.btnSignUp);
            this.panelHeader.Controls.Add(this.btnLogin);
            this.panelHeader.Controls.Add(this.picLogo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(2198, 125);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            // 
            // All
            // 
            this.All.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(235)))));
            this.All.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(20)))), ((int)(((byte)(50)))));
            this.All.Location = new System.Drawing.Point(1035, 34);
            this.All.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.All.Name = "All";
            this.All.Size = new System.Drawing.Size(129, 56);
            this.All.TabIndex = 5;
            this.All.Text = "All Movies";
            this.All.UseVisualStyleBackColor = false;
            this.All.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnWatchlistTab
            // 
            this.btnWatchlistTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(235)))));
            this.btnWatchlistTab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(20)))), ((int)(((byte)(50)))));
            this.btnWatchlistTab.Location = new System.Drawing.Point(1550, 41);
            this.btnWatchlistTab.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnWatchlistTab.Name = "btnWatchlistTab";
            this.btnWatchlistTab.Size = new System.Drawing.Size(129, 56);
            this.btnWatchlistTab.TabIndex = 4;
            this.btnWatchlistTab.Text = "Watchlist";
            this.btnWatchlistTab.UseVisualStyleBackColor = false;
            this.btnWatchlistTab.Click += new System.EventHandler(this.btnWatchlistTab_Click);
            // 
            // btnSignUp
            // 
            this.btnSignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(20)))), ((int)(((byte)(50)))));
            this.btnSignUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(235)))));
            this.btnSignUp.FlatAppearance.BorderSize = 2;
            this.btnSignUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignUp.Font = new System.Drawing.Font("Times New Roman", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSignUp.ForeColor = System.Drawing.Color.White;
            this.btnSignUp.Location = new System.Drawing.Point(1942, 41);
            this.btnSignUp.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnSignUp.Name = "btnSignUp";
            this.btnSignUp.Size = new System.Drawing.Size(190, 56);
            this.btnSignUp.TabIndex = 3;
            this.btnSignUp.Text = "Sign Up";
            this.btnSignUp.UseVisualStyleBackColor = false;
            this.btnSignUp.Click += new System.EventHandler(this.btnSignUp_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(235)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Times New Roman", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(20)))), ((int)(((byte)(50)))));
            this.btnLogin.Location = new System.Drawing.Point(1718, 41);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(190, 56);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(5, 0);
            this.picLogo.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(199, 125);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 1;
            this.picLogo.TabStop = false;
            // 
            // panelBody
            // 
            this.panelBody.AutoScroll = true;
            this.panelBody.Controls.Add(this.flowAction);
            this.panelBody.Controls.Add(this.lblAction);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 125);
            this.panelBody.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(2198, 1020);
            this.panelBody.TabIndex = 1;
            // 
            // flowAction
            // 
            this.flowAction.BackColor = System.Drawing.Color.Transparent;
            this.flowAction.Location = new System.Drawing.Point(86, 153);
            this.flowAction.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.flowAction.Name = "flowAction";
            this.flowAction.Size = new System.Drawing.Size(2057, 438);
            this.flowAction.TabIndex = 1;
            this.flowAction.Paint += new System.Windows.Forms.PaintEventHandler(this.flowAction_Paint);
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.BackColor = System.Drawing.Color.Transparent;
            this.lblAction.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(235)))));
            this.lblAction.Location = new System.Drawing.Point(69, 88);
            this.lblAction.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(161, 60);
            this.lblAction.TabIndex = 0;
            this.lblAction.Text = "Action";
            this.lblAction.Click += new System.EventHandler(this.lblAction_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(24)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(2198, 1145);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHeader);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnSignUp;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.FlowLayoutPanel flowAction;
        private System.Windows.Forms.Button btnWatchlistTab;
        private System.Windows.Forms.Button All;
    }
}


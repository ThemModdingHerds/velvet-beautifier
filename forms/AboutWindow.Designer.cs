namespace ThemModdingHerds.VelvetBeautifier
{
    partial class AboutWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
            VelvetPicture = new PictureBox();
            Title = new Label();
            GitHubLink = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)VelvetPicture).BeginInit();
            SuspendLayout();
            // 
            // VelvetPicture
            // 
            VelvetPicture.Cursor = Cursors.Hand;
            VelvetPicture.Image = (Image)resources.GetObject("VelvetPicture.Image");
            VelvetPicture.Location = new Point(12, 12);
            VelvetPicture.Name = "VelvetPicture";
            VelvetPicture.Size = new Size(297, 477);
            VelvetPicture.TabIndex = 0;
            VelvetPicture.TabStop = false;
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("French Script MT", 44F);
            Title.Location = new Point(479, 12);
            Title.Name = "Title";
            Title.Size = new Size(309, 67);
            Title.TabIndex = 1;
            Title.Text = "VelvetBeautifier";
            // 
            // GitHubLink
            // 
            GitHubLink.AutoSize = true;
            GitHubLink.Location = new Point(743, 426);
            GitHubLink.Name = "GitHubLink";
            GitHubLink.Size = new Size(45, 15);
            GitHubLink.TabIndex = 2;
            GitHubLink.TabStop = true;
            GitHubLink.Text = "GitHub";
            GitHubLink.LinkClicked += GitHubLink_LinkClicked;
            // 
            // AboutWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(GitHubLink);
            Controls.Add(Title);
            Controls.Add(VelvetPicture);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AboutWindow";
            Text = "About";
            ((System.ComponentModel.ISupportInitialize)VelvetPicture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox VelvetPicture;
        private Label Title;
        private LinkLabel GitHubLink;
    }
}
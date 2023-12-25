namespace ThemModdingHerds.VelvetBeautifier;
partial class AboutForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
        VelvetPicture = new PictureBox();
        Title = new Label();
        GitHubLink = new LinkLabel();
        GameBananaLink = new LinkLabel();
        ByLabel = new Label();
        NightTheFoxLink = new LinkLabel();
        VelvetQuote = new Label();
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
        // GameBananaLink
        // 
        GameBananaLink.AutoSize = true;
        GameBananaLink.Location = new Point(660, 426);
        GameBananaLink.Name = "GameBananaLink";
        GameBananaLink.Size = new Size(77, 15);
        GameBananaLink.TabIndex = 3;
        GameBananaLink.TabStop = true;
        GameBananaLink.Text = "GameBanana";
        GameBananaLink.LinkClicked += GameBananaLink_LinkClicked;
        // 
        // ByLabel
        // 
        ByLabel.AutoSize = true;
        ByLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        ByLabel.Location = new Point(526, 79);
        ByLabel.Name = "ByLabel";
        ByLabel.Size = new Size(20, 15);
        ByLabel.TabIndex = 4;
        ByLabel.Text = "by";
        // 
        // NightTheFoxLink
        // 
        NightTheFoxLink.AutoSize = true;
        NightTheFoxLink.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        NightTheFoxLink.Location = new Point(543, 79);
        NightTheFoxLink.Name = "NightTheFoxLink";
        NightTheFoxLink.Size = new Size(79, 15);
        NightTheFoxLink.TabIndex = 5;
        NightTheFoxLink.TabStop = true;
        NightTheFoxLink.Text = "Night The Fox";
        NightTheFoxLink.LinkClicked += NightTheFoxLink_LinkClicked;
        // 
        // VelvetQuote
        // 
        VelvetQuote.AutoSize = true;
        VelvetQuote.Font = new Font("French Script MT", 23F, FontStyle.Italic);
        VelvetQuote.Location = new Point(348, 141);
        VelvetQuote.Name = "VelvetQuote";
        VelvetQuote.Size = new Size(440, 35);
        VelvetQuote.TabIndex = 6;
        VelvetQuote.Text = "\"Want some more? Of course you do.\" - Velvet";
        // 
        // AboutForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(VelvetQuote);
        Controls.Add(NightTheFoxLink);
        Controls.Add(ByLabel);
        Controls.Add(GameBananaLink);
        Controls.Add(GitHubLink);
        Controls.Add(Title);
        Controls.Add(VelvetPicture);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "AboutForm";
        Text = "About";
        ((System.ComponentModel.ISupportInitialize)VelvetPicture).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private PictureBox VelvetPicture;
    private Label Title;
    private LinkLabel GitHubLink;
    private LinkLabel GameBananaLink;
    private Label ByLabel;
    private LinkLabel NightTheFoxLink;
    private Label VelvetQuote;
}

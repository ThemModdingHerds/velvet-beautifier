namespace ThemModdingHerds.VelvetBeautifier.Forms;
partial class DownloadForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadForm));
        ModNameLabel = new Label();
        ModAuthorLabel = new Label();
        ModDescriptionBox = new TextBox();
        InstallButton = new Button();
        SuspendLayout();
        // 
        // ModNameLabel
        // 
        ModNameLabel.AutoSize = true;
        ModNameLabel.Font = new Font("Segoe UI", 15F);
        ModNameLabel.Location = new Point(12, 9);
        ModNameLabel.Name = "ModNameLabel";
        ModNameLabel.Size = new Size(106, 28);
        ModNameLabel.TabIndex = 0;
        ModNameLabel.Text = "ModName";
        // 
        // ModAuthorLabel
        // 
        ModAuthorLabel.AutoSize = true;
        ModAuthorLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        ModAuthorLabel.Location = new Point(21, 37);
        ModAuthorLabel.Name = "ModAuthorLabel";
        ModAuthorLabel.Size = new Size(69, 15);
        ModAuthorLabel.TabIndex = 1;
        ModAuthorLabel.Text = "by someone";
        // 
        // ModDescriptionBox
        // 
        ModDescriptionBox.Location = new Point(12, 55);
        ModDescriptionBox.Multiline = true;
        ModDescriptionBox.Name = "ModDescriptionBox";
        ModDescriptionBox.ReadOnly = true;
        ModDescriptionBox.Size = new Size(776, 318);
        ModDescriptionBox.TabIndex = 2;
        // 
        // InstallButton
        // 
        InstallButton.Font = new Font("Segoe UI", 20F);
        InstallButton.Location = new Point(12, 379);
        InstallButton.Name = "InstallButton";
        InstallButton.Size = new Size(776, 59);
        InstallButton.TabIndex = 3;
        InstallButton.Text = "Install";
        InstallButton.UseVisualStyleBackColor = true;
        InstallButton.Click += InstallButton_Click;
        // 
        // DownloadForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(InstallButton);
        Controls.Add(ModDescriptionBox);
        Controls.Add(ModAuthorLabel);
        Controls.Add(ModNameLabel);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "DownloadForm";
        Text = "DownloadForm";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label ModNameLabel;
    private Label ModAuthorLabel;
    private TextBox ModDescriptionBox;
    private Button InstallButton;
}
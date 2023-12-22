namespace ThemModdingHerds.VelvetBeautifier;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        FindTFHFolder = new FolderBrowserDialog();
        SuspendLayout();
        // 
        // FindTFHFolder
        // 
        FindTFHFolder.Description = "Select Installation Folder of Them's Fightin' Herds";
        FindTFHFolder.RootFolder = Environment.SpecialFolder.MyComputer;
        // 
        // MainForm
        // 
        AccessibleDescription = "Them's Fightin' Herds Mod Loader";
        AccessibleName = "VelvetBeautifier";
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Name = "MainForm";
        Text = "VelvetBeautifier";
        ResumeLayout(false);
    }

    #endregion

    private FolderBrowserDialog FindTFHFolder;
}

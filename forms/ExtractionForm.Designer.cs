namespace ThemModdingHerds.VelvetBeautifier;
partial class ExtractionForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractionForm));
        FileFormat = new ComboBox();
        ExtractButton = new Button();
        ExtractSelectFile = new Button();
        SelectFileDialog = new OpenFileDialog();
        SelectOutputFolder = new FolderBrowserDialog();
        SuspendLayout();
        // 
        // FileFormat
        // 
        FileFormat.AccessibleDescription = "Extract assets from Them's Fightin' Herds";
        FileFormat.AccessibleName = "Extraction Window";
        FileFormat.FormattingEnabled = true;
        FileFormat.Items.AddRange(new object[] { "tfhres", "gfs" });
        FileFormat.Location = new Point(12, 12);
        FileFormat.Name = "FileFormat";
        FileFormat.Size = new Size(159, 23);
        FileFormat.TabIndex = 0;
        // 
        // ExtractButton
        // 
        ExtractButton.Enabled = false;
        ExtractButton.Location = new Point(12, 41);
        ExtractButton.Name = "ExtractButton";
        ExtractButton.Size = new Size(75, 23);
        ExtractButton.TabIndex = 1;
        ExtractButton.Text = "Extract";
        ExtractButton.UseVisualStyleBackColor = true;
        ExtractButton.Click += ExtractButton_Click;
        // 
        // ExtractSelectFile
        // 
        ExtractSelectFile.Location = new Point(96, 41);
        ExtractSelectFile.Name = "ExtractSelectFile";
        ExtractSelectFile.Size = new Size(75, 23);
        ExtractSelectFile.TabIndex = 2;
        ExtractSelectFile.Text = "Select FIle";
        ExtractSelectFile.UseVisualStyleBackColor = true;
        ExtractSelectFile.Click += ExtractSelectFile_Click;
        // 
        // ExtractionWindow
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(183, 75);
        Controls.Add(ExtractSelectFile);
        Controls.Add(ExtractButton);
        Controls.Add(FileFormat);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "ExtractionWindow";
        Text = "Extraction";
        ResumeLayout(false);
    }

    #endregion

    private ComboBox FileFormat;
    private Button ExtractButton;
    private Button ExtractSelectFile;
    private OpenFileDialog SelectFileDialog;
    private FolderBrowserDialog SelectOutputFolder;
}
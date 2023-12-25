﻿namespace ThemModdingHerds.VelvetBeautifier;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        FindTFHFolder = new FolderBrowserDialog();
        MenuBar = new MenuStrip();
        MenuFile = new ToolStripMenuItem();
        MenuFileRefreshMods = new ToolStripMenuItem();
        MenuFileApplyMods = new ToolStripMenuItem();
        MenuFileSeperator = new ToolStripSeparator();
        MenuFileExit = new ToolStripMenuItem();
        MenuTools = new ToolStripMenuItem();
        MenuToolsExtraction = new ToolStripMenuItem();
        MenuToolsSeperator = new ToolStripSeparator();
        MenuToolsConfigure = new ToolStripMenuItem();
        MenuToolsConfigureTFHFolder = new ToolStripMenuItem();
        MenuToolsRegisterScheme = new ToolStripMenuItem();
        MenuHelp = new ToolStripMenuItem();
        MenuHelpFI = new ToolStripMenuItem();
        MenuHelpAbout = new ToolStripMenuItem();
        ModList = new CheckedListBox();
        ApplyButton = new Button();
        ModNameLabel = new Label();
        ModAuthorLabel = new Label();
        ModDescriptionBox = new TextBox();
        MenuFileInstallMod = new ToolStripMenuItem();
        MenuBar.SuspendLayout();
        SuspendLayout();
        // 
        // FindTFHFolder
        // 
        FindTFHFolder.Description = "Select Installation Folder of Them's Fightin' Herds";
        FindTFHFolder.RootFolder = Environment.SpecialFolder.MyComputer;
        // 
        // MenuBar
        // 
        MenuBar.Items.AddRange(new ToolStripItem[] { MenuFile, MenuTools, MenuHelp });
        MenuBar.Location = new Point(0, 0);
        MenuBar.Name = "MenuBar";
        MenuBar.Size = new Size(700, 24);
        MenuBar.TabIndex = 0;
        MenuBar.Text = "menuStrip1";
        // 
        // MenuFile
        // 
        MenuFile.DropDownItems.AddRange(new ToolStripItem[] { MenuFileInstallMod, MenuFileRefreshMods, MenuFileApplyMods, MenuFileSeperator, MenuFileExit });
        MenuFile.Name = "MenuFile";
        MenuFile.Size = new Size(37, 20);
        MenuFile.Text = "File";
        // 
        // MenuFileRefreshMods
        // 
        MenuFileRefreshMods.Name = "MenuFileRefreshMods";
        MenuFileRefreshMods.ShortcutKeys = Keys.Control | Keys.R;
        MenuFileRefreshMods.Size = new Size(187, 22);
        MenuFileRefreshMods.Text = "Refresh Mods";
        MenuFileRefreshMods.Click += MenuFileRefreshMods_Click;
        // 
        // MenuFileApplyMods
        // 
        MenuFileApplyMods.Name = "MenuFileApplyMods";
        MenuFileApplyMods.ShortcutKeys = Keys.Control | Keys.S;
        MenuFileApplyMods.Size = new Size(187, 22);
        MenuFileApplyMods.Text = "Apply Mods";
        MenuFileApplyMods.Click += MenuFileApplyMods_Click;
        // 
        // MenuFileSeperator
        // 
        MenuFileSeperator.Name = "MenuFileSeperator";
        MenuFileSeperator.Size = new Size(184, 6);
        // 
        // MenuFileExit
        // 
        MenuFileExit.Name = "MenuFileExit";
        MenuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
        MenuFileExit.Size = new Size(187, 22);
        MenuFileExit.Text = "Exit";
        MenuFileExit.Click += MenuFileExit_Click;
        // 
        // MenuTools
        // 
        MenuTools.DropDownItems.AddRange(new ToolStripItem[] { MenuToolsExtraction, MenuToolsSeperator, MenuToolsConfigure, MenuToolsRegisterScheme });
        MenuTools.Name = "MenuTools";
        MenuTools.Size = new Size(46, 20);
        MenuTools.Text = "Tools";
        // 
        // MenuToolsExtraction
        // 
        MenuToolsExtraction.Name = "MenuToolsExtraction";
        MenuToolsExtraction.ShortcutKeys = Keys.Control | Keys.E;
        MenuToolsExtraction.Size = new Size(180, 22);
        MenuToolsExtraction.Text = "Extraction";
        MenuToolsExtraction.Click += MenuToolsExtraction_Click;
        // 
        // MenuToolsSeperator
        // 
        MenuToolsSeperator.Name = "MenuToolsSeperator";
        MenuToolsSeperator.Size = new Size(177, 6);
        // 
        // MenuToolsConfigure
        // 
        MenuToolsConfigure.DropDownItems.AddRange(new ToolStripItem[] { MenuToolsConfigureTFHFolder });
        MenuToolsConfigure.Name = "MenuToolsConfigure";
        MenuToolsConfigure.Size = new Size(180, 22);
        MenuToolsConfigure.Text = "Configure";
        // 
        // MenuToolsConfigureTFHFolder
        // 
        MenuToolsConfigureTFHFolder.Name = "MenuToolsConfigureTFHFolder";
        MenuToolsConfigureTFHFolder.Size = new Size(141, 22);
        MenuToolsConfigureTFHFolder.Text = "Install Folder";
        MenuToolsConfigureTFHFolder.Click += MenuToolsConfigureTFHFolder_Click;
        // 
        // MenuToolsRegisterScheme
        // 
        MenuToolsRegisterScheme.Name = "MenuToolsRegisterScheme";
        MenuToolsRegisterScheme.Size = new Size(180, 22);
        MenuToolsRegisterScheme.Text = "Register Scheme";
        MenuToolsRegisterScheme.Click += MenuToolsRegisterScheme_Click;
        // 
        // MenuHelp
        // 
        MenuHelp.DropDownItems.AddRange(new ToolStripItem[] { MenuHelpFI, MenuHelpAbout });
        MenuHelp.Name = "MenuHelp";
        MenuHelp.Size = new Size(44, 20);
        MenuHelp.Text = "Help";
        // 
        // MenuHelpFI
        // 
        MenuHelpFI.Name = "MenuHelpFI";
        MenuHelpFI.Size = new Size(142, 22);
        MenuHelpFI.Text = "Found Issue?";
        MenuHelpFI.Click += MenuHelpFI_Click;
        // 
        // MenuHelpAbout
        // 
        MenuHelpAbout.Name = "MenuHelpAbout";
        MenuHelpAbout.Size = new Size(142, 22);
        MenuHelpAbout.Text = "About";
        MenuHelpAbout.Click += MenuHelpAbout_Click;
        // 
        // ModList
        // 
        ModList.FormattingEnabled = true;
        ModList.Location = new Point(12, 27);
        ModList.Name = "ModList";
        ModList.Size = new Size(220, 292);
        ModList.TabIndex = 1;
        ModList.ItemCheck += ModList_ItemCheck;
        ModList.SelectedIndexChanged += ModList_SelectedIndexChanged;
        // 
        // ApplyButton
        // 
        ApplyButton.Location = new Point(238, 296);
        ApplyButton.Name = "ApplyButton";
        ApplyButton.Size = new Size(75, 23);
        ApplyButton.TabIndex = 2;
        ApplyButton.Text = "Apply";
        ApplyButton.UseVisualStyleBackColor = true;
        ApplyButton.Click += ApplyButton_Click;
        // 
        // ModNameLabel
        // 
        ModNameLabel.AutoSize = true;
        ModNameLabel.Font = new Font("Segoe UI", 15F);
        ModNameLabel.Location = new Point(238, 27);
        ModNameLabel.Name = "ModNameLabel";
        ModNameLabel.Size = new Size(111, 28);
        ModNameLabel.TabIndex = 3;
        ModNameLabel.Text = "Mod Name";
        ModNameLabel.Visible = false;
        // 
        // ModAuthorLabel
        // 
        ModAuthorLabel.AutoSize = true;
        ModAuthorLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        ModAuthorLabel.Location = new Point(244, 55);
        ModAuthorLabel.Name = "ModAuthorLabel";
        ModAuthorLabel.Size = new Size(69, 15);
        ModAuthorLabel.TabIndex = 4;
        ModAuthorLabel.Text = "by someone";
        ModAuthorLabel.Visible = false;
        // 
        // ModDescriptionBox
        // 
        ModDescriptionBox.Location = new Point(244, 82);
        ModDescriptionBox.Multiline = true;
        ModDescriptionBox.Name = "ModDescriptionBox";
        ModDescriptionBox.ReadOnly = true;
        ModDescriptionBox.Size = new Size(444, 208);
        ModDescriptionBox.TabIndex = 5;
        ModDescriptionBox.Visible = false;
        // 
        // MenuFileInstallMod
        // 
        MenuFileInstallMod.Name = "MenuFileInstallMod";
        MenuFileInstallMod.ShortcutKeys = Keys.Control | Keys.I;
        MenuFileInstallMod.Size = new Size(187, 22);
        MenuFileInstallMod.Text = "Install Mod";
        MenuFileInstallMod.Click += MenuFileInstallMod_Click;
        // 
        // MainForm
        // 
        AccessibleDescription = "Them's Fightin' Herds Mod Loader";
        AccessibleName = "VelvetBeautifier";
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(700, 338);
        Controls.Add(ModDescriptionBox);
        Controls.Add(ModAuthorLabel);
        Controls.Add(ModNameLabel);
        Controls.Add(ApplyButton);
        Controls.Add(ModList);
        Controls.Add(MenuBar);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = MenuBar;
        Margin = new Padding(3, 2, 3, 2);
        Name = "MainForm";
        Text = "VelvetBeautifier";
        MenuBar.ResumeLayout(false);
        MenuBar.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FolderBrowserDialog FindTFHFolder;
    private MenuStrip MenuBar;
    private ToolStripMenuItem MenuFile;
    private ToolStripMenuItem MenuFileExit;
    private ToolStripMenuItem MenuTools;
    private ToolStripMenuItem MenuToolsExtraction;
    private ToolStripMenuItem MenuHelp;
    private ToolStripMenuItem MenuHelpFI;
    private ToolStripMenuItem MenuHelpAbout;
    private ToolStripSeparator MenuToolsSeperator;
    private ToolStripMenuItem MenuToolsConfigure;
    private ToolStripMenuItem MenuToolsConfigureTFHFolder;
    private CheckedListBox ModList;
    private ToolStripMenuItem MenuFileRefreshMods;
    private ToolStripSeparator MenuFileSeperator;
    private Button ApplyButton;
    private Label ModNameLabel;
    private Label ModAuthorLabel;
    private TextBox ModDescriptionBox;
    private ToolStripMenuItem MenuFileApplyMods;
    private ToolStripMenuItem MenuToolsRegisterScheme;
    private ToolStripMenuItem MenuFileInstallMod;
}

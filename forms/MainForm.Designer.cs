namespace ThemModdingHerds.VelvetBeautifier.Forms;
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
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        SelectFolder = new FolderBrowserDialog();
        MenuBar = new MenuStrip();
        MenuFile = new ToolStripMenuItem();
        MenuFileInstallMod = new ToolStripMenuItem();
        MenuInstallFromURL = new ToolStripMenuItem();
        MenuInstallFromFolder = new ToolStripMenuItem();
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
        ContextInstallFromFolder = new ToolStripMenuItem();
        ContextInstallFromUrl = new ToolStripMenuItem();
        ModList = new CheckedListBox();
        ApplyButton = new Button();
        ModNameLabel = new Label();
        ModAuthorLabel = new Label();
        ModDescriptionBox = new TextBox();
        ModListContextMenu = new ContextMenuStrip(components);
        MenuInstallMod = new ToolStripMenuItem();
        ContextUninstallMod = new ToolStripMenuItem();
        ContextVisitModPage = new ToolStripMenuItem();
        ExtractionSelection = new OpenFileDialog();
        aToolStripMenuItem = new ToolStripMenuItem();
        aToolStripMenuItem1 = new ToolStripMenuItem();
        ButtonUninstall = new Button();
        MenuBar.SuspendLayout();
        ModListContextMenu.SuspendLayout();
        SuspendLayout();
        // 
        // SelectFolder
        // 
        SelectFolder.RootFolder = Environment.SpecialFolder.MyComputer;
        // 
        // MenuBar
        // 
        MenuBar.Items.AddRange(new ToolStripItem[] { MenuFile, MenuTools, MenuHelp });
        MenuBar.Location = new Point(0, 0);
        MenuBar.Name = "MenuBar";
        MenuBar.Size = new Size(700, 24);
        MenuBar.TabIndex = 0;
        MenuBar.Text = "MenuBar";
        // 
        // MenuFile
        // 
        MenuFile.DropDownItems.AddRange(new ToolStripItem[] { MenuFileInstallMod, MenuFileRefreshMods, MenuFileApplyMods, MenuFileSeperator, MenuFileExit });
        MenuFile.Name = "MenuFile";
        MenuFile.Size = new Size(37, 20);
        MenuFile.Text = "File";
        // 
        // MenuFileInstallMod
        // 
        MenuFileInstallMod.DropDownItems.AddRange(new ToolStripItem[] { MenuInstallFromURL, MenuInstallFromFolder });
        MenuFileInstallMod.Name = "MenuFileInstallMod";
        MenuFileInstallMod.Size = new Size(154, 22);
        MenuFileInstallMod.Text = "Install";
        // 
        // MenuInstallFromURL
        // 
        MenuInstallFromURL.Name = "MenuInstallFromURL";
        MenuInstallFromURL.Size = new Size(138, 22);
        MenuInstallFromURL.Text = "From URL";
        MenuInstallFromURL.Click += MenuInstallFromURL_Click;
        // 
        // MenuInstallFromFolder
        // 
        MenuInstallFromFolder.Name = "MenuInstallFromFolder";
        MenuInstallFromFolder.Size = new Size(138, 22);
        MenuInstallFromFolder.Text = "From Folder";
        MenuInstallFromFolder.Click += MenuInstallFromFolder_Click;
        // 
        // MenuFileRefreshMods
        // 
        MenuFileRefreshMods.Name = "MenuFileRefreshMods";
        MenuFileRefreshMods.ShortcutKeys = Keys.Control | Keys.R;
        MenuFileRefreshMods.Size = new Size(154, 22);
        MenuFileRefreshMods.Text = "Refresh";
        MenuFileRefreshMods.Click += MenuFileRefreshMods_Click;
        // 
        // MenuFileApplyMods
        // 
        MenuFileApplyMods.Name = "MenuFileApplyMods";
        MenuFileApplyMods.ShortcutKeys = Keys.Control | Keys.S;
        MenuFileApplyMods.Size = new Size(154, 22);
        MenuFileApplyMods.Text = "Apply";
        MenuFileApplyMods.Click += MenuFileApplyMods_Click;
        // 
        // MenuFileSeperator
        // 
        MenuFileSeperator.Name = "MenuFileSeperator";
        MenuFileSeperator.Size = new Size(151, 6);
        // 
        // MenuFileExit
        // 
        MenuFileExit.Name = "MenuFileExit";
        MenuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
        MenuFileExit.Size = new Size(154, 22);
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
        MenuToolsExtraction.Size = new Size(167, 22);
        MenuToolsExtraction.Text = "Extraction";
        MenuToolsExtraction.Click += MenuToolsExtraction_Click;
        // 
        // MenuToolsSeperator
        // 
        MenuToolsSeperator.Name = "MenuToolsSeperator";
        MenuToolsSeperator.Size = new Size(164, 6);
        // 
        // MenuToolsConfigure
        // 
        MenuToolsConfigure.DropDownItems.AddRange(new ToolStripItem[] { MenuToolsConfigureTFHFolder });
        MenuToolsConfigure.Name = "MenuToolsConfigure";
        MenuToolsConfigure.Size = new Size(167, 22);
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
        MenuToolsRegisterScheme.Size = new Size(167, 22);
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
        // ContextInstallFromFolder
        // 
        ContextInstallFromFolder.Name = "ContextInstallFromFolder";
        ContextInstallFromFolder.Size = new Size(138, 22);
        ContextInstallFromFolder.Text = "From Folder";
        ContextInstallFromFolder.Click += ContextInstallFromFolder_Click;
        // 
        // ContextInstallFromUrl
        // 
        ContextInstallFromUrl.Name = "ContextInstallFromUrl";
        ContextInstallFromUrl.Size = new Size(138, 22);
        ContextInstallFromUrl.Text = "From URL";
        ContextInstallFromUrl.Click += ContextInstallFromURL_Click;
        // 
        // ModList
        // 
        ModList.FormattingEnabled = true;
        ModList.Location = new Point(18, 34);
        ModList.Name = "ModList";
        ModList.Size = new Size(220, 292);
        ModList.TabIndex = 1;
        ModList.ItemCheck += ModList_ItemCheck;
        ModList.SelectedIndexChanged += ModList_SelectedIndexChanged;
        // 
        // ApplyButton
        // 
        ApplyButton.Location = new Point(244, 296);
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
        // ModListContextMenu
        // 
        ModListContextMenu.Items.AddRange(new ToolStripItem[] { MenuInstallMod, ContextUninstallMod, ContextVisitModPage });
        ModListContextMenu.Name = "contextMenuStrip1";
        ModListContextMenu.Size = new Size(154, 70);
        // 
        // MenuInstallMod
        // 
        MenuInstallMod.DropDownItems.AddRange(new ToolStripItem[] { ContextInstallFromFolder, ContextInstallFromUrl });
        MenuInstallMod.Name = "MenuInstallMod";
        MenuInstallMod.Size = new Size(153, 22);
        MenuInstallMod.Text = "Install";
        // 
        // ContextUninstallMod
        // 
        ContextUninstallMod.Name = "ContextUninstallMod";
        ContextUninstallMod.Size = new Size(153, 22);
        ContextUninstallMod.Text = "Uninstall";
        ContextUninstallMod.Click += UninstallMod_Click;
        // 
        // ContextVisitModPage
        // 
        ContextVisitModPage.Name = "ContextVisitModPage";
        ContextVisitModPage.Size = new Size(153, 22);
        ContextVisitModPage.Text = "Visit Mod Page";
        ContextVisitModPage.Click += VisitModPage_Click;
        // 
        // ExtractionSelection
        // 
        ExtractionSelection.Filter = "TFHResource|*.tfhres|Reverge Package File|*.gfs|All files|*.*";
        ExtractionSelection.Multiselect = true;
        // 
        // aToolStripMenuItem
        // 
        aToolStripMenuItem.Name = "aToolStripMenuItem";
        aToolStripMenuItem.Size = new Size(32, 19);
        // 
        // aToolStripMenuItem1
        // 
        aToolStripMenuItem1.Name = "aToolStripMenuItem1";
        aToolStripMenuItem1.Size = new Size(32, 19);
        // 
        // ButtonUninstall
        // 
        ButtonUninstall.Enabled = false;
        ButtonUninstall.Location = new Point(325, 296);
        ButtonUninstall.Name = "ButtonUninstall";
        ButtonUninstall.Size = new Size(75, 23);
        ButtonUninstall.TabIndex = 6;
        ButtonUninstall.Text = "Uninstall";
        ButtonUninstall.UseVisualStyleBackColor = true;
        ButtonUninstall.Click += ButtonUninstall_Click;
        // 
        // MainForm
        // 
        AccessibleDescription = "Them's Fightin' Herds Mod Loader";
        AccessibleName = "VelvetBeautifier";
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(700, 338);
        Controls.Add(ButtonUninstall);
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
        ModListContextMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FolderBrowserDialog SelectFolder;
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
    private ContextMenuStrip ModListContextMenu;
    private ToolStripMenuItem ContextUninstallMod;
    private ToolStripMenuItem MenuInstallMod;
    private ToolStripMenuItem ContextVisitModPage;
    private OpenFileDialog ExtractionSelection;
    private ToolStripMenuItem ContextInstallFromFolder;
    private ToolStripMenuItem ContextInstallFromUrl;
    private ToolStripMenuItem aToolStripMenuItem;
    private ToolStripMenuItem aToolStripMenuItem1;
    private ToolStripMenuItem MenuInstallFromURL;
    private ToolStripMenuItem MenuInstallFromFolder;
    private Button ButtonUninstall;
}

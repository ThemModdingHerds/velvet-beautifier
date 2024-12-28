using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public partial class InstallTextModForm : Form, IMainFormItem
{
    private readonly MainForm mainForm;
    public MainForm MainForm => mainForm;
    private readonly TextBox url;
    private readonly Button installButton;
    public InstallTextModForm(MainForm parent)
    {
        mainForm = parent;
        url = new()
        {
            PlaceholderText = Velvet.Velvetify("URL, GameBanana ID")
        };
        url.TextChanging += OnInputChanged;
        installButton = new(OnInstallButton)
        {
            Text = Velvet.Velvetify("Install")
        };
        Content = new StackLayout()
        {
            Items = {
                url,
                installButton
            }
        };
    }
    private void OnInputChanged(object? sender,TextChangingEventArgs e)
    {
        installButton.Enabled = int.TryParse(e.Text,out int _) || Url.IsUrl(e.Text);
    }
    private void OnInstallButton(object? sender, EventArgs e)
    {
        mainForm.JoinThread(InstallMod);
    }
    private void InstallMod()
    {
        ModInstallResult result = ModDB.InstallMod(url.Text);
        switch(result)
        {
            case ModInstallResult.Ok:
            case ModInstallResult.AlreadyExists:
                mainForm.ModList.RefreshModList();
                break;
            case ModInstallResult.Invalid:
            case ModInstallResult.Failed:
                VelvetEto.ShowMessageBox("Error installing mod","couldn't install mod! Make sure the text is a valid URL/GameBanana ID");
                break;
        }
        Close();
    }
}
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
    private async void OnInstallButton(object? sender, EventArgs e)
    {
        ModInstallResult result = await ModDB.InstallMod(url.Text);
        mainForm.ModList.RefreshModList();
        Close();
    }
}
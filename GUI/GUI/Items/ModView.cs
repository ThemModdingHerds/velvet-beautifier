using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ModView : TableLayout, IMainFormItem
{
    public MainForm MainForm {get;}
    public string Title {get => ModEnabled.Text;set => ModEnabled.Text = value;}
    public Label Description {get;} = new Label();
    public Label Author {get;} = new Label();
    public CheckBox ModEnabled {get;} = new CheckBox();
    private Mod? current = null;
    public ModView(MainForm parent)
    {
        MainForm = parent;
        Visible = false;
        Rows.Add(new([ModEnabled,Author]));
        Rows.Add(new([Description]));
        //Items.Add(new StackLayoutItem(Title));
        //Items.Add(new StackLayoutItem(Description));
        //Items.Add(new StackLayoutItem(ModEnabled));
        ModEnabled.CheckedChanged += OnCheckBoxChanged;
    }
    private void OnCheckBoxChanged(object? sender, EventArgs e)
    {
        if(ModEnabled.Checked.HasValue)
            current?.SetEnabled(ModEnabled.Checked.Value);
    }
    public void SetContent(Mod? mod)
    {
        current = mod;
        if(mod == null)
        {
            Visible = false;
            Title = Author.Text = Description.Text = string.Empty;
            ModEnabled.Checked = false;
            return;
        }
        Visible = true;
        Title = Velvet.Velvetify(mod.Info.Name);
        Author.Text = Velvet.Velvetify($"by {mod.Info.Author}");
        Description.Text = Velvet.Velvetify(mod.Info.Description);
        ModEnabled.Checked = mod.Enabled;
    }
}
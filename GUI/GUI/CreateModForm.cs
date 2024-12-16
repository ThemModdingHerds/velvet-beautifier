using System;
using System.IO;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public partial class CreateModForm : Form, IMainFormItem
{
    private readonly MainForm mainForm;
    public MainForm MainForm => mainForm;
    private readonly Property id = new("Id","a unique identifier for the mod");
    private readonly Property name = new("Name","the reader-friendly name of the mod");
    private readonly Property author = new("Author","the person/people who made this mod");
    private readonly Property desc = new("Description","a description of what the mod does");
    public CreateModForm(MainForm parent)
    {
        mainForm = parent;
        TableLayout properties = new()
        {
            
            Rows = {
                id,
                name,
                author,
                desc
            }
        };
        Button create = new(OnCreate)
        {
            Text = Velvet.Velvetify("Create")
        };
        Content = new StackLayout(properties,create);
        Title = Velvet.Velvetify("Create a new mod");
    }
    private void OnCreate(object? sender,EventArgs e)
    {
        ModInfo info = CreateModInfo();
        Mod mod = Mod.Create(info);
        mainForm.ModLoaderTool.ModDB.InstallMod(mod);
        VelvetEto.ShowMessageBox($"Created mod '{info.Name}'");
        Close();
    }
    private ModInfo CreateModInfo()
    {
        return new()
        {
            Id = id.Value,
            Name = name.Value,
            Author = author.Value,
            Description = desc.Value
        };
    }
    public class Property : TableRow
    {
        public Label Label {get;} = new();
        public TextBox TextBox {get;} = new();
        public string Value {get => TextBox.Text;}
        public Property(string name,string tooltip)
        {
            ScaleHeight = true;
            Label.Text = Velvet.Velvetify(name);
            TextBox.ToolTip = Velvet.Velvetify(tooltip);
            Cells.Add(Label);
            Cells.Add(TextBox);
        }
    }
}
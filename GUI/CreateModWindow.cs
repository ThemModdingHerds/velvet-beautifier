using Gtk;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public class CreateModWindow : Window
{
    public const string ID = "CreateModWindow";
    public Button CreateButton {get;}
    public Entry IdentifierEntry {get;}
    public Entry NameEntry {get;}
    public Entry AuthorEntry {get;}
    public Entry DescriptionEntry {get;}
    public CreateModWindow(): this(new Builder(VelvetGtk.GLADEFILE)) {}
    private CreateModWindow(Builder builder): base(builder.GetRawOwnedObject(ID))
    {
        Icon = Utils.VelvetIcon;
        builder.Autoconnect(this);

        CreateButton = new(builder.GetRawOwnedObject("ModCreateButton"));
        IdentifierEntry = new(builder.GetRawOwnedObject("ModIdentifierEntry"));
        NameEntry = new(builder.GetRawOwnedObject("ModNameEntry"));
        AuthorEntry = new(builder.GetRawOwnedObject("ModAuthorEntry"));
        DescriptionEntry = new(builder.GetRawOwnedObject("ModDescEntry"));

        IdentifierEntry.Changed += OnTextChange;
        NameEntry.Changed += OnTextChange;
        AuthorEntry.Changed += OnTextChange;
        DescriptionEntry.Changed += OnTextChange;
        CreateButton.Clicked += OnCreateButton;
    }
    private void OnTextChange(object? sender,EventArgs e) => UpdateButtonSensitive();
    private void UpdateButtonSensitive()
    {
        bool id = IdentifierEntry.TextLength != 0;
        bool name = NameEntry.TextLength != 0;
        bool author = AuthorEntry.TextLength != 0;
        bool desc = DescriptionEntry.TextLength != 0;
        CreateButton.Sensitive = id && name && author && desc;
    }
    private void OnCreateButton(object? sender,EventArgs e)
    {
        string id = IdentifierEntry.Text;
        string name = NameEntry.Text;
        string author = AuthorEntry.Text;
        string desc = DescriptionEntry.Text;
        ModDB.Create(new ModInfo()
        {
            Id = id,
            Name = name,
            Author = author,
            Description = desc
        });
        Destroy();
    }
}
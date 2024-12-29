using Gtk;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public class CreateModWindow : Window
{
    public Grid Root => (Grid)Children[0];
    public Button CreateButton => (Button)Root.Children[0];
    public Entry IdentifierEntry => (Entry)Root.Children[8];
    public Entry NameEntry => (Entry)Root.Children[7];
    public Entry AuthorEntry => (Entry)Root.Children[6];
    public Entry DescriptionEntry => (Entry)Root.Children[5];
    public CreateModWindow(): this(new Builder("MainWindow.glade")) {}
    private CreateModWindow(Builder builder): base(builder.GetRawOwnedObject("CreateModWindow"))
    {
        builder.Autoconnect(this);
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
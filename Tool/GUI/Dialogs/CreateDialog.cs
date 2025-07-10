using System.Collections;
using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;

public static class CreateDialog
{
    public enum Type
    {
        RevergePackage,
        TFHResource,
        Mod
    }
    public static Action Show(Type type)
    {
        return type switch
        {
            Type.RevergePackage => CreateGFS,
            Type.TFHResource => CreateTFHRES,
            Type.Mod => CreateModDialog.Show,
            _ => throw new Exception()
        };
    }
    private static void CreateGFS()
    {
        OpenDialog dialog = new("Create Reverge Package", "Create a Reverge Package from a folder", null, OpenDialog.OpenMode.Directory)
        {
            AllowsMultipleSelection = false
        };
        Application.Run(dialog);
        string? folderpath = dialog.FilePath.ToString();
        if (folderpath == null || !Directory.Exists(folderpath)) return;
        bool result = GFS.Utils.Create(folderpath, $"{folderpath}.gfs");
        if (result)
            MessageBox.Query("Created Reverge Package", "Created a Reverge Package file from the provided folder", "Ok");
    }
    private static void CreateTFHRES()
    {
        SaveDialog dialog = new("Create empty TFHResource", "Create a new empty TFHResource file", null);
        Application.Run(dialog);
        string? filepath = dialog.FilePath.ToString();
        bool result = TFHResource.Utils.CreateEmpty(filepath);
        if (result)
            MessageBox.Query("Created empty TFHResource", "Created a empty TFHResource file at the provided filepath", "Ok");
    }
}
public class CreateModDialog : Dialog
{
    private class CreateButton : Button
    {
        public CreateButton(Action onCreate) : base("Create", true)
        {
            Clicked += onCreate;
        }
    }
    private class CancelButton : Button
    {
        public CancelButton() : base("Cancel")
        {
            Clicked += () => Application.RequestStop();
        }
    }
    private CreateButton _create_button;
    private const string IdInputLabel = "Id";
    private readonly Label _id_input_label = new(IdInputLabel)
    {
        X = 1,
        Y = 1,
        Width = Dim.Fill()
    };
    private readonly TextField _id_input = new()
    {
        X = IdInputLabel.Length + 2,
        Y = 1,
        Width = Dim.Fill(1)
    };
    private const string NameInputLabel = "Name";
    private readonly Label _name_input_label = new(NameInputLabel)
    {
        X = 1,
        Y = 2,
        Width = Dim.Fill(),
    };
    private readonly TextField _name_input = new()
    {
        X = NameInputLabel.Length + 2,
        Y = 2,
        Width = Dim.Fill(1)
    };
    private const string AuthorInputLabel = "Author";
    private readonly Label _author_input_label = new(AuthorInputLabel)
    {
        X = 1,
        Y = 3,
        Width = Dim.Fill()
    };
    private readonly TextField _author_input = new()
    {
        X = AuthorInputLabel.Length + 2,
        Y = 3,
        Width = Dim.Fill(1),
        Text = Environment.UserName
    };
    private const string DescriptionInputLabel = "Description";
    private readonly Label _desc_input_label = new(DescriptionInputLabel)
    {
        X = 1,
        Y = 4,
        Width = Dim.Fill()
    };
    private readonly TextField _desc_input = new()
    {
        X = DescriptionInputLabel.Length + 2,
        Y = 4,
        Width = Dim.Fill(1)
    };
    public static void Show() => Application.Run(new CreateModDialog());
    public CreateModDialog() : base("Create new mod", 40, 10)
    {
        // id, name, author, description
        _create_button = new(OnCreate);
        Add(
            _id_input_label, _id_input,
            _name_input_label, _name_input,
            _author_input_label, _author_input,
            _desc_input_label, _desc_input
        );
        AddButton(_create_button);
        AddButton(new CancelButton());
    }
    private void OnCreate()
    {
        ModDB.Create(new ModInfo()
        {
            Id = _id_input.Text.ToString() ?? throw new Exception(),
            Name = _name_input.Text.ToString() ?? throw new Exception(),
            Author = _author_input.Text.ToString() ?? throw new Exception(),
            Description = _desc_input.Text.ToString() ?? throw new Exception()
        });
        Application.RequestStop();
    }
}
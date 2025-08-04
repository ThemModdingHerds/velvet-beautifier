using Terminal.Gui;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;

public class ProcessDialog : Dialog
{
    private readonly Label _content = new()
    {
        X = Pos.Center(),
        Y = Pos.Center(),
    };
    private readonly Action _process;
    private readonly string _done;
    public static void Show(Action process, string working, string done)
    {
        ProcessDialog dialog = new(process, working, done);
        Application.Run(dialog);
    }
    public ProcessDialog(Action process, string working, string done) : base("Processing...", 40, 8)
    {
        _process = process;
        _done = done;
        _content.Text = working;
        Add(_content);
        Loaded += OnLoad;
    }
    private void OnLoad()
    {
        Thread t = new(Process);
        t.Start();
    }
    private void Process()
    {
        try
        {
            _process();
            Title = "Done!";
            _content.Text = _done;
        }
        catch (Exception)
        {
            _content.Text = "Something went wrong! Contact developers!";
        }
        Button ok = new("Ok", true);
        ok.Clicked += () => Application.RequestStop();
        AddButton(ok);
    }
}
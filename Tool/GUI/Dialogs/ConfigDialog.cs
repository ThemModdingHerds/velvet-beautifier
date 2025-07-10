using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;

public class ConfigDialog : Dialog
{
    private class ApplyButton : Button
    {
        public ApplyButton(Action onApply) : base("Apply", true)
        {
            Clicked += onApply;
        }
    }
    private class BackButton : Button
    {
        public BackButton() : base("Back")
        {
            Clicked += () => Application.RequestStop();
        }
    }
    private const string ClientPathLabel = "Client Path";
    private readonly Label _client_path_label = new(ClientPathLabel)
    {
        X = 1,
        Y = 1,
        Width = Dim.Fill(),
        Height = 1
    };
    private readonly TextField _client_path = new()
    {
        X = ClientPathLabel.Length + 2,
        Y = 1,
        Width = Dim.Fill(1),
        Height = Dim.Fill(),
        Text = Config.ClientPath ?? ""
    };
    private const string ServerPathLabel = "Server Path";
    private readonly Label _server_path_label = new(ServerPathLabel)
    {
        X = 1,
        Y = 2,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    private readonly TextField _server_path = new()
    {
        X = ServerPathLabel.Length + 2,
        Y = 2,
        Width = Dim.Fill(1),
        Height = Dim.Fill(),
        Text = Config.ServerPath ?? ""
    };
    private readonly ApplyButton _apply_button;
    public static void Show() => Application.Run(new ConfigDialog());
    private ConfigDialog() : base("Configuration",30,8)
    {
        _apply_button = new(OnApply);
        _client_path.TextChanged += (path) => _apply_button.Enabled = Client.Valid(_client_path.Text.ToString()) || _client_path.Text == NStack.ustring.Empty;
        _server_path.TextChanged += (path) => _apply_button.Enabled = Server.Valid(_server_path.Text.ToString()) || _server_path.Text == NStack.ustring.Empty;
        Add(_client_path_label, _client_path, _server_path_label, _server_path);
        AddButton(_apply_button);
        AddButton(new BackButton());
    }
    private void OnApply()
    {
        if (_client_path.Text != string.Empty)
            Config.ClientPath = _client_path.Text.ToString();
        if (_server_path.Text != string.Empty)
            Config.ServerPath = _server_path.Text.ToString();
        Config.Write();
        Application.RequestStop();
    }
}
namespace ThemModdingHerds.VelvetBeautifier;

public partial class MainForm : Form
{
    private readonly Config _config = Config.Read();
    public MainForm()
    {
        InitializeComponent();
        if(!_config.ExistsTFHFolder())
        {
            MessageBox.Show("No Them's Fightin' Herds Folder found! Please select the folder");
            DialogResult result = FindTFHFolder.ShowDialog();
            if(result == DialogResult.OK)
            {
                _config.TfhPath = FindTFHFolder.SelectedPath;
                _config.Save();
            }
        }
    }
}

using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.GameBanana;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;
using Mod = ThemModdingHerds.VelvetBeautifier.Modding.Mod;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class ModListView : FrameView
{
    public enum Mode
    {
        Local,
        Online
    }
    public interface IModItem
    {
        string Name { get; }
        string Author { get; }
        string Description { get; }
        string Version { get; }
        Mode FromMode { get; }
    }
    public class LocalModItem(Mod mod) : IModItem
    {
        public Mod Mod => mod;
        public string Name => mod.Info.Name;
        public string Author => mod.Info.Author;
        public string Description => mod.Info.Description;
        public string Version => mod.Info.Version.ToString();
        public Mode FromMode => Mode.Local;
        public override string ToString()
        {
            return Name;
        }
    }
    public class OnlineModItem(Search.Record record) : IModItem
    {
        private readonly string desc = GameBanana.Mod.Fetch(record.Id)?.Body ?? string.Empty;
        public Search.Record Record => record;
        public string Name => record.Name;
        public string Author => record.User.Name;
        public string Description => desc;
        public string Version => record.Version;
        public Mode FromMode => Mode.Online;
        public override string ToString()
        {
            return Name;
        }
    }
    public event Action<IModItem>? OnModSelect;
    public event Action<Mode>? OnModeChange;
    private ListView _list = new()
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    private Mode _mode = Mode.Local;
    public ModListView() : base("Mods")
    {
        Add(_list);
        Refresh();
        IModItem? mod = (IModItem?)_list.Source.ToList()[0];
        if (mod != null)
            OnModSelect?.Invoke(mod);
        _list.SelectedItemChanged += OnItemSelected;
    }
    private void OnItemSelected(ListViewItemEventArgs args)
    {
        OnModSelect?.Invoke((IModItem)args.Value);
    }
    private void SetMods(List<IModItem> mods) => _list.SetSource(mods);
    public void Refresh(Mode mode)
    {
        _mode = mode;
        OnModeChange?.Invoke(_mode);
        Refresh();
    }
    public void Refresh()
    {
        switch (_mode)
        {
            case Mode.Local:
                SetMods([.. ModDB.Mods.Select(mod => new LocalModItem(mod))]);
                break;
            case Mode.Online:
                SetMods([.. Search.FetchMods().Select(record => new OnlineModItem(record))]);
                break;
        }
    }
}
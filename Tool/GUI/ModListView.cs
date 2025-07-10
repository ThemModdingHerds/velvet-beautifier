using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.GameBanana;
using ThemModdingHerds.VelvetBeautifier.Modding;
using Mod = ThemModdingHerds.VelvetBeautifier.Modding.Mod;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class ModListView : ListView
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
        public Search.Record Record => record;
        public string Name => record.Name;
        public string Author => record.User.Name;
        public string Description => GameBanana.Mod.Fetch(record.Id)?.Body ?? string.Empty;
        public string Version => record.Version;
        public Mode FromMode => Mode.Online;
        public override string ToString()
        {
            return Name;
        }
    }
    public delegate void ModSelectHandler(IModItem item);
    public event ModSelectHandler? OnModSelect;
    private Mode _mode = Mode.Local;
    public ModListView()
    {
        Refresh();
        SelectedItemChanged += OnItemSelected;
    }

    private void OnItemSelected(ListViewItemEventArgs args)
    {
        OnModSelect?.Invoke((IModItem)args.Value);
    }

    private void SetMods(List<IModItem> mods) => SetSource(mods);
    public void Refresh(Mode mode)
    {
        _mode = mode;
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
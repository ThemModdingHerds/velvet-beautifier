using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public static class StatusBarUtilities
{
    public static StatusBar Create(ModListView.IModItem item)
    {
        return item.FromMode switch
        {
            ModListView.Mode.Local => CreateLocalMods((ModListView.LocalModItem)item),
            ModListView.Mode.Online => CreateOnlineMods((ModListView.OnlineModItem)item),
            _ => throw new Exception(),
        };
    }
    private static StatusBar CreateOnlineMods(ModListView.OnlineModItem item)
    {
        void OnInstall()
        {
            if (MessageBox.Query("Install", $"Are you sure you want to install {item.Name}?", "Yes", "No") != 0)
                return;
            ProcessDialog.Show(() => ModDB.InstallGameBananaMod(item.Record.Id), "Downloading & installing...", "Installed mod!");
        }
        return new([
            new StatusItem(Key.ShiftMask | Key.I,"Install",OnInstall),
            new StatusItem(Key.ShiftMask | Key.O,"Open mod page",() => Url.OpenUrl(item.Record.Url))
        ]);
    }
    private static StatusBar CreateLocalMods(ModListView.LocalModItem item)
    {
        void OnUninstall()
        {
            if (MessageBox.Query("Remove", $"Are you sure you want to remove {item.Name}?", "Yes", "No") != 0)
                return;
            ProcessDialog.Show(() => ModDB.UninstallMod(item.Mod), "Uninstalling...", "Uninstalled mod!");
            MainTopLevel.Instance.modList.Refresh();
        }
        return new([
            new StatusItem(Key.ShiftMask | Key.R,"Remove",OnUninstall),
            new StatusItem(Key.ShiftMask | Key.O,"Show folder",() => FileSystem.OpenFolder(item.Mod.Folder))
        ]);
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThemModdingHerds.VelvetBeautifier
{
    public partial class DownloadForm : Form
    {
        private string? Url {get;}
        private string? UnzippedPath {get;}
        public DownloadForm(GameBananaMod mod,string url)
        {
            InitializeComponent();
            Url = url;
            Text = "Install " + mod.ModName + '?';
            ModNameLabel.Text = mod.ModName;
            ModAuthorLabel.Text = "by " + mod.OwnerName;
            ModDescriptionBox.Text = mod.Body;
        }
        public DownloadForm(Mod mod,string unzippedpath)
        {
            InitializeComponent();
            UnzippedPath = unzippedpath;
            Text = "Install " + mod.Info.Name;
            ModNameLabel.Text = mod.Info.Name;
            ModAuthorLabel.Text = "by " + mod.Info.Author;
            ModDescriptionBox.Text = mod.Info.Description;
        }
        public DownloadForm(GameBananaMod mod): this(mod,mod.Files.Values.ToArray()[0].DownloadUrl)
        {

        }
    }
}

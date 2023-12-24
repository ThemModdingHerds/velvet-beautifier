using ThemModdingHerds.Resource;

namespace ThemModdingHerds.VelvetBeautifier
{
    public partial class ExtractionForm : Form
    {
        private string extractFilePath = "";
        public ExtractionForm()
        {
            InitializeComponent();
            ExtractButton.Enabled = false;
        }
        private void ExtractButton_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectOutputFolder.ShowDialog();
            if(result == DialogResult.OK)
            {
                try
                {
                    switch(FileFormat.Text)
                    {
                        case "tfhres":
                            ExtractTFHRES(SelectOutputFolder.SelectedPath);
                            break;
                        default:
                            Velvet.ShowMessageBox("You didn't choose an file format","No file format specified");
                            break;
                    }
                    Velvet.ShowMessageBox("Done","Extraction");
                }
                catch(Exception err)
                {
                    Velvet.ShowMessageBox(err.ToString());
                }
            }
        }
        private void ExtractSelectFile_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                extractFilePath = SelectFileDialog.FileName;
                ExtractButton.Enabled = true;
                return;
            }
        }
        private void ExtractTFHRES(string output)
        {
            static string find_path(CachedImage image, List<CacheRecord> records)
            {
                foreach (CacheRecord record in records)
                    if (image.ShortName == record.ShortName)
                        return record.SourcePath;
                return image.ShortName.Replace("database:/","");
            }
            Database db = new(extractFilePath);
            List<CacheRecord> records = db.GetCacheRecords();
            List<CachedImage> images = db.GetCachedImages();
            List<CachedTextfile> textfiles = db.GetCachedTextfiles();
            foreach(CachedImage image in images)
            {
                string filepath = Path.Combine(output,find_path(image,records));
                string? dirpath = Path.GetDirectoryName(filepath);
                if(dirpath == null)
                {
                    continue;
                }
                Directory.CreateDirectory(dirpath);
                File.WriteAllBytes(filepath,image.ImageData);
            }
            foreach(CachedTextfile textfile in textfiles)
            {
                string filepath = Path.Combine(output,textfile.SourceFile);
                string? dirpath = Path.GetDirectoryName(filepath);
                if(dirpath == null)
                {
                    continue;
                }
                Directory.CreateDirectory(dirpath);
                File.WriteAllBytes(filepath,textfile.TextData);
            }
        }
    }
}

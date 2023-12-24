namespace ThemModdingHerds.VelvetBeautifier.forms
{
    partial class InstallForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallForm));
            InstallModUrl = new TextBox();
            FetchButton = new Button();
            SuspendLayout();
            // 
            // InstallModUrl
            // 
            InstallModUrl.Location = new Point(12, 12);
            InstallModUrl.Name = "InstallModUrl";
            InstallModUrl.PlaceholderText = "GameBanana or Zip File Link";
            InstallModUrl.Size = new Size(272, 23);
            InstallModUrl.TabIndex = 0;
            InstallModUrl.TextChanged += InstallModUrl_TextChanged;
            // 
            // FetchButton
            // 
            FetchButton.Enabled = false;
            FetchButton.Location = new Point(12, 41);
            FetchButton.Name = "FetchButton";
            FetchButton.Size = new Size(270, 23);
            FetchButton.TabIndex = 1;
            FetchButton.Text = "Fetch";
            FetchButton.UseVisualStyleBackColor = true;
            FetchButton.Click += FetchButton_Click;
            // 
            // InstallForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(294, 78);
            Controls.Add(FetchButton);
            Controls.Add(InstallModUrl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "InstallForm";
            Text = "Install Mod";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InstallModUrl;
        private Button FetchButton;
    }
}
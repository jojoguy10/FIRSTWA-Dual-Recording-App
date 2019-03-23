using System;
using System.IO;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    public partial class YoutubeUpload : Form
    {

        public string programFileName;
        public string wideFileName;

        private bool wideFileGrabbed, programFileGrabbed = false;

        public YoutubeUpload(string program, string wide, string description, string tags)
        {
            InitializeComponent();

            

            programFileName = program;
            wideFileName = wide;

            txtProgramTitle.Text = program.Replace(".mp4", "");
            txtWideTitle.Text = wide.Replace(".mp4", "");
            txtDescription.Text = description.Replace("\n", "\r\n");
            txtTags.Text = tags;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            (sender as TextBox).SelectAll();
            Clipboard.SetText((sender as TextBox).Text);
        }

        private void btnCopyProgram_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string[] init = new string[] { @"C:\Temp\" + programFileName };

                DataObject dobj = new DataObject(DataFormats.FileDrop, init);
                btnCopyProgram.DoDragDrop(dobj, DragDropEffects.All);

                programFileGrabbed = true;
            }
        }
        private void btnCopyWide_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string[] init = new string[] { @"C:\Temp\" + wideFileName };

                DataObject dobj = new DataObject(DataFormats.FileDrop, init);
                btnCopyProgram.DoDragDrop(dobj, DragDropEffects.All);

                wideFileGrabbed = true;
            }
        }

        private void YoutubeUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!programFileGrabbed || !wideFileGrabbed)
            {
                var result = MessageBox.Show("WARNING: It does not look like you copied both files!\n\nDo you want to close, anyway?", "WARNING", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void YoutubeUpload_Load(object sender, EventArgs e)
        {

        }
    }
}

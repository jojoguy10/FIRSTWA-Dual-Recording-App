using System;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    public partial class YoutubeUpload : Form
    {

        public string programFileName;
        public string wideFileName;

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

        //private void TextBox_Click(object sender, EventArgs e)
        //{
        //    (sender as TextBox).SelectAll();
        //    Clipboard.SetText((sender as TextBox).Text);
        //}

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}

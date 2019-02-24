using System;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    public partial class YoutubeUpload : Form
    {
        public YoutubeUpload(string programTitle, string wideTitle, string description, string tags)
        {
            InitializeComponent();

            txtProgramTitle.Text = programTitle;
            txtWideTitle.Text = wideTitle;
            txtDescription.Text = description.Replace("\n", "\r\n");
            txtTags.Text = tags;
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            (sender as TextBox).SelectAll();
            Clipboard.SetText((sender as TextBox).Text);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

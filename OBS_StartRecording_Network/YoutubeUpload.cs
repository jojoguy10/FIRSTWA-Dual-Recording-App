using System;
using System.IO;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    public partial class YoutubeUpload : Form
    {

        public string programFileName;
        public string wideFileName;
        public UploadManager ManagingWindow;

        public YoutubeUpload(string program, string wide, string description, string tags, UploadManager _ManagingWindow)
        {
            InitializeComponent();

            programFileName = program;
            wideFileName = wide;

            txtProgramTitle.Text = program.Replace(".mp4", "");
            txtWideTitle.Text = wide.Replace(".mp4", "");
            txtDescription.Text = description.Replace("\n", "\r\n");
            txtTags.Text = tags;

            ManagingWindow = _ManagingWindow;
        }

        //private void TextBox_Click(object sender, EventArgs e)
        //{
        //    (sender as TextBox).SelectAll();
        //    Clipboard.SetText((sender as TextBox).Text);
        //}

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            YoutubeJob programJob = new YoutubeJob(programFileName, txtProgramTitle.Text, txtDescription.Text, txtTags.Text, @"");
            YoutubeJob wideJob = new YoutubeJob(wideFileName, txtWideTitle.Text, txtDescription.Text, txtTags.Text, @"");

            ManagingWindow.addVideo(programJob);
            ManagingWindow.addVideo(wideJob);
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            File.Delete(@"C:\Temp\" + programFileName);
            File.Delete(@"C:\Temp\" + wideFileName);
            Close();
        }

    }
}

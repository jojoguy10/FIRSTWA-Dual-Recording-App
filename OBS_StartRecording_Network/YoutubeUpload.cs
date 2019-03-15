using System;
using System.IO;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            File.Delete(@"C:\Temp\" + programFileName);
            File.Delete(@"C:\Temp\" + wideFileName);
            Close();
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            (sender as TextBox).SelectAll();
            Clipboard.SetText((sender as TextBox).Text);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                button1.DoDragDrop("Here's some text!",
                    DragDropEffects.Copy);
            }
        }
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                button1.DoDragDrop("Here's some text!",
                    DragDropEffects.Copy);
            }
        }
    }
}

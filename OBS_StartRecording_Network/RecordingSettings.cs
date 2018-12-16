using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OBS_StartRecording_Network
{
    public partial class RecordingSettings : Form
    {
        public string IPAddressPROGRAM { get; private set; } = @"192.168.0.104";
        public string IPAddressWIDE { get; private set; } = @"192.168.0.103";
        public string Password { get; private set; } = @"password";
        public string Folder { get; private set; } = @"D:\__USER\Videos\VM Captures";
        public int PortPROGRAM { get; private set; } = 4444;
        public int PortWIDE { get; private set; } = 4445;

        public RecordingSettings()
        {
            InitializeComponent();

            txtFolderLocation.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "VM Captures");
        }


        private void btnAccept_Click(object sender, EventArgs e)
        {
            IPAddressPROGRAM = txtIPAddressPROGRAM.Text;
            IPAddressWIDE = txtIPAddressWIDE.Text;
            Password = txtPassword.Text;
            PortPROGRAM = (int)numPort1.Value;
            PortWIDE = (int)numPort2.Value;
            Folder = txtFolderLocation.Text;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            switch (result)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    txtFolderLocation.Text = folderBrowserDialog1.SelectedPath;
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    txtFolderLocation.Text = folderBrowserDialog1.SelectedPath;
                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }
        }
    }
}

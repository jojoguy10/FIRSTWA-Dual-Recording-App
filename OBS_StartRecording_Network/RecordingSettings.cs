using System;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    public partial class RecordingSettings : Form
    {
        public string IPAddressPROGRAM { get; private set; } = @"192.168.100.35";
        public string IPAddressWIDE { get; private set; } = @"192.168.100.34";
        public string IPAddressPC { get; private set; } = @"192.168.100.70";
        
        public RecordingSettings(string pc, string program, string wide)
        {
            InitializeComponent();

            IPAddressPROGRAM = pc;
            IPAddressPROGRAM = program;
            IPAddressWIDE = wide;

            txtIPAddressPC.Text = pc;
            txtIPAddressPROGRAM.Text = program;
            txtIPAddressWIDE.Text = wide;
        }


        private void btnAccept_Click(object sender, EventArgs e)
        {
            IPAddressPROGRAM = txtIPAddressPROGRAM.Text;
            IPAddressWIDE = txtIPAddressWIDE.Text;
            IPAddressPC = txtIPAddressPC.Text;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

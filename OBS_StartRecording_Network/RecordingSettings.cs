using System;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    public partial class RecordingSettings : Form
    {
        public string Year { get; private set; } = "2020";
        public string IPAddressPROGRAM { get; private set; } = @"192.168.100.35";
        public string IPAddressWIDE { get; private set; } = @"192.168.100.34";
        public string IPAddressPC { get; private set; } = @"192.168.100.70";
        
        public RecordingSettings(string year, string pc, string program, string wide)
        {
            InitializeComponent();

            Year = year;
            IPAddressPROGRAM = pc;
            IPAddressPROGRAM = program;
            IPAddressWIDE = wide;

            numYear.Value = Convert.ToInt16(year);
            txtIPAddressPC.Text = pc;
            txtIPAddressPROGRAM.Text = program;
            txtIPAddressWIDE.Text = wide;
        }


        private void btnAccept_Click(object sender, EventArgs e)
        {
            Year = numYear.Value.ToString();
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

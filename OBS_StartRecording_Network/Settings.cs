using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OBS_StartRecording_Network
{
    public partial class Settings : Form
    {
        public string IPAddressPROGRAM { get; private set; } = @"192.168.0.103";
        public string IPAddressWIDE { get; private set; } = @"192.168.0.104";
        public string Password { get; private set; } = @"password";
        public int PortPROGRAM { get; private set; } = 4444;
        public int PortWIDE { get; private set; } = 4445;

        public Settings()
        {
            InitializeComponent();
        }


        private void btnAccept_Click(object sender, EventArgs e)
        {
            IPAddressPROGRAM = txtIPAddressPROGRAM.Text;
            IPAddressWIDE = txtIPAddressWIDE.Text;
            Password = txtPassword.Text;
            PortPROGRAM = (int)numPort1.Value;
            PortWIDE = (int)numPort2.Value;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }
    }
}

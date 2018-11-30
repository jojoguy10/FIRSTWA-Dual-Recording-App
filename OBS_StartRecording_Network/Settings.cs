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
        public string IPAddress { get; private set; } = @"192.168.100.10";
        public string Password { get; private set; } = @"password";
        public int Port1 { get; private set; } = 4444;
        public int Port2 { get; private set; } = 4445;

        public Settings()
        {
            InitializeComponent();
        }


        private void btnAccept_Click(object sender, EventArgs e)
        {
            IPAddress = txtIPAddress.Text;
            Password = txtPassword.Text;
            Port1 = (int)numPort1.Value;
            Port2 = (int)numPort2.Value;
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

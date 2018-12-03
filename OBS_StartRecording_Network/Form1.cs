using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using OBSWebsocketDotNet;

namespace OBS_StartRecording_Network
{
    public partial class frmMain : Form
    {
        Settings frmSetting = new Settings();
        Socket sock;
        IPAddress serverAddr;
        private int intUdpPort = 1234;
        IPEndPoint endPoint;
        private string videoFolder = @"C:\Recordings";
        private string strIPaddressHOST = @"127.0.0.1";
        private string strIPAddressPROGRAM = @"192.168.0.103";
        private string strIPAddressWIDE = @"192.168.0.104";
        private string strPassword = @"password";
        private string strPortPROGRAM = "4444";
        private string strPortWIDE = "4445";
        private DateTime startTime;

        public frmMain()
        {
            InitializeComponent();

            groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = false;

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverAddr = IPAddress.Parse(strIPaddressHOST);
            endPoint = new IPEndPoint(serverAddr, intUdpPort);

            Directory.CreateDirectory(Path.Combine(videoFolder, "Main"));
            Directory.CreateDirectory(Path.Combine(videoFolder, "Wide"));

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult saveResult = folderBrowserDialog1.ShowDialog();
            if (saveResult == DialogResult.OK)
            {
                videoFolder = folderBrowserDialog1.SelectedPath;
                txtPath.Text = videoFolder;

                Directory.CreateDirectory(Path.Combine(videoFolder, "Main"));
                Directory.CreateDirectory(Path.Combine(videoFolder, "Wide"));
                
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult settingsResult = frmSetting.ShowDialog();
            if (settingsResult == DialogResult.OK)
            {
                strIPAddressPROGRAM = frmSetting.IPAddressPROGRAM;
                strIPAddressWIDE = frmSetting.IPAddressWIDE;
                strPassword = frmSetting.Password;
                strPortPROGRAM = frmSetting.PortPROGRAM.ToString();
                strPortWIDE = frmSetting.PortWIDE.ToString();
            }
            
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = true;

            byte[] sendBuffer = Encoding.ASCII.GetBytes("Start Recording");
            sock.SendTo(sendBuffer, endPoint);

            startTime = DateTime.Now;
            timerElapsed.Start();
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            groupEvent.Enabled = true;
            groupMatch.Enabled = true;
            btnStartRecording.Enabled = true;
            btnStopRecording.Enabled = false;

            byte[] sendBuffer = Encoding.ASCII.GetBytes("Stop Recording");
            sock.SendTo(sendBuffer, endPoint);

            timerElapsed.Stop();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text.Contains("Connect"))
            {
                groupEvent.Enabled = true;
                groupMatch.Enabled = true;
                btnStartRecording.Enabled = true;
                btnStopRecording.Enabled = false;
            }
            else
            {
                groupEvent.Enabled = false;
                groupMatch.Enabled = false;
                btnStartRecording.Enabled = false;
                btnStopRecording.Enabled = false;
            }
        }

        private void timerElapsed_Tick(object sender, EventArgs e)
        {
            lblElapsedTime.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss\.ff");
        }
    }
}
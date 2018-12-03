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
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace OBS_StartRecording_Network
{
    public partial class frmMain : Form
    {
        Settings frmSetting = new Settings();
        protected OBSWebsocket obsProgram, obsWide;

        private string videoFolder = @"D:\__USER\Videos\VM Captures";
        private string strIPAddressPROGRAM = @"192.168.0.104";
        private string strIPAddressWIDE = @"192.168.0.103";
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

            obsProgram = new OBSWebsocket();
            obsProgram.Connected += obsProgramOnConnect;
            obsProgram.Disconnected += obsProgramOnDisconnect;
            obsProgram.RecordingStateChanged += obsProgramRecordingStateChanged;

            obsWide = new OBSWebsocket();
            obsWide.Connected += obsWideOnConnect;
            obsWide.Disconnected += obsWideOnDisconnect;
            obsWide.RecordingStateChanged += obsWideRecordingStateChanged;

            Directory.CreateDirectory(Path.Combine(videoFolder, "Main"));
            Directory.CreateDirectory(Path.Combine(videoFolder, "Wide"));

        }

        private void obsWideRecordingStateChanged(OBSWebsocket sender, OutputState type)
        {
            switch (type)
            {
                case OutputState.Starting:
                    ledRecordPROGRAM.BackColor = Color.Green;
                    break;
                case OutputState.Started:
                    ledRecordPROGRAM.BackColor = Color.Lime;
                    break;
                case OutputState.Stopping:
                    ledRecordPROGRAM.BackColor = Color.Yellow;
                    break;
                case OutputState.Stopped:
                    ledRecordPROGRAM.BackColor = Color.Gray;
                    break;
                default:
                    break;
            }
        }

        private void obsProgramRecordingStateChanged(OBSWebsocket sender, OutputState type)
        {
            switch (type)
            {
                case OutputState.Starting:
                    ledRecordWIDE.BackColor = Color.Green;
                    break;
                case OutputState.Started:
                    ledRecordWIDE.BackColor = Color.Lime;
                    break;
                case OutputState.Stopping:
                    ledRecordWIDE.BackColor = Color.Yellow;
                    break;
                case OutputState.Stopped:
                    ledRecordWIDE.BackColor = Color.Gray;
                    break;
                default:
                    break;
            }
        }

        private void obsWideOnDisconnect(object sender, EventArgs e)
        {
            Console.WriteLine("WideOBS is disconnected");
        }

        private void obsWideOnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("WideOBS is connected");
        }

        private void obsProgramOnDisconnect(object sender, EventArgs e)
        {
            Console.WriteLine("ProgramOBS is disconnected");
        }

        private void obsProgramOnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("ProgramOBS is connected");
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
                videoFolder = frmSetting.Folder;
            }
            
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = true;

            startTime = DateTime.Now;
            timerElapsed.Start();

            obsProgram.StartRecording();
            obsWide.StartRecording();
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            groupEvent.Enabled = true;
            groupMatch.Enabled = true;
            btnStartRecording.Enabled = true;
            btnStopRecording.Enabled = false;

            timerElapsed.Stop();

            obsProgram.StopRecording();
            obsWide.StopRecording();
        }

        private void btnConnectProgram_Click(object sender, EventArgs e)
        {
            if (btnConnectProgram.Text.Contains("Connect"))
            {
                if (!obsProgram.IsConnected)
                {
                    try
                    {
                        string connectString = @"ws:\\" + strIPAddressPROGRAM + ":" + strPortPROGRAM;
                        Console.WriteLine(connectString);
                        obsProgram.Connect(connectString, strPassword);
                        btnConnectProgram.Text = "Disconnect from Program OBS";
                    }
                    catch (AuthFailureException)
                    {
                        MessageBox.Show("Authentication failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    catch (ErrorResponseException ex)
                    {
                        MessageBox.Show("Connect failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (obsProgram.IsConnected && obsWide.IsConnected)
                {
                    groupEvent.Enabled = true;
                    groupMatch.Enabled = true;
                    btnStartRecording.Enabled = true;
                    btnStopRecording.Enabled = false;
                }
            }
            else
            {
                if (obsProgram.IsConnected)
                {
                    try
                    {
                        obsProgram.Disconnect();
                        btnConnectProgram.Text = "Connect to Program OBS";
                    }
                    catch (ErrorResponseException ex)
                    {
                        MessageBox.Show("Disconnection failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
        }

        private void btnConnectWide_Click(object sender, EventArgs e)
        {
            if (btnConnectWide.Text.Contains("Connect"))
            {
                if (!obsWide.IsConnected)
                {
                    try
                    {
                        obsWide.Connect(@"ws:\\" + strIPAddressWIDE + ":" + strPortWIDE, strPassword);
                        btnConnectWide.Text = "Disconnect from Wide OBS";
                    }
                    catch (AuthFailureException)
                    {
                        MessageBox.Show("Authentication failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    catch (ErrorResponseException ex)
                    {
                        MessageBox.Show("Connect failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (obsProgram.IsConnected && obsWide.IsConnected)
                {
                    groupEvent.Enabled = true;
                    groupMatch.Enabled = true;
                    btnStartRecording.Enabled = true;
                    btnStopRecording.Enabled = false;
                }
            }
            else
            {
                if (obsWide.IsConnected)
                {
                    try
                    {
                        obsWide.Disconnect();
                        btnConnectWide.Text = "Connect to Wide OBS";
                    }
                    catch (ErrorResponseException ex)
                    {
                        MessageBox.Show("Disconnection failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
        }

        private void btnOpenRecordings_Click(object sender, EventArgs e)
        {
            Process.Start(videoFolder);
        }

        private void timerElapsed_Tick(object sender, EventArgs e)
        {
            lblElapsedTime.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss\.ff");
        }
    }
}
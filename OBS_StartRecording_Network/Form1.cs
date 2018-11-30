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
using OBSWebsocketDotNet;

namespace OBS_StartRecording_Network
{
    public partial class frmMain : Form
    {
        Settings frmSetting = new Settings();
        private string videoFolder = @"C:\Recordings";
        private string strIPAddress = @"127.0.0.1";
        private string strPassword = @"password";
        private string strPort1 = "4444";
        private string strPort2 = "4445";
        private DateTime startTime;

        private OBSWebsocket _obsMain;
        private OBSWebsocket _obsWide;

        public frmMain()
        {
            InitializeComponent();

            groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = false;

            Directory.CreateDirectory(Path.Combine(videoFolder, "Main"));
            Directory.CreateDirectory(Path.Combine(videoFolder, "Wide"));

            _obsMain = new OBSWebsocket();
            _obsMain.Connected += onConnected;
            _obsMain.Disconnected += onDisconnected;
            _obsMain.RecordingStateChanged += onMainRecordingStateChanged;
            _obsMain.StreamStatus += mainStreamStatus;

            _obsWide = new OBSWebsocket();
            _obsWide.Connected += onConnected;
            _obsWide.Disconnected += onDisconnected;
            _obsWide.RecordingStateChanged += onWideRecordingStateChanged;
            _obsWide.StreamStatus += wideStreamStatus;

        }

        private void mainStreamStatus(OBSWebsocket sender, StreamStatus status)
        {
        }

        private void wideStreamStatus(OBSWebsocket sender, StreamStatus status)
        {
        }

        private void onMainRecordingStateChanged(OBSWebsocket sender, OutputState type)
        {
        }

        private void onWideRecordingStateChanged(OBSWebsocket sender, OutputState type)
        {
        }

        private void onDisconnected(object sender, EventArgs e)
        {
            groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = false;
        }

        private void onConnected(object sender, EventArgs e)
        {
            groupEvent.Enabled = true;
            groupMatch.Enabled = true;
            btnStartRecording.Enabled = true;
            btnStopRecording.Enabled = false;
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
                strIPAddress = frmSetting.IPAddress;
                strPassword = frmSetting.Password;
                strPort1 = frmSetting.Port1.ToString();
                strPort2 = frmSetting.Port2.ToString();
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

            _obsMain.StartRecording();
            _obsWide.StartRecording();
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            groupEvent.Enabled = true;
            groupMatch.Enabled = true;
            btnStartRecording.Enabled = true;
            btnStopRecording.Enabled = false;

            timerElapsed.Stop();

            _obsMain.StopRecording();
            _obsWide.StopRecording();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text.Contains("Connect"))
            {
                if (!_obsMain.IsConnected)
                {
                    try
                    {
                        _obsMain.Connect(@"ws:\\" + strIPAddress + ":" + strPort1, strPassword);
                        btnConnect.Text = "Disconnect from OBS";
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
                Console.WriteLine(_obsMain.GetRecordingFolder());
                _obsMain.SetRecordingFolder(@"C:\Main");

                if (!_obsWide.IsConnected)
                {
                    try
                    {
                        _obsWide.Connect(@"ws:\\" + strIPAddress + ":" + strPort2, strPassword);
                        btnConnect.Text = "Disconnect from OBS";
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
            }
            else
            {
                if (_obsMain.IsConnected)
                {
                    try
                    {
                        _obsMain.Disconnect();
                        btnConnect.Text = "Connect to OBS";
                    }
                    catch (ErrorResponseException ex)
                    {
                        MessageBox.Show("Disconnection failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (_obsWide.IsConnected)
                {
                    try
                    {
                        _obsWide.Disconnect();
                        btnConnect.Text = "Connect to OBS";
                    }
                    catch (ErrorResponseException ex)
                    {
                        MessageBox.Show("Disconnection failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
        }

        private void timerElapsed_Tick(object sender, EventArgs e)
        {
            lblElapsedTime.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss\.ff");
        }
    }
}
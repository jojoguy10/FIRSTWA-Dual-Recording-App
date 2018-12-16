using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBSWebsocketDotNet;
using System.Diagnostics;
using RestSharp;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Threading;
using System.Reflection;
using System.Xml.Linq;

/* TODO:
 * Upload to YouTube to playlist
 * Settings for streaming
 * Settings for uploading (if any)
 * Send YouTube video to TBA
 * Error handling
 * Commenting
 * Layout/UI Design
 */

namespace OBS_StartRecording_Network
{
    public partial class frmMain : Form
    {
        RecordingSettings frmRecordingSetting = new RecordingSettings();

        RestClient tbaClient = new RestClient("http://www.thebluealliance.com/api/v3");
        RestRequest tbaRequest = new RestRequest($"district/2018pnw/events", Method.GET);
        private string TBAKEY;

        List<District> eventDistrict = new List<District>();
        List<Event> eventDetails = new List<Event>();
        Event currentEvent = new Event();
        Match currentMatch;
        string matchKey;

        string[] matchKeys;

        protected OBSWebsocket obsProgram, obsWide;

        private string videoFolder;
        DirectoryInfo dirProgram;
        DirectoryInfo dirWide;

        private string strIPAddressPROGRAM = @"172.19.249.253";
        private string strIPAddressWIDE = @"172.19.249.254";
        private string strPassword = @"password";
        private string strPortPROGRAM = "4444";
        private string strPortWIDE = "4445";

        private DateTime startTime;

        private FileInfo credFile = new FileInfo(@"D:\__USER\Documents\GitHub\FIRSTWA_PC_RecordingApplication\FIRSTWA_StartRecording_Network\client_secret_613443767055-pvnp5ugap7kgj1i7rid6in7tnm3podmv.apps.googleusercontent.com.json");
        private Video videoYT;

        private string fileProgram, fileWide;
        private string replay = "";

        enum MatchType
        {
            Practice,
            Qualification,
            Quarterfinal,
            Semifinal,
            Final
        }
        MatchType currentMatchType = MatchType.Practice;

        public frmMain()
        {
            InitializeComponent();

            XDocument keysFile = XDocument.Load(@"C:\Users\jojog\Source\Repos\FIRSTWA_StartRecording_Network\OBS_StartRecording_Network\keys.xml");

            var xmlValues = keysFile.Descendants("item").Descendants("value").ToArray();

            TBAKEY = xmlValues[0].Value.Replace('\n',' ').Trim();
            
            tbaRequest.AddHeader
            (
                "X-TBA-Auth-Key",
                TBAKEY
            );
            IRestResponse tbaResponse = tbaClient.Execute(tbaRequest);
            string tbaContent = tbaResponse.Content;
            tbaContent = tbaContent.Trim('"');
            Console.WriteLine(tbaContent.Trim('"'));

            eventDistrict = JsonConvert.DeserializeObject<List<District>>(tbaContent);
            eventDetails = JsonConvert.DeserializeObject<List<Event>>(tbaContent);
            
            eventDetails.ForEach(x => comboEventName.Items.Add((x.week + 1) + " - " + x.first_event_code + " - " + x.location_name));
            comboEventName.Sorted = true;

            lblProgramPath.Text = "";
            lblWidePath.Text = "";

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

            videoFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "VM Captures");

            Directory.CreateDirectory(Path.Combine(videoFolder, "Program"));
            Directory.CreateDirectory(Path.Combine(videoFolder, "Wide"));

            dirProgram = new DirectoryInfo(Path.Combine(videoFolder, "Program"));
            dirWide = new DirectoryInfo(Path.Combine(videoFolder, "Wide"));
        }

        private void obsProgramRecordingStateChanged(OBSWebsocket sender, OutputState type)
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

        private void obsWideRecordingStateChanged(OBSWebsocket sender, OutputState type)
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

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            if(comboEventName.SelectedItem == null)
            {
                MessageBox.Show("Please choose an event before recording");
                return;
            }

            groupEvent.Enabled = false;
            groupMatch.Enabled = false;

            if (chkProgramRecord.Checked)
            {
                fileProgram = Path.Combine(dirProgram.FullName, currentEvent.year + " " + currentEvent.name + " " + currentMatchType + " " + numMatchNumber.Value + replay + ".mp4");

                FileInfo[] files = dirProgram.GetFiles();
                if(File.Exists(fileProgram))
                {
                    DialogResult result = MessageBox.Show("Match recording for the Program View already exists.  Overwrite?\n\n" + fileProgram, "Overwrite?", MessageBoxButtons.YesNo);
                    if(result == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        DialogResult confirm = MessageBox.Show("Are you sure you want to overwrite?", "Confirm", MessageBoxButtons.YesNo);
                        if(confirm == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            File.Delete(fileProgram);
                        }
                    }
                }

                lblProgramPath.Text = fileProgram;
            }

            if (chkRecordWide.Checked)
            {
                fileWide = Path.Combine(dirWide.FullName, currentEvent.year + " " + currentEvent.name + " WIDE " + currentMatchType + " " + numMatchNumber.Value + replay + ".mp4");

                FileInfo[] files = dirWide.GetFiles();
                if (File.Exists(fileWide))
                {
                    DialogResult result = MessageBox.Show("Match recording for the Wide View already exists.  Overwrite?\n\n" + fileWide, "Overwrite?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        DialogResult confirm = MessageBox.Show("Are you sure you want to overwrite?", "Confirm", MessageBoxButtons.YesNo);
                        if (confirm == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            File.Delete(fileWide);
                        }
                    }
                }

                lblWidePath.Text = fileWide;
            }

            if (chkProgramRecord.Checked)
            {
                try
                {
                    obsProgram.StartRecording();
                }
                catch
                {
                    obsProgram.StopRecording();
                    System.Threading.Thread.Sleep(1000);
                    obsProgram.StartRecording();
                }
            }
            if (chkRecordWide.Checked)
            {
                try
                {
                    obsWide.StartRecording();
                }
                catch
                {
                    obsWide.StopRecording();
                    System.Threading.Thread.Sleep(1000);
                    obsWide.StartRecording();
                }
            }

            startTime = DateTime.Now;
            timerElapsed.Start();
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = true;
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            groupEvent.Enabled = true;
            groupMatch.Enabled = true;
            btnStopRecording.Enabled = false;

            timerElapsed.Stop();

            if (chkReplay.Checked)
            {
                chkReplay.Checked = false;
                replay = "";
            }

            if (ledRecordPROGRAM.BackColor == Color.Lime)
            {
                obsProgram.StopRecording();
            }

            if (ledRecordWIDE.BackColor == Color.Lime)
            {
                obsWide.StopRecording();
            }

            lblProgramPath.Text = fileProgram;
            lblWidePath.Text = fileWide;
            FileInfo recordedProgramFile = dirProgram.GetFiles()
                                            .OrderByDescending(f => f.LastWriteTime)
                                            .First();
            FileInfo recordedWideFile = dirWide.GetFiles()
                                            .OrderByDescending(f => f.LastWriteTime)
                                            .First();

            while (!IsFileReady(recordedProgramFile.FullName)) { }
            while (!IsFileReady(recordedWideFile.FullName)) { }
            File.Move(recordedProgramFile.FullName, fileProgram);
            File.Move(recordedWideFile.FullName, fileWide);

            btnStartRecording.Enabled = true;

            numMatchNumber.Value++;
        }

        private void btnConnectProgram_Click(object sender, EventArgs e)
        {
            if (btnConnectProgram.Text.Contains("Connect"))
            {
                if (!obsProgram.IsConnected)
                {
                    try
                    {
                        Ping p1 = new Ping();
                        PingReply PR = p1.Send(strIPAddressPROGRAM);
                        if (PR.Status == IPStatus.Success)
                        {
                            string connectString = @"ws:\\" + strIPAddressPROGRAM + ":" + strPortPROGRAM;
                            obsProgram.Connect(connectString, strPassword);
                            btnConnectProgram.Text = "Disconnect from Program OBS";
                        }
                        else
                        {
                            throw new ErrorResponseException("The VM is not turned on or OBS isn't started");
                        }
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
                        Ping p1 = new Ping();
                        PingReply PR = p1.Send(strIPAddressWIDE);
                        if (PR.Status == IPStatus.Success)
                        {
                            obsWide.Connect(@"ws:\\" + strIPAddressWIDE + ":" + strPortWIDE, strPassword);
                            btnConnectWide.Text = "Disconnect from Wide OBS";
                        }
                        else
                        {
                            throw new ErrorResponseException("The VM is not turned on or OBS isn't started");
                        }
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

        private void recordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult settingsResult = frmRecordingSetting.ShowDialog();
            if (settingsResult == DialogResult.OK)
            {
                strIPAddressPROGRAM = frmRecordingSetting.IPAddressPROGRAM;
                strIPAddressWIDE = frmRecordingSetting.IPAddressWIDE;
                strPassword = frmRecordingSetting.Password;
                strPortPROGRAM = frmRecordingSetting.PortPROGRAM.ToString();
                strPortWIDE = frmRecordingSetting.PortWIDE.ToString();
                videoFolder = frmRecordingSetting.Folder;
                
                Directory.CreateDirectory(Path.Combine(videoFolder, "Program"));
                Directory.CreateDirectory(Path.Combine(videoFolder, "Wide"));

                dirProgram = new DirectoryInfo(Path.Combine(videoFolder, "Program"));
                dirWide = new DirectoryInfo(Path.Combine(videoFolder, "Wide"));
            }
        }

        private void uploadsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void streamingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void radioBtnMatchType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if(btn.Checked == true)
            {
                switch (btn.Text)
                {
                    case "Practice":
                        currentMatchType = MatchType.Practice;
                        break;
                    case "Qualification":
                        currentMatchType = MatchType.Qualification;
                        break;
                    case "Quarterfinal":
                        currentMatchType = MatchType.Quarterfinal;
                        break;
                    case "Semifinal":
                        currentMatchType = MatchType.Semifinal;
                        break;
                    case "Final":
                        currentMatchType = MatchType.Final;
                        break;
                    default:
                        break;
                }
                Console.WriteLine(currentMatchType);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                UploadVideo().Wait();
                AddToPlaylist(videoYT.Id);
            }
            catch (AggregateException ex)
            {
                foreach (var i in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + i.Message);
                }
            }
        }

        private void timerElapsed_Tick(object sender, EventArgs e)
        {
            lblElapsedTime.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss\.ff");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(obsProgram.IsConnected) { obsProgram.Disconnect(); }
            if(obsWide.IsConnected) { obsWide.Disconnect(); }
        }

        private void comboEventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < eventDetails.Count; i++)
            {
                if (comboEventName.SelectedItem.ToString().Contains(eventDetails[i].first_event_code))
                {
                    currentEvent = eventDetails[i];
                    Console.WriteLine(currentEvent.name);

                    tbaRequest = new RestRequest(string.Format("event/{0}/matches/keys", currentEvent.key), Method.GET);

                    tbaRequest.AddHeader
                    (
                        "X-TBA-Auth-Key",
                        TBAKEY
                    );

                    IRestResponse tbaResponse = tbaClient.Execute(tbaRequest);
                    string tbaContent = tbaResponse.Content;
                    tbaContent = tbaContent.Trim('"');
                    matchKeys = JsonConvert.DeserializeObject<string[]>(tbaContent);

                    foreach (string match in matchKeys)
                    {
                        string matchIndex;
                        string newMatch = match.Remove(0, match.IndexOf("_"));
                        string type = null;
                        string matchNo = null;
                        string finalNo = null;

                        if (newMatch.Contains("qm")) { type = "Qualification"; }
                        else if (newMatch.Contains("qf")) { type = "Quarterfinal"; finalNo = newMatch.Substring(newMatch.IndexOf("qf") + 2, newMatch.IndexOf("m") - 3); }
                        else if (newMatch.Contains("sf")) { type = "Semifinal"; finalNo = newMatch.Substring(newMatch.IndexOf("sf") + 2, newMatch.IndexOf("m") - 3); }
                        else if (newMatch.Contains("f")) { type = "Final"; finalNo = newMatch.Substring(newMatch.IndexOf("f") + 1, newMatch.IndexOf("m") - 2); }

                        matchNo = newMatch.Substring(newMatch.IndexOf("m") + 1);

                        if (type == "Qualification")
                        {
                            newMatch = string.Format("{0} {1}", type, matchNo);
                        }
                        else
                        {
                            newMatch = string.Format("{0} {1} {2} {3}", type, finalNo, "Match", matchNo);
                        }

                        comboMatches.Items.Add(newMatch);
                    }
                }
            }
        }

        private void chkReplay_CheckedChanged(object sender, EventArgs e)
        {
            if(chkReplay.Checked)
            {
                replay = " Replay " + numReplayNumber.Value;
            }
            else
            {
                replay = "";
            }
        }

        private void numReplayNumber_ValueChanged(object sender, EventArgs e)
        {
            if (chkReplay.Checked)
            {
                replay = " Replay " + numReplayNumber.Value;
            }
            else
            {
                replay = "";
            }
        }

        private void lblProgramPath_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(btnStartRecording.Enabled == true)
            {
                Process.Start(fileProgram);
            }
        }

        private void lblWidePath_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (btnStartRecording.Enabled == true)
            {
                Process.Start(fileWide);
            }
        }

        private static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UploadVideo()
        {
            SetText("Uploading...");

            UserCredential credential;
            using (FileStream stream = new FileStream(credFile.FullName, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.YoutubeUpload },
                    "user",
                    CancellationToken.None
                );
            }
            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            videoYT = new Video();
            videoYT.Snippet = new VideoSnippet();
            videoYT.Snippet.Title = "Default Video Title";
            videoYT.Snippet.Description = "Default Video Description";
            videoYT.Snippet.Tags = new string[] { "tag1", "tag2" };
            videoYT.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            videoYT.Status = new VideoStatus();
            videoYT.Status.PrivacyStatus = "unlisted"; // or "private" or "public"
            string filePath = @"D:\__USER\Videos\VM Captures\Program\2018-12-05 18-55-27.mp4"; // Replace with path to actual movie file.
            Console.WriteLine("Video");

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(videoYT, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
            }
        }

        public async Task AddToPlaylist(string videoID)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = "Test Playlist";
            newPlaylist.Snippet.Description = "A playlist created with the YouTube API v3";
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "unlisted";
            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

            // Add a video to the newly created playlist.
            var newPlaylistItem = new PlaylistItem();
            newPlaylistItem.Snippet = new PlaylistItemSnippet();
            newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
            newPlaylistItem.Snippet.ResourceId = new ResourceId();
            newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
            newPlaylistItem.Snippet.ResourceId.VideoId = videoID;
            newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();
            
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
            SetText("Upload successful");
        }

        private void btnGetMatchDetails_Click(object sender, EventArgs e)
        {
            /* This button will look at the selected match and match type from the UI and 
             * query TBA to get the specifics.  When the Match
             */

            
        }

        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnUpload.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.btnUpload.Text = text;
            }
        }
    }
}
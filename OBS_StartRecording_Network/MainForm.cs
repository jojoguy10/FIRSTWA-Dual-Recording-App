using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
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
using Microsoft.Win32;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

/* TODO:
 * Parse CSV file from FMS for team information
 * Upload to YouTube to playlist
 * Settings for streaming
 * Settings for uploading (if any)
 * Send YouTube video to TBA
 * Error handling
 * Commenting
 * Layout/UI Design
 */

namespace FIRSTWA_Recorder
{
    public partial class MainForm : Form
    {
        RecordingSettings frmRecordingSetting = new RecordingSettings();

        RestClient tbaClient = new RestClient("http://www.thebluealliance.com/api/v3");
        RestRequest tbaRequest = new RestRequest($"district/2019pnw/events", Method.GET);
        private string TBAKEY;

        List<District> eventDistrict = new List<District>();
        List<Event> eventDetails = new List<Event>();
        Event currentEvent = new Event();
        Match currentMatch;
        string matchKey;

        string[] matchKeys;

        private string strIPAddressPC = @"192.168.100.70";
        private string strIPAddressPROGRAM = @"192.168.100.35";
        private string strIPAddressWIDE = @"192.168.100.34";
        private string strPortPROGRAM = "9993";
        private string strPortWIDE = "9993";

        DeckLink dlProgram, dlWide;

        private string fileNameProgram, fileNameWide;

        private DateTime startTime;

        private FileInfo credFile = new FileInfo(@"D:\__USER\Documents\GitHub\FIRSTWA_PC_RecordingApplication\FIRSTWA_StartRecording_Network\client_secret_613443767055-pvnp5ugap7kgj1i7rid6in7tnm3podmv.apps.googleusercontent.com.json");
        private Video videoYT;
        
        private string replay = "";

        private string programPlaylistTitle, programPlaylistId, widePlaylistTitle, widePlaylistId;
        private string programVideoTitle, programVideoId, wideVideoTitle, wideVideoId;

        enum MatchType
        {
            Practice,
            Qualification,
            Quarterfinal,
            Semifinal,
            Final
        }
        MatchType currentMatchType = MatchType.Practice;

        public MainForm()
        {
            InitializeComponent();

            RegistryKey apiKey = Registry.CurrentUser.OpenSubKey(@"Software\FIRSTWA", false);

            TBAKEY = apiKey.GetValue("apikey").ToString();
            
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

            //groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = false;
        }

        private bool CopyFTPFile(string filename)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo("ftp://" + strIPAddressPROGRAM);
                FileInfo file = (from f in dir.GetFiles()
                            orderby f.LastWriteTime descending
                            select f).First();
                Console.WriteLine(file.FullName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + strIPAddressPROGRAM + "/" + filename);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential("FTP_User", "");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                return true;
            }
            catch
            {
                return false;
            }

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
                try
                {
                    dlProgram.Write("record");

                    string matchType, matchNumber;
                    switch (currentMatchType)
                    {
                        case MatchType.Practice:
                            matchType = "Practice";
                            break;
                        case MatchType.Qualification:
                            matchType = "Qual";
                            break;
                        case MatchType.Quarterfinal:
                            matchType = "Quarterfinal";
                            break;
                        case MatchType.Semifinal:
                            matchType = "Semifinal";
                            break;
                        case MatchType.Final:
                            matchType = "Final";
                            break;
                        default:
                            matchType = "";
                            break;
                    }

                    if (currentMatchType == MatchType.Practice || currentMatchType == MatchType.Qualification)
                    {
                        matchNumber = numMatchNumber.Value.ToString();
                    }
                    else
                    {
                        matchNumber = string.Format("{0}-{1}", numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                    }

                    fileNameProgram = string.Format("{0} {1} {2} {3}.mp4", currentEvent.year, currentEvent.name, matchType, matchNumber);
                    //obsProgram.StartRecording();
                }
                catch
                {
                    //obsProgram.StopRecording();
                    //System.Threading.Thread.Sleep(1000);
                    //obsProgram.StartRecording();
                }
            }
            if (chkRecordWide.Checked)
            {
                try
                {
                    dlWide.Write("record");

                    string matchType, matchNumber;
                    switch (currentMatchType)
                    {
                        case MatchType.Practice:
                            matchType = "Practice";
                            break;
                        case MatchType.Qualification:
                            matchType = "Qual";
                            break;
                        case MatchType.Quarterfinal:
                            matchType = "Quarterfinal";
                            break;
                        case MatchType.Semifinal:
                            matchType = "Semifinal";
                            break;
                        case MatchType.Final:
                            matchType = "Final";
                            break;
                        default:
                            matchType = "";
                            break;
                    }

                    if (currentMatchType == MatchType.Practice || currentMatchType == MatchType.Qualification)
                    {
                        matchNumber = numMatchNumber.Value.ToString();
                    }
                    else
                    {
                        matchNumber = string.Format("{0}-{1}", numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                    }

                    fileNameWide = string.Format("{0} {1} WIDE {2} {3}.mp4", currentEvent.year, currentEvent.name, matchType, matchNumber);
                }
                catch
                {
                    //obsWide.StopRecording();
                    //System.Threading.Thread.Sleep(1000);
                    //obsWide.StartRecording();
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
            
            dlProgram.Write("stop");
            
            dlWide.Write("stop");

            bgWorker_FTP.RunWorkerAsync();

            btnStartRecording.Enabled = true;

            numMatchNumber.Value++;
        }
        
        private void btnOpenRecordings_Click(object sender, EventArgs e)
        {

        }

        private void CreateEventDirectory(string uriPath)
        {
            try
            {
                WebRequest request = WebRequest.Create(uriPath);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential("FTP_User", "");
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine(resp.StatusCode);
                }
            }
            catch
            {
                Console.WriteLine("Path a;ready exists: " + uriPath);
            }
        }

        private void CopyFTPFile(string fromURI, string toURI, string fromFilename, string toFilename)
        {
            string video = DownloadFileFTP(fromURI, fromFilename, "temp.mp4");
            UploadFileFTP(toURI + "/" + toFilename, video);
        }

        private string DownloadFileFTP(string uri, string ftpFileName, string fileName)
        {
            if (!Directory.Exists(@"C:\Temp"))
            {
                Directory.CreateDirectory(@"C:\Temp");
            }
            string inputfilepath = @"C:\Temp\" + fileName;

            string ftpfullpath = uri + "/" + ftpFileName;

            using (WebClient request = new WebClient())
            {
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(inputfilepath))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
                Console.WriteLine("Download from Recorder: Complete");
            }

            return inputfilepath;
        }

        public void UploadFileFTP(string uri, string filePath)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("FTP_User", "");
                client.UploadFile(uri, WebRequestMethods.Ftp.UploadFile, filePath);
            }
            Console.WriteLine("Upload to PC: Complete");
        }

        private void DeleteFTPFile(string uri, string filename)
        {
            string fullDir = uri + "/" + filename;
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(fullDir);
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            
            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            Console.WriteLine("Delete status of {0}: {0}", filename, response.StatusDescription);
            response.Close();
        }

        private List<string> GetFTPFiles(string uri)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(uri);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            List<string> directories = new List<string>();

            string line = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                directories.Add(line);
                line = streamReader.ReadLine();
            }

            streamReader.Close();

            return directories;
        }

        private void recordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult settingsResult = frmRecordingSetting.ShowDialog();
            if (settingsResult == DialogResult.OK)
            {
                strIPAddressPROGRAM = frmRecordingSetting.IPAddressPROGRAM;
                strIPAddressWIDE = frmRecordingSetting.IPAddressWIDE;
                strIPAddressPC = frmRecordingSetting.IPAddressPC;
            }
        }

        private void uploadsToolStripMenuItem_Click(object sender, EventArgs e)
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
                        lblFinalNo.Visible = false;
                        numFinalNo.Visible = false;
                        break;
                    case "Qualification":
                        currentMatchType = MatchType.Qualification;
                        lblFinalNo.Visible = false;
                        numFinalNo.Visible = false;
                        break;
                    case "Quarterfinal":
                        currentMatchType = MatchType.Quarterfinal;
                        lblFinalNo.Visible = true;
                        numFinalNo.Visible = true;
                        break;
                    case "Semifinal":
                        currentMatchType = MatchType.Semifinal;
                        lblFinalNo.Visible = true;
                        numFinalNo.Visible = true;
                        break;
                    case "Final":
                        currentMatchType = MatchType.Final;
                        lblFinalNo.Visible = true;
                        numFinalNo.Visible = true;
                        break;
                    default:
                        break;
                }
                Console.WriteLine(currentMatchType);
            }
        }

        private void timerElapsed_Tick(object sender, EventArgs e)
        {
            lblElapsedTime.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss\.ff");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(obsProgram.IsConnected) { obsProgram.Disconnect(); }
            //if(obsWide.IsConnected) { obsWide.Disconnect(); }
        }

        private void comboEventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < eventDetails.Count; i++)
            {
                if (comboEventName.SelectedItem.ToString().Contains(eventDetails[i].first_event_code))
                {
                    currentEvent = eventDetails[i];

                    programPlaylistTitle = currentEvent.year + " " + currentEvent.name + " " + currentEvent.week;
                    widePlaylistTitle = "(WIDE) " + currentEvent.year + " " + currentEvent.name + " " + currentEvent.week;

                    programVideoTitle = currentEvent.year + " " + currentEvent.name + " " + matchType + " " + numMatchNumber.Value + replay;
                    wideVideoTitle = currentEvent.year + " " + currentEvent.name + " WIDE " + matchType + " " + numMatchNumber.Value + replay;
                    Console.WriteLine(currentEvent.name);
                    GetMatches();
                    groupMatch.Enabled = true;
                }
            }
        }

        private async Task GetMatches()
        {

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
            List<string> newMatchNames = new List<string>();
            List<Tuple<string, int>> qm = new List<Tuple<string, int>>(); // Qualification matches
            List<Tuple<string, int, int>> qf = new List<Tuple<string, int, int>>(); // Qualification matches
            List<Tuple<string, int, int>> sf = new List<Tuple<string, int, int>>(); // Qualification matches
            List<Tuple<string, int, int>> f = new List<Tuple<string, int, int>>(); // Qualification matches

            foreach (string match in matchKeys)
            {
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

                switch (type)
                {
                    case "Qualification":
                        qm.Add(new Tuple<string, int>(newMatch, Convert.ToInt16(matchNo)));
                        break;
                    case "Quarterfinal":
                        qf.Add(new Tuple<string, int, int>(newMatch, Convert.ToInt16(finalNo), Convert.ToInt16(matchNo)));
                        break;
                    case "Semifinal":
                        sf.Add(new Tuple<string, int, int>(newMatch, Convert.ToInt16(finalNo), Convert.ToInt16(matchNo)));
                        break;
                    case "Final":
                        f.Add(new Tuple<string, int, int>(newMatch, Convert.ToInt16(finalNo), Convert.ToInt16(matchNo)));
                        break;

                    default:
                        break;
                }

                newMatchNames.Add(newMatch);
            }
            qm = qm.OrderBy(t => t.Item2).ToList();
            qf = qf.OrderBy(t => t.Item3).ToList();
            sf = sf.OrderBy(t => t.Item3).ToList();
            f = f.OrderBy(t => t.Item3).ToList();
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                UploadVideo();
            }
            catch (AggregateException ex)
            {
                foreach (var i in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + i.Message);
                }
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
            videoYT.Snippet.Title = Path.GetFileNameWithoutExtension("");
            videoYT.Snippet.Description = "Default Video Description";
            videoYT.Snippet.Tags = new string[] { "robots", "frc" };
            videoYT.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            videoYT.Status = new VideoStatus();
            videoYT.Status.PrivacyStatus = "unlisted"; // or "private" or "public"
            string filePath = ""; // Replace with path to actual movie file.
            Console.WriteLine("Video");

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(videoYT, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
                Console.WriteLine("Upload Done");
                await AddToPlaylist(videoYT.Id);
                Console.WriteLine("Playlist Done");
            }
        }

        public async Task ListPlaylists(string playlistName)
        {
            UserCredential credential;
            using (var stream = new FileStream(credFile.FullName, FileMode.Open, FileAccess.Read))
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

            var channelsListRequest = youtubeService.Channels.List("contentDetails");
            channelsListRequest.Mine = true;
            var playlists = youtubeService.Playlists.List("snippit");
            playlists.PageToken = "";
            playlists.MaxResults = 50;
            playlists.Mine = true;
            PlaylistListResponse presponse = await playlists.ExecuteAsync();
            
            foreach (var pl in presponse.Items)
            {
                if(pl.Snippet.Title == playlistName)
                {
                    Console.WriteLine("Playlist already created");
                    return;
                }
            }
        }

        public async Task AddToPlaylist(string videoID)
        {
            UserCredential credential;
            using (var stream = new FileStream(credFile.FullName, FileMode.Open, FileAccess.Read))
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
            Console.WriteLine("Playlist Created");

            // Add a video to the newly created playlist.
            var newPlaylistItem = new PlaylistItem();
            newPlaylistItem.Snippet = new PlaylistItemSnippet();
            newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
            newPlaylistItem.Snippet.ResourceId = new ResourceId();
            newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
            newPlaylistItem.Snippet.ResourceId.VideoId = videoID;
            newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();
        }

        void videosInsertRequest_ProgressChanged(IUploadProgress progress)
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

        private void timerTCPWatchDog_Tick(object sender, EventArgs e)
        {
            // Poll the DeckLinks and update LEDs if necessary

            // Send a "ping" command to decklinks
            dlProgram.Write("ping");
            Console.WriteLine(dlProgram.Read());

            dlWide.Write("ping");
            Console.WriteLine(dlWide.Read());
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
            string shortMatchType = "";
            string currentMatchKey = "";
            switch (currentMatchType)
            {
                case MatchType.Practice:
                    return;
                case MatchType.Qualification:
                    currentMatchKey = string.Format("{0}_qm{1}", currentEvent.key, numMatchNumber.Value);
                    break;
                case MatchType.Quarterfinal:
                    currentMatchKey = string.Format("{0}_qf{1}m{2}", currentEvent.key, numMatchNumber.Value, numFinalNo.Value);
                    break;
                case MatchType.Semifinal:
                    currentMatchKey = string.Format("{0}_sf{1}m{2}", currentEvent.key, numMatchNumber.Value, numFinalNo.Value);
                    break;
                case MatchType.Final:
                    currentMatchKey = string.Format("{0}_f{1}m{2}", currentEvent.key, numMatchNumber.Value, numFinalNo.Value);
                    break;
                default:
                    break;
            }
            Console.WriteLine(currentMatchKey);

            RestClient tbaClient = new RestClient("http://www.thebluealliance.com/api/v3");
            RestRequest tbaRequest = new RestRequest(string.Format("match/{0}", currentMatchKey), Method.GET);

            tbaRequest.AddHeader
            (
                "X-TBA-Auth-Key",
                TBAKEY
            );

            IRestResponse tbaResponse = tbaClient.Execute(tbaRequest);
            string tbaContent = tbaResponse.Content;
            tbaContent = tbaContent.Trim('"');
            currentMatch = JsonConvert.DeserializeObject<Match>(tbaContent);
            
            if (currentMatch.CompLevel == "qm")
            {
                lblMatchNumber.Text = string.Format("{0} {1}", currentMatch.CompLevel.ToUpper(), currentMatch.MatchNumber);
            }
            else
            {
                lblMatchNumber.Text = string.Format("{0} {1}-{2}", currentMatch.CompLevel.ToUpper(), currentMatch.SetNumber, currentMatch.MatchNumber);
            }
            lblRed1.Text = string.Format("RED 1: {0}", currentMatch.Alliances.Red.TeamKeys[0].ToString().Substring(3));
            lblRed2.Text = string.Format("RED 2: {0}", currentMatch.Alliances.Red.TeamKeys[1].ToString().Substring(3));
            lblRed3.Text = string.Format("RED 3: {0}", currentMatch.Alliances.Red.TeamKeys[2].ToString().Substring(3));
            lblBlue1.Text = string.Format("BLUE 1: {0}", currentMatch.Alliances.Blue.TeamKeys[0].ToString().Substring(3));
            lblBlue2.Text = string.Format("BLUE 2: {0}", currentMatch.Alliances.Blue.TeamKeys[1].ToString().Substring(3));
            lblBlue3.Text = string.Format("BLUE 3: {0}", currentMatch.Alliances.Blue.TeamKeys[2].ToString().Substring(3));
        }

        private void btnConnectProgram_Click(object sender, EventArgs e)
        {
            try
            {
                dlProgram = new DeckLink(strIPAddressPROGRAM, Convert.ToInt32(strPortPROGRAM));
                Console.WriteLine("Program Connected");
                dlProgram.Write("ping");
                Console.WriteLine(dlProgram.Read());
                btnStartRecording.Enabled = true;
            }
            catch
            {
                MessageBox.Show(string.Format("Could not connect to the Program recorder\nat the IP address: {0}", strIPAddressPROGRAM));
            }
        }

        private void btnConnectWide_Click(object sender, EventArgs e)
        {
            try
            {
                dlWide = new DeckLink(strIPAddressWIDE, Convert.ToInt32(strPortWIDE));
                Console.WriteLine("Wide Connected");
                dlProgram.Write("ping");
                Console.WriteLine(dlWide.Read());
                btnStartRecording.Enabled = true;
            }
            catch
            {
                MessageBox.Show(string.Format("Could not connect to the Wide recorder\nat the IP address: {0}", strIPAddressPROGRAM));
            }
        }

        delegate void SetTextCallback(string text);

        private void bgWorker_FTP_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> URIs = new List<string>();
            List<string> FilePaths = new List<string>();
            List<string> FileNames = new List<string>();
            string programURI = string.Format("ftp://{0}/1", strIPAddressPROGRAM);
            string wideURI = string.Format("ftp://{0}/1", strIPAddressWIDE);
            string programPath = string.Format("ftp://{0}/2019/{1}/PROGRAM", strIPAddressPC, currentEvent.short_name);
            string widePath = string.Format("ftp://{0}/2019/{1}/WIDE", strIPAddressPC, currentEvent.short_name);

            URIs.Add(programURI);
            URIs.Add(wideURI);
            
            CreateEventDirectory(programPath);
            CreateEventDirectory(widePath);

            FilePaths.Add(programPath);
            FilePaths.Add(widePath);

            FileNames.Add(fileNameProgram);
            FileNames.Add(fileNameWide);

            for (int i = 0; i < URIs.Count; i++)
            {
                List<string> directories = GetFTPFiles(URIs[i]);
                List<DateTime> timestamps = new List<DateTime>();
                List<string> fileNames = new List<string>();

                Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                foreach (string file in directories)
                {
                    System.Text.RegularExpressions.Match match = regex.Match(file);
                    Console.WriteLine(match.Groups[5].ToString());
                    timestamps.Add(DateTime.Parse(match.Groups[5].ToString()));
                    fileNames.Add(match.Groups[6].ToString());
                }

                if (directories.Count > 5)
                {
                    DeleteFTPFile(URIs[i], fileNames[timestamps.IndexOf(timestamps.Min())]);

                    directories = GetFTPFiles(URIs[i]);

                    foreach (string file in directories)
                    {
                        Console.WriteLine(file);
                    }
                }

                CopyFTPFile(URIs[i], FilePaths[i], fileNames[timestamps.IndexOf(timestamps.Max())], FileNames[i]);
            }

            File.Delete(@"C:\Temp\temp.mp4");
        }

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
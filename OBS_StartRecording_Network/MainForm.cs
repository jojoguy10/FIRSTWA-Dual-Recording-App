using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;
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
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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

//using FileName = string;
//using FilePath = string;

namespace FIRSTWA_Recorder
{

    public partial class MainForm : Form
    {
        RecordingSettings frmRecordingSetting;

        RestClient tbaClient = new RestClient("http://www.thebluealliance.com/api/v3");
        RestRequest tbaRequest = new RestRequest($"district/2019pnw/events", Method.GET);
        private string TBAKEY;

        List<District> eventDistrict = new List<District>();
        List<Event> eventDetails = new List<Event>();
        Event currentEvent = new Event();
        Match currentMatch;
        string matchType;

        Match[] matches;

        private string strIPAddressPC = @"192.168.100.70";
        private string strIPAddressPROGRAM = @"192.168.100.35";
        private string strIPAddressWIDE = @"192.168.100.34";
        private string strPortPROGRAM = "9993";
        private string strPortWIDE = "9993";

        string regPROGRAM = "PROGRAM_IPAddress";
        string regWIDE = "WIDE_IPAddress";
        string regPC = "PC_IPAddress";

        int progress = 0;

        HyperDeck hdProgram, hdWide;

        string matchNameProgram = "";
        string matchNameWide = "";
        string matchABV = "";

        private string fileNameProgram, fileNameWide;
        private string ytDescription, ytTags;

        private DateTime startTime;

        private struct YoutubeTask
        {
            string targetRemoteFile;
        }

        private FileInfo credFile = new FileInfo(@"D:\__USER\Documents\GitHub\FIRSTWA_PC_RecordingApplication\FIRSTWA_StartRecording_Network\client_secret_613443767055-pvnp5ugap7kgj1i7rid6in7tnm3podmv.apps.googleusercontent.com.json");
        private Video videoYT;

        private UploadManager ManagingWindow;

        private string programPlaylistTitle, programPlaylistId, widePlaylistTitle, widePlaylistId;
        private string programVideoTitle, programVideoId, wideVideoTitle, wideVideoId;

        enum MatchType
        {
            Qualification,
            Quarterfinal,
            Semifinal,
            Final
        }
        MatchType currentMatchType = MatchType.Qualification;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                TBAKEY = ReadRegistryKey("apikey");
            }
            catch
            {
                DialogResult dr = MessageBox.Show("Could not find a TBA API key in the registry.  Closing...");

                if (dr == DialogResult.OK)
                {
                    Application.Exit();
                }
            }

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

            try
            {
                if (ReadRegistryKey(regPC) == "")
                {
                    UpdateRegistryKeys();
                }
                else
                {
                    strIPAddressPC = ReadRegistryKey(regPC);
                    strIPAddressPROGRAM = ReadRegistryKey(regPROGRAM);
                    strIPAddressWIDE = ReadRegistryKey(regWIDE);
                }
            }
            catch
            {
                MessageBox.Show("There was a problem reading the registry" + regPC + ".  Have you created the registry keys?");
                Application.Exit();
            }

            if (!Directory.Exists(@"C:\Temp"))
            {
                Directory.CreateDirectory(@"C:\Temp");
            }

            ManagingWindow = new UploadManager(@"https://www.youtube.com/playlist?list=PLwES-SbpsnxwE2oV4EpA-Cb9Szt8UG1vk");

            frmRecordingSetting = new RecordingSettings(strIPAddressPC, strIPAddressPROGRAM, strIPAddressWIDE);
            
            groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = false;
        }

        #region Registry
        private string ReadRegistryKey(string key)
        {
            RegistryKey firstwaKey = Registry.CurrentUser.OpenSubKey(@"Software\FIRSTWA", true);
            if (firstwaKey == null)
            {
                return "";
            }
            else
            {
                return firstwaKey.GetValue(key).ToString();
            }

        }

        private void UpdateRegistryKeys()
        {
            WriteRegistryKey(regPC, strIPAddressPC);
            WriteRegistryKey(regPROGRAM, strIPAddressPROGRAM);
            WriteRegistryKey(regWIDE, strIPAddressWIDE);
        }

        private void WriteRegistryKey(string key, string value)
        {
            RegistryKey firstwaKey = Registry.CurrentUser.OpenSubKey(@"Software\FIRSTWA", true);
            if (firstwaKey == null)
            {
                firstwaKey = Registry.CurrentUser.CreateSubKey(@"Software\FIRSTWA");
            }
            
            firstwaKey.SetValue(key, value);
        }
        #endregion

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            if(comboEventName.SelectedItem == null)
            {
                MessageBox.Show("Please choose an event before recording");
                return;
            }

            string matchAbrev = "qm";
            switch (currentMatchType)
            {
                case MatchType.Qualification:
                    matchType = "Qual";
                    matchAbrev = "qm";
                    break;
                case MatchType.Quarterfinal:
                    matchType = "Quarterfinal";
                    matchAbrev = "qf";
                    break;
                case MatchType.Semifinal:
                    matchType = "Semifinal";
                    matchAbrev = "sf";
                    break;
                case MatchType.Final:
                    matchType = "Final";
                    matchAbrev = "f";
                    break;
                default:
                    matchType = "";
                    matchAbrev = "";
                    break;
            }

            if (currentMatchType == MatchType.Qualification || currentMatchType == MatchType.Final)
            {
                matchABV = string.Format("{0}_{1}{2}", currentEvent.event_code, matchAbrev, numMatchNumber.Value.ToString());
            }
            else
            {
                matchABV = string.Format("{0}_{1}{2}m{3}", currentEvent.event_code, matchAbrev, numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
            }

            currentMatch = null;
            foreach (Match match in matches)
            {
                if (match.CompLevel.Equals(matchAbrev))
                {
                    if (match.MatchNumber == numMatchNumber.Value && match.SetNumber == numFinalNo.Value)
                    {
                        currentMatch = match;
                        break;
                    }
                }
            }
            if (currentMatch == null)
            {
                var result = MessageBox.Show("Match does not exist!\n\nDo you want to continue recording?", "Error", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            groupEvent.Enabled = false;
            groupMatch.Enabled = false;

            if (chkProgramRecord.Checked)
            {
                if (currentMatchType == MatchType.Qualification || currentMatchType == MatchType.Final)
                {
                    matchNameProgram = string.Format("{0} {1} {2} {3}", currentEvent.year, currentEvent.name, matchType, numMatchNumber.Value.ToString());
                }
                else
                {
                    matchNameProgram = string.Format("{0} {1} {2} {3} Match {4}", currentEvent.year, currentEvent.name, matchType, numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                }

                fileNameProgram = matchNameProgram + ".mp4";

                hdProgram.Write("record: name: " + matchABV +"_program");
            }

            if (chkRecordWide.Checked)
            {
                if (currentMatchType == MatchType.Qualification || currentMatchType == MatchType.Final)
                {
                    matchNameWide = string.Format("{0} {1} WIDE {2} {3}", currentEvent.year, currentEvent.name, matchType, numMatchNumber.Value.ToString());
                }
                else
                {
                    matchNameWide = string.Format("{0} {1} WIDE {2} {3} Match {4}", currentEvent.year, currentEvent.name, matchType, numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                }
                fileNameWide = matchNameWide + ".mp4";

                hdWide.Write("record: name: " + matchABV + "_wide");
            }

            startTime = DateTime.Now;
            timerElapsed.Start();

            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = true;

            SetProgress(0);
            progress = 0;

            ledProgram.BackColor = Color.Red;
            ledWide.BackColor = Color.Red;
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            groupEvent.Enabled = true;
            groupMatch.Enabled = true;
            btnStopRecording.Enabled = false;

            timerElapsed.Stop();

            hdProgram.Write("stop");

            hdWide.Write("stop");

            bgWorker_FTP_Program.RunWorkerAsync();
            bgWorker_FTP_Wide.RunWorkerAsync();

            btnStartRecording.Enabled = true;

            if(currentMatchType == MatchType.Qualification || currentMatchType == MatchType.Final)
            {
                if (numMatchNumber.Value < numMatchNumber.Maximum)
                {
                    numMatchNumber.Value++;
                }
            }
            else
            {
                if(numFinalNo.Value < 4)
                {
                    numFinalNo.Value++;
                }
                else
                {
                    numFinalNo.Value = 1;
                    if (numMatchNumber.Value < numMatchNumber.Maximum)
                    {
                        numMatchNumber.Value++;
                    }
                }
            }

            GetMatches();
        }

        #region FTP Stuff
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

        //convert the mp4 from uncompressed audio to mp3 audio using ffmpeg
        //videoPath - filepath of mp4 to convert
        private void ConvertVideo(string videoPath, bool mapMono = false)
        {
            string videoName = videoPath.Substring(0,videoPath.Length - 4);
            string outVideo = videoName + "test.mp4";

            StringBuilder args_proto = new StringBuilder();

            if (mapMono)
            {
                args_proto.AppendFormat("-y -acodec pcm_s24le -i \"{0}\" -acodec mp3 -vcodec copy -af \"pan=mono|c0=c0\" \"{1}\"", videoPath, outVideo);
                Console.WriteLine("Mono");
            }
            else
            {
                args_proto.AppendFormat("-y -acodec pcm_s24le -i \"{0}\" -acodec mp3 -vcodec copy \"{1}\"", videoPath, outVideo);
                Console.WriteLine("Standard");
            }

            string args = args_proto.ToString();
            Console.WriteLine(mapMono.ToString() + "::" + args);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    FileName = "ffmpeg.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = args
                }
            };

            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs.Data);
                MessageBox.Show(eventArgs.Data);
            };

            process.Start();

            process.WaitForExit();

            Console.WriteLine("Audio Conversion: Done!");

            File.Delete(videoPath);

            File.Move(outVideo, videoPath);
        }

        //download an mp4 from a remote server, convert its audio track, and upload it to a remote server
        //fromURI - server connection to download from
        //toURI - server connection to upload to
        //fromFilePath - file path to downloaded file
        //toFilePath - file path to upload
        private void CopyFTPFile(string fromURI, string toURI, string fromFilePath, string toFilePath, string localTempFileName, bool mapMono=false)
        {
            progress++;
            SetProgress(progress);

            //string localTempFilePath = @"C:\Temp" + localTempFileName;

            DownloadFileFTP(fromURI +"/" + fromFilePath, localTempFileName);

            ConvertVideo(localTempFileName, mapMono);
            UploadFileFTP(toURI + "/" + toFilePath, localTempFileName);
            progress++;
            SetProgress(progress);
        }

        //download an mp4 from a remote server
        //uri - connection to download from
        //ftpFileName - file path at target remote server
        //localFilePath - file path at local
        private void DownloadFileFTP(string remotePath, string localFilePath)
        {
            progress++;
            SetProgress(progress);
            string ftpfullpath = remotePath.Replace(".mcc", ".mp4");

            using (WebClient request = new WebClient())
            {
                request.DownloadFile(ftpfullpath, localFilePath);

                //using (FileStream file = File.Create(inputfilepath))
                //{

                //    file.Write(fileData, 0, fileData.Length);
                //    file.Close();
                //}
                Console.WriteLine("Download from Recorder: Complete");
            }
            progress++;
            SetProgress(progress);
        }

        //upload an mp4 to a remote server
        //uri - connection and file path at remote to upload to
        //filePath - file path at local to upload from
        public void UploadFileFTP(string uri, string filePath)
        {
            progress++;
            SetProgress(progress);
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("FTP_User", "");
                client.UploadFile(uri, WebRequestMethods.Ftp.UploadFile, filePath);
            }
            progress++;
            SetProgress(progress);
            Console.WriteLine("Upload to PC: Complete");
        }

        //delete a file at a remote server
        //uri - remote server to delete at
        //filename - 
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
            progress++;
            SetProgress(progress);
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
            progress++;
            SetProgress(progress);
            return directories;
        }
        #endregion

        private void recordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult settingsResult = frmRecordingSetting.ShowDialog();
            if (settingsResult == DialogResult.OK)
            {
                strIPAddressPROGRAM = frmRecordingSetting.IPAddressPROGRAM;
                strIPAddressWIDE = frmRecordingSetting.IPAddressWIDE;
                strIPAddressPC = frmRecordingSetting.IPAddressPC;

                UpdateRegistryKeys();
            }
        }

        private void radioBtnMatchType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if(btn.Checked == true)
            {
                numMatchNumber.Value = 1;
                numFinalNo.Value = 1;

                switch (btn.Text)
                {
                    case "Qualification":
                        currentMatchType = MatchType.Qualification;
                        lblFinalNo.Visible = false;
                        numFinalNo.Visible = false;
                        numMatchNumber.Maximum = 200;
                        numFinalNo.Maximum = 1;
                        break;
                    case "Quarterfinal":
                        currentMatchType = MatchType.Quarterfinal;
                        lblFinalNo.Visible = true;
                        numFinalNo.Visible = true;
                        numMatchNumber.Maximum = 3;
                        numFinalNo.Maximum = 4;
                        break;
                    case "Semifinal":
                        currentMatchType = MatchType.Semifinal;
                        lblFinalNo.Visible = true;
                        numFinalNo.Visible = true;
                        numMatchNumber.Maximum = 3;
                        numFinalNo.Maximum = 2;
                        break;
                    case "Final":
                        currentMatchType = MatchType.Final;
                        lblFinalNo.Visible = false;
                        numFinalNo.Visible = false;
                        numMatchNumber.Maximum = 3;
                        numFinalNo.Maximum = 1;
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

        private void comboEventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < eventDetails.Count; i++)
            {
                if (comboEventName.SelectedItem.ToString().Contains(eventDetails[i].first_event_code))
                {
                    currentEvent = eventDetails[i];

                    programPlaylistTitle = currentEvent.year + " " + currentEvent.name + " " + currentEvent.week;
                    widePlaylistTitle = "(WIDE) " + currentEvent.year + " " + currentEvent.name + " " + currentEvent.week;

                    programVideoTitle = currentEvent.year + " " + currentEvent.name + " " + matchType + " " + numMatchNumber.Value;
                    wideVideoTitle = currentEvent.year + " " + currentEvent.name + " WIDE " + matchType + " " + numMatchNumber.Value;
                    Console.WriteLine(currentEvent.name);
                    GetMatches();
                    groupMatch.Enabled = true;
                }
            }
        }

        private async Task GetMatches()
        {
            tbaRequest = new RestRequest(string.Format("event/{0}/matches/simple", currentEvent.key), Method.GET);

            tbaRequest.AddHeader
            (
                "X-TBA-Auth-Key",
                TBAKEY
            );

            IRestResponse tbaResponse = tbaClient.Execute(tbaRequest);
            string tbaContent = tbaResponse.Content;
            matches = JsonConvert.DeserializeObject<Match[]>(tbaContent);
            Console.WriteLine("Done");
        }
        
        private void btnConnectProgram_Click(object sender, EventArgs e)
        {
            try
            {
                hdProgram = new HyperDeck(strIPAddressPROGRAM, Convert.ToInt32(strPortPROGRAM));
                Console.WriteLine("Program Connected");
                hdProgram.Write("ping");
                Console.WriteLine(hdProgram.Read());
                btnStartRecording.Enabled = true;
                groupEvent.Enabled = true;
                btnConnectProgram.BackColor = Color.Green;

                //var result = MessageBox.Show("Clear SSD? This will erase any existing clips in the hyperdeck recorder. Not clearing the SD may result in corrupted clips or naming conflicts", "Error", MessageBoxButtons.YesNo);
                //if (result == DialogResult.Yes)
                //{
                //    string programURI = string.Format("ftp://{0}/1", strIPAddressPROGRAM);

                //    Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                //RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                //    List<string> directories = GetFTPFiles(programURI);
                //    List<string> fileNames = new List<string>();

                //    foreach (string file in directories)
                //    {
                //        System.Text.RegularExpressions.Match match = regex.Match(file);
                //        DeleteFTPFile(programURI, match.Groups[6].ToString());
                //    }
                //}
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
                hdWide = new HyperDeck(strIPAddressWIDE, Convert.ToInt32(strPortWIDE));
                Console.WriteLine("Wide Connected");
                hdProgram.Write("ping");
                Console.WriteLine(hdWide.Read());
                btnStartRecording.Enabled = true;
                groupEvent.Enabled = true;
                btnConnectWide.BackColor = Color.Green;

                //var result = MessageBox.Show("Clear SSD? This will erase any existing clips in the hyperdeck recorder. Not clearing the SD may result in corrupted clips or naming conflicts", "Error", MessageBoxButtons.YesNo);
                //if (result == DialogResult.Yes)
                //{
                //    string programURI = string.Format("ftp://{0}/1", strIPAddressWIDE);

                //    Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                //RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                //    List<string> directories = GetFTPFiles(programURI);
                //    List<string> fileNames = new List<string>();

                //    foreach (string file in directories)
                //    {
                //        System.Text.RegularExpressions.Match match = regex.Match(file);
                //        DeleteFTPFile(programURI, match.Groups[6].ToString());
                //    }
                //}
            }
            catch
            {
                MessageBox.Show(string.Format("Could not connect to the Wide recorder\nat the IP address: {0}", strIPAddressPROGRAM));
            }
        }

        #region Background Workers
        private void bgWorker_FTP_Wide_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);

            progress++;
            SetProgress(progress);
            string wideURI = string.Format("ftp://{0}/1", strIPAddressWIDE);
            string widePath = string.Format("ftp://{0}/2019/{1}/WIDE", strIPAddressPC, currentEvent.short_name);

            CreateEventDirectory(widePath);
            List<string> directories = GetFTPFiles(wideURI);
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
                while (directories.Count > 5)
                {
                    int minTimstampIndex = timestamps.IndexOf(timestamps.Min());

                    DeleteFTPFile(wideURI, fileNames[minTimstampIndex]);
                    fileNames.RemoveAt(minTimstampIndex);
                    timestamps.RemoveAt(minTimstampIndex);
                    directories.RemoveAt(minTimstampIndex);

                    //directories = GetFTPFiles(wideURI);
                    //timestamps.Clear();
                    //fileNames.Clear();

                    //foreach (string file in directories)
                    //{
                    //    System.Text.RegularExpressions.Match match = regex.Match(file);
                    //    Console.WriteLine(match.Groups[5].ToString());
                    //    timestamps.Add(DateTime.Parse(match.Groups[5].ToString()));
                    //    fileNames.Add(match.Groups[6].ToString());
                    //}
                }

                directories = GetFTPFiles(wideURI);

                foreach (string file in directories)
                {
                    Console.WriteLine(file);
                }
            }
            progress++;
            SetProgress(progress);

            string tempFile = @"C:\Temp\" + fileNameWide;

            int matchIndex = 0;
            int most_recent = -1;
            foreach (string file in fileNames)
            {
                if (file.Substring(0, file.Length - 10).Equals(matchABV + "_wide"))
                {
                    int revision = Int32.Parse(file.Substring(file.Length - 9, 4));
                    if (revision > most_recent)
                    {
                        matchIndex = fileNames.IndexOf(file);
                        most_recent = revision;
                    }
                }
            }

            CopyFTPFile(wideURI, widePath, fileNames[matchIndex], fileNameWide, tempFile, false);
            progress++;
            SetProgress(progress);
            Console.WriteLine("Wide: Done!");
            Console.WriteLine("Wide: Progress = " + progress);
            ledWide.BackColor = Color.Green;
        }

        private void bgWorker_FTP_Program_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);

            progress++;
            SetProgress(progress);
            string programURI = string.Format("ftp://{0}/1", strIPAddressPROGRAM);
            string programPath = string.Format("ftp://{0}/2019/{1}/PROGRAM", strIPAddressPC, currentEvent.short_name);

            CreateEventDirectory(programPath);
            List<string> directories = GetFTPFiles(programURI);
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
                while (directories.Count > 5)
                {
                    int minTimstampIndex = timestamps.IndexOf(timestamps.Min());
                    DeleteFTPFile(programURI, fileNames[minTimstampIndex]);

                    fileNames.RemoveAt(minTimstampIndex);
                    timestamps.RemoveAt(minTimstampIndex);
                    directories.RemoveAt(minTimstampIndex);
                    //directories = GetFTPFiles(programURI);
                    //timestamps.Clear();
                    //fileNames.Clear();
                    //foreach (string file in directories)
                    //{
                    //    System.Text.RegularExpressions.Match match = regex.Match(file);
                    //    Console.WriteLine(match.Groups[5].ToString());
                    //    timestamps.Add(DateTime.Parse(match.Groups[5].ToString()));
                    //    fileNames.Add(match.Groups[6].ToString());
                    //}
                }

                directories = GetFTPFiles(programURI);

                foreach (string file in directories)
                {
                    Console.WriteLine(file);
                }
            }
            progress++;
            SetProgress(progress);

            string tempFile = @"C:\Temp\" + fileNameProgram;

            int matchIndex = 0;
            int most_recent = -1;
            foreach (string file in fileNames)
            {
                if(file.Substring(0, file.Length - 10).Equals(matchABV + "_program"))
                {
                    int revision = Int32.Parse(file.Substring(file.Length - 9, 4));
                    if (revision > most_recent)
                    {
                        matchIndex = fileNames.IndexOf(file);
                        most_recent = revision;
                    }
                }
            }

            CopyFTPFile(programURI, programPath, fileNames[matchIndex], fileNameProgram, tempFile, true);
            progress++;
            SetProgress(progress);
            Console.WriteLine("Program: Done!");
            Console.WriteLine("Prgram: Progress = " + progress);
            ledProgram.BackColor = Color.Green;
        }

        private void launch_youtube()
        {
            if (currentMatch != null)
            {
                ytDescription = string.Format("{0} FRC {1} Week #{2}\n" +
                           "Red Alliance: {3} {4} {5}\n" +
                           "Blue Alliance: {6} {7} {8}\n\n" +
                           "Footage of the {0} FRC {1} is coutesy of the FIRST Washington A/V Crew\n\n" +
                           //"To view match schedules and results for this event, visit the FRC Event Results Portal:\n" +
                           //"{9}\n\n" +
                           "Folow the PNW District social media accounts for updates throughout the season!\n" +
                           "Facebook: Washington FIRST Robotics / OregonFRC\n" +
                           "Twitter: @first_wa / @OregonRobotics\n" +
                           "Youtube: Washington FIRST Robotics\n\n" +
                           "For more information and future event schedules, visit our websites:\n" +
                           "http://www.firstwa.org | http://www.oregonfirst.org \n\n" +
                           "Thanks for watching!",
                           currentEvent.year,
                           currentEvent.name,
                           currentEvent.week + 1,
                           currentMatch.Alliances.Red.TeamKeys[0].ToString().Substring(3),
                           currentMatch.Alliances.Red.TeamKeys[1].ToString().Substring(3),
                           currentMatch.Alliances.Red.TeamKeys[2].ToString().Substring(3),
                           currentMatch.Alliances.Blue.TeamKeys[0].ToString().Substring(3),
                           currentMatch.Alliances.Blue.TeamKeys[1].ToString().Substring(3),
                           currentMatch.Alliances.Blue.TeamKeys[2].ToString().Substring(3));
            }
            else
            {
                MessageBox.Show("Please enter team numbers in the description template.");
                ytDescription = string.Format("{0} FRC {1} Week #{2}\n" +
                           "Red Alliance: [RED 1] [RED 2] [RED3]\n" +
                           "Blue Alliance: [BLUE 1] [BLUE 2] [BLUE 3]\n\n" +
                           "Footage of the {0} FRC {1} is coutesy of the FIRST Washington A/V Crew\n\n" +
                           //"To view match schedules and results for this event, visit the FRC Event Results Portal:\n" +
                           //"{9}\n\n" +
                           "Folow the PNW District social media accounts for updates throughout the season!\n" +
                           "Facebook: Washington FIRST Robotics / OregonFRC\n" +
                           "Twitter: @first_wa / @OregonRobotics\n" +
                           "Youtube: Washington FIRST Robotics\n\n" +
                           "For more information and future event schedules, visit our websites:\n" +
                           "http://www.firstwa.org | http://www.oregonfirst.org \n\n" +
                           "Thanks for watching!",
                           currentEvent.year,
                           currentEvent.name,
                           currentEvent.week + 1);
            }
            

            ytTags = "first,robotics,frc," + currentEvent.year.ToString() + "," + currentEvent.event_code;

            YoutubeUpload ytForm = new YoutubeUpload(
                                    fileNameProgram,
                                    fileNameWide,
                                    ytDescription,
                                    ytTags, ManagingWindow);
            ytForm.ShowDialog();
        }

        private void bgWorker_FTP_Program_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgWorker_FTP_Wide.IsBusy)
            {
                SetProgress(progressBar1.Maximum);
                launch_youtube();
            }
        }

        private void bgWorker_FTP_Wide_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgWorker_FTP_Program.IsBusy)
            {
                SetProgress(progressBar1.Maximum);
                launch_youtube();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("WARNING!  This operation will ccancel the file copy process!\n\nDo you want to continue?", "Warning!", MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                if (bgWorker_FTP_Program.IsBusy)
                {
                    bgWorker_FTP_Program.CancelAsync();
                }

                if (bgWorker_FTP_Wide.IsBusy)
                {
                    bgWorker_FTP_Wide.CancelAsync();
                }
            }
            btnCancel.Enabled = false;
        }
        #endregion

        private void btnOpenRecordings_Click(object sender, EventArgs e)
        {
        }

        //youtube upload handler
        //

        #region Callbacks
        delegate void SetTextCallback(string text);

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

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

        delegate void SetProgressCallback(int progress);
        private void SetProgress(int progress)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressBar1.InvokeRequired)
            {
                SetProgressCallback d = new SetProgressCallback(SetProgress);
                this.Invoke(d, new object[] { progress });
            }
            else
            {
                this.progressBar1.Value = progress;
            }
        }
        #endregion
    }
}
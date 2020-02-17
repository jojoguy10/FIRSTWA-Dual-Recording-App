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
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Win32;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using NLog;
using Microsoft.VisualBasic;

/* TODO:
 * Error handling for videos that failed to record, transfer, or convert
 * Figure out a good versioning scheme
 *      Start with "year.version"
 * Figure out how to record long-run ceremonies (i.e. Opening/Closing ceremonies and awards).  
 *      I want to be able to record up to 1.5 hours to be safe.
 * If the match isn't found when the "start recording" button is pressed, don't halt the recording.
 *      The user might have forgotton to switch off of quarterfinals and needs to start recording.
 *      Give the user the option to change the match type and number is it can't be found in TBA.
 * 
 * Error handling
 * Commenting
 * Layout/UI Design
 */

using FileName = System.String;
using FilePath = System.String;
using URI = System.String;
using IPAddress = System.String;
using TCPPort = System.String;
using RegistryKeyName = System.String;

namespace FIRSTWA_Recorder
{

    public enum MapMono
    {
        None,
        Left,
        Right
    };

    public enum FormState
    {
        Idle,
        Recording,
        Processing
    };

    public partial class MainForm : Form
    {
        private const string BASESUBKEY = @"Software\FIRSTWA";

        RecordingSettings frmRecordingSetting;
        AudioSettings frmAudioSetting;
        
        RestClient tbaClient = new RestClient("http://www.thebluealliance.com/api/v3");
        RestRequest tbaRequest;
        private string TBAKEY;

        List<District> eventDistrict = new List<District>();
        List<Event> eventDetails = new List<Event>();
        Event currentEvent = new Event();
        Match currentMatch;
        string matchType;

        Match[] matches;

        string strYear = "2020";
        IPAddress strIPAddressPC = @"192.168.100.70";
        IPAddress strIPAddressPROGRAM = @"192.168.100.35";
        IPAddress strIPAddressWIDE = @"192.168.100.34";
        TCPPort strPortPROGRAM = "9993";
        TCPPort strPortWIDE = "9993";

        MapMono progChannels = MapMono.None;
        MapMono wideChannels = MapMono.None;

        FormState state;

        RegistryKeyName regYear = "YEAR";
        RegistryKeyName regPROGRAM = "PROGRAM_IPAddress";
        RegistryKeyName regWIDE = "WIDE_IPAddress";
        RegistryKeyName regPC = "PC_IPAddress";
        RegistryKeyName regProgAudio = "PROGRAM_AudioChannel";
        RegistryKeyName regWideAudio = "WIDE_AudioChannel";
        RegistryKeyName regAPIKey = "apikey";
        List<RegistryKeyName> registryKeyNames = new List<FileName>();

        int progress = 0;

        HyperDeck hdProgram, hdWide;

        string matchNameProgram = "";
        string matchNameWide = "";
        string matchABV = "";

        FileName fileNameProgram, fileNameWide;
        private FilePath tempFolder;
        private string ytDescription, ytTags;

        private DateTime startTime;

        private string programVideoTitle, wideVideoTitle;

        private bool wideFTPUploadFail = false;
        private bool programFTPUploadFail = false;

        private bool PCPingable = false;
        private Ping pinger = null;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        enum MatchType
        {
            Qualification,
            Quarterfinal,
            Semifinal,
            Final,
            Ceremony
        }
        MatchType currentMatchType = MatchType.Qualification;

        public MainForm()
        {
            InitializeComponent();

            logger.Info("Initializing Main Form");

            state = FormState.Idle;

            registryKeyNames.Add(regPROGRAM);
            registryKeyNames.Add(regWIDE);
            registryKeyNames.Add(regYear);
            registryKeyNames.Add(regPC);
            registryKeyNames.Add(regProgAudio);
            registryKeyNames.Add(regWideAudio);

            logger.Info("... Reading form variables from registry");
            try
            {
                foreach (RegistryKeyName keyName in registryKeyNames)
                {
                    if (ReadRegistryKey(keyName) == "")
                    {
                        UpdateRegistryKeys();
                    }
                }

                strYear = ReadRegistryKey(regYear);
                strIPAddressPC = ReadRegistryKey(regPC);
                strIPAddressPROGRAM = ReadRegistryKey(regPROGRAM);
                strIPAddressWIDE = ReadRegistryKey(regWIDE);

                Enum.TryParse(ReadRegistryKey(regWideAudio), out MapMono _wideChannels);
                Enum.TryParse(ReadRegistryKey(regProgAudio), out MapMono _progChannels);

                wideChannels = _wideChannels;
                progChannels = _progChannels;
            }
            catch
            {
                logger.Error("... Could not find form registry keys, creating defaults");
                UpdateRegistryKeys();
                MessageBox.Show("Initialized the registry keys.  Please check that the registry keys are correct.");
            }

            logger.Info("... Reading TBA API key from registry");
            tbaRequest = new RestRequest($"district/" + strYear + "pnw/events", Method.GET);
            TBAKEY = ReadRegistryKey(regAPIKey);

            if (TBAKEY == "")
            {
                logger.Fatal("- Failed: Could not find TBA API key");
                TBAKEY = Interaction.InputBox("Please enter the TBA API key.", "TBA API Key", "");

                if (TBAKEY == "")
                {
                    MessageBox.Show("Could not find a TBA API key in the registry.  Closing...");
                    Application.Exit();
                }

                WriteRegistryKey(regAPIKey, TBAKEY);
            }

            logger.Info("... Getting event list from TBA");
            tbaRequest.AddHeader
            (
                "X-TBA-Auth-Key",
                TBAKEY
            );
            IRestResponse tbaResponse = tbaClient.Execute(tbaRequest);
            string tbaContent = tbaResponse.Content;
            tbaContent = tbaContent.Trim('"');

            eventDistrict = JsonConvert.DeserializeObject<List<District>>(tbaContent);
            eventDetails = JsonConvert.DeserializeObject<List<Event>>(tbaContent);

            eventDetails.ForEach(x => comboEventName.Items.Add((x.week + 1) + " - " + x.first_event_code + " - " + x.location_name));
            comboEventName.Sorted = true;
            comboEventName.Items.Add("Custom Event");

            logger.Info("... Creating Temp directory");
            tempFolder = Path.GetTempPath() + "FIRSTWA-Recorder\\";
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            logger.Info("... Initializing Settings Forms");
            frmRecordingSetting = new RecordingSettings(strYear, strIPAddressPC, strIPAddressPROGRAM, strIPAddressWIDE);
            frmAudioSetting = new AudioSettings(wideChannels,progChannels);

            if (!Debugger.IsAttached)
            {
                groupEvent.Enabled = false;
                groupMatch.Enabled = false;
                btnStartRecording.Enabled = false;
                btnStopRecording.Enabled = false;
                logger.Info("- Done");
            }
        }

        #region Registry
        private string ReadRegistryKey(string key)
        {
            RegistryKey firstwaKey = Registry.CurrentUser.OpenSubKey(BASESUBKEY, true);
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
            WriteRegistryKey(regYear, strYear);
            WriteRegistryKey(regPC, strIPAddressPC);
            WriteRegistryKey(regPROGRAM, strIPAddressPROGRAM);
            WriteRegistryKey(regWIDE, strIPAddressWIDE);
            WriteRegistryKey(regWideAudio, wideChannels.ToString());
            WriteRegistryKey(regProgAudio, progChannels.ToString());
        }

        private void WriteRegistryKey(string key, string value)
        {
            RegistryKey firstwaKey = Registry.CurrentUser.OpenSubKey(BASESUBKEY, true);
            if (firstwaKey == null)
            {
                firstwaKey = Registry.CurrentUser.CreateSubKey(BASESUBKEY);
            }
            
            firstwaKey.SetValue(key, value);
        }
        #endregion

        private bool SearchValidMatch()
        {
            string matchAbrev = "qm";
            switch (currentMatchType)
            {
                case MatchType.Qualification:
                    matchType = "Q";
                    matchAbrev = "qm";
                    break;
                case MatchType.Quarterfinal:
                    matchType = "QF";
                    matchAbrev = "qf";
                    break;
                case MatchType.Semifinal:
                    matchType = "SF";
                    matchAbrev = "sf";
                    break;
                case MatchType.Final:
                    matchType = "F";
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
            else if (currentMatchType != MatchType.Ceremony)
            {
                matchABV = string.Format("{0}_{1}{2}m{3}", currentEvent.event_code, matchAbrev, numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
            }
            else
            {
                matchABV = string.Format("{0}c{1}", currentEvent.event_code, txtCeremonyTitle.Text.ToString());
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
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            logger.Info("Attempting to start recording");
            if (comboEventName.SelectedItem == null)
            {
                logger.Error("- Failed: no event selected");
                MessageBox.Show("Please choose an event before recording");
                return;
            }

            programFTPUploadFail = false;
            wideFTPUploadFail = false;
            
            if (!SearchValidMatch())
            {
                GetMatches();

                SearchValidMatch();
                if (!SearchValidMatch())
                {
                    logger.Info("... match not found");
                    var result = MessageBox.Show("Match does not exist!\n\nDo you want to continue recording?", "Error", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes)
                    {
                        logger.Error("- Failed: User canceled since match was not found");
                        return;
                    }
                }
            }

            logger.Info("... Match abreviation: {0}", matchABV);

            groupEvent.Enabled = false;
            groupMatch.Enabled = false;

            btnStartRecording.Enabled = false;

            Regex sanatize = new Regex("[^a-zA-Z0-9_ ]");
            txtCeremonyTitle.Text = sanatize.Replace(txtCeremonyTitle.Text.ToString(), "");

            if (chkProgramRecord.Checked)
            {
                logger.Info("... Beginning program recording");
                if (currentMatchType == MatchType.Qualification || currentMatchType == MatchType.Final)
                {
                    matchNameProgram = string.Format("{2}{3} {0} {1}", currentEvent.year, currentEvent.name, matchType, numMatchNumber.Value.ToString());
                }
                else if (currentMatchType != MatchType.Ceremony)
                {
                    matchNameProgram = string.Format("{2}{3}-{4} {0} {1}", currentEvent.year, currentEvent.name, matchType, numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                }
                else
                {
                    matchNameProgram = string.Format("{0} {1} Ceremony {2}", currentEvent.year, currentEvent.name, txtCeremonyTitle.Text.ToString());
                }

                fileNameProgram = matchNameProgram + ".mp4";

                hdProgram.Write("record: name: " + matchABV +"_program");
                
                logger.Info("... Program file name: {0}", fileNameProgram);

                string status = hdProgram.Read();
                if (!status.Contains("200"))
                {
                    logger.Error("... Program recording failed to start:");
                    logger.Info(status);
                    btnConnectWide.BackColor = Color.Yellow;
                }
            }

            if (chkRecordWide.Checked && currentMatchType != MatchType.Ceremony)
            {
                logger.Info("... Beginning wide recording");
                if (currentMatchType == MatchType.Qualification || currentMatchType == MatchType.Final)
                {
                    matchNameWide = string.Format("{2}{3} {0} {1} - WIDE ", currentEvent.year, currentEvent.name, matchType, numMatchNumber.Value.ToString());
                }
                else
                {
                    matchNameWide = string.Format("{2}{3}-{4} {0} {1} - WIDE", currentEvent.year, currentEvent.name, matchType, numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                }
                fileNameWide = matchNameWide + ".mp4";

                hdWide.Write("record: name: " + matchABV + "_wide");

                
                logger.Info("... Wide file name: {0}", fileNameWide);

                string status = hdWide.Read();
                if (!status.Contains("200"))
                {
                    logger.Error("... Wide recording failed to start:");
                    logger.Info(status);
                    btnConnectWide.BackColor = Color.Yellow;
                }
            }

            startTime = DateTime.Now;
            timerElapsed.Start();

            btnStopRecording.Enabled = true;
            state = FormState.Recording;
            bgWorker_WD.RunWorkerAsync();

            SetProgress(0);
            progress = 0;

            ledProgram.BackColor = Color.Red;
            ledWide.BackColor = Color.Red;
            logger.Info("- Done");
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            logger.Info("Stopping recording");
            btnStopRecording.Enabled = false;
            state = FormState.Processing;

            timerElapsed.Stop();

            if (chkProgramRecord.Checked)
            {
                logger.Info("... Stopping Program Recording");
                hdProgram.Write("stop");
                string status = hdProgram.Read();
                if (!status.Contains("200"))
                {
                    btnConnectWide.BackColor = Color.Yellow;
                    logger.Error("... Program recording failed to stop:");
                    logger.Info(status);
                }

                bgWorker_FTP_Program.RunWorkerAsync();
            }


            if (currentMatchType != MatchType.Ceremony && chkRecordWide.Checked) {
                logger.Info("... Stopping Wide Recording");
                hdWide.Write("stop");
                string status = hdWide.Read();
                if (!status.Contains("200"))
                {
                    btnConnectWide.BackColor = Color.Yellow;
                    logger.Error("... Wide recording failed to stop:");
                    logger.Info(status);
                }

                bgWorker_FTP_Wide.RunWorkerAsync();
            }

            //
            //  Clear Old files from TEMP folder
            //

            logger.Info("... Clearing old temp files");
            List<string> directories = Directory.GetFiles(tempFolder).ToList();
            List<DateTime> timestamps = new List<DateTime>();

            foreach (string file in directories)
            {
                timestamps.Add(File.GetCreationTime(file));
            }

            if (directories.Count > 10)
            {
                while (directories.Count > 10)
                {
                    int minTimstampIndex = timestamps.IndexOf(timestamps.Min());
                    File.Delete(directories[minTimstampIndex]);

                    timestamps.RemoveAt(minTimstampIndex);
                    directories.RemoveAt(minTimstampIndex);
                }
            }

            logger.Info("... incrementing match numbers");
            if (currentMatchType == MatchType.Qualification || currentMatchType == MatchType.Final)
            {
                if (numMatchNumber.Value < numMatchNumber.Maximum)
                {
                    numMatchNumber.Value++;
                }
            }
            else if(currentMatchType != MatchType.Ceremony)
            {
                if(numFinalNo.Value < numFinalNo.Maximum)
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

            btnCancel.Enabled = true;
            groupEvent.Enabled = true;
            groupMatch.Enabled = true;
            btnStartRecording.Enabled = true;
            logger.Info("- Done");
        }

        #region FTP Stuff
        private void CreateEventDirectory(URI uriPath)
        {
            logger.Info("Connecting to PC and creating remote directory");
            try
            {
                WebRequest request = WebRequest.Create(uriPath);
                request.Timeout = 2000;
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential("FTP_User", "");
                
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    logger.Info("... response:\n{0}", resp.StatusCode);
                }
            }
            catch (WebException e)
            {
                logger.Error("... Failed to connect to PC");
                logger.Info(e.Status);
                logger.Info(e.Message);
                bgWorker_FTP_Program.CancelAsync();
                bgWorker_FTP_Wide.CancelAsync();
                return;
            }
            catch (Exception e)
            {
                logger.Error("... unhandled error");
                logger.Info(e.Message);
            }

        }

        //convert the mp4 from uncompressed audio to mp3 audio using ffmpeg
        //videoPath - filepath of mp4 to convert
        private void ConvertVideo(FilePath videoPath, MapMono style)
        {
            logger.Info("Starting Conversion for {0}", videoPath);
            string videoName = videoPath.Substring(0,videoPath.Length - 4);
            string outVideo = videoName + "test.mp4";

            StringBuilder args_proto = new StringBuilder();

            if (style == MapMono.Left)
            {
                args_proto.AppendFormat("-y -acodec pcm_s24le -i \"{0}\" -acodec mp3 -vcodec copy -af \"pan=mono|c0=c0\" \"{1}\"", videoPath, outVideo);
                logger.Info("... Left Mono Pipe for {0}", videoPath);
            }
            else if (style == MapMono.Right)
            {
                args_proto.AppendFormat("-y -acodec pcm_s24le -i \"{0}\" -acodec mp3 -vcodec copy -af \"pan=mono|c0=c1\" \"{1}\"", videoPath, outVideo);
                logger.Info("... Right Mono Pipe for {0}", videoPath);
            }
            else
            {
                args_proto.AppendFormat("-y -acodec pcm_s24le -i \"{0}\" -acodec mp3 -vcodec copy \"{1}\"", videoPath, outVideo);
                logger.Info("... Stereo Pipe for {0}", videoPath);
            }

            string args = args_proto.ToString();

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
                logger.Error(eventArgs);
                MessageBox.Show(eventArgs.Data);
            };

            process.Start();

            process.WaitForExit();

            logger.Info("Conversion Done: {0}", videoPath);

            File.Delete(videoPath);

            File.Move(outVideo, videoPath);
        }

        //download an mp4 from a remote server
        //uri - connection to download from
        //ftpFileName - file path at target remote server
        //localFilePath - file path at local
        private void DownloadFileFTP(FilePath remotePath, FilePath localFilePath)
        {
            logger.Info("Copying {0} from hyperdeck", localFilePath);
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
            }
            progress++;
            SetProgress(progress);
            logger.Info("Done Copying {0} from hyperdeck", localFilePath);
        }

        //upload an mp4 to a remote server
        //uri - connection and file path at remote to upload to
        //filePath - file path at local to upload from
        public void UploadFileFTP(URI uri, FilePath filePath)
        {
            logger.Info("Copying {0} to remote", filePath);
            progress++;
            SetProgress(progress);
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Credentials = new NetworkCredential("FTP_User", "");
                    client.UploadFile(uri, WebRequestMethods.Ftp.UploadFile, filePath);
                }
                catch
                {
                    if (filePath.Contains(fileNameWide))
                    {
                        wideFTPUploadFail = true;
                    }
                    else
                    {
                        programFTPUploadFail = true;
                    }
                }
            }
            progress++;
            SetProgress(progress);
            logger.Info("Done Copying {0} to remote", filePath);
        }

        //delete a file at a remote server
        //uri - remote server to delete at
        //filename - 
        private void DeleteFTPFile(URI uri, string filename)
        {
            string fullDir = uri + "/" + filename;
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(fullDir);
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            
            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            response.Close();
        }

        private List<string> GetFTPFiles(URI uri)
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

                lblFinalNo.Visible = true;
                numFinalNo.Visible = true;
                lblMatchNumber.Visible = true;
                numMatchNumber.Visible = true;
                lblCeremonyTitle.Visible = false;
                txtCeremonyTitle.Visible = false;

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
                        numMatchNumber.Maximum = 3;
                        numFinalNo.Maximum = 4;
                        break;
                    case "Semifinal":
                        currentMatchType = MatchType.Semifinal;
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
                    case "Ceremony":
                        currentMatchType = MatchType.Ceremony;
                        lblMatchNumber.Visible = false;
                        numMatchNumber.Visible = false;
                        lblFinalNo.Visible = false;
                        numFinalNo.Visible = false;
                        lblCeremonyTitle.Visible = true;
                        txtCeremonyTitle.Visible = true;
                        numMatchNumber.Maximum = 1;
                        numFinalNo.Maximum = 1;
                        break;
                    default:
                        break;
                }
                GetMatches();
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

                    programVideoTitle = currentEvent.year + " " + currentEvent.name + " " + matchType + " " + numMatchNumber.Value;
                    wideVideoTitle = currentEvent.year + " " + currentEvent.name + " WIDE " + matchType + " " + numMatchNumber.Value;
                    GetMatches();
                    groupMatch.Enabled = true;
                }
                else
                {
                    // Custom Event

                    programVideoTitle = "<CUSTOM EVENT> " + matchType + " " + numMatchNumber.Value;
                    wideVideoTitle = "<CUSTOM EVENT> WIDE " + matchType + " " + numMatchNumber.Value;
                    groupMatch.Enabled = true;
                }
            }
        }

        private void GetMatches()
        {
            logger.Info("Getting Match List from TBA");
            tbaRequest = new RestRequest(string.Format("event/{0}/matches/simple", currentEvent.key), Method.GET);

            tbaRequest.AddHeader
            (
                "X-TBA-Auth-Key",
                TBAKEY
            );

            IRestResponse tbaResponse = tbaClient.Execute(tbaRequest);
            string tbaContent = tbaResponse.Content;
            matches = JsonConvert.DeserializeObject<Match[]>(tbaContent);
            logger.Info("Done Getting Match List from TBA");
        }
        
        private void btnConnectProgram_Click(object sender, EventArgs e)
        {
            logger.Info("Connecting to Program Hyperdeck");
            try
            {
                hdProgram = new HyperDeck(strIPAddressPROGRAM, Convert.ToInt32(strPortPROGRAM));
                logger.Info("... Status:\n{0}", hdProgram.Read());
                
                btnConnectProgram.BackColor = Color.Green;
                logger.Info("- Done");
            }
            catch (Exception ue)
            {
                logger.Error(ue);
                logger.Error("- Failed");
                MessageBox.Show(string.Format("Could not connect to the Program recorder\nat the IP address: {0}", strIPAddressPROGRAM));
            }
        }

        private void btnConnectWide_Click(object sender, EventArgs e)
        {
            logger.Info("Connecting to Wide");
            try
            {
                hdWide = new HyperDeck(strIPAddressWIDE, Convert.ToInt32(strPortWIDE));
                logger.Info("... Status:\n{0}", hdWide.Read());

                btnConnectWide.BackColor = Color.Green;
                logger.Info("- Done");
            }
            catch (Exception ue)
            {
                logger.Error(ue);
                logger.Error("- Failed");
                MessageBox.Show(string.Format("Could not connect to the Wide recorder\nat the IP address: {0}", strIPAddressWIDE));
            }
        }

        private void btnConnectPC_Click(object sender, EventArgs e)
        {
            logger.Info("Connecting to PC");
            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(strIPAddressPC);
                PCPingable = reply.Status == IPStatus.Success;
            }
            catch (PingException pe)
            {
                logger.Error(pe);
                
                btnConnectPC.BackColor = Color.Green;
                groupEvent.Enabled = true;
                btnStartRecording.Enabled = true;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            if (PCPingable)
            {
                btnConnectPC.BackColor = Color.Green;
                groupEvent.Enabled = true;
                btnStartRecording.Enabled = true;
                logger.Info("- Done");
            }
            else
            {
                btnConnectPC.BackColor = Color.Red;
                groupEvent.Enabled = false;
                btnStartRecording.Enabled = false;
                MessageBox.Show("Could not connect to the PC.  Please check the IP address is correct.");
                logger.Error("- Failed");
            }
        }

        #region Background Workers
        private void bgWorker_FTP_Wide_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Info("Starting Wide worker");
            logger.Info("... Wide worker waiting");
            lblReportA.Invoke((Action)(() => { lblReportA.Text = "Waiting"; }));
            Thread.Sleep(1000);

            logger.Info("... Wide cleaning HyperDeck SD");
            lblReportA.Invoke((Action)(() => { lblReportA.Text = "Clearing SD"; }));

            progress++;
            SetProgress(progress);
            URI wideURI = string.Format("ftp://{0}/1", strIPAddressWIDE);
            FilePath widePath = string.Format("ftp://{0}/2019/{1}/WIDE", strIPAddressPC, currentEvent.short_name);

            CreateEventDirectory(widePath);
            List<string> directories = GetFTPFiles(wideURI);
            List<DateTime> timestamps = new List<DateTime>();
            List<string> fileNames = new List<string>();

            Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            foreach (string file in directories)
            {
                System.Text.RegularExpressions.Match match = regex.Match(file);
                timestamps.Add(DateTime.Parse(match.Groups[5].ToString()));
                fileNames.Add(match.Groups[6].ToString());
            }

            if (directories.Count > 2)
            {
                while (directories.Count > 2)
                {
                    int minTimstampIndex = timestamps.IndexOf(timestamps.Min());

                    DeleteFTPFile(wideURI, fileNames[minTimstampIndex]);
                    fileNames.RemoveAt(minTimstampIndex);
                    timestamps.RemoveAt(minTimstampIndex);
                    directories.RemoveAt(minTimstampIndex);
                }
            }
            
            progress++;
            SetProgress(progress);

            lblReportA.Invoke((Action)(()=> { lblReportA.Text = "Finding Video"; }));
            logger.Info("... Finding Wide source file");

            string tempFile = tempFolder + fileNameWide;

            int matchIndex = -1;
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
            if(matchIndex < 0)
            {
                lblReportA.Invoke((Action)(()=> { lblReportA.Text = "Failed to Find"; }));
                logger.Info("- Failed: Wide source file not found");
                return;
            }

            lblReportA.Invoke((Action)(()=> { lblReportA.Text = "Downloading Video"; }));
            logger.Info("... Wide worker downloading source");
            DownloadFileFTP(wideURI + "/" + fileNames[matchIndex], tempFile);

            lblReportA.Invoke((Action)(()=> { lblReportA.Text = "Converting Video"; }));
            logger.Info("... Wide worker processing");
            ConvertVideo(tempFile, wideChannels);

            lblReportA.Invoke((Action)(()=> { lblReportA.Text = "Uploading Video"; }));
            logger.Info("... Wide worker uploading");
            UploadFileFTP(widePath + "/" + fileNameWide, tempFile);

            logger.Info("- Done: Wide");
            lblReportA.Invoke((Action)(()=> { lblReportA.Text = "Done"; }));

            if ((wideFTPUploadFail || programFTPUploadFail) && !bgWorker_FTP_Wide.IsBusy)
            {
                MessageBox.Show("WARNING: The last match did not copy to the FTP folder!");
            }

            ledWide.BackColor = Color.Green;
        }

        private void bgWorker_FTP_Program_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Info("Starting Program worker");
            logger.Info("... Program worker waiting");
            lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Waiting"; }));
            Thread.Sleep(1000);

            logger.Info("... Program cleaning HyperDeck SD");
            lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Clearing SD"; }));

            progress++;
            SetProgress(progress);
            URI programURI = string.Format("ftp://{0}/1", strIPAddressPROGRAM);
            FilePath programPath = string.Format("ftp://{0}/2019/{1}/PROGRAM", strIPAddressPC, currentEvent.short_name);

            CreateEventDirectory(programPath);
            List<string> directories = GetFTPFiles(programURI);
            List<DateTime> timestamps = new List<DateTime>();
            List<string> fileNames = new List<string>();

            Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            foreach (string file in directories)
            {
                System.Text.RegularExpressions.Match match = regex.Match(file);
                timestamps.Add(DateTime.Parse(match.Groups[5].ToString()));
                fileNames.Add(match.Groups[6].ToString());
            }

            if (directories.Count > 2)
            {
                while (directories.Count > 2)
                {
                    int minTimstampIndex = timestamps.IndexOf(timestamps.Min());
                    DeleteFTPFile(programURI, fileNames[minTimstampIndex]);

                    fileNames.RemoveAt(minTimstampIndex);
                    timestamps.RemoveAt(minTimstampIndex);
                    directories.RemoveAt(minTimstampIndex);
                }
            }

            progress++;
            SetProgress(progress);

            lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Downloading Video"; }));
            logger.Info("... Finding Program source file");


            string tempFile = tempFolder + fileNameProgram;

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

            if (matchIndex < 0)
            {
                lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Failed to Find"; }));
                logger.Info("- Failed: Program source file not found");
                return;
            }

            logger.Info("... Program worker downloading source");
            lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Downloading Video"; }));
            DownloadFileFTP(programURI + "/" + fileNames[matchIndex], tempFile);

            logger.Info("... Program worker processing");
            lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Converting Video"; }));
            ConvertVideo(tempFile, progChannels);

            logger.Info("... Program worker uploading");
            lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Uploading Video"; }));
            UploadFileFTP(programPath + "/" + fileNameProgram, tempFile);

            logger.Info("- Done: Program");
            lblReportB.Invoke((Action)(()=> { lblReportB.Text = "Done"; }));

            if ((wideFTPUploadFail || programFTPUploadFail) && !bgWorker_FTP_Program.IsBusy)
            {
                MessageBox.Show("WARNING: The last match did not copy to the FTP folder!");
            }

            ledProgram.BackColor = Color.Green;
        }

        private void bgWorker_WD_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
            while(state == FormState.Recording)
            {
                hdProgram.Write("transport info");
                string progStatus = hdProgram.Read();

                hdWide.Write("transport info");
                string wideStatus = hdWide.Read();
                
                logger.Info("Live Record Status:");
                logger.Info(wideStatus);
                logger.Info(progStatus);

                if (!wideStatus.Contains("record"))
                {
                    btnConnectWide.BackColor = Color.Yellow;
                }
                if (!progStatus.Contains("record"))
                {
                    btnConnectProgram.BackColor = Color.Yellow;
                }
                Thread.Sleep(1000);
            }
        }

        private void btnShowYT_Click(object sender, EventArgs e)
        {
            YoutubeUpload YTForm = new YoutubeUpload(
                                    fileNameProgram,
                                    fileNameWide,
                                    ytDescription,
                                    ytTags);
            YTForm.StartPosition = FormStartPosition.CenterParent;
            YTForm.Show();
        }

        private void version001ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutPage about = new AboutPage();
            about.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpPage help = new HelpPage();
            help.Show();
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


            YoutubeUpload YTForm = new YoutubeUpload(
                                    fileNameProgram,
                                    fileNameWide,
                                    ytDescription,
                                    ytTags);
            YTForm.StartPosition = FormStartPosition.CenterParent;
            YTForm.Show();
            btnShowYT.Enabled = true;
        }

        private void audioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult settingsResult = frmAudioSetting.ShowDialog();
            if (settingsResult == DialogResult.OK)
            {
                wideChannels = frmAudioSetting.wide;
                progChannels = frmAudioSetting.prog;

                UpdateRegistryKeys();
            }
        }

        private void txtCeremonyTitle_Leave(object sender, EventArgs e)
        {
            Regex sanatize = new Regex("[^a-zA-Z0-9_ ]");
            txtCeremonyTitle.Text = sanatize.Replace(txtCeremonyTitle.Text.ToString(), "");
        }

        private void bgWorker_FTP_Program_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgWorker_FTP_Wide.IsBusy)
            {
                SetProgress(progressBar1.Maximum);
                state = FormState.Idle;
                launch_youtube();
            }
        }

        private void btnTempAccess_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(tempFolder);
            }
            catch(Exception ue)
            {
                logger.Error("Failed to access temp folder for user");
                logger.Info(ue.Message);
            }
        }

        private void bgWorker_FTP_Wide_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgWorker_FTP_Program.IsBusy)
            {
                SetProgress(progressBar1.Maximum);
                state = FormState.Idle;
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
            state = FormState.Idle;
        }
        #endregion

        private void btnOpenRecordings_Click(object sender, EventArgs e)
        {
        }

        //youtube upload handler
        //

        #region Callbacks
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
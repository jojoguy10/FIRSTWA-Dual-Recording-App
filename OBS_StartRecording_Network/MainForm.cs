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
        RecordingSettings frmRecordingSetting;

        RestClient tbaClient = new RestClient("http://www.thebluealliance.com/api/v3");
        RestRequest tbaRequest = new RestRequest($"district/2019pnw/events", Method.GET);
        private string TBAKEY;

        List<District> eventDistrict = new List<District>();
        List<Event> eventDetails = new List<Event>();
        Event currentEvent = new Event();
        Match currentMatch;
        string matchKey;

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

        DeckLink dlProgram, dlWide;

        private string fileNameProgram, fileNameWide;
        private string ytDescription, ytTags;

        private DateTime startTime;

        private FileInfo credFile = new FileInfo(@"D:\__USER\Documents\GitHub\FIRSTWA_PC_RecordingApplication\FIRSTWA_StartRecording_Network\client_secret_613443767055-pvnp5ugap7kgj1i7rid6in7tnm3podmv.apps.googleusercontent.com.json");
        private Video videoYT;
        
        private string replay = "";

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

            frmRecordingSetting = new RecordingSettings(strIPAddressPC, strIPAddressPROGRAM, strIPAddressWIDE);
            
            groupEvent.Enabled = false;
            groupMatch.Enabled = false;
            btnStartRecording.Enabled = false;
            btnStopRecording.Enabled = false;
        }

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
            string matchAbrev = "qm";
            
            if (chkProgramRecord.Checked)
            {
                //dlProgram.Write("record");

                string matchType, matchNumber;
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

                if (currentMatchType == MatchType.Qualification)
                {
                    matchNumber = numMatchNumber.Value.ToString();
                }
                else
                {
                    matchNumber = string.Format("{0}-{1}", numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                }

                string matchNameProgram = string.Format("{0} {1} {2} {3}", currentEvent.year, currentEvent.name, matchType, matchNumber);
                fileNameProgram = matchNameProgram + ".mp4";

                dlProgram.Write("record: name: " + matchNameProgram);
            }

            if (chkRecordWide.Checked)
            {
                //dlWide.Write("record");

                string matchType, matchNumber;
                switch (currentMatchType)
                {
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

                if (currentMatchType == MatchType.Qualification)
                {
                    matchNumber = numMatchNumber.Value.ToString();
                }
                else
                {
                    matchNumber = string.Format("{0}-{1}", numFinalNo.Value.ToString(), numMatchNumber.Value.ToString());
                }

                string matchNameWide = string.Format("{0} {1} WIDE {2} {3}", currentEvent.year, currentEvent.name, matchType, matchNumber);
                fileNameWide = matchNameWide + ".mp4";

                dlWide.Write("record: name: " + matchNameWide);

            }

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
                MessageBox.Show("Match does not exist!");
                return;
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

            dlProgram.Write("stop");

            dlWide.Write("stop");

            bgWorker_FTP_Program.RunWorkerAsync();
            bgWorker_FTP_Wide.RunWorkerAsync();

            btnStartRecording.Enabled = true;

            numMatchNumber.Value++;

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

            ytTags = "first,robotics,frc," + currentEvent.year.ToString() + "," + currentEvent.event_code;

            YoutubeUpload ytForm = new YoutubeUpload(
                                    fileNameProgram.Replace(".mp4", ""),
                                    fileNameWide.Replace(".mp4", ""),
                                    ytDescription,
                                    ytTags);
            ytForm.ShowDialog();
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

        private void CopyFTPFile(string fromURI, string toURI, string fromFilename, string toFilename, string tempFile)
        {
            progress++;
            SetProgress(progress);
            string video = DownloadFileFTP(fromURI, fromFilename, tempFile);
            UploadFileFTP(toURI + "/" + toFilename, video);
            progress++;
            SetProgress(progress);
        }

        private string DownloadFileFTP(string uri, string ftpFileName, string fileName)
        {
            progress++;
            SetProgress(progress);
            Console.WriteLine(ftpFileName);
            ftpFileName = ftpFileName.Replace(".mcc", ".mp4");
            if (!Directory.Exists(@"C:\Temp"))
            {
                Directory.CreateDirectory(@"C:\Temp");
            }
            string inputfilepath = fileName;

            string ftpfullpath = uri + "/" + ftpFileName;

            using (WebClient request = new WebClient())
            {
                request.DownloadFile(ftpfullpath, inputfilepath);

                //using (FileStream file = File.Create(inputfilepath))
                //{

                //    file.Write(fileData, 0, fileData.Length);
                //    file.Close();
                //}
                Console.WriteLine("Download from Recorder: Complete");
            }
            progress++;
            SetProgress(progress);
            return inputfilepath;
        }

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
                switch (btn.Text)
                {
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
            //List<string> newMatchNames = new List<string>();
            //List<Tuple<string, int>> qm = new List<Tuple<string, int>>(); // Qualification matches
            //List<Tuple<string, int, int>> qf = new List<Tuple<string, int, int>>(); // Qualification matches
            //List<Tuple<string, int, int>> sf = new List<Tuple<string, int, int>>(); // Qualification matches
            //List<Tuple<string, int, int>> f = new List<Tuple<string, int, int>>(); // Qualification matches

            //foreach (Match match in matches)
            //{
            //    string type = null;
            //    int matchNo = 0;
            //    int setNo = 0;

            //    if (match.CompLevel.Equals("qm")) { type = "Qualification"; }
            //    else if (match.CompLevel.Equals("qf")) { type = "Quarterfinal"; setNo = match.SetNumber; }
            //    else if (match.CompLevel.Equals("sf")) { type = "Semifinal"; setNo = match.SetNumber; }
            //    else if (match.CompLevel.Equals("f")) { type = "Final"; setNo = match.SetNumber; }

            //    matchNo = match.MatchNumber;

            //    if (type == "Qualification")
            //    {
            //        newMatch = string.Format("{0} {1}", type, matchNo);
            //    }
            //    else
            //    {
            //        newMatch = string.Format("{0} {1} {2} {3}", type, setNo, "Match", matchNo);
            //    }

            //    switch (type)
            //    {
            //        case "Qualification":
            //            qm.Add(new Tuple<string, int>(newMatch, Convert.ToInt16(matchNo)));
            //            break;
            //        case "Quarterfinal":
            //            qf.Add(new Tuple<string, int, int>(newMatch, Convert.ToInt16(setNo), Convert.ToInt16(matchNo)));
            //            break;
            //        case "Semifinal":
            //            sf.Add(new Tuple<string, int, int>(newMatch, Convert.ToInt16(setNo), Convert.ToInt16(matchNo)));
            //            break;
            //        case "Final":
            //            f.Add(new Tuple<string, int, int>(newMatch, Convert.ToInt16(setNo), Convert.ToInt16(matchNo)));
            //            break;

            //        default:
            //            break;
            //    }

            //    newMatchNames.Add(newMatch);
            //}
            //qm = qm.OrderBy(t => t.Item2).ToList();
            //qf = qf.OrderBy(t => t.Item3).ToList();
            //sf = sf.OrderBy(t => t.Item3).ToList();
            //f = f.OrderBy(t => t.Item3).ToList();
        }
        
        private async Task GetMatchDetails()
        {
            /* This button will look at the selected match and match type from the UI and 
             * query TBA to get the specifics.  When the Match
             */
            string shortMatchType = "";
            string currentMatchKey = "";
            switch (currentMatchType)
            {
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
            RestRequest tbaRequest = new RestRequest(string.Format("event/{0}/matches", currentEvent.key), Method.GET);

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

        private void btnGetMatchDetails_Click(object sender, EventArgs e)
        {
            GetMatchDetails();
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
                groupEvent.Enabled = true;
                btnConnectProgram.BackColor = Color.Green;
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
                groupEvent.Enabled = true;
                btnConnectWide.BackColor = Color.Green;
            }
            catch
            {
                MessageBox.Show(string.Format("Could not connect to the Wide recorder\nat the IP address: {0}", strIPAddressPROGRAM));
            }
        }

        private void bgWorker_FTP_Wide_DoWork(object sender, DoWorkEventArgs e)
        {
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
                DeleteFTPFile(wideURI, fileNames[timestamps.IndexOf(timestamps.Min())]);

                directories = GetFTPFiles(wideURI);

                foreach (string file in directories)
                {
                    Console.WriteLine(file);
                }
            }
            progress++;
            SetProgress(progress);
            string tempFile = @"C:\Temp\temp_Wide.mp4";
            CopyFTPFile(wideURI, widePath, fileNames[timestamps.IndexOf(timestamps.Max())], fileNameWide, tempFile);
            progress++;
            SetProgress(progress);
            File.Delete(tempFile);
            Console.WriteLine("Wide: Done!");
            Console.WriteLine("Wide: Progress = " + progress);
            ledWide.BackColor = Color.Green;
        }

        private void bgWorker_FTP_Program_DoWork(object sender, DoWorkEventArgs e)
        {
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
                    DeleteFTPFile(programURI, fileNames[timestamps.IndexOf(timestamps.Min())]);
                    directories = GetFTPFiles(programURI);

                    foreach (string file in directories)
                    {
                        System.Text.RegularExpressions.Match match = regex.Match(file);
                        Console.WriteLine(match.Groups[5].ToString());
                        timestamps.Add(DateTime.Parse(match.Groups[5].ToString()));
                        fileNames.Add(match.Groups[6].ToString());
                    }
                }

                directories = GetFTPFiles(programURI);

                foreach (string file in directories)
                {
                    Console.WriteLine(file);
                }
            }
            progress++;
            SetProgress(progress);
            string tempFile = @"C:\Temp\temp_Program.mp4";
            CopyFTPFile(programURI, programPath, fileNames[timestamps.IndexOf(timestamps.Max())], fileNameProgram, tempFile);
            progress++;
            SetProgress(progress);
            File.Delete(tempFile);
            Console.WriteLine("Program: Done!");
            Console.WriteLine("Prgram: Progress = " + progress);
            ledProgram.BackColor = Color.Green;
        }

        delegate void SetTextCallback(string text);

        private void bgWorker_FTP_Program_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgWorker_FTP_Wide.IsBusy)
            {
                SetProgress(progressBar1.Maximum);
            }
        }

        private void bgWorker_FTP_Wide_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgWorker_FTP_Program.IsBusy)
            {
                SetProgress(progressBar1.Maximum);
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

        private void btnOpenRecordings_Click(object sender, EventArgs e)
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
    }
}
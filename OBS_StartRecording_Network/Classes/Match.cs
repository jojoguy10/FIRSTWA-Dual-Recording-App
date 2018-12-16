namespace OBS_StartRecording_Network
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using RestSharp;

    public partial class Match
    {
        public Match(string TBAKEY, string matchKey)
        {
            RestClient tbaClient = new RestClient("http://www.thebluealliance.com/api/v3");
            RestRequest tbaRequest = new RestRequest(string.Format("match/{0}", matchKey), Method.GET);

            tbaRequest.AddHeader
            (
                "X-TBA-Auth-Key",
                TBAKEY
            );

            IRestResponse tbaResponse = tbaClient.Execute(tbaRequest);
            string tbaContent = tbaResponse.Content;
            tbaContent = tbaContent.Trim('"');
        }

        [JsonProperty("actual_time")]
        public long ActualTime { get; set; }

        [JsonProperty("alliances")]
        public Alliances Alliances { get; set; }

        [JsonProperty("comp_level")]
        public string CompLevel { get; set; }

        [JsonProperty("event_key")]
        public string EventKey { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("match_number")]
        public long MatchNumber { get; set; }

        [JsonProperty("post_result_time")]
        public long PostResultTime { get; set; }

        [JsonProperty("predicted_time")]
        public long PredictedTime { get; set; }

        [JsonProperty("score_breakdown")]
        public ScoreBreakdown ScoreBreakdown { get; set; }

        [JsonProperty("set_number")]
        public long SetNumber { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("videos")]
        public List<MatchVideo> Videos { get; set; }

        [JsonProperty("winning_alliance")]
        public string WinningAlliance { get; set; }
    }

    public partial class Alliances
    {
        [JsonProperty("blue")]
        public Alliance Blue { get; set; }

        [JsonProperty("red")]
        public Alliance Red { get; set; }
    }

    public partial class Alliance
    {
        [JsonProperty("dq_team_keys")]
        public List<object> DqTeamKeys { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("surrogate_team_keys")]
        public List<object> SurrogateTeamKeys { get; set; }

        [JsonProperty("team_keys")]
        public List<string> TeamKeys { get; set; }
    }

    public partial class ScoreBreakdown
    {
        [JsonProperty("blue")]
        public ScoreBreakdownBlue Blue { get; set; }

        [JsonProperty("red")]
        public ScoreBreakdownBlue Red { get; set; }
    }

    public partial class ScoreBreakdownBlue
    {
        [JsonProperty("adjustPoints")]
        public long AdjustPoints { get; set; }

        [JsonProperty("autoOwnershipPoints")]
        public long AutoOwnershipPoints { get; set; }

        [JsonProperty("autoPoints")]
        public long AutoPoints { get; set; }

        [JsonProperty("autoQuestRankingPoint")]
        public bool AutoQuestRankingPoint { get; set; }

        [JsonProperty("autoRobot1")]
        public string AutoRobot1 { get; set; }

        [JsonProperty("autoRobot2")]
        public string AutoRobot2 { get; set; }

        [JsonProperty("autoRobot3")]
        public string AutoRobot3 { get; set; }

        [JsonProperty("autoRunPoints")]
        public long AutoRunPoints { get; set; }

        [JsonProperty("autoScaleOwnershipSec")]
        public long AutoScaleOwnershipSec { get; set; }

        [JsonProperty("autoSwitchAtZero")]
        public bool AutoSwitchAtZero { get; set; }

        [JsonProperty("autoSwitchOwnershipSec")]
        public long AutoSwitchOwnershipSec { get; set; }

        [JsonProperty("endgamePoints")]
        public long EndgamePoints { get; set; }

        [JsonProperty("endgameRobot1")]
        public string EndgameRobot1 { get; set; }

        [JsonProperty("endgameRobot2")]
        public string EndgameRobot2 { get; set; }

        [JsonProperty("endgameRobot3")]
        public string EndgameRobot3 { get; set; }

        [JsonProperty("faceTheBossRankingPoint")]
        public bool FaceTheBossRankingPoint { get; set; }

        [JsonProperty("foulCount")]
        public long FoulCount { get; set; }

        [JsonProperty("foulPoints")]
        public long FoulPoints { get; set; }

        [JsonProperty("rp")]
        public long Rp { get; set; }

        [JsonProperty("tba_gameData")]
        public string TbaGameData { get; set; }

        [JsonProperty("techFoulCount")]
        public long TechFoulCount { get; set; }

        [JsonProperty("teleopOwnershipPoints")]
        public long TeleopOwnershipPoints { get; set; }

        [JsonProperty("teleopPoints")]
        public long TeleopPoints { get; set; }

        [JsonProperty("teleopScaleBoostSec")]
        public long TeleopScaleBoostSec { get; set; }

        [JsonProperty("teleopScaleForceSec")]
        public long TeleopScaleForceSec { get; set; }

        [JsonProperty("teleopScaleOwnershipSec")]
        public long TeleopScaleOwnershipSec { get; set; }

        [JsonProperty("teleopSwitchBoostSec")]
        public long TeleopSwitchBoostSec { get; set; }

        [JsonProperty("teleopSwitchForceSec")]
        public long TeleopSwitchForceSec { get; set; }

        [JsonProperty("teleopSwitchOwnershipSec")]
        public long TeleopSwitchOwnershipSec { get; set; }

        [JsonProperty("totalPoints")]
        public long TotalPoints { get; set; }

        [JsonProperty("vaultBoostPlayed")]
        public long VaultBoostPlayed { get; set; }

        [JsonProperty("vaultBoostTotal")]
        public long VaultBoostTotal { get; set; }

        [JsonProperty("vaultForcePlayed")]
        public long VaultForcePlayed { get; set; }

        [JsonProperty("vaultForceTotal")]
        public long VaultForceTotal { get; set; }

        [JsonProperty("vaultLevitatePlayed")]
        public long VaultLevitatePlayed { get; set; }

        [JsonProperty("vaultLevitateTotal")]
        public long VaultLevitateTotal { get; set; }

        [JsonProperty("vaultPoints")]
        public long VaultPoints { get; set; }
    }

    public partial class MatchVideo
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}

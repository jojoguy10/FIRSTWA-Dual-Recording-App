namespace FIRSTWA_Recorder
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Match
    {
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("comp_level", NullValueHandling = NullValueHandling.Ignore)]
        public string CompLevel { get; set; }

        [JsonProperty("set_number", NullValueHandling = NullValueHandling.Ignore)]
        public int SetNumber { get; set; }

        [JsonProperty("match_number", NullValueHandling = NullValueHandling.Ignore)]
        public int MatchNumber { get; set; }

        [JsonProperty("alliances", NullValueHandling = NullValueHandling.Ignore)]
        public Alliances Alliances { get; set; }

        [JsonProperty("winning_alliance", NullValueHandling = NullValueHandling.Ignore)]
        public string WinningAlliance { get; set; }

        [JsonProperty("event_key", NullValueHandling = NullValueHandling.Ignore)]
        public string EventKey { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public long? Time { get; set; }

        [JsonProperty("predicted_time", NullValueHandling = NullValueHandling.Ignore)]
        public long? PredictedTime { get; set; }

        [JsonProperty("actual_time", NullValueHandling = NullValueHandling.Ignore)]
        public long? ActualTime { get; set; }
    }

    public partial class Alliances
    {
        [JsonProperty("blue", NullValueHandling = NullValueHandling.Ignore)]
        public Alliance Blue { get; set; }

        [JsonProperty("red", NullValueHandling = NullValueHandling.Ignore)]
        public Alliance Red { get; set; }
    }

    public partial class Alliance
    {
        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public long? Score { get; set; }

        [JsonProperty("team_keys", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> TeamKeys { get; set; }

        [JsonProperty("surrogate_team_keys", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SurrogateTeamKeys { get; set; }

        [JsonProperty("dq_team_keys", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> DqTeamKeys { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIRSTWA_Recorder
{
    class Event
    {
        public string address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public District district { get; set; }
        public List<object> division_keys { get; set; }
        public string end_date { get; set; }
        public string event_code { get; set; }
        public int event_type { get; set; }
        public string event_type_string { get; set; }
        public string first_event_code { get; set; }
        public string first_event_id { get; set; }
        public string gmaps_place_id { get; set; }
        public string gmaps_url { get; set; }
        public string key { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string location_name { get; set; }
        public string name { get; set; }
        public object parent_event_key { get; set; }
        public object playoff_type { get; set; }
        public object playoff_type_string { get; set; }
        public string postal_code { get; set; }
        public string short_name { get; set; }
        public string start_date { get; set; }
        public string state_prov { get; set; }
        public string timezone { get; set; }
        public List<object> webcasts { get; set; }
        public string website { get; set; }
        public int week { get; set; }
        public int year { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ValoStats.Models
{
    public class CurrentMMR
    {
        public Rank tier { get; set; }
        public int rr { get; set; }
        public int last_change { get; set; }
        public int elo { get; set; }
        public int games_needed_for_rating { get; set; }
    }

    public class MMRData
    {
        public Player player { get; set; }
        public Peak peak { get; set; }
        public CurrentMMR current { get; set; }
        
        public ObservableCollection<Episode> seasonal { get; set; }
    }

    public class Images
    {
        public string small { get; set; }
        public string large { get; set; }
        public string triangle_down { get; set; }
        public string triangle_up { get; set; }
    }
    
    public class MMRResponse
    {
        public int status { get; set; }
        public MMRData data { get; set; }
    }

    public class Peak
    {
        public Season season { get; set; }
        public string ranking_schema { get; set; }
        public Rank tier { get; set; }
    }

    public class Season
    {
        public string id { get; set; }
        public string @short { get; set; }
    }

    public class Rank
    {
        public string name { get; set; }
    }

    public class Episode
    {
        public Season season { get; set; }
        public Rank end_tier { get; set; }
        public string ranking_schema { get; set; }
        public List<ActWin> act_wins { get; set; }
    }
    public class ActWin
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}

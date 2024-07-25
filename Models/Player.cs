﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorantTrackerApp.Models
{
    public class Player
    {
        public string puuid { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
        public string team { get; set; }
        public int level { get; set; }
        public string character { get; set; }
        public int currenttier { get; set; }
        public string currenttier_patched { get; set; }
        public string player_card { get; set; }
        public string player_title { get; set; }
        public string party_id { get; set; }
        public SessionPlaytime session_playtime { get; set; }
        public Stats stats { get; set; }
        public int damage_made { get; set; }
        public int damage_received { get; set; }


    }

    public class PlayerData
    {
        public string puuid { get; set; }
        public string region { get; set; }
        public int account_level { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
        public string last_update { get; set; }
        public int last_update_raw { get; set; }

    }




    public class PlayerResponse
    {
        public int status { get; set; }
        public PlayerData data { get; set; }

    }
}

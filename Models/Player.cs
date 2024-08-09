using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace ValoStats.Models
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
        public string card { get; set; }
        public Bitmap cardImg { get; set; }
        public string player_title { get; set; }
        public string party_id { get; set; }
        public SessionPlaytime session_playtime { get; set; }
        public Stats stats { get; set; }
        public int damage_made { get; set; }
        public int damage_received { get; set; }
        public DateTime updated_at { get; set; }
    }
    
    public class PlayerData
    {
        public string puuid { get; set; }
        public string region { get; set; }
        public string name { get; set; }
        public int account_level { get; set; }
        public string tag { get; set; }
        public string card { get; set; }
        public string title { get; set; }
        public List<string> platforms { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class PlayerResponse
    {
        public PlayerData data { get; set; }
    }
    
    public class Card
    {
        public string small { get; set; }
        public string wide { get; set; }
        public string large { get; set; }
    }
    
}

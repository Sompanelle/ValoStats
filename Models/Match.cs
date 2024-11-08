using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ValoStats.Models
{
    public class Match
    {
        public string map { get; set; }
        public string game_version { get; set; }
        public int game_length { get; set; }
        public int game_start { get; set; }
        public string game_start_patched { get; set; }
        public int rounds_played { get; set; }
        public string mode { get; set; }
        public string mode_id { get; set; }
        public string queue { get; set; }
        public string season_id { get; set; }
        public string platform { get; set; }
        public string matchid { get; set; }
        public string region { get; set; }
        public string cluster { get; set; }
        public int kills { get; set; }
    }

    public class PlayedMatch
    {
        public string Map { get; set; }
        public string KD { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public string Mode { get; set; }
        public string Region { get; set; }
        public string Platform { get; set; }
        public Team Team { get; set; }
        public string Agent { get; set; }
        public string Score { get; set; }
        public AllPlayer Player { get; set; }
        public bool? Result { get; set; }
    }
    public class Team
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
        public Stats stats { get; set; }
        public int damage_made { get; set; }
        public int damage_received { get; set; }
        public bool? has_won { get; set ; }
        public int? rounds_won { get; set; }
        public int? rounds_lost { get; set; }
        public Roaster roaster { get; set; }
    }
    

    
    public class Agent
    {
        public string small { get; set; }
        public string full { get; set; }
        public string bust { get; set; }
        public string killfeed { get; set; }
    }

    public class AllPlayer
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
        public Stats stats { get; set; }
        public int damage_made { get; set; }
        public int damage_received { get; set; }
    }

    public class MatchDatum
    {
        public Metadata metadata { get; set; }
        public Players players { get; set; }
        public Teams teams { get; set; }
        public List<Round> rounds { get; set; }
        public List<Kill> kills { get; set; }
    }

    public class MatchListResponse
    {
        public int status { get; set; }
        public List<MatchDatum> data { get; set; }
    }
    public class Kill
    {
        public int kill_time_in_round { get; set; }
        public int kill_time_in_match { get; set; }
        public string killer_puuid { get; set; }
        public string killer_display_name { get; set; }
        public string killer_team { get; set; }
        public string victim_puuid { get; set; }
        public string victim_display_name { get; set; }
        public string victim_team { get; set; }
    }
    
    public class Metadata
    {
        public string map { get; set; }
        public string game_version { get; set; }
        public int game_length { get; set; }
        public int game_start { get; set; }
        public string game_start_patched { get; set; }
        public int rounds_played { get; set; }
        public string mode { get; set; }
        public string mode_id { get; set; }
        public string queue { get; set; }
        public string season_id { get; set; }
        public string platform { get; set; }
        public string matchid { get; set; }
        public string region { get; set; }
        public string cluster { get; set; }
    }

    public class Players
    {
        public List<AllPlayer> all_players { get; set; }
        public List<Team> red { get; set; }
        public List<Team> blue { get; set; }
    }

    public class PlayerStat
    {
        public string player_puuid { get; set; }
        public string player_display_name { get; set; }
        public string player_team { get; set; }
        public int damage { get; set; }
        public int headshots { get; set; }
        public int kills { get; set; }
        public int score { get; set; }
    }
    
    

    public class Roaster
    {
        public List<string> members { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
    }

    public class MatchResponse
    {
        public int status { get; set; }
        public List<MatchDatum> data { get; set; }
    }

    public class Round
    {
        public string winning_team { get; set; }
        public string end_type { get; set; }
        public bool bomb_planted { get; set; }
        public bool bomb_defused { get; set; }
        public List<PlayerStat> player_stats { get; set; }
    }
    
    public class Stats
    {
        public int score { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int bodyshots { get; set; }
        public int headshots { get; set; }
        public int legshots { get; set; }
    }

    public class Teams
    {
        public Team red { get; set; }
        public Team blue { get; set; }
    }
    
}
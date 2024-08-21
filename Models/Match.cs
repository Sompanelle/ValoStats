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
        public PremierInfo premier_info { get; set; }
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
        public SessionPlaytime session_playtime { get; set; }
        public Assets assets { get; set; }
        public Behaviour behaviour { get; set; }
        public Platform platform { get; set; }
        public Stats stats { get; set; }
        public int damage_made { get; set; }
        public int damage_received { get; set; }
        public bool? has_won { get; set ; }
        public int? rounds_won { get; set; }
        public int? rounds_lost { get; set; }
        public Roaster roaster { get; set; }
    }
    



    public class AbilityCasts
    {
        public int? c_cast { get; set; }
        public int? q_cast { get; set; }
        public int? e_cast { get; set; }
        public int? x_cast { get; set; }
        public int? c_casts { get; set; }
        public int? q_casts { get; set; }
        public int? e_casts { get; set; }
        public int? x_casts { get; set; }
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
        public SessionPlaytime session_playtime { get; set; }
        public Assets assets { get; set; }
        public Behaviour behaviour { get; set; }
        public Platform platform { get; set; }
        public AbilityCasts ability_casts { get; set; }
        public Stats stats { get; set; }
        public int damage_made { get; set; }
        public int damage_received { get; set; }
    }

    public class Armor
    {
        public string id { get; set; }
        public string name { get; set; }
        public Assets assets { get; set; }
    }

    public class Assets
    {
        public Card card { get; set; }
        public Agent agent { get; set; }
        public string display_icon { get; set; }
        public string killfeed_icon { get; set; }
    }

    public class Assistant
    {
        public string assistant_puuid { get; set; }
        public string assistant_display_name { get; set; }
        public string assistant_team { get; set; }
    }

    public class Behaviour
    {
        public int afk_rounds { get; set; }
        public FriendlyFire friendly_fire { get; set; }
        public int rounds_in_spawn { get; set; }
    }
    

    public class Coach
    {
        public string puuid { get; set; }
        public string team { get; set; }
    }

    public class Customization
    {
        public string icon { get; set; }
        public string image { get; set; }
        public string primary { get; set; }
        public string secondary { get; set; }
        public string tertiary { get; set; }
    }

    public class DamageEvent
    {
        public string receiver_puuid { get; set; }
        public string receiver_display_name { get; set; }
        public string receiver_team { get; set; }
        public int bodyshots { get; set; }
        public int damage { get; set; }
        public int headshots { get; set; }
        public int legshots { get; set; }
    }

    public class DamageWeaponAssets
    {
        public string display_icon { get; set; }
        public string killfeed_icon { get; set; }
    }

    public class MatchDatum
    {
        public Metadata metadata { get; set; }
        public Players players { get; set; }
        public List<Observer> observers { get; set; }
        public List<Coach> coaches { get; set; }
        public Teams teams { get; set; }
        public List<Round> rounds { get; set; }
        public List<Kill> kills { get; set; }
    }

    public class DefusedBy
    {
        public string puuid { get; set; }
        public string display_name { get; set; }
        public string team { get; set; }
    }

    public class DefuseEvents
    {
        public DefuseLocation defuse_location { get; set; }
        public DefusedBy defused_by { get; set; }
        public List<PlayerLocations> player_locations_on_defuse { get; set; }
    }

    public class DefuseLocation
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Economy
    {
        public Spent spent { get; set; }
        public LoadoutValue loadout_value { get; set; }
        public Weapon weapon { get; set; }
        public Armor armor { get; set; }
        public int remaining { get; set; }
    }

    public class FriendlyFire
    {
        public int incoming { get; set; }
        public int outgoing { get; set; }
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
        public VictimDeathLocation victim_death_location { get; set; }
        public string damage_weapon_id { get; set; }
        public string damage_weapon_name { get; set; }
        public DamageWeaponAssets damage_weapon_assets { get; set; }
        public bool secondary_fire_mode { get; set; }
        public List<PlayerLocations> player_locations_on_kill { get; set; }
        public List<Assistant> assistants { get; set; }
    }

    public class KillEvent
    {
        public int kill_time_in_round { get; set; }
        public int kill_time_in_match { get; set; }
        public string killer_puuid { get; set; }
        public string killer_display_name { get; set; }
        public string killer_team { get; set; }
        public string victim_puuid { get; set; }
        public string victim_display_name { get; set; }
        public string victim_team { get; set; }
        public VictimDeathLocation victim_death_location { get; set; }
        public string damage_weapon_id { get; set; }
        public string damage_weapon_name { get; set; }
        public DamageWeaponAssets damage_weapon_assets { get; set; }
        public bool secondary_fire_mode { get; set; }
        public List<PlayerLocations> player_locations_on_kill { get; set; }
        public List<Assistant> assistants { get; set; }
    }

    public class LoadoutValue
    {
        public int overall { get; set; }
        public int average { get; set; }
    }

    public class Location
    {
        public int x { get; set; }
        public int y { get; set; }
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
        public PremierInfo premier_info { get; set; }
        public string region { get; set; }
        public string cluster { get; set; }
    }

    public class Observer
    {
        public string puuid { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
        public Platform platform { get; set; }
        public SessionPlaytime session_playtime { get; set; }
        public string team { get; set; }
        public int level { get; set; }
        public string player_card { get; set; }
        public string player_title { get; set; }
        public string party_id { get; set; }
    }

    public class Os
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class PlantedBy
    {
        public string puuid { get; set; }
        public string display_name { get; set; }
        public string team { get; set; }
    }

    public class PlantEvents
    {
        public PlantLocation plant_location { get; set; }
        public PlantedBy planted_by { get; set; }
        public string plant_site { get; set; }
        public List<PlayerLocations> player_locations_on_plant { get; set; }
    }

    public class PlantLocation
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Platform
    {
        public string type { get; set; }
        public Os os { get; set; }
    }
    

    public class PlayerLocations
    {
        public string player_puuid { get; set; }
        public string player_display_name { get; set; }
        public string player_team { get; set; }
        public Location location { get; set; }
        public double view_radians { get; set; }
    }
    

    public class Players
    {
        public List<AllPlayer> all_players { get; set; }
        public List<Team> red { get; set; }
        public List<Team> blue { get; set; }
    }

    public class PlayerStat
    {
        public AbilityCasts ability_casts { get; set; }
        public string player_puuid { get; set; }
        public string player_display_name { get; set; }
        public string player_team { get; set; }
        public List<DamageEvent> damage_events { get; set; }
        public int damage { get; set; }
        public int bodyshots { get; set; }
        public int headshots { get; set; }
        public int legshots { get; set; }
        public List<KillEvent> kill_events { get; set; }
        public int kills { get; set; }
        public int score { get; set; }
        public bool was_afk { get; set; }
        public bool was_penalized { get; set; }
        public bool stayed_in_spawn { get; set; }
    }

    public class PremierInfo
    {
        public string tournament_id { get; set; }
        public string matchup_id { get; set; }
    }

    

    public class Roaster
    {
        public List<string> members { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
        public Customization customization { get; set; }
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
        public PlantEvents plant_events { get; set; }
        public DefuseEvents defuse_events { get; set; }
        public List<PlayerStat> player_stats { get; set; }
    }

    public class SessionPlaytime
    {
        public int minutes { get; set; }
        public int seconds { get; set; }
        public int milliseconds { get; set; }
    }

    public class Spent
    {
        public int overall { get; set; }
        public int average { get; set; }
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

    public class VictimDeathLocation
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Weapon
    {
        public string id { get; set; }
        public string name { get; set; }
        public Assets assets { get; set; }
    }

}
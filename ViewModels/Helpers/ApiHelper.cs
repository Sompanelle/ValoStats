using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;
using ValoStats.Models;
using System.Net.Http;
using ValoStats.ViewModels.DTOs;
using System.Configuration;
using ValorantTrackerApp.Models;

namespace ValoStats.ViewModels.Helpers
{
    public class ApiHelper
    {
        private static Config Config = FileHelper.ReadConfig();
        private static HttpClient ApiClient;
        private static string requrl = @"https://api.henrikdev.xyz/valorant";
        private static string key = Config.Key;
        private static string region = Config.Region;


        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(requrl);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
        }

        public static async Task<Player?> GetPlayer(string name, string tag)
        {
            string playerUrl = $"{requrl}/v2/account/{name}/{tag}?api_key={key}";

            using (HttpResponseMessage response = await ApiClient.GetAsync(playerUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    PlayerResponse content = JsonSerializer.Deserialize<PlayerResponse>(json);
                    Player player = PlayerDTO.PlayerResponseToPlayer(content);
                    return player;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
            }
        }

        public static async Task<CurrentMMR?> GetMMR(string name, string tag)
        {
            string mmrUrl = $"{requrl}/v3/mmr/na/pc/{name}/{tag}?api_key={key}";

            using (HttpResponseMessage response = await ApiClient.GetAsync(mmrUrl))
            {
                string json = await response.Content.ReadAsStringAsync();
                MMRResponse content = JsonSerializer.Deserialize<MMRResponse>(json);
                CurrentMMR mmr = MMRDTO.MMRResponseToMMR(content);
                return mmr;
            }

        }

        public static async Task<MMRData?> GetMMRData(string name, string tag)
        {
            string mmrUrl = $"{requrl}/v3/mmr/na/pc/{name}/{tag}?api_key={key}";

            using (HttpResponseMessage response = await ApiClient.GetAsync(mmrUrl))
            {
                string json = await response.Content.ReadAsStringAsync();
                MMRResponse content = JsonSerializer.Deserialize<MMRResponse>(json);
                MMRData mmr = MMRDTO.MMRResponseToMMRData(content);
                return mmr;
            }

        }

        public static async Task<Datum?> GetLastMatchData(string name, string tag)
        {
            string lastMatchUrl = $"{requrl}/v3/matches/{region}/{name}/{tag}?api_key={key}&size=1";
            using (HttpResponseMessage response = await ApiClient.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse content = JsonSerializer.Deserialize<MatchResponse>(json);
                    Datum matchData = MatchDTO.MatchResponseToDatum(content);
                    return matchData;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
            }
        }


        public static async Task<List<Datum>?> GetLastMatchDatas(string name, string tag)
        {
            string lastMatchUrl = $"{requrl}/v3/matches/{region}/{name}/{tag}?api_key={key}&size=5";
            using (HttpResponseMessage response = await ApiClient.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse content = JsonSerializer.Deserialize<MatchResponse>(json);
                    List<Datum> MatchData = MatchDTO.MatchesResponseToMatchDatas(content);
                    return MatchData;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<PlayedMatch>?> GetLastTenPlayedMatches(string name, string tag)
        {
            string lastMatchUrl = $"{requrl}/v3/matches/{region}/{name}/{tag}?api_key={key}&size=10";
            using (HttpResponseMessage response = await ApiClient.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    ObservableCollection<PlayedMatch> lastMatches = new();
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse content = JsonSerializer.Deserialize<MatchResponse>(json);
                    List<Datum> MatchData = MatchDTO.MatchesResponseToMatchDatas(content);
                    foreach(Datum _ in MatchData)
                    {
                        lastMatches.Add(MatchDTO.DatumToPlayedMatch(_));
                    }
                    return lastMatches;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
            }
        }



    }
}

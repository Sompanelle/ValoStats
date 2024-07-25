using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using ValorantTrackerApp.Models;
using ValorantTrackerApp.DTOs;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace ValorantTrackerApp.Helpers
{
    public class ApiHelper
    {
        static string region = "na";
        static string name = "Sompanelle";
        static string tag = "N0IR";
        private static HttpClient ApiClient;
        static string requrl = @"https://api.henrikdev.xyz/valorant";
        static string key = "api_key=HDEV-0c96ec83-5097-4be1-94c7-b24166521240";


        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(requrl);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
        }

        public static async Task<Player?> GetPlayer()
        {
            string playerUrl = $"{requrl}/v1/account/{name}/{tag}?{key}";

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

        public static async Task<CurrentMMR?> GetMMR()
        {
            string mmrUrl = $"{requrl}/v3/mmr/na/pc/{name}/{tag}?{key}";

            using (HttpResponseMessage response = await ApiClient.GetAsync(mmrUrl))
            {
                string json = await response.Content.ReadAsStringAsync();
                MMRResponse content = JsonSerializer.Deserialize<MMRResponse>(json);
                CurrentMMR mmr = MMRDTO.MMRResponseToMMR(content);
                return mmr;
            }

        }

        public static async Task<Match?> GetLastMatch()
        {
            string lastMatchUrl = $"{requrl}/v3/matches/{region}/{name}/{tag}?{key}&size=1";
            using (HttpResponseMessage response = await ApiClient.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse content = JsonSerializer.Deserialize<MatchResponse>(json);
                    Match match = MatchDTO.OneMatchResponseToDTO(content);
                    return match;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<Match?>> GetLastFiveMatches()
        {
            string lastMatchUrl = $"{requrl}/v3/matches/{region}/{name}/{tag}?{key}&size=5";
            using (HttpResponseMessage response = await ApiClient.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse content = JsonSerializer.Deserialize<MatchResponse>(json);
                    ObservableCollection<Match>match = MatchDTO.MatchesResponseToDTO(content);
                    return match;
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
}

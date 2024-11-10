using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol.Viewport;
using ValoStats.Models;

namespace ValoStats.ViewModels.Helpers
{
    public static class ApiHelper
    {
        


        public static async Task<MemoryStream?> GetCard(string Asset, HttpClient ApiClient)
        {
            string cardUrl = @$"https://media.valorant-api.com/playercards/{Asset}/wideart.png";
            var data = await ApiClient.GetByteArrayAsync(cardUrl);
            return new MemoryStream(data);
        }
        
        public static async Task<MatchDatum?> GetLastMatchDatum(string Puuid, HttpClient ApiClient, Config Config)
        {
            string lastMatchUrl = $"https://api.henrikdev.xyz/valorant/v3/by-puuid/matches/{Config.Region}/{Puuid}?api_key={Config.Key}&size=1";
            using (HttpResponseMessage response = await ApiClient.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse content = JsonSerializer.Deserialize<MatchResponse>(json);
                    if (content != null)
                    {
                        MatchDatum MatchDatas = MatchDTO.MatchResponseToMatchDatum(content);
                        return MatchDatas;
                    }
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }
        
        public static async Task<MMRData?> GetEpisodeHistory(string Name, string Tag, HttpClient ApiClient, Config Config)
        {
            string mmrUrl = $"https://api.henrikdev.xyz/valorant/v3/mmr/na/pc/{Name}/{Tag}?api_key={Config.Key}";

            using (HttpResponseMessage response = await ApiClient.GetAsync(mmrUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MMRResponse content = JsonSerializer.Deserialize<MMRResponse>(json);
                    if (content.status != 200 )
                        return null;
                    MMRData mmr = MMRDTO.MMRResponseToMMRData(content);
                    return mmr;
                }
                else return null;
            }
        }
        
        
        public static async Task<MMRData?> GetMMRData(string Puuid, HttpClient ApiClient, Config Config)
        {
            string mmrUrl = $"https://api.henrikdev.xyz/valorant/v3/by-puuid/mmr/na/pc/{Puuid}?api_key={Config.Key}";

            using (HttpResponseMessage response = await ApiClient.GetAsync(mmrUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MMRResponse content = JsonSerializer.Deserialize<MMRResponse>(json);
                    if (content.status != 200)
                        return null;
                    MMRData mmr = MMRDTO.MMRResponseToMMRData(content);
                    return mmr;
                }
                else return null;
            }
        }

        public static async Task<MemoryStream?> GetRankImg (int Id,HttpClient Client)
        {
            string rankUrl = @$"https://media.valorant-api.com/competitivetiers/564d8e28-c226-3180-6285-e48a390db8b1/{Id}/smallicon.png";
            Byte[] data = await Client.GetByteArrayAsync(rankUrl);
            return new MemoryStream(data);
        }
        
        
        public static async Task<Player?> GetPlayer(string Name, string Tag, HttpClient Client, Config Config)
        {
            string playerUrl = $"https://api.henrikdev.xyz/valorant/v2/account/{Name}/{Tag}?api_key={Config.Key}";

            using (HttpResponseMessage response = await Client.GetAsync(playerUrl))
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
                    return null;
                }
            }
        }
        
        
        public static async Task<TitleData?> GetTitle(string Asset, HttpClient Client)
        {
            string titleUrl = $@"https://valorant-api.com/v1/playertitles/{Asset}";
            using (HttpResponseMessage response = await Client.GetAsync(titleUrl))
            {
                string json = await response.Content.ReadAsStringAsync();
                TitleResponse content = JsonSerializer.Deserialize<TitleResponse>(json);
                TitleData Title = TitleDTO.TitleResponseToTitleData(content);
                return Title;
            }
        }
        
        public static HttpClient InitializeClient()
        {
            string requrl = @"https://api.henrikdev.xyz/valorant";
            HttpClient apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(requrl);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            return apiClient;
        }
        
        public static async Task<ObservableCollection<PlayedMatch>?> GetLastMatchList(string Puuid, HttpClient Client, Config Config)
        {
            string lastMatchUrl = $"https://api.henrikdev.xyz/valorant/v3/by-puuid/matches/{Config.Region}/{Puuid}?api_key={Config.Key}&size=6";
            Debug.WriteLine("Sending Request");
            using (HttpResponseMessage response = await Client.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Sucessful Request");
                    string json = await response.Content.ReadAsStringAsync();
                    var content = JsonSerializer.Deserialize<MatchListResponse>(json);
                    if (content != null)
                    {
                        var matches = MatchDTO.MatchListResponseToPlayedMatches(content, Puuid);
                        return matches;
                    }
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<PlayedMatch>?> GetNextMatches(String Puuid, HttpClient Client,
            Config Config, int index)
        {
            string url = $"https://api.henrikdev.xyz/valorant/v3/by-puuid/matches/{{Config.Region}}/{{Puuid}}?api_key={{Config.Key}}&size=6&after={index}";
            Debug.WriteLine("Sending Request");
            using (HttpResponseMessage response = await Client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    var content = JsonSerializer.Deserialize<MatchListResponse>(responseJson);
                    if (content != null)
                    {
                        var matches = MatchDTO.MatchListResponseToPlayedMatches(content, Puuid);
                        return matches;
                    }
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }
        
        public static async Task<ObservableCollection<PlayedMatch>> GetMatchListByMode(string Puuid, HttpClient ApiClient, Config Config, string Mode)
        {
            string Url = $"https://api.henrikdev.xyz/valorant/v3/by-puuid/matches/{Config.Region}/{Puuid}?api_key={Config.Key}&mode={Mode}&size=6";
            
            Debug.WriteLine("Sending Request");
            using (HttpResponseMessage response = await ApiClient.GetAsync(Url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Sucessful Request");
                    var json = await response.Content.ReadAsStringAsync();
                    var content = JsonSerializer.Deserialize<MatchListResponse>(json);
                    if (content.status != 200)
                        return null;
                    Debug.WriteLine("Beginning Deserializing");
                    var matches = MatchDTO.MatchListResponseToPlayedMatches(content, Puuid);
                    return matches;
                }
                else return null;
            }
            
            
        }
        


         
    }
        

    }



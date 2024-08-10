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
using System.IO;
using System.Net;
using System.Net.Mime;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol.Viewport;
using ValoStats.Models;

namespace ValoStats.ViewModels.Helpers
{
    public class ApiHelper
    {
        private static Config config = FileHelper.ReadConfig();
        private static string requrl = @"https://api.henrikdev.xyz/valorant";
        private static string key = config.Key;
        private static string region = config.Region;


        public static async Task<MemoryStream?> GetCard(string Asset, HttpClient ApiClient)
        {
            string cardUrl = @$"https://media.valorant-api.com/playercards/{Asset}/wideart.png";
            var data = await ApiClient.GetByteArrayAsync(cardUrl);
            return new MemoryStream(data);
        }
        
        public static async Task<List<Datum>?> GetLastFiveMatchDatas(string Name, string Tag, HttpClient ApiClient)
        {
            string lastMatchUrl = $"{requrl}/v3/matches/{region}/{Name}/{Tag}?api_key={key}&size=5";
            using (HttpResponseMessage response = await ApiClient.GetAsync(lastMatchUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchResponse content = JsonSerializer.Deserialize<MatchResponse>(json);
                    if (content != null)
                    {
                        List<Datum> MatchDatas = MatchDTO.MatchesResponseToDatums(content);
                        return MatchDatas;
                    }
                    else return null;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
            }
        }
        
        public static async Task<MMRData?> GetMMRData(string Name, string Tag, HttpClient ApiClient)
        {
            string mmrUrl = $"{requrl}/v3/mmr/na/pc/{Name}/{Tag}?api_key={key}";

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

        public static async Task<MemoryStream?> GetRankImg (HttpClient Client, int Id)
        {
            string rankUrl = @$"https://media.valorant-api.com/competitivetiers/564d8e28-c226-3180-6285-e48a390db8b1/{Id}/smallicon.png";
            Byte[] data = await Client.GetByteArrayAsync(rankUrl);
            return new MemoryStream(data);
        }
        
        
        public static async Task<Player?> GetPlayer(string Name, string Tag, HttpClient Client)
        {
            string playerUrl = $"{requrl}/v2/account/{Name}/{Tag}?api_key={key}";

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
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
            }
        }
        
        public static async Task<TitleData?> GetTitle(string Asset, HttpClient ApiClient)
        {
            string titleUrl = $@"https://valorant-api.com/v1/playertitles/{Asset}";
            using (HttpResponseMessage response = await ApiClient.GetAsync(titleUrl))
            {
                string json = await response.Content.ReadAsStringAsync();
                TitleResponse content = JsonSerializer.Deserialize<TitleResponse>(json);
                TitleData Title = TitleDTO.TitleResponseToTitleData(content);
                return Title;
            }
        }
        
        public static HttpClient InitializeClient()
        {
            HttpClient apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(requrl);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            return apiClient;
        }
        

        


         
    }
        

    }



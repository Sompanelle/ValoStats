﻿using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ValoStats.Models;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        private HttpClient client;

        private Config? config;

        private string selectedMode = "all";
        
        public string SelectedMode
        {
            get
            {
                return selectedMode;
            }
            set
            {
                selectedMode = value;
                switch (value)
                {
                    case "all":
                        GetAllMatchPlayed(player.puuid, client, config);
                        break;
                    default:
                        GetMatchList(player.puuid, value ,client, config);
                        break;
                } 

            }
        }
        
        public ObservableCollection<string> modes { get; set; } = new() { "all", "competitive", "unrated", "deathmatch", "teamdeathmatch"};
        
        [ObservableProperty]
        private bool badRequest;

        [ObservableProperty]
        private bool isLoaded;

        [ObservableProperty]
        private int pbar = 0;

        [ObservableProperty]
        private Player? player;

        [ObservableProperty]
        private Bitmap cardImage;

        [ObservableProperty]
        private double displayKd = 0;

        [ObservableProperty]
        private double displayWr = 0;

        [ObservableProperty] 
        private MMRData mmrData;

        [ObservableProperty]
        private TitleData title;

        [ObservableProperty]
        private DateTime updatedAt;

        [ObservableProperty] 
        private string displayName;

        [ObservableProperty]
        private Bitmap peak;
        
        [ObservableProperty]
        private Bitmap current;

        [ObservableProperty]
        private bool isMatchesLoading;

        public ObservableCollection<PlayedMatch> Matches { get; set; }
        
        public ObservableCollection<PlayedMatch> DisplayedMatches { get; set; }
        
        public HomePageViewModel()
        {

            if (Design.IsDesignMode)
            {
                IsLoaded = true;
                Matches = new()
                {
                    new PlayedMatch() { Map = "Ascent", Mode = "Competitive", Kills = 22, Deaths = 11 , Agent = "Yoru", Score = "13-3" , Result = true },  
                    new PlayedMatch() { Map = "Sunset", Mode = "Competitive", Kills = 11, Deaths= 13, Agent = "Chamber", Score = "13-3" , Result = false }, 
                    new PlayedMatch() { Map = "Sunset", Mode = "Deathmatch", Kills = 36, Deaths = 23, Agent  = "Breach", Result = null }, 
                };
                
                DisplayName = "Sompanelle#NOIR";
                Title = new() { titleText = "Unserious" };
                Player = new() { level = 60 };
                DisplayKd = 1.2;
                DisplayWr = .89;
            }
            else
            {
                config = FileHelper.ReadConfig();
                client = ApiHelper.InitializeClient();
                Matches = new();
                DisplayedMatches = new ObservableCollection<PlayedMatch>();
                DisplayName = $"{config.Name}#{config.Tag}";
                InitializeHome(config,client);
            }

        }

        private async Task InitializeHome(Config Config, HttpClient Client) 
        {
            string settingName = Config.Name;
            string settingTag = Config.Tag;
            if ( !string.IsNullOrEmpty(settingName) || !string.IsNullOrEmpty(settingTag))
            {
                
                    Player = await GetPlayerAsync(settingName, settingTag, Client, Config);
                    Pbar += 10;
                    MmrData = await ApiHelper.GetMMRData(Player.puuid, Client, Config);
                    Pbar += 10;
                    CardImage = await GetCardAsync(Player.card, Client);
                    Pbar += 10;
                    Title = await GetTitleAsync(Player.player_title, Client);
                    await GetRankImg(Client);
                    Debug.WriteLine($"puuid: {player.puuid}");
                    Debug.WriteLine($"key: {config.Key}");
                    Pbar += 10;
                    Debug.WriteLine("Getting Matches");
                    await GetAllMatchPlayed(Player.puuid , Client, Config);
                    Pbar += 10;
                    IsLoaded = true;
            }
            else
            {
                BadRequest = true;
            }


        }
        private async Task<Player?> GetPlayerAsync(string Name, string Tag, HttpClient Client, Config Config)
        {
            var resultPlayer= await ApiHelper.GetPlayer(Name, Tag, Client, Config);
            if (resultPlayer != null)
            {
                return resultPlayer;
            }
            else return null;
        }
        

        private async Task<Bitmap?> GetCardAsync(string assetId, HttpClient Client)
        {
            var cardData = await ApiHelper.GetCard(assetId, Client);
            if (cardData != null)
            {
                return Bitmap.DecodeToHeight(cardData, 320);
            }
            else return null;
        }

        private async Task<TitleData?> GetTitleAsync(string Asset, HttpClient Client)
        {
            var titleData = await ApiHelper.GetTitle(Asset, Client);
            if (titleData != null)
            {
                return titleData;
            }
            else return null;
        }

        private async Task GetRankImg(HttpClient Client)
        {
                var currentData = await ApiHelper.GetRankImg(MmrData.current.tier.id,Client);
                if (currentData != null)
                {
                    Current = Bitmap.DecodeToWidth(currentData, 55);
                }
                var peakData = await ApiHelper.GetRankImg(MmrData.peak.tier.id,Client);
                if (peakData != null)
                {
                    Peak = Bitmap.DecodeToWidth(peakData, 55);
                }
        }
        
        
        private async Task GetAllMatchPlayed(string Puuid, HttpClient Client, Config Config)
        {
            Matches.Clear();
            var matches = await ApiHelper.GetLastMatchList(Puuid, Client, Config);
            if (matches != null)
            {
                double rawKd = 0;
                double rawWr = 0;
                double kills = 0;
                double deaths = 0;
                double wins = 0;
                double losses = 0;
                foreach (PlayedMatch match in matches)
                {
                    kills += match.Kills;
                    deaths += match.Deaths;
                    if (match.Result == true)
                        wins += 1;
                    else
                        losses += 1;
                    Matches.Add(match);
                }
                rawKd = double.Round((kills / deaths), 2);
                rawWr = wins / matches.Count * 100;
                DisplayKd = rawKd;
                DisplayWr = rawWr;
            }
            else BadRequest = true;
        }
        
        private async Task GetMatchList(string Puuid, string Mode, HttpClient Client, Config Config)
        {
            Matches.Clear();
            var MatchList = await ApiHelper.GetMatchListByMode(Puuid, Client, Config, Mode);
            if (MatchList != null)
            {
                isMatchesLoading = false;
                foreach (PlayedMatch match in MatchList)
                {
                    Matches.Add(match);
                }
                
            }
        }
        
        }
    
        
    }



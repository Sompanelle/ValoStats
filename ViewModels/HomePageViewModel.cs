using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using ValoStats.Models;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        private HttpClient client;

        private Config? config;

        private string selectedMode = "All";

        [ObservableProperty]
        private int matchPageNum;
        
        [ObservableProperty]
        private int matchPageSize = 3;
        
        public string SelectedMode
        {
            get {
                return selectedMode;
            }
            set{
                selectedMode = value;
                switch (value)
                {
                    case "All":
                        GetAllMatchPlayed(player.puuid, client, config);
                        break;
                    default:
                        GetMatchList(player.puuid, value.ToLower() ,client, config);
                        break;
                } 
            }
        }
        
        public ObservableCollection<string> modes { get; set; } = new() { "All", "Competitive", "Unrated", "Deathmatch", "TeamDeathmatch"};
        
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
        private Bitmap peakImage;
        
        [ObservableProperty]
        private Bitmap currentImage;

        [ObservableProperty]
        private bool isMatchesLoading;

        public ObservableCollection<PlayedMatch> MatchList { get; set; }
        
        public ObservableCollection<PlayedMatch> DisplayMatches { get; set; }
        
        public HomePageViewModel()
        {
                        if (Design.IsDesignMode)
                        {
                            IsLoaded = true;
                            DisplayMatches = new()
                            {
                                new PlayedMatch() { Map = "Ascent", Mode = "Competitive", Kills = 22, Deaths = 11 , Agent = "Yoru", Score = "13-3" , Result = true },  
                                new PlayedMatch() { Map = "Sunset", Mode = "Competitive", Kills = 11, Deaths= 13, Agent = "Chamber", Score = "13-3" , Result = false }, 
                                new PlayedMatch() { Map = "Sunset", Mode = "Deathmatch", Kills = 36, Deaths = 23, Agent  = "Breach", Result = null }, 
                            };
                            
                            DisplayName = "Sompanelle#NOIR";
                            Title = new() { titleText = "Unserious" };
                            DisplayKd = 1.2;
                            DisplayWr = .89;
                        }
                        else
                        {
                            config = FileHelper.ReadConfig();
                            client = ApiHelper.InitializeClient();
                            MatchList = new();
                            DisplayMatches = new();
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
                //get player
                await GetPlayerAsync(Client, Config);
                await GetPlayerStats(Player, Config, Client);
                await GetRankImg(Client, MmrData);
                GetPlayerImg(Client, Player);
                IsLoaded = true;
                Pbar += 10;
                await GetAllMatchPlayed(Player.puuid , Client, Config);
            }
            else
            {
                BadRequest = true;
            }
        }
        
        private async void GetPlayerImg(HttpClient Client, Player Player)
        {
            CardImage = await GetCardAsync(Client,Player.card);
            Title = await GetTitleAsync(Player.player_title, Client);
        }
        
        private async Task GetPlayerStats(Player Player, Config Config, HttpClient Client)
        {
            var mmrData = await ApiHelper.GetMMRDataByPuuid(Player.puuid,Client,Config);
            if (mmrData != null)
                MmrData = mmrData;
            BadRequest = true;
        }
        
        private async Task GetPlayerAsync(HttpClient Client, Config Config)
        {
            var resultPlayer= await ApiHelper.GetPlayer(Config.Name, Config.Tag, Client, Config);
            if (resultPlayer != null)
            {
                Player = resultPlayer;
            }
            else BadRequest = true;
        }
        
        private async Task<Bitmap?> GetCardAsync(HttpClient Client, String Card)
        {
            var cardData = await ApiHelper.GetCard(Card, Client);
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

        private async Task GetRankImg(HttpClient Client, MMRData MMRData)
        {
                var currentData = await ApiHelper.GetRankImg(MMRData.current.tier.id,Client);
                if (currentData != null)
                {
                    CurrentImage = Bitmap.DecodeToWidth(currentData, 55);
                }
                var peakData = await ApiHelper.GetRankImg(MMRData.peak.tier.id,Client);
                if (peakData != null)
                {
                    PeakImage = Bitmap.DecodeToWidth(peakData, 55);
                }
        }
        
        private async Task GetAllMatchPlayed(string Puuid, HttpClient Client, Config Config)
        {
            MatchList.Clear();
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
                    MatchList.Add(match);
                }
                rawKd = double.Round((kills / deaths), 2);
                rawWr = double.Round((wins / matches.Count), 2) * 100;
                DisplayKd = rawKd;
                DisplayWr = rawWr;
                var list = MatchList.Take(3);
                foreach (var m in list)
                {
                    DisplayMatches.Add(m);
                }
            }
            else BadRequest = true;
        }
        
        private async Task GetMatchList(string Puuid, string Mode, HttpClient Client, Config Config)
        {
            DisplayMatches.Clear();
            MatchList.Clear();
            var matchList = await ApiHelper.GetMatchListByMode(Puuid, Client, Config, Mode);
            if (matchList == null)
                BadRequest = true;
            else
                IsMatchesLoading = false;
                foreach (PlayedMatch match in matchList)
                {
                    MatchList.Add(match);
                }
                var list = MatchList.Take(3);
                foreach (var m in list)
                {
                    DisplayMatches.Add(m);
                }
        }
        
        [RelayCommand(CanExecute = nameof(CanNextMatch))]
        public void NextMatches()
        {
            MatchPageNum++;
            var matches = MatchList.Skip(MatchPageSize*MatchPageNum);
            var list = matches.Take(MatchPageSize);
            DisplayMatches.Clear();
            foreach(PlayedMatch m in list)
            {
                DisplayMatches.Add(m);
            }
        }
        
        [RelayCommand(CanExecute = nameof(CanPrevMatch))]
        public void PrevMatches()
        {
            MatchPageNum--;
            var matches = MatchList.Skip(MatchPageSize*MatchPageNum);
            var list = matches.Take(MatchPageSize);
            DisplayMatches.Clear();
            foreach(PlayedMatch m in list)
            {
                DisplayMatches.Add(m);
            }
        }
        
        public bool CanPrevMatch()
        {
            if (MatchPageNum > 0)
            {
                return true;
            }
            else return false;
        }
        
        public bool CanNextMatch()
        {
            if (MatchPageNum > MatchList.Count/MatchPageSize)
            {
                return false;
            }
            else return true;
        }
        
    }
    
        
    }



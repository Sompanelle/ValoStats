using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using SkiaSharp;
using ValoStats.Models;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class PlayerLookupPageViewModel : ViewModelBase
    {
        private HttpClient client = ApiHelper.InitializeClient();
        private Config config = FileHelper.ReadConfig();
        [ObservableProperty]
        private string playerQuery;

        [ObservableProperty]
        private int epPageSize = 5;
        
        [ObservableProperty]
        private int epPageNum;

        [ObservableProperty]
        private int matchPageNum;

        [ObservableProperty] 
        private int matchPageSize = 3;

        [ObservableProperty]
        private bool isLoaded;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private int pbar;
        
        [ObservableProperty]
        private bool badSearch = false;

        [ObservableProperty]
        private Player resultPlayerData;

        [ObservableProperty]
        private MMRData resultMMRData;
        
        [ObservableProperty]
        private double displayKd;

        [ObservableProperty] 
        private double displayWr;

        [ObservableProperty]
        private Bitmap cardImage;

        [ObservableProperty]
        private TitleData title;
        
        [ObservableProperty]
        private Bitmap peak;
        
        [ObservableProperty]
        private Bitmap current;
        
        

        private ObservableCollection<PlayedMatch> matchList { get; set; } 
        public ObservableCollection<PlayedMatch> DisplayMatches { get; set; }
        private ObservableCollection<Episode> episodes { get; set; }
        public ObservableCollection<Episode> DisplayEpisodes {get; set;}

        [ObservableProperty]
        private bool isSearchCompelete = false;

        public PlayerLookupPageViewModel()
        {
            if (Design.IsDesignMode)
            {
                IsSearchCompelete = true;
                ResultPlayerData = new Player() { name = "Sompanelle" };
                ResultMMRData = new MMRData() { current = new CurrentMMR() { tier = new Rank() { name = "Gold 1" } }, peak = new Peak() { tier = new Rank() { name = "Platinum 3" } } };
                DisplayEpisodes = new()
                {
                    new Episode() { end_tier = new Rank() { name = "Gold 1" }, season = new() { @short = "e9a1" } },
                    new Episode() { end_tier = new Rank() { name = "Gold 3" }, season = new() { @short = "e8a3" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                };
                DisplayMatches = new()
                {
                    new PlayedMatch() { Map = "Ascent", Mode = "Competitive", Kills = 22, Deaths = 11 , Agent = "Yoru", Score = "13-3" , Result = true },  
                    new PlayedMatch() { Map = "Sunset", Mode = "Competitive", Kills = 11, Deaths= 13, Agent = "Chamber", Score = "13-3" , Result = false }, 
                    new PlayedMatch() { Map = "Sunset", Mode = "Deathmatch", Kills = 36, Deaths = 23, Agent  = "Breach", Result = null }, 
                };
            }
            else
            {
                episodes = new();
                matchList = new();
                DisplayMatches = new();
                DisplayEpisodes = new();
            }
        }

        [RelayCommand(CanExecute = nameof(CanSearch))]
        public async Task PlayerSearch()
        {
            BadSearch = false;
            IsLoading = true;
            //split Name & Tag from search bar and make MMR call
            var _ = PlayerQuery.Split(new[] { '#' });
            
            ResultMMRData = await ApiHelper.GetMMRDataByName(_[0], _[1], client, config);
            Pbar += 10; 
            if (ResultMMRData == null)
                FailSearch();
                
            else
            {
                //if Episode Count isn't zero make a list of the episodes and paginate it
                if (ResultMMRData.seasonal.Count != 0)
                {
                    foreach (Episode ep in ResultMMRData.seasonal)
                    {
                        episodes.Add(ep);
                    }

                    var list = episodes.Take(epPageSize);
                    DisplayEpisodes.Clear();
                    foreach (Episode ep in list)
                    {
                        DisplayEpisodes.Add(ep);
                    }
                }

                if (matchList.Count != 0)
                {
                    
                }
                
                Pbar += 10;
            }

            

            ResultPlayerData = await ApiHelper.GetPlayer(_[0], _[1], client, config);
            if (ResultPlayerData == null)
                FailSearch();
            else
            {
                await GetAllMatchPlayed(ResultPlayerData.puuid, client, config);
                
                Title = await GetTitleAsync(ResultPlayerData.player_title, client);
                if (Title == null)
                    FailSearch();
                
                Pbar += 10; 
                await GetRankImg(client, ResultMMRData);
            
                Pbar += 10; 
                CardImage = await GetCardAsync(ResultPlayerData.card, client);
                if (CardImage == null)
                    FailSearch();
                
                else
                {
                    Pbar += 10; 
                    IsLoading = false;
                    IsSearchCompelete = true;
                    IsLoaded = true;
                }
                
                
            }                
            


        }

        public bool CanSearch()
        {
            if (!string.IsNullOrEmpty(playerQuery))
            {
                if (PlayerQuery.Contains("#"))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        [RelayCommand(CanExecute = nameof(CanNextEp))]
        public void NextEpisodes()
        {
            EpPageNum++;
            var eps = episodes.Skip(EpPageSize*EpPageNum);
            var list = eps.Take(EpPageSize);
            DisplayEpisodes.Clear();
            foreach(Episode ep in list)
            {
                DisplayEpisodes.Add(ep);
            }
        }
        
        [RelayCommand(CanExecute = nameof(CanPrevEp))]
        public void PrevEpisodes()
        {
            EpPageNum--;
            var eps = episodes.Skip(EpPageSize*EpPageNum);
            var list = eps.Take(EpPageSize);
            DisplayEpisodes.Clear();
            foreach(Episode ep in list)
            {
                DisplayEpisodes.Add(ep);
            }
        }
        
        [RelayCommand(CanExecute = nameof(CanNextMatch))]
        public void NextMatches()
        {
            MatchPageNum++;
            var matches = matchList.Skip(MatchPageSize*MatchPageNum);
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
            var matches = matchList.Skip(MatchPageSize*MatchPageNum);
            var list = matches.Take(MatchPageSize);
            DisplayMatches.Clear();
            foreach(PlayedMatch m in list)
            {
                DisplayMatches.Add(m);
            }
        }


        public bool CanPrevEp()
        {
            if (EpPageNum > 0)
            {
                return true;
            }
            else return false;
        }

        public bool CanNextEp()
        {
            if (EpPageNum > episodes.Count/EpPageSize)
            {
                return false;
            }
            else return true;
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
            if (MatchPageNum > matchList.Count/MatchPageSize)
            {
                return false;
            }
            else return true;
        }
        
        private async Task<Bitmap?> GetCardAsync(string AssetId, HttpClient Client)
        {
            var data = await ApiHelper.GetCard(AssetId, Client);
            if (data == null) return null;
            else return Bitmap.DecodeToHeight(data, 320);
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
                Current = Bitmap.DecodeToWidth(currentData, 55);
            }
            var peakData = await ApiHelper.GetRankImg(MMRData.peak.tier.id,Client);
            if (peakData != null)
            {
                Peak = Bitmap.DecodeToWidth(peakData, 55);
            }
        }

        private void FailSearch()
        {
            BadSearch = true;
            IsLoading = false;
        }
        
        private async Task GetAllMatchPlayed(string Puuid, HttpClient Client, Config Config)
        {
            DisplayKd = 0;
            DisplayWr = 0;
            DisplayMatches.Clear();
            matchList.Clear();
            var newMatches = await ApiHelper.GetLastMatchList(Puuid, Client, Config);
            if (newMatches != null)
            {
                double rawKd = 0;
                double rawWr = 0;
                double kills = 0;
                double deaths = 0;
                double wins = 0;
                double losses = 0;
                foreach (var match in newMatches)
                {
                    kills += match.Kills;
                    deaths += match.Deaths;
                    if (match.Result == true)
                        wins += 1;
                    else
                        losses += 1;
                    matchList.Add(match);
                }
                var list = matchList.Take(3);
                foreach (var m in list)
                {
                    DisplayMatches.Add(m);
                }
                rawKd = double.Round((kills / deaths), 2);
                rawWr = double.Round((wins / matchList.Count), 2) * 100;
                DisplayKd = rawKd;
                DisplayWr = rawWr;
            }
            else
                FailSearch();
        }
        
    }
}

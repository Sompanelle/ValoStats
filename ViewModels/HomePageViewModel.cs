using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.OpenGL.Egl;
using ValoStats.Models;
using ValoStats.Models;
using ValoStats.ViewModels.DTOs;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        private HttpClient client;

        private Config? config;
        
        [ObservableProperty]
        private bool badRequest;

        [ObservableProperty]
        private bool isLoaded;

        [ObservableProperty]
        private int pbar;

        [ObservableProperty]
        private Player? player;

        [ObservableProperty]
        private Bitmap cardImage;
        
        [ObservableProperty]
        private int kd;

        [ObservableProperty] 
        private MMRData mmrData;

        [ObservableProperty]
        private TitleData title;

        [ObservableProperty]
        private DateTime updatedAt;

        [ObservableProperty]
        private int level;

        [ObservableProperty]
        private string concatName;
        
        

        public ObservableCollection<PlayedMatch> Matches { get; set; }

        public HomePageViewModel()
        {

            if (Design.IsDesignMode)
            {
                IsLoaded = true;
                Matches = new() { new PlayedMatch() { Map = "Ascent", Mode = "Competitive", KD = "22/12", Agent = "Yoru", Score = "13-3" ,Team = new Team() {has_won = true}}, };
                ConcatName = "Sompanelle#NOIR";
                Title = new() { titleText = "Unserious" };
            }
            else
            {
                config = FileHelper.ReadConfig();
                client = ApiHelper.InitializeClient();
                Matches = new();
                concatName = $"{config.Name}#{config.Tag}";
                InitiliazeHome(config,client);
            }

        }

        private async Task InitiliazeHome(Config Config, HttpClient Client)
        {
            string settingName = Config.Name;
            string settingTag = Config.Tag;
            if ( !string.IsNullOrEmpty(settingName) || !string.IsNullOrEmpty(settingTag))
            {
                var resultPlayer= await ApiHelper.GetPlayer(settingName, settingTag, Client);
                if (resultPlayer != null)
                {
                    Player = resultPlayer;
                    Pbar += 10;
                    ConcatName = $"{settingName}#{settingTag}";
                    Pbar += 10;
                    MmrData = await ApiHelper.GetMMRData(Player.name, Player.tag, Client);
                    Debug.WriteLine(player.card);
                    Pbar += 10;
                    CardImage = await GetCardAsync(Player.card, Client);
                    Pbar += 10;
                    Title = await GetTitleAsync(Player.player_title, Client);
                    Pbar += 10;
                    await GetMatchPlayed(Player.name, Player.tag, Client);
                    Pbar += 10;
                    IsLoaded = true;
                }
            }
            else
            {
                BadRequest = true;
            }


        }

        private async Task GetMatchPlayed(string Name, string Tag, HttpClient Client)
        {
            var lastTen = await ApiHelper.GetLastFiveMatchDatas(Name, Tag, Client);
            if (lastTen != null)
            {
                foreach (Datum matchData in lastTen)
                {
                    var match = MatchDTO.DatumToPlayedMatch(matchData);
                    Matches.Add(match);
                }
            }
            else BadRequest = true;
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

    }

}


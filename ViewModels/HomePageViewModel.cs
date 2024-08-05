using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.OpenGL.Egl;
using ValorantTrackerApp.Models;
using ValoStats.Models;
using ValoStats.ViewModels.DTOs;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        
        [ObservableProperty]
        private bool badRequest;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private Player? player;

        [ObservableProperty]
        public Bitmap cardImage;

        
        
        [ObservableProperty]
        private int kd;

        [ObservableProperty] 
        private MMRData mmrData;

        [ObservableProperty]
        private string tag;

        [ObservableProperty]
        private string title;

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
                Matches = new();
                var appSettings = ConfigurationManager.AppSettings;
                Name = "Sompanelle";
                Tag = "N0IR";
                title = ":3";
                updatedAt = DateTime.Now;
                level = 49;
                concatName = $"{Name}#{Tag}";
                Matches.Add(new PlayedMatch() { Agent = "Iso", Map = "Ascent", Mode= "Competitive", KD = "20/12" } );
                Matches.Add(new PlayedMatch() { Agent = "Reyna", Map = "Bind", Mode = "Competitive", KD = "12/12" });
                Matches.Add(new PlayedMatch() { Agent = "Skye",Map = "Sunset", Mode = "Competitive", KD = "9/12" });
                Matches.Add(new PlayedMatch() { Agent = "Iso", Map = "Abyss", Mode = "Competitive", KD = "22/11" });
                Matches.Add(new PlayedMatch() { Agent = "Iso", Map = "Abysss", Mode = "Competitive", KD = "24/12" });
                MmrData = new MMRData() { current = new CurrentMMR() { tier = new Rank() { name = "Gold 1" } }, peak = new Peak() { tier = new Rank() { name = "Platinum 3" } } };
            }
            else
            {
                ApiHelper.InitializeClient();
                Matches = new();
                concatName = $"{name}#{tag}";
                GetPlayer();
            }

        }

        private async Task GetPlayer()
        {
            Config? Config = FileHelper.ReadConfig();
            string settingName = Config.Name;
            string settingTag = Config.Tag;
            if ( !string.IsNullOrEmpty(settingName) || !string.IsNullOrEmpty(settingTag))
            {
                var resultPlayer= await ApiHelper.GetPlayer(settingName, settingTag);
                if (resultPlayer != null)
                {
                    Player = resultPlayer;
                    ConcatName = $"{settingName}#{settingTag}";
                    UpdatedAt = Player.updated_at;
                    Level = Player.level;
                    MmrData = await ApiHelper.GetMMRData(Player.name, Player.tag);
                    CardImage = await GetCardAsync(Player.card);
                    await GetMatchPlayed(Player.name, Player.tag);
                }
            }
            else
            {
                BadRequest = true;
            }


        }

        private async Task GetMatchPlayed(string Name, string Tag)
        {
            var lastTen = await ApiHelper.GetLastMatchDatas(Name, Tag);
            if (lastTen != null)
            {
                foreach(Datum matchData in lastTen)
                {
                    var match = MatchDTO.DatumToPlayedMatch(matchData);
                    Matches.Add(match);
                }
                
            }
            else
            {
                BadRequest = true; 
            }
        }
        

        private async Task<Bitmap> GetCardAsync(string assetId)
        {
            var data = await ApiHelper.GetCard(assetId);
            return  Bitmap.DecodeToHeight(data, 320);
        }

    }

}


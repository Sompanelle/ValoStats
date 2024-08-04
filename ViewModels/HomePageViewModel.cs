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
        private int kd;

        [ObservableProperty] 
        private MMRData mmrData;

        [ObservableProperty]
        private string tag;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private DateTime updated_at;

        [ObservableProperty]
        private int level;

        [ObservableProperty]
        private string concatName;

        [ObservableProperty]
        private float avgkd;

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
                updated_at = DateTime.Now;
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

        public async Task GetPlayer()
        {
            Config? Config = FileHelper.ReadConfig();
            string settingName = Config.Name;
            string settingTag = Config.Tag;
            if ( !string.IsNullOrEmpty(settingName) || !string.IsNullOrEmpty(settingTag))
            {
                Player? player = await ApiHelper.GetPlayer(settingName, settingTag);
                if (player != null)
                {
                    ConcatName = $"{settingName}#{settingTag}";
                    Updated_at = player.updated_at;
                    Level = player.level;
                    MmrData = await ApiHelper.GetMMRData(settingName, settingTag);
                    await GetMatchPlayed(settingName, settingTag);
                }
            }
            else
            {
                BadRequest = true;
            }


        }

        public async Task GetMatchPlayed(string Name, string Tag)
        {
            var lastTen = await ApiHelper.GetLastTenPlayedMatches(Name, Tag);
            if (lastTen != null)
            {
                foreach(PlayedMatch match in lastTen)
                {
                    Matches.Add(match);
                }
                
            }
            else
            {
                BadRequest = true; 
            }
        }

    }

}


using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
using ValoStats.Models;
using ValoStats.ViewModels.DTOs;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int kd;

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
            ApiHelper.InitializeClient();
            Matches = new();
            if (Design.IsDesignMode)
            {
                var appSettings = ConfigurationManager.AppSettings;
                name = appSettings["Name"];
                tag = appSettings["Tag"];
                title = ":3";
                updated_at = DateTime.Now;
                level = 49;
                concatName = $"{name}#{tag}";
                Matches.Add(new PlayedMatch() { Agent = "Iso", Map = "Ascent", Mode= "Competitive", KD = "20/12" } );
                Matches.Add(new PlayedMatch() { Agent = "Reyna", Map = "Bind", Mode = "Competitive", KD = "12/12" });
                Matches.Add(new PlayedMatch() { Agent = "Skye",Map = "Sunset", Mode = "Competitive", KD = "9/12" });
                Matches.Add(new PlayedMatch() { Agent = "Iso", Map = "Abyss", Mode = "Competitive", KD = "22/11" });
                Matches.Add(new PlayedMatch() { Agent = "Iso", Map = "Abysss", Mode = "Competitive", KD = "24/12" });
            }
            concatName = $"{name}#{tag}";
            GetPlayer();
        }

        public async void GetPlayer()
        {
            var appSettings = ConfigurationManager.AppSettings;
            if (appSettings.Count == 0)
            {
                Debug.WriteLine("Error Reading Config");
            }
            string? settingName = appSettings["Name"];
            string? settingTag = appSettings["Tag"];
            if (settingName != null || settingTag != null )
            {
                Player? player = await ApiHelper.GetPlayer(settingName, settingTag);
                if (player != null)
                {
                    ConcatName = $"{settingName}#{settingTag}";
                    Updated_at = player.updated_at;
                    Level = player.level;
                    GetMatchPlayed(settingName,settingTag);
                }
                else
                {
                    throw new NotImplementedException();
                }

            }
            

            
        }

        public async void GetMatchPlayed(string name, string tag)
        {
            var lastFive = await ApiHelper.GetLastFivePlayedMatches(name, tag);
            if (lastFive != null)
            {
                foreach(PlayedMatch match in lastFive)
                {
                    Matches.Add(match);
                }
            }
        }

    }
}

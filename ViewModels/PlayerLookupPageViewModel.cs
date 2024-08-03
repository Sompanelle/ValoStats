using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ValoStats.Models;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class PlayerLookupPageViewModel : ViewModelBase
    {

        [ObservableProperty]
        private string playerQuery;

        [ObservableProperty]
        private bool badSearch = false;

        [ObservableProperty]
        private Player resultPlayerData;

        [ObservableProperty]
        private MMRData resultMMRData;

        [ObservableProperty]
        private int pageNum = 0;

        [ObservableProperty]
        private int pageSize = 5;

        public ObservableCollection<Episode> DisplayEpisodes {get; set;}

        public ObservableCollection<Episode> Episodes { get; set; }

        [ObservableProperty]
        private bool isSearchCompelete = false;

        public PlayerLookupPageViewModel()
        {
            Episodes = new();
            DisplayEpisodes = new();
            if (Design.IsDesignMode)
            {
                IsSearchCompelete = true;
                ResultPlayerData = new Player() { name = "Sompanelle" };
                ResultMMRData = new MMRData() { current = new CurrentMMR() { tier = new Rank() { name = "Gold 1" } }, peak = new Peak() { tier = new Rank() { name = "Platinum 3" } } };
                DisplayEpisodes = new ObservableCollection<Episode>()
                {
                    new Episode() { end_tier = new Rank() { name = "Gold 1" }, season = new() { @short = "e9a1" } },
                    new Episode() { end_tier = new Rank() { name = "Gold 3" }, season = new() { @short = "e8a3" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                    new Episode() { end_tier = new Rank() { name = "Platinum 1" }, season = new() { @short = "e8a2" } },
                };
            }
        }

        [RelayCommand(CanExecute = nameof(CanSearch))]
        public async Task PlayerSearch()
        {
            var _ = PlayerQuery.Split(new[] {'#'});
            var mmr = await ApiHelper.GetMMRData(_[0], _[1]);
            if (mmr != null)
            {
                IsSearchCompelete = true;
                BadSearch = false;
                ResultMMRData = mmr;
                if(mmr.seasonal.Count != 0)
                {
                    foreach(Episode ep in mmr.seasonal)
                    {
                        Episodes.Add(ep);
                    }
                    var list = Episodes.Take(PageSize);
                    DisplayEpisodes.Clear();
                    foreach (Episode ep in list)
                    {
                        DisplayEpisodes.Add(ep);
                    }
                }
                var player = await ApiHelper.GetPlayer(_[0], _[1]);
                if (player != null)
                {
                    IsSearchCompelete = true;
                    ResultPlayerData = player;
                }
            }
            else
            {
                IsSearchCompelete = false;
                BadSearch = true;
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

        [RelayCommand(CanExecute = nameof(CanNext))]
        public void NextEpisodes()
        {
            PageNum++;
            var eps = Episodes.Skip(PageSize*PageNum);
            var list = eps.Take(PageSize);
            DisplayEpisodes.Clear();
            foreach(Episode ep in list)
            {
                DisplayEpisodes.Add(ep);
            }
        }

        [RelayCommand(CanExecute = nameof(CanPrev))]
        public void PrevEpisodes()
        {
            PageNum--;
            var eps = Episodes.Skip(PageSize*PageNum);
            var list = eps.Take(PageSize);
            DisplayEpisodes.Clear();
            foreach(Episode ep in list)
            {
                DisplayEpisodes.Add(ep);
            }
        }
        public bool CanPrev()
        {
            if (PageNum > 0)
            {
                return true;
            }
            else return false;
        }

        public bool CanNext()
        {
            if (PageNum > Episodes.Count/PageSize)
            {
                return false;
            }
            else return true;
        }



    }
}

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
        [ObservableProperty]
        private string playerQuery;

        [ObservableProperty]
        private bool isLoaded;

        [ObservableProperty]
        private int pbar;
        
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

        [ObservableProperty]
        private Bitmap cardImage;

        [ObservableProperty]
        private TitleData title;
        
        [ObservableProperty]
        private Bitmap peak;
        
        [ObservableProperty]
        private Bitmap current;
        

        public ObservableCollection<Episode> DisplayEpisodes {get; set;}

        public ObservableCollection<Episode> Episodes { get; set; }

        [ObservableProperty]
        private bool isSearchCompelete = false;

        public PlayerLookupPageViewModel()
        {
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
            else
            {
                Episodes = new();
                DisplayEpisodes = new();
            }
        }

        [RelayCommand(CanExecute = nameof(CanSearch))]
        public async Task PlayerSearch()
        {

            //split Name & Tag from search bar and make MMR call
            var _ = PlayerQuery.Split(new[] { '#' });
            
            ResultMMRData = await ApiHelper.GetMMRData(_[0], _[1], client);
            if (ResultMMRData == null)
                BadSearch = true;
            else
            {
                //if Episode Count isn't zero make a list of the episodes and paginate it
                if (ResultMMRData.seasonal.Count != 0)
                {
                    foreach (Episode ep in ResultMMRData.seasonal)
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
            }
            

            ResultPlayerData = await ApiHelper.GetPlayer(_[0], _[1], client);
            if (ResultPlayerData == null)
                BadSearch = true;
            else
            {
                Title = await GetTitleAsync(ResultPlayerData.player_title, client);
                if (Title == null)
                    BadSearch = true;
                
                await GetRankImg(client, ResultMMRData);
            
                CardImage = await GetCardAsync(ResultPlayerData.card, client);
                if (CardImage == null)
                    BadSearch = true;
                else
                {
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
            var currentData = await ApiHelper.GetRankImg(Client, MMRData.current.tier.id);
            if (currentData != null)
            {
                Current = Bitmap.DecodeToWidth(currentData, 55);
            }
            var peakData = await ApiHelper.GetRankImg(Client, MMRData.peak.tier.id);
            if (peakData != null)
            {
                Peak = Bitmap.DecodeToWidth(peakData, 55);
            }
        }
        
    }
}

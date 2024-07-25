using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ValorantTrackerApp.Helpers;
using ValorantTrackerApp.Models;

namespace ValorantTrackerApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel viewModel { get; set; }

        public static ObservableCollection<Match> matches { get; set; }

        public MainWindowViewModel()
        {
            ApiHelper.InitializeClient();
            matches = new ObservableCollection<Match>();
            GetLastFiveMatches();
        }


        public async void GetLastFiveMatches()
        {
            ObservableCollection<Match> matchList = await ApiHelper.GetLastFiveMatches();

            if (matchList != null)
            {
                foreach (Match match in matchList)
                {
                    matches.Add(match);
                }
            }
        }
    }
}

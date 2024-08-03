using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ValorantTrackerApp.Models;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class SettingsPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string tag;

        [ObservableProperty]
        private string region;

        [ObservableProperty]
        private string key;


        [RelayCommand]
        public void Save()
        {
            Config config = new Config()
            {
                Name = name,
                Tag = tag,
                Region = region,
                Key = key
            };
            FileHelper.WriteConfig(config);
        }


        public SettingsPageViewModel()
        {
            Config? savedConfig = FileHelper.ReadConfig();
            if(savedConfig != null)
            {
                Name = savedConfig.Name;
                Tag = savedConfig.Tag;
                Region = savedConfig.Region;
                Key = savedConfig.Key;
            }

        }
    }
}

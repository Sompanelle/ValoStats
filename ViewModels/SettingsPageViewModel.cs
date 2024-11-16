using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using ValoStats.Models;
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
            Config config = new Config(Name, Tag, Region, Key);
            FileHelper.WriteConfig(config);
        }


        public SettingsPageViewModel()
        {

            if (Design.IsDesignMode)
            {
                Name = String.Empty;
                Tag = String.Empty;
                Region = String.Empty;
                Key = String.Empty;
            }
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

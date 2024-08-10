using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Raw;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValoStats.Models;
using ValoStats.ViewModels.Helpers;

namespace ValoStats.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        [ObservableProperty]
        private bool isPanelOpen = false;

        
        [ObservableProperty]
        private ViewModelBase currentPage =  FileHelper.SettingsExist() ? new HomePageViewModel() : new SettingsPageViewModel();


        [ObservableProperty]
        private PageItemTemplate selectedPage;

        partial void OnSelectedPageChanged(PageItemTemplate? NewPage)
        {
            if (NewPage == null) return;
            var instance = Activator.CreateInstance(NewPage.ModelType);
            if (instance == null) return;
            CurrentPage = (ViewModelBase)instance;

        }

        public ObservableCollection<PageItemTemplate> Pages { get; } = new()
        {
            new PageItemTemplate(typeof(HomePageViewModel), "Home"),
            new PageItemTemplate(typeof(PlayerLookupPageViewModel), "Lookup"),
            new PageItemTemplate(typeof(SettingsPageViewModel), "Settings"),

        };


        public MainWindowViewModel()
        {

            if (Design.IsDesignMode)
            {

            }
            else
            {
                if (FileHelper.SettingsExist() != true)
                {
                    CurrentPage = new SettingsPageViewModel();
                }
                else
                {
                    CurrentPage = new HomePageViewModel();
                    ApiHelper.InitializeClient();
                }
            }

        }

        [RelayCommand]
        public void Panel_Click()
        {
            IsPanelOpen = !IsPanelOpen;
        }

        


    }
}

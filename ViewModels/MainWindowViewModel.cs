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
        private ViewModelBase currentPage = new HomePageViewModel();


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
            new PageItemTemplate(typeof(HomePageViewModel)),
            new PageItemTemplate(typeof(PlayerLookupPageViewModel)),
            new PageItemTemplate(typeof(SettingsPageViewModel)),

        };


        public MainWindowViewModel()
        {
            ApiHelper.InitializeClient();
        }

        [RelayCommand]
        public void Panel_Click()
        {
            IsPanelOpen = !IsPanelOpen;
        }

        public class PageItemTemplate
        {

            public PageItemTemplate(Type Type)
            {
                ModelType = Type;
                Label = Type.Name.Replace("PageViewModel", "");
            }


            public string Label { get; }
            public Type ModelType { get; }

        }


    }
}

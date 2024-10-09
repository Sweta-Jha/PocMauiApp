using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocMauiApp.Database;
using PocMauiApp.Model;
using PocMauiApp.View;
using System.Collections.ObjectModel;

namespace PocMauiApp.ViewModel
{
    public partial class PublisherViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public PublisherViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Task.Run(async () => await LoadPublisherEntries());
        }

        [ObservableProperty]
        private ObservableCollection<Publisher> _publishers;

        [ObservableProperty]
        private Publisher _selectedPublisher;

        [RelayCommand]
        public async Task NavigateToAddPublisher()
        {
            var addPublisherPage = new AddPublisherPage(new AddPublisherViewModel(_databaseService));
            await addPublisherPage.ShowPopup();
        }

        [RelayCommand]
        public async Task DeletePublisher(Publisher publisher)
        {
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert("Confirm Deletion", "Are you sure you want to delete this publisher?", "Yes", "No");
            if (isConfirmed)
            {
                _databaseService.DeleteUser(publisher.Id);
                await Application.Current.MainPage.DisplayAlert("Success", "Publisher entry deleted successfully!", "OK");
                await LoadPublisherEntries();
            }
        }

        [RelayCommand]
        public async Task EditPublisher(Publisher publisher)
        {
            var editPublisherPage = new AddPublisherPage(new AddPublisherViewModel(_databaseService, publisher));
            await editPublisherPage.ShowPopup();
        }

        public async Task LoadPublisherEntries()
        {
            var publisherList = await Task.Run(() => _databaseService.GetAllUsers().ToList());

            // Update the ObservableCollection to reflect changes in the UI
            Publishers = new ObservableCollection<Publisher>(publisherList);
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocMauiApp.Database;
using PocMauiApp.Model;
using PocMauiApp.View;

namespace PocMauiApp.ViewModel
{
    public partial class StatusViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public StatusViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Statuses = new List<Status>();
            LoadStatuses();
        }

        [ObservableProperty]
        private List<Status> _statuses;

        [RelayCommand]
        public async Task EditStatus(Status status)
        {
            var statusPage = new EditStatusPage(new EditStatusViewModel(_databaseService, status));
            await statusPage.ShowPopup();
        }

        [RelayCommand]
        public async Task DeleteStatus(Status status)
        {
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert("Confirm Deletion", "Are you sure you want to delete this status?", "Yes", "No");

            if (isConfirmed)
            {
                _databaseService.DeleteStatus(status.Id);
                LoadStatuses();
                await Application.Current.MainPage.DisplayAlert("Success", "Status deleted successfully!", "OK");
            }
        }

        [RelayCommand]
        public async Task NavigateToAddNewStatus()
        {
            var statusPage = new EditStatusPage(new EditStatusViewModel(_databaseService));
            await statusPage.ShowPopup();
        }

        public void LoadStatuses()
        {
            var statuses = _databaseService.GetAllStatuses();
            Statuses = new List<Status>(statuses);
        }
    }
}

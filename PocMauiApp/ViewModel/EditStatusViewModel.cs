using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using PocMauiApp.Database;
using PocMauiApp.Model;

namespace PocMauiApp.ViewModel
{
    public partial class EditStatusViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public EditStatusViewModel(DatabaseService databaseService, Status status = null)
        {
            _databaseService = databaseService;
            Status = status ?? new Status();
        }

        [ObservableProperty]
        private Status _status;

        [RelayCommand]
        public async Task SaveStatus()
        {
            if (!string.IsNullOrWhiteSpace(Status.StatusName))
            {
                if (Status.Id == 0)
                {
                    // Add new status
                    _databaseService.AddStatus(Status);
                    await Application.Current.MainPage.DisplayAlert("Success", "Status entry added successfully!", "OK");
                }
                else
                {
                    // Update existing status
                    _databaseService.UpdateStatus(Status);
                    await Application.Current.MainPage.DisplayAlert("Success", "Status entry updated successfully!", "OK");
                }
                await MopupService.Instance.PopAsync();
                MessagingCenter.Send<object>(this, "RefreshStatusList");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Information", "Please fill in all required fields.", "OK");
            }
        }
    }
}

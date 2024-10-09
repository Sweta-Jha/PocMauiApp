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
                try
                {
                    // Add or update the status
                    if (Status.Id == 0)
                    {
                        _databaseService.AddStatus(Status);
                        await ShowSuccessMessage("Status entry added successfully!");
                    }
                    else
                    {
                        _databaseService.UpdateStatus(Status);
                        await ShowSuccessMessage("Status entry updated successfully!");
                    }

                    // Close the popup and refresh the status list
                    await MopupService.Instance.PopAsync();
                    MessagingCenter.Send<object>(this, "RefreshStatusList");
                }
                catch (Exception ex)
                {
                    await ShowErrorMessage($"Failed to save status: {ex.Message}");
                }
            }
            else
            {
                await ShowErrorMessage("Please fill in all required fields.");
            }
        }
        private async Task ShowErrorMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }
        private async Task ShowSuccessMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Success", message, "OK");
        }
    }
}

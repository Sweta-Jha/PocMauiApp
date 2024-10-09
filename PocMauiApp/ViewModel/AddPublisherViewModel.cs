using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using PocMauiApp.Database;
using PocMauiApp.Model;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace PocMauiApp.ViewModel
{
    public partial class AddPublisherViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private Publisher _newPublisher = new();

        [ObservableProperty]
        private bool _canSaveNewPublisher;

        public ICommand SaveCommand { get; }

        public AddPublisherViewModel(DatabaseService databaseService, Publisher publisher = null)
        {
            _databaseService = databaseService;
            NewPublisher = publisher ?? new Publisher();

            // Initialize the SaveCommand with CanSaveNewPublisher condition
            SaveCommand = new RelayCommand(async () => await SaveNewPublisher(), () => CanSaveNewPublisher);

            // Subscribe to property changes to validate input fields
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(NewPublisher.Name) ||
                    e.PropertyName == nameof(NewPublisher.Email))
                {
                    ValidateInputFields();
                }
            };

            ValidateInputFields();
        }

        private void ValidateInputFields()
        {
            bool isValidEmail = ValidateEmail(NewPublisher.Email);

            CanSaveNewPublisher = !string.IsNullOrWhiteSpace(NewPublisher.Name) &&
                                  !string.IsNullOrWhiteSpace(NewPublisher.Email) &&
                                  isValidEmail;

            // Notify the UI about the change in CanSaveNewPublisher status
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
  
        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regular expression to validate email format
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }

        [RelayCommand]
        private async Task SaveNewPublisher()
        {
            await Task.Run(() => ValidateInputFields());

            if (CanSaveNewPublisher)
            {
                if (NewPublisher.Id == 0)
                {
                    _databaseService.AddUser(NewPublisher);
                    await Application.Current.MainPage.DisplayAlert("Success", "Publisher entry added successfully!", "OK");
                }
                else
                {
                    _databaseService.UpdateUser(NewPublisher);
                    await Application.Current.MainPage.DisplayAlert("Success", "Publisher entry updated successfully!", "OK");
                }

                // Navigate back to the previous page
                await MopupService.Instance.PopAsync();
                MessagingCenter.Send<object>(this, "RefreshPublisherList");
            }
            else
            {
                var errorMessage = "Please fill in all required fields.";
                if (!ValidateEmail(NewPublisher.Email))
                {
                    errorMessage = "Please enter a valid email address.";
                }

                await Application.Current.MainPage.DisplayAlert("Information", errorMessage, "OK");
            }
        }

        [RelayCommand]
        private async Task OnCancel()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}

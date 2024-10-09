using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocMauiApp.Database;
using PocMauiApp.Model;
using PocMauiApp.View;


namespace PocMauiApp.ViewModel
{
    public partial class BlogListViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly PublisherPage _publisherPage;
        private readonly StatusPage _statusPage;
        public BlogListViewModel(DatabaseService databaseService, PublisherPage publisher, StatusPage statusPage)
        {
            _databaseService = databaseService;
            _publisherPage = publisher;
            _statusPage = statusPage;
            LoadBlogEntries();
        }

        [ObservableProperty]
        private List<BlogEntry> _blogEntries;

        [RelayCommand]
        public async Task NavigateToAddNew()
        {
            var blogEntryPage = new BlogEntryPage(new BlogEntryViewModel(_databaseService));
            await blogEntryPage.ShowPopup();
        }
        [RelayCommand]
        public async Task DeleteBlogEntry(BlogEntry blogEntry)
        {
            if (blogEntry == null)
            {
                await ShowErrorMessage("Blog entry not found!");
                return;
            }

            bool confirmDelete = await ShowConfirmationDialog("Confirm Deletion", "Are you sure you want to delete this blog entry?");
            if (!confirmDelete) return;

            try
            {
                _databaseService.DeleteBlogEntry(blogEntry.Id);
                await ShowSuccessMessage("Blog entry deleted successfully!");
                LoadBlogEntries();
            }
            catch (Exception ex)
            {
                await ShowErrorMessage($"Failed to delete blog entry: {ex.Message}");
            }
        }

        private async Task ShowErrorMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }

        private async Task<bool> ShowConfirmationDialog(string title, string message)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
        }

        private async Task ShowSuccessMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Success", message, "OK");
        }

        [RelayCommand]
        public async Task EditBlogEntry(BlogEntry blogEntry)
        {
            var blogEntryPage = new BlogEntryPage(new BlogEntryViewModel(_databaseService, blogEntry));
            await blogEntryPage.ShowPopup();
        }

        [RelayCommand]
        private async Task NavigateToStatus()
        {
            await Application.Current.MainPage.Navigation.PushAsync(_statusPage);
        }
        [RelayCommand]
        private async Task NavigateToPublishers()
        {
            await Application.Current.MainPage.Navigation.PushAsync(_publisherPage);
        }
        public void LoadBlogEntries()
        {
            BlogEntries = _databaseService.GetAllBlogEntries().ToList();
        }
    }
}

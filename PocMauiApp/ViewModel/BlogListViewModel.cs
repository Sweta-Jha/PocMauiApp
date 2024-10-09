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
                await Application.Current.MainPage.DisplayAlert("Error", "Blog entry not found!", "OK");
                return;
            }

            bool confirmDelete = await Application.Current.MainPage.DisplayAlert(
                "Confirm Deletion",
                "Are you sure you want to delete this blog entry?",
                "Yes",
                "No"
            );

            if (!confirmDelete) return;

            try
            {
                _databaseService.DeleteBlogEntry(blogEntry.Id);

                await Application.Current.MainPage.DisplayAlert("Success", "Blog entry deleted successfully!", "OK");

                LoadBlogEntries();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete blog entry: {ex.Message}", "OK");
            }
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

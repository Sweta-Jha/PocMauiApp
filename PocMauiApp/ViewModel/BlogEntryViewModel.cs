using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using PocMauiApp.Database;
using PocMauiApp.Model;

namespace PocMauiApp.ViewModel;
public partial class BlogEntryViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    public BlogEntryViewModel(DatabaseService databaseService, BlogEntry blogEntry = null)
    {
        _databaseService = databaseService;
        SelectedBlogEntry = blogEntry ?? new BlogEntry();
        Publishers = new List<Publisher>();
        Statuses = new List<Status>();
        if (blogEntry == null)
        {
            SelectedBlogEntry.AssignmentDate = SelectedBlogEntry.PublishedDate = DateTime.Now;
        }
        LoadData();
    }
    [ObservableProperty]
    private List<Publisher> _publishers;

    [ObservableProperty]
    private List<Status> _statuses;

    [ObservableProperty]
    private BlogEntry _selectedBlogEntry;

    [ObservableProperty]
    private Publisher _selectedPublisher;

    [ObservableProperty]
    private Status _selectedStatus;

    partial void OnSelectedPublisherChanged(Publisher value)
    {
        if (value != null)
        {
            SelectedBlogEntry.AssignedToId = value.Id;
        }
    }
    // Called when selected status changes
    partial void OnSelectedStatusChanged(Status value)
    {
        if (value != null)
        {
            SelectedBlogEntry.StatusId = value.Id;
        }
    }

    [RelayCommand]
    public async Task SaveBlogEntry()
    {
        if (ValidateEntry())
        {
            SelectedBlogEntry.AssignedToId = SelectedPublisher?.Id ?? 0;
            SelectedBlogEntry.StatusId = SelectedStatus?.Id ?? 0;

            try
            {
                // Add or update the blog entry
                if (SelectedBlogEntry.Id == 0)
                {
                    _databaseService.AddBlogEntry(SelectedBlogEntry);
                    await Application.Current.MainPage.DisplayAlert("Success", "Blog entry added successfully!", "OK");
                }
                else
                {
                    _databaseService.UpdateBlogEntry(SelectedBlogEntry);
                    await Application.Current.MainPage.DisplayAlert("Success", "Blog entry updated successfully!", "OK");
                }

                // Close the popup and refresh the blog list
                await MopupService.Instance.PopAsync();
                MessagingCenter.Send<object>(this, "RefreshBlogList");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save the blog entry: {ex.Message}", "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Information", "Please fill in all required fields!", "OK");
        }
    }
    private bool ValidateEntry()
    {
        return !string.IsNullOrWhiteSpace(SelectedBlogEntry.BlogTopic) &&
               !string.IsNullOrWhiteSpace(SelectedBlogEntry.TeamLeadName) &&
               SelectedBlogEntry.AssignmentDate != DateTime.MinValue &&
               SelectedBlogEntry.PublishedDate != DateTime.MinValue &&
               SelectedPublisher != null && SelectedStatus != null;
    }

    [RelayCommand]
    public async Task OnCancel()
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private void LoadPublishers()
    {
        Publishers = _databaseService.GetAllUsers().ToList();
    }

    private void LoadStatuses()
    {
        Statuses = _databaseService.GetAllStatuses().ToList();
    }
    private void LoadData()
    {
        LoadPublishers();
        LoadStatuses();
        // Set selected values once the data is loaded
        SelectedPublisher = Publishers.Find(p => p.Id == SelectedBlogEntry.AssignedToId);
        SelectedStatus = Statuses.Find(s => s.Id == SelectedBlogEntry.StatusId);
    }
}

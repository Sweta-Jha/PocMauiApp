using PocMauiApp.ViewModel;

namespace PocMauiApp.View;

public partial class BlogListPage : ContentPage
{
    private BlogListViewModel _viewModel;
    public BlogListPage(BlogListViewModel blogListViewModel)
    {
        InitializeComponent();
        BindingContext = blogListViewModel;
        _viewModel = blogListViewModel;
        MessagingCenter.Subscribe<object>(this, "RefreshBlogList", (sender) =>
        {
            _viewModel.LoadBlogEntries();
        });
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadBlogEntries();
    }
}
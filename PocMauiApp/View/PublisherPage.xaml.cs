using PocMauiApp.ViewModel;

namespace PocMauiApp.View;

public partial class PublisherPage : ContentPage
{
    private readonly PublisherViewModel _publisherViewModel;
    public PublisherPage(PublisherViewModel publisherViewModel)
    {
        InitializeComponent();
        BindingContext = publisherViewModel;
        _publisherViewModel=publisherViewModel;
        MessagingCenter.Subscribe<object>(this, "RefreshPublisherList", (sender) =>
        {
            _publisherViewModel.LoadPublisherEntries();
        });
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _publisherViewModel.LoadPublisherEntries();
    }
}
using PocMauiApp.ViewModel;

namespace PocMauiApp.View;

public partial class StatusPage : ContentPage
{
    private readonly StatusViewModel _statusViewModel;
    public StatusPage(StatusViewModel statusViewModel)
    {
        InitializeComponent();
        BindingContext = _statusViewModel= statusViewModel;
        MessagingCenter.Subscribe<object>(this, "RefreshStatusList", (sender) =>
        {
            _statusViewModel.LoadStatuses();
        });
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _statusViewModel.LoadStatuses();
    }
}
using Mopups.Pages;
using Mopups.Services;
using PocMauiApp.Model;
using PocMauiApp.ViewModel;

namespace PocMauiApp.View;

public partial class EditStatusPage : PopupPage
{
    public EditStatusPage(EditStatusViewModel statusViewModel)
    {
        InitializeComponent();
        BindingContext = statusViewModel;
    }
    public async Task ShowPopup()
    {
        await MopupService.Instance.PushAsync(this);
    }

    public async Task ClosePopup()
    {
        await MopupService.Instance.PopAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await ClosePopup();
    }
}

namespace PocMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Created += Window_Created;
        return window;
    }

    private async void Window_Created(object sender, EventArgs e)
    {
        var window = (Window)sender;

        // Ensure the window has been created before adjusting its properties
        await window.Dispatcher.DispatchAsync(() => { });

        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

        // Set window width and height to the screen's width and height
        window.Width = displayInfo.Width / displayInfo.Density;  // Convert to DIPs
        window.Height = displayInfo.Height / displayInfo.Density; // Convert to DIPs

        //  Center the window
        window.X = 0; // Align to the left
        window.Y = 0; // Align to the top
    }
}

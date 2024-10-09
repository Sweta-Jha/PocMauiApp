# PocMauiApp - Blog Entry Management Application

## Overview
PocMauiApp is a cross-platform MAUI application that demonstrates CRUD (Create, Read, Update, Delete) operations for managing blog entries, publishers, and statuses using SQLite as the local database. The application provides a user-friendly interface for managing blog-related data effectively.

## Features
- **CRUD Operations**: Supports creating, reading, updating, and deleting blog entries, publishers, and statuses.
- **SQLite Database**: Uses SQLite for data persistence and management.
- **Data Relationships**: Implements foreign key relationships between blog entries, publishers, and statuses.

## Project Structure
- **Model**: Contains the data models for `BlogEntry`, `Publisher`, and `Status`.
- **Database**: Contains the `DatabaseService` class, which handles all database operations.
- **View**: Contains the UI pages for displaying and interacting with the data.
- **ViewModel**: Contains the view models that manage the data and logic for the views.

## Setup
The `MauiProgram.cs` file is the entry point for configuring the MAUI application. Here's how it works:

### Configuration
```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .ConfigureMopups()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
```
- UseMauiApp: Specifies the main application class.
- ConfigureMopups: Configures Mopups for displaying pop-up dialogs.
- ConfigureFonts: Adds custom fonts for the application.

### Logging
```csharp
#if DEBUG
    builder.Logging.AddDebug();
#endif
```

- Debug Logging: Enables logging during development to help with troubleshooting.

### Database Service
```csharp
var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Blog3.db");
builder.Services.AddSingleton(new DatabaseService(dbPath));
```
- Database Path: Defines the path for the SQLite database.
- Database Service Registration: Registers the DatabaseService as a singleton, allowing it to be reused across the application.
 
### Dependency Injection
The following view models and pages are registered with the service container:
```csharp
builder.Services.AddTransient<BlogEntryViewModel>();
builder.Services.AddTransient<BlogListViewModel>();
builder.Services.AddTransient<AddPublisherViewModel>();
builder.Services.AddTransient<PublisherViewModel>();
builder.Services.AddTransient<StatusViewModel>();
builder.Services.AddTransient<EditStatusViewModel>();
builder.Services.AddTransient<BlogEntryPage>();
builder.Services.AddTransient<BlogListPage>();
builder.Services.AddTransient<PublisherPage>();
builder.Services.AddTransient<AddPublisherPage>();
builder.Services.AddTransient<StatusPage>();
builder.Services.AddTransient<EditStatusPage>();
```
- Transient Services: Each service is registered as transient, meaning a new instance will be created each time it is requested.

## Application Configuration

### App.xaml
The `App.xaml` file defines the application’s overall structure, including resources and styles used throughout the application. Here's a brief overview of its contents:

```xml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:PocMauiApp"
             x:Class="PocMauiApp.App">
    <Application.Resources>
        <ResourceDictionary>
            <!-- Define color resources for the application -->
            <Color x:Key="PageBackgroundColor">#F5F5F5</Color>
            <Color x:Key="CardBackgroundColor">#FFFFFF</Color>
            <Color x:Key="LabelTextColor">#333333</Color>
            <Color x:Key="TextColor">#555555</Color>
            <Color x:Key="MenuBackgroundColor">#FFFFFF</Color>
            <Color x:Key="MenuItemTextColor">#333333</Color>
            <Color x:Key="HeaderBackgroundColor">#FF6200EE</Color>
            <Color x:Key="HeaderTextColor">#FFFFFF</Color>
            <Color x:Key="FrameBorderColor">#CCCCCC</Color>
            <Color x:Key="EntryTextColor">#333333</Color>
            <Color x:Key="TitleTextColor">#333333</Color>
            <Color x:Key="CollectionViewBackgroundColor">#FFFFFF</Color>
            <Color x:Key="ItemBackgroundColor">#FAFAFA</Color>
            <Color x:Key="ItemTextColor">#666666</Color>
            <Color x:Key="EntryBackgroundColor">#FFFFFF</Color>
            <Color x:Key="ButtonBackgroundColor">#6200EE</Color>
            <Color x:Key="ButtonTextColor">#FFFFFF</Color>
            <Color x:Key="CancelButtonBackgroundColor">#B00020</Color>
            <Color x:Key="CancelButtonTextColor">#FFFFFF</Color>
            <Color x:Key="DeleteButtonBackgroundColor">#B00020</Color>
            <Color x:Key="DeleteButtonTextColor">#FFFFFF</Color>
            <Color x:Key="AddButtonBackgroundColor">#03DAC5</Color>
            <Color x:Key="AddButtonTextColor">#FFFFFF</Color>
            <Color x:Key="EditButtonColor">#4CAF50</Color>
            <Color x:Key="DeleteButtonColor">#F44336</Color>
            <Color x:Key="ViewDetailsButtonColor">#2196F3</Color>
            <Color x:Key="AddNewButtonColor">#2196F3</Color>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```
- Resources: This section defines various color resources that can be reused throughout the application, ensuring consistent styling.
- Merged Dictionaries: Additional resource dictionaries can be merged here for modularity and maintainability.

### App.xaml.cs
The App.xaml.cs file contains the code-behind for the App.xaml file, initializing the application and setting up the main page. Here's an overview:
```csharp
namespace PocMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell(); // Set the main page of the application to AppShell
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Created += Window_Created; // Event handler for window creation
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

        // Center the window
        window.X = 0; // Align to the left
        window.Y = 0; // Align to the top
    }
}
```
- Initialization: The InitializeComponent method initializes the components defined in App.xaml.
- Main Page: The MainPage is set to an instance of AppShell, which acts as the navigation container.
- Window Creation: The CreateWindow method allows for custom behavior upon window creation, such as adjusting its size to fit the display.

### AppShell.xaml
The AppShell.xaml file defines the navigation structure of the application using the Shell component, which simplifies the navigation experience in a MAUI application.
```csharp
<Shell
    x:Class="PocMauiApp.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:PocMauiApp.View"
    Shell.FlyoutBehavior="Disabled"
    Title="PocMauiApp">

    <ShellContent
        ContentTemplate="{DataTemplate local:BlogListPage}"
        Route="MainPage" />
</Shell>
```
- Shell Structure: The Shell element defines the overall navigation structure of the application.
- Flyout Behavior: Set to Disabled, meaning there is no flyout menu, and navigation is handled directly through the pages defined.
- Shell Content: ShellContent defines the main content of the application, using a DataTemplate to load the BlogListPage as the initial page.

### AppShell.xaml.cs
The code-behind for AppShell.xaml is defined in AppShell.xaml.cs, where you can add logic related to navigation or application behavior within the shell context.
```csharp
namespace PocMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
    }
}
```
- Initialization: Similar to App.xaml.cs, InitializeComponent initializes the components defined in AppShell.xaml.

**To run your MAUI application**, ensure you have the desired platform configured in your development environment. You can select the target platform (e.g., Android, iOS, Windows) in Visual Studio from the toolbar dropdown or use the command line with commands like dotnet build -t:Run -f net8.0-android for Android, -f net8.0-ios for iOS, or -f net8.0-windows for Windows, ensuring you have the necessary SDKs and emulators installed for each platform.  

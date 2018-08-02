Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Media.Animation
Imports Microsoft.Extensions.DependencyInjection
Imports Weather.ViewModels
Imports Weather.Services

Class Application

    Private _container As IServiceProvider
    Friend ReadOnly Property Container As IServiceProvider
        Get
            Return _container
        End Get
    End Property


    Private _mainWindow As New MainWindow
    Public ReadOnly Property ApplicationMainWindow As MainWindow
        Get
            Return _mainWindow
        End Get
    End Property


    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        _mainWindow = ApplicationMainWindow
        ApplicationMainWindow.Show()

        Dim navigationService = _container.GetRequiredService(Of INavigationService)()
        navigationService.NavigateTo(Of MainViewModel)()
    End Sub


    ''' <summary>
    ''' Initializes the singleton application object. This is the first line of authored code
    ''' executed, and as such is the logical equivalent of main() or WinMain().
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Dim services As New ServiceCollection

        ' Services
        services.AddSingleton(Of INavigationService, Services.NavigationService)
        services.AddSingleton(Of IMessageBus, MessageBus)

        services.AddTransient(Of IDialogService, Services.DialogService)
        services.AddTransient(Of ISettingsService, Services.SettingsService)
        services.AddTransient(Of ILocationService, Services.LocationService)
        services.AddTransient(Of IWeatherService, Services.WeatherService)

        ' ViewModels
        services.AddTransient(Of MainViewModel)

        _container = services.BuildServiceProvider

    End Sub


End Class

Imports System.Threading
Imports System.Windows.Input
Imports Weather.Services
Imports Microsoft.Extensions.Logging

Namespace ViewModels
    Public Class MainViewModel
        Inherits ViewModelBase

        Private ReadOnly _messageBus As IMessageBus
        Private ReadOnly _dialogService As IDialogService
        Private ReadOnly _navigationService As INavigationService
        Private ReadOnly _locationService As ILocationService
        Private ReadOnly _settingsService As ISettingsService
        Private ReadOnly _weatherService As IWeatherService



#Region "Constructors"

        'Public Sub New()

        'End Sub

        Public Sub New(messageBus As IMessageBus,
                       dialogService As IDialogService,
                       navigationService As INavigationService,
                       logger As ILogger(Of MainViewModel),
                       locationService As ILocationService,
                       settingsService As ISettingsService,
                       weatherService As IWeatherService)

            _messageBus = messageBus
            _dialogService = dialogService
            _navigationService = navigationService
            _locationService = locationService
            _logger = logger

            _settingsService = settingsService
            _weatherService = weatherService

            _messageBus.Subscribe(Of StatusMessage)(Sub(message)
                                                        Status = message.Status
                                                        SubStatus = message.SubStatus
                                                        Progress = message.Progress
                                                    End Sub)

            WeatherSourcesViewModel = New WeatherSourcesViewModel(messageBus, dialogService, navigationService, settingsService)
        End Sub

#End Region

#Region "Properties"

        Public Property WeatherSourcesViewModel As New WeatherSourcesViewModel

        Public Property LocationViewModels As New ObservableCollection(Of LocationViewModel)


#Region "SubStatus"

        Dim _SubStatus As String
        Public Property SubStatus As String
            Get
                Return _SubStatus
            End Get
            Set(value As String)
                _SubStatus = value
                OnPropertyChanged("SubStatus")
            End Set
        End Property

#End Region

#Region "Progress"

        Dim _Progress As Integer
        Public Property Progress As Integer
            Get
                Return _Progress
            End Get
            Set(value As Integer)
                _Progress = value
                OnPropertyChanged("Progress")
            End Set
        End Property

#End Region

#End Region

#Region "Commands"

#Region "SettingsCommand"
        Dim _SettingsCommand As ICommand
        Public ReadOnly Property SettingsCommand As ICommand
            Get
                If _SettingsCommand Is Nothing Then
                    _SettingsCommand = New Commands.RelayCommand(AddressOf ExecuteSettings, AddressOf CanExecuteSettings)
                End If

                Return _SettingsCommand
            End Get
        End Property

        Private Function CanExecuteSettings() As Boolean
            Return True
        End Function

        Private Sub ExecuteSettings()

            _navigationService.NavigateTo(Of SettingsViewModel)()
        End Sub

#End Region

#Region "AddLocationCommand"
        Dim _AddLocationCommand As ICommand
        Public ReadOnly Property AddLocationCommand As ICommand
            Get
                If _AddLocationCommand Is Nothing Then
                    _AddLocationCommand = New Commands.RelayCommand(AddressOf ExecuteAddLocation, AddressOf CanExecuteAddLocation)
                End If

                Return _AddLocationCommand
            End Get
        End Property

        Private Function CanExecuteAddLocation() As Boolean
            Return True
        End Function

        Private Sub ExecuteAddLocation()
            _navigationService.NavigateTo(Of AddWeatherSourceViewModel)()
        End Sub

#End Region

#End Region

#Region "Methods"

        ''' <summary>
        ''' Overridden. Method that is invoked when the view has loaded.
        ''' </summary>
        ''' <param name="parameter">Optional. A parameter that can be passed into the view model as the time of initialization.</param>
        ''' <remarks>
        ''' This is the trunk logic for the view model. So, the try/catch will go here.
        ''' </remarks>
        Public Overrides Async Function InitializeAsync(Optional parameter As Object = Nothing) As Task
            Dim locations As IEnumerable(Of Models.Location) = Nothing


            Using _logger.BeginScope("InitializeAsync")
                Try
                    If _IsIntilizing Then
                        _logger.LogInformation("ViewModel is Initializing. Bailing...")
                        Return
                    Else
                        _cancellationTokenSource = New CancellationTokenSource()

                        Using _logger.BeginScope("Locations")
                            ' Get saved locations
                            locations = Await GetLocationsAsync()
                            If locations Is Nothing Then
                                ' Get the current location
                                Dim currentLocation As Models.GeoCoordinate = Await GetCurrentLocationAsync()
                                If currentLocation Is Nothing Then
                                    ' Ask user to enter a location.
                                    _logger.LogDebug("Navigating to AddWeatherSourceViewModel")
                                    _navigationService.NavigateTo(Of AddWeatherSourceViewModel)()
                                    Return
                                End If

                            End If
                        End Using
                        _IsIntilizing = False
                    End If
                Catch ex As Exception
                    _logger.LogError("There was an error: {0}", ex.Message)
                End Try
            End Using
        End Function

        Private Async Function GetLocationsAsync() As Task(Of IEnumerable(Of Models.Location))
            Dim locations As IEnumerable(Of Models.Location) = Nothing

            Using _logger.BeginScope("Saved Locations")
                Try
                    _logger.LogInformation("Getting locations...")
                    locations = Await _settingsService.GetLocationsAsync(_cancellationTokenSource.Token)
                    If locations Is Nothing Then _logger.LogInformation("There are no saved locations")
                Catch ex As Exception
                    _logger.LogError("There was an error: {0}", ex.Message)
                End Try
            End Using

            Return locations
        End Function

        Private Async Function GetCurrentLocationAsync() As Task(Of IEnumerable(Of Models.GeoCoordinate))
            Dim location As Models.Location = Nothing

            Using _logger.BeginScope("Current Location")
                Try
                    _logger.LogInformation("Getting Current Location...")
                    Dim currentGeoCoordinate As Models.GeoCoordinate = Await _settingsService.GetCurrentLocationAsync(_cancellationTokenSource.Token)
                    If _cancellationTokenSource.Token.IsCancellationRequested Then Return Nothing
                    If currentGeoCoordinate Is Nothing Then
                        _logger.LogWarning("Application cannot retrieve current geographical coordinate.")
                        Return Nothing
                    Else
                        _logger.LogDebug("Retrieved geographical coordinate: {0}:{1}", currentGeoCoordinate.Latitude, currentGeoCoordinate.Longitude)
                    End If

                    _logger.LogInformation("Getting location by latitude, longitude", currentGeoCoordinate.Latitude, currentGeoCoordinate.Longitude)

                    location = Await _locationService.GetLocationByLatitudeLongitudeAsync(currentGeoCoordinate.Latitude, currentGeoCoordinate.Longitude, 3, _cancellationTokenSource.Token)
                    If _cancellationTokenSource.IsCancellationRequested Then Return Nothing
                    If location Is Nothing Then
                        _logger.LogWarning("Application cannot retrieve current location.")
                        Return Nothing
                    Else
                        _logger.LogDebug("Retrieved location: {0}", location.Address.DisplayString)
                    End If

                Catch ex As Exception
                    _logger.LogError("There was an error: {0}", ex.Message)
                End Try
            End Using

            Return location
        End Function
#End Region



    End Class
End Namespace

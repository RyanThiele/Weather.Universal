Imports System.Threading
Imports Weather.Models
Imports Weather.Services

Public Class SettingsService
    Implements ISettingsService

    Private ReadOnly _messageBus As IMessageBus
    Private Const RESET_STATUS As String = "Resetting Data. This could take a while depending on your phone speed."
    Private Const METAR_STATIONS_ADDRESS As String = "https://www.aviationweather.gov/static/adds/metars/stations.txt"
    'Private _settings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(messageBus As IMessageBus)
        _messageBus = messageBus
    End Sub

#End Region


    Public Function ResetDatabaseAsync() As Task Implements ISettingsService.ResetDatabaseAsync
        Throw New NotImplementedException()
    End Function

    Public Function RefreshLocationsAsync(deleteExisting As Boolean) As Task Implements ISettingsService.RefreshLocationsAsync
        Throw New NotImplementedException()
    End Function

    Public Function RefreshStationsAsync(deleteExisting As Boolean) As Task Implements ISettingsService.RefreshStationsAsync
        Throw New NotImplementedException()
    End Function

    Public Function SetSelectedLocationsAsync(locations As IEnumerable(Of Location)) As Task Implements ISettingsService.SetSelectedLocationsAsync
        Throw New NotImplementedException()
    End Function

    Public Function GetSelectedLocationsAsync() As Task(Of IEnumerable(Of Location)) Implements ISettingsService.GetSelectedLocationsAsync
        Throw New NotImplementedException()
    End Function

    Public Function GetCurrentLocationAsync(token As CancellationToken) As Task(Of GeoCoordinate) Implements ISettingsService.GetCurrentLocationAsync
        Throw New NotImplementedException()
    End Function



End Class

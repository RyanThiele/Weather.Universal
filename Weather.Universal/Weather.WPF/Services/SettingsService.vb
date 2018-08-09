Imports Weather.Models
Imports Weather.Services
Imports System.Threading

Namespace Services

    Public Class SettingsService
        Implements ISettingsService
        Public Function GetCurrentLocationAsync(token As CancellationToken) As Task(Of Models.GeoCoordinate) Implements ISettingsService.GetCurrentLocationAsync
            Return Task.Delay(0)
        End Function

        Public Async Function GetLocationsAsync(token As CancellationToken) As Task(Of IEnumerable(Of Location)) Implements ISettingsService.GetLocationsAsync
            Await Task.Delay(0)
            Return Nothing
        End Function

        Public Function RefreshLocationsAsync(deleteExisting As Boolean) As Task Implements ISettingsService.RefreshLocationsAsync
            Throw New NotImplementedException()
        End Function

        Public Function RefreshStationsAsync(deleteExisting As Boolean) As Task Implements ISettingsService.RefreshStationsAsync
            Throw New NotImplementedException()
        End Function

        Public Function ResetDatabaseAsync() As Task Implements ISettingsService.ResetDatabaseAsync
            Throw New NotImplementedException()
        End Function

        Public Function SetSelectedLocationsAsync(locations As IEnumerable(Of Location)) As Task Implements ISettingsService.SetSelectedLocationsAsync
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
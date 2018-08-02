Imports Weather.Models
Imports Weather.Services
Imports Windows.Devices.Geolocation
Imports System.Threading

Namespace Services

    Public Class SettingsService
        Implements ISettingsService
        Public Async Function GetCurrentLocationAsync(token As CancellationToken) As Task(Of Models.GeoCoordinate) Implements ISettingsService.GetCurrentLocationAsync
            Dim geolocation As New Geolocator
            Dim result As Geoposition = Await geolocation.GetGeopositionAsync.AsTask

            If result Is Nothing Then
                Return Nothing
            Else
                Return New Models.GeoCoordinate With {
                    .Latitude = CType(result.Coordinate.Latitude, Decimal),
                    .Longitude = CType(result.Coordinate.Longitude, Decimal)}
            End If
        End Function

        Public Async Function GetSelectedLocationsAsync() As Task(Of IEnumerable(Of Location)) Implements ISettingsService.GetSelectedLocationsAsync
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
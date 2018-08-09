Imports System.Threading
Imports Weather.Models
Imports Weather.Services

Namespace Services

    Public Class LocationService
        Implements ILocationService

        Public Function GetLocationByCityAndStateAsync(city As String, state As String, numberOfStations As Integer, token As CancellationToken) As Task(Of Location) Implements ILocationService.GetLocationByCityAndStateAsync
            Throw New NotImplementedException()
        End Function

        Public Function GetLocationByLatitudeLongitudeAsync(latitude As Double, longitude As Double, numberOfStations As Integer, token As CancellationToken) As Task(Of Location) Implements ILocationService.GetLocationByLatitudeLongitudeAsync
            Throw New NotImplementedException()
        End Function

        Public Function GetLocationByPostalCodeAsync(postalCode As String, numberOfStations As Integer, token As CancellationToken) As Task(Of Location) Implements ILocationService.GetLocationByPostalCodeAsync
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

Imports System.Threading
Imports Weather.Models
Imports Weather.Services

Namespace Services

    Public Class LocationService
        Implements ILocationService

        Public Function GetLocationByLatitudeLongitudeAsync(latitude As Decimal, longitude As Decimal, numberOfStations As Integer, token As CancellationToken) As Task(Of Location) Implements ILocationService.GetLocationByLatitudeLongitudeAsync
            Throw New NotImplementedException()
        End Function

        Public Function GetLocationByPostalCodeAsync(postalCode As String, numberOfStations As Integer, token As CancellationToken) As Task(Of Location) Implements ILocationService.GetLocationByPostalCodeAsync
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

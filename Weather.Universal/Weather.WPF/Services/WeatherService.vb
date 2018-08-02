Imports System.Threading
Imports Weather.Models
Imports Weather.Services

Namespace Services
    Public Class WeatherService
        Implements IWeatherService

        Public Function GetCurrentObservationByIcaoAsync(icao As String, token As CancellationToken) As Task(Of CurrentObservations) Implements IWeatherService.GetCurrentObservationByIcaoAsync
            Throw New NotImplementedException()
        End Function

        Public Function GetWeatherStationsByPostalCodeAsync(postalCode As String, numberOfStations As Integer, token As CancellationToken) As Task(Of IEnumerable(Of WeatherStation)) Implements IWeatherService.GetWeatherStationsByPostalCodeAsync
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

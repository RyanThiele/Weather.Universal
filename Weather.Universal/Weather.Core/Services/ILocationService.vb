Imports System.Threading
Imports Weather.Models

Namespace Services

    Public Interface ILocationService
        Function GetLocationByPostalCodeAsync(postalCode As String, numberOfStations As Integer, token As CancellationToken) As Task(Of Models.Location)
        Function GetLocationByCityAndStateAsync(city As String, state As String, numberOfStations As Integer, token As CancellationToken) As Task(Of Models.Location)
        Function GetLocationByLatitudeLongitudeAsync(latitude As Double, longitude As Double, numberOfStations As Integer, token As CancellationToken) As Task(Of Models.Location)
        Function CreateLocationAsync(location As Location) As Task
    End Interface

End Namespace

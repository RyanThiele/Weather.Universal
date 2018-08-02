Imports System.Threading.Tasks
Imports System.Threading

Namespace Services

    Public Interface ISettingsService
        Function ResetDatabaseAsync() As Task
        Function RefreshLocationsAsync(deleteExisting As Boolean) As Task
        Function RefreshStationsAsync(deleteExisting As Boolean) As Task
        Function SetSelectedLocationsAsync(locations As IEnumerable(Of Models.Location)) As Task
        Function GetSelectedLocationsAsync() As Task(Of IEnumerable(Of Models.Location))
        Function GetCurrentLocationAsync(token As CancellationToken) As Task(Of Models.GeoCoordinate)

    End Interface

End Namespace

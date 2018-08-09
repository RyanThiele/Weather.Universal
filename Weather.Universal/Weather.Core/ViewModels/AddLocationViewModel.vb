Imports System.Threading
Imports System.Windows.Input
Imports Weather.Services
Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions

Namespace ViewModels

    Public Class AddLocationViewModel
        Inherits EditViewModelBase

        Private ReadOnly _messageBus As IMessageBus
        Private ReadOnly _dialogService As IDialogService
        Private ReadOnly _navigationService As INavigationService
        'Private ReadOnly _weatherService As IWeatherService
        'Private ReadOnly _settingsService As ISettingsService

        Private ReadOnly _locationService As ILocationService
        Private _searchCancelTokenSource As CancellationTokenSource

        Private Const EMPTY_SEARCH_STRING_ERROR_MSG As String = "There must be some place you want to search!"

#Region "Constructors"

        Public Sub New()

        End Sub

        Public Sub New(messageBus As IMessageBus,
                   dialogService As IDialogService,
                   navigationService As INavigationService,
                   locationService As ILocationService)

            _messageBus = messageBus
            _dialogService = dialogService
            _navigationService = navigationService
            _locationService = locationService
            '_weatherService = weatherService
            '_settingsService = settingsService

#If DEBUG Then
            PostalCode = "46845"
#End If
        End Sub

#End Region

#Region "Properties"

#Region "PostalCode"

        Dim _PostalCode As String
        Public Property PostalCode As String
            Get
                Return _PostalCode
            End Get
            Set(value As String)
                _PostalCode = value
                OnPropertyChanged("PostalCode")
            End Set
        End Property

#End Region

#Region "SearchString"

        Dim _SearchString As String
        Public Property SearchString As String
            Get
                Return _SearchString
            End Get
            Set
                _SearchString = Value
                OnPropertyChanged("SearchString")
            End Set
        End Property

#End Region

#Region "IsSearching"

        Dim _IsSearching As Boolean
        Public Property IsSearching As Boolean
            Get
                Return _IsSearching
            End Get
            Set(value As Boolean)
                _IsSearching = value
                OnPropertyChanged("IsSearching")
            End Set
        End Property

#End Region

#End Region

#Region "Commands"

#Region "SearchCommand"

        Dim _SearchCommand As ICommand
        Public ReadOnly Property SearchCommand As ICommand
            Get
                If _SearchCommand Is Nothing Then
                    _SearchCommand = New Commands.RelayCommand(AddressOf ExecuteSearch, AddressOf CanExecuteSearch)
                End If

                Return _SearchCommand
            End Get
        End Property

        Private Function CanExecuteSearch() As Boolean
            Return Not String.IsNullOrWhiteSpace(_SearchString)
        End Function

        Private Async Sub ExecuteSearch()
            IsSearching = True
            Await PerformSearchAsync()
            IsSearching = False

            'Try
            '    Status = "Searching..."
            '    If String.IsNullOrWhiteSpace(SearchString) Then
            '        Return
            '    End If

            '    ' search for the location
            '    _searchCancelTokenSource = New CancellationTokenSource
            '    Dim location As Models.Location = Await _locationService.GetLocationByPostalCodeAsync(PostalCode, 3, _searchCancelTokenSource.Token)

            '    ' location is not found. notify user and bail.
            '    If location Is Nothing Then
            '        Status = "Could not find a weather station for postal code: " & PostalCode & ". Please try again."
            '        Return
            '    End If

            '    ' check if location already in cache.
            '    Dim locations As IEnumerable(Of Models.Location) = Await _settingsService.GetLocationsAsync(New CancellationToken)
            '    If locations IsNot Nothing Then
            '        Dim existingLocation As Models.Location = locations.Where(Function(o) o.Address.PostalCode.Equals(PostalCode)).SingleOrDefault
            '        ' location already exits, notify user and bail.
            '        If existingLocation IsNot Nothing Then
            '            Status = PostalCode & " already exists. Please try again."
            '            Return
            '        End If
            '    End If

            '    ' if we get to here, all conditions are met to enter the location.
            '    ' ask the user if they want to add the location.
            '    If _dialogService.ShowYesNoDialog("Weather Station Found!", "Found a station for '" & PostalCode & ": " & location.WeatherStations.First.Name & ". Do you want to use this station?") Then
            '        Dim locationList As New List(Of Models.Location)
            '        If locations IsNot Nothing Then locationList = New List(Of Models.Location)(locations)
            '        locationList.Add(location)
            '        Await _settingsService.SetSelectedLocationsAsync(locationList)
            '    End If

            '    Status = "Added " & PostalCode & " to cache."

            'Catch ex As Exception
            '    Status = "There was a problem with the search: " & ex.Message
            'End Try

        End Sub

#End Region

#Region "CancelCommand"
        Dim _CancelCommand As ICommand
        Public ReadOnly Property CancelCommand As ICommand
            Get
                If _CancelCommand Is Nothing Then
                    _CancelCommand = New Commands.RelayCommand(AddressOf ExecuteCancel, AddressOf CanExecuteCancel)
                End If

                Return _CancelCommand
            End Get
        End Property

        Private Function CanExecuteCancel() As Boolean
            Return True
        End Function

        Private Sub ExecuteCancel()
            If Not IsSearching Then Return
            If _searchCancelTokenSource Is Nothing OrElse Not _searchCancelTokenSource.IsCancellationRequested Then Return
            _searchCancelTokenSource.Cancel()
        End Sub

#End Region


#End Region

#Region "Methods"

        Private Function CheckSearchTokenStatus() As Boolean
            If _searchCancelTokenSource.IsCancellationRequested Then
                Status = "User canceled the search."
            End If

            Return _searchCancelTokenSource.IsCancellationRequested
        End Function

        Private Async Function PerformSearchAsync() As Task
            ' Sanity check
            If String.IsNullOrWhiteSpace(_SearchString) Then
                AddError(EMPTY_SEARCH_STRING_ERROR_MSG, "SearchString")
                Return
            Else
                RemoveError(EMPTY_SEARCH_STRING_ERROR_MSG, "SearchString")
            End If

            SearchString = SearchString.Trim

            ' Determine what the user is searching for.
            _cancellationTokenSource = New CancellationTokenSource

            ' Get the Location.
            Dim location As Models.Location = Await GetLocationAsync()
            If location Is Nothing Then
                ' location is not found. notify user and bail.
                Status = "Could not find a weather station for postal code: " & PostalCode & ". Please try again."
                Return
            End If

            If _dialogService.ShowYesNoDialog("Location Found!", "There was a location found: " & location.Address.DisplayString & Environment.NewLine &
                                           "Do you want to use this location?") Then
                Dim locationList As New List(Of Models.Location)
            End If
        End Function

        Private Async Function GetLocationAsync() As Task(Of Models.Location)
            Dim location As Models.Location = Nothing

            Dim postalCodeCountry As Countries? = SearchString.PostalCodeRegionFromPostalCode
            If postalCodeCountry.HasValue Then
                location = Await _locationService.GetLocationByPostalCodeAsync(SearchString, 3, _cancellationTokenSource.Token)
                Return location
            End If

            Dim latLong As LatLong? = SearchString.ToLatLong
            If latLong.HasValue Then
                location = Await _locationService.GetLocationByLatitudeLongitudeAsync(latLong.Value.Latitude, latLong.Value.Longitude, 3, _cancellationTokenSource.Token)
                Return location
            End If

            Dim cityAndRegion As CityAndRegion? = SearchString.ToCityAndRegion
            If cityAndRegion.HasValue Then
                location = Await _locationService.GetLocationByCityAndStateAsync(cityAndRegion.Value.City, cityAndRegion.Value.Region, 3, _cancellationTokenSource.Token)
                Return location
            End If

            Return location
        End Function
#End Region

    End Class

End Namespace

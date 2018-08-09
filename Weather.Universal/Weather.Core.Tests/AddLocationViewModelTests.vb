Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class AddLocationViewModelTests
    Inherits UnitTestBase


    <TestMethod()>
    Public Sub EmptySearchString_ShouldNotPerformSearch()
        ' Prepare
        Dim isUsingLocationService As Boolean = False
        Dim locationService As New Services.Fakes.StubILocationService
        With locationService
            .GetLocationByCityAndStateAsyncStringStringInt32CancellationToken = Async Function()
                                                                                    isUsingLocationService = True
                                                                                    Await Task.Delay(0)
                                                                                    Return Nothing
                                                                                End Function
            .GetLocationByLatitudeLongitudeAsyncDoubleDoubleInt32CancellationToken = Async Function()
                                                                                         isUsingLocationService = True
                                                                                         Await Task.Delay(0)
                                                                                         Return Nothing
                                                                                     End Function
            .GetLocationByPostalCodeAsyncStringInt32CancellationToken = Async Function()
                                                                            isUsingLocationService = True
                                                                            Await Task.Delay(0)
                                                                            Return Nothing
                                                                        End Function
        End With
        Dim viewModel As New ViewModels.AddLocationViewModel(CreateMessageBus, CreateDialogService, CreateNavigationService, locationService)

        ' Execute
        viewModel.SearchCommand.Execute(Nothing)

        ' Assert
        Assert.IsFalse(isUsingLocationService, "AddLocationViewModel is still trying to search when an empty search string.")
    End Sub

    <TestMethod()>
    Public Sub USZipCodeSearchString_ShouldPerformSearchWithPostalCodeQuery()
        ' Prepare
        Dim isUsingLocationServiceByPostalCode As Boolean = False
        Dim isUsingLocationServiceByCityState As Boolean = False
        Dim isUsingLocationServiceByLatLon As Boolean = False
        Dim locationService As New Services.Fakes.StubILocationService
        With locationService
            .GetLocationByCityAndStateAsyncStringStringInt32CancellationToken = Async Function()
                                                                                    isUsingLocationServiceByCityState = True
                                                                                    Await Task.Delay(0)
                                                                                    Return Nothing
                                                                                End Function

            .GetLocationByLatitudeLongitudeAsyncDoubleDoubleInt32CancellationToken = Async Function()
                                                                                         isUsingLocationServiceByLatLon = True
                                                                                         Await Task.Delay(0)
                                                                                         Return Nothing
                                                                                     End Function

            .GetLocationByPostalCodeAsyncStringInt32CancellationToken = Async Function()
                                                                            isUsingLocationServiceByPostalCode = True
                                                                            Await Task.Delay(0)
                                                                            Return Nothing
                                                                        End Function
        End With
        Dim viewModel As New ViewModels.AddLocationViewModel(CreateMessageBus, CreateDialogService, CreateNavigationService, locationService)
        viewModel.SearchString = "12345"

        ' Execute
        viewModel.SearchCommand.Execute(Nothing)

        ' Assert
        Assert.IsFalse(isUsingLocationServiceByCityState, "AddLocationViewModel is using City/State when using a US Postal Code search string.")
        Assert.IsFalse(isUsingLocationServiceByLatLon, "AddLocationViewModel is using Latitude/Longitude when using a US Postal Code search string.")
        Assert.IsTrue(isUsingLocationServiceByPostalCode, "AddLocationViewModel is not using Postal Code when using a US Postal Code search string.")

    End Sub

    <TestMethod()>
    Public Sub CityStateSearchString_ShouldPerformSearchWithPostalCodeQuery()
        ' Prepare
        Dim isUsingLocationServiceByPostalCode As Boolean = False
        Dim isUsingLocationServiceByCityState As Boolean = False
        Dim isUsingLocationServiceByLatLon As Boolean = False
        Dim locationService As New Services.Fakes.StubILocationService
        With locationService
            .GetLocationByCityAndStateAsyncStringStringInt32CancellationToken = Async Function()
                                                                                    isUsingLocationServiceByCityState = True
                                                                                    Await Task.Delay(0)
                                                                                    Return Nothing
                                                                                End Function

            .GetLocationByLatitudeLongitudeAsyncDoubleDoubleInt32CancellationToken = Async Function()
                                                                                         isUsingLocationServiceByLatLon = True
                                                                                         Await Task.Delay(0)
                                                                                         Return Nothing
                                                                                     End Function

            .GetLocationByPostalCodeAsyncStringInt32CancellationToken = Async Function()
                                                                            isUsingLocationServiceByPostalCode = True
                                                                            Await Task.Delay(0)
                                                                            Return Nothing
                                                                        End Function
        End With

        Dim viewModel As New ViewModels.AddLocationViewModel(CreateMessageBus, CreateDialogService, CreateNavigationService, locationService)
        viewModel.SearchString = "City, State"

        ' Execute
        viewModel.SearchCommand.Execute(Nothing)

        ' Assert
        Assert.IsTrue(isUsingLocationServiceByCityState, "AddLocationViewModel is not using City/State when using a <City>, <State> search string.")
        Assert.IsFalse(isUsingLocationServiceByLatLon, "AddLocationViewModel is using Latitude/Longitude when using a <City>, <State> search string.")
        Assert.IsFalse(isUsingLocationServiceByPostalCode, "AddLocationViewModel is using Postal Code when using a <City>, <State> search string.")
    End Sub

    <TestMethod()>
    Public Sub LatLonSearchString_ShouldPerformSearchWithPostalCodeQuery()
        ' Prepare
        Dim isUsingLocationServiceByPostalCode As Boolean = False
        Dim isUsingLocationServiceByCityState As Boolean = False
        Dim isUsingLocationServiceByLatLon As Boolean = False
        Dim locationService As New Services.Fakes.StubILocationService
        With locationService
            .GetLocationByCityAndStateAsyncStringStringInt32CancellationToken = Async Function()
                                                                                    isUsingLocationServiceByCityState = True
                                                                                    Await Task.Delay(0)
                                                                                    Return Nothing
                                                                                End Function

            .GetLocationByLatitudeLongitudeAsyncDoubleDoubleInt32CancellationToken = Async Function()
                                                                                         isUsingLocationServiceByLatLon = True
                                                                                         Await Task.Delay(0)
                                                                                         Return Nothing
                                                                                     End Function

            .GetLocationByPostalCodeAsyncStringInt32CancellationToken = Async Function()
                                                                            isUsingLocationServiceByPostalCode = True
                                                                            Await Task.Delay(0)
                                                                            Return Nothing
                                                                        End Function
        End With
        Dim viewModel As New ViewModels.AddLocationViewModel(CreateMessageBus, CreateDialogService, CreateNavigationService, locationService)
        viewModel.SearchString = "123.45, 123.45"

        ' Execute
        viewModel.SearchCommand.Execute(Nothing)

        ' Assert
        Assert.IsFalse(isUsingLocationServiceByCityState, "AddLocationViewModel is using City/State when using a <Latitude>, <Longitude> search string.")
        Assert.IsTrue(isUsingLocationServiceByLatLon, "AddLocationViewModel is not using Latitude/Longitude when using a <Latitude>, <Longitude> search string.")
        Assert.IsFalse(isUsingLocationServiceByPostalCode, "AddLocationViewModel is using Postal Code when using a <Latitude>, <Longitude> search string.")
    End Sub


End Class
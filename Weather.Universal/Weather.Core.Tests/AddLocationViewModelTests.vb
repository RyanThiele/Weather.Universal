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

End Class
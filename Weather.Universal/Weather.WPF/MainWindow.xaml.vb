Imports Microsoft.Extensions.DependencyInjection
Imports Weather.Services

Class MainWindow

    Private _navigationService As INavigationService

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        _navigationService = CType(Application.Current, Application).Container.GetRequiredService(Of INavigationService)
    End Sub

    Private Sub PreviousButton_Click(sender As Object, e As RoutedEventArgs) Handles PreviousButton.Click
        _navigationService.NavigatePrevious()
    End Sub

    Private Sub NextButton_Click(sender As Object, e As RoutedEventArgs) Handles NextButton.Click
        _navigationService.NavigateNext()
    End Sub

End Class

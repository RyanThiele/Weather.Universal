Imports System.ComponentModel
Imports System.Reflection
Imports System.Globalization
Imports System.Linq
Imports System.Threading
Imports Weather.ViewModels
Imports Weather.Services
Imports Microsoft.Extensions.DependencyInjection

Namespace Services

    Public Class NavigationService
        Implements INavigationService

        Private _history As New List(Of Type)
        Private _currentIndex As Integer = 0

        Private ReadOnly Property Application As Application
            Get
                Return DirectCast(Application.Current, Application)
            End Get
        End Property

        Private Sub UpdateNavigationButtons()
            If _history.Count = 0 Then
                Application.ApplicationMainWindow.PreviousButton.Visibility = Visibility.Collapsed
            Else
                Application.ApplicationMainWindow.PreviousButton.Visibility = Visibility.Visible
            End If

            If _currentIndex < _history.Count - 1 Then
                Application.ApplicationMainWindow.NextButton.Visibility = Visibility.Visible
            Else
                Application.ApplicationMainWindow.NextButton.Visibility = Visibility.Collapsed
            End If
        End Sub

        Public Sub RemoveBackStack() Implements INavigationService.RemoveBackStack
            _history.Clear()
            UpdateNavigationButtons()
        End Sub

        Public Sub RemoveLastFromBackStack() Implements INavigationService.RemoveLastFromBackStack
            If _history.Count > 0 Then
                _history.RemoveAt(_history.Count - 1)
                UpdateNavigationButtons()
            End If
        End Sub

        Public Async Sub NavigatePrevious() Implements INavigationService.NavigatePrevious
            If _currentIndex = 0 Then Return
            _currentIndex -= 1

            Dim t As Type = _history(_currentIndex)
            If t IsNot Nothing Then
                Dim viewModel As ViewModelBase = Application.Container.GetRequiredService(t)
                Application.ApplicationMainWindow.MainContent.Content = viewModel
                UpdateNavigationButtons()
                Await viewModel.InitializeAsync
            End If
        End Sub

        Public Async Sub NavigateNext() Implements INavigationService.NavigateNext
            If _currentIndex = _history.Count - 1 Then Return
            _currentIndex += 1

            Dim t As Type = _history(_currentIndex)
            If t IsNot Nothing Then
                Dim viewModel As ViewModelBase = Application.Container.GetRequiredService(t)
                Application.ApplicationMainWindow.MainContent.Content = viewModel
                RemoveLastFromBackStack()
                Await viewModel.InitializeAsync
            End If
        End Sub

        Public Async Sub NavigateTo(Of TViewModel As ViewModelBase)(Optional addToHistory As Boolean = True) Implements INavigationService.NavigateTo
            Dim viewModel As ViewModelBase = Application.Container.GetRequiredService(GetType(TViewModel))
            Application.ApplicationMainWindow.MainContent.Content = viewModel
            If addToHistory Then
                _history.Add(GetType(TViewModel))
                _currentIndex = _history.Count - 1
            End If

            UpdateNavigationButtons()
            Await viewModel.InitializeAsync
        End Sub
    End Class


End Namespace

Imports System.Threading.Tasks
Imports Weather.ViewModels

Namespace Services

    Public Interface INavigationService
        Sub NavigatePrevious()
        Sub NavigateNext()
        Sub NavigateTo(Of TViewModel As ViewModelBase)(Optional addToHistory As Boolean = True)

        Sub RemoveLastFromBackStack()
        Sub RemoveBackStack()
    End Interface

End Namespace

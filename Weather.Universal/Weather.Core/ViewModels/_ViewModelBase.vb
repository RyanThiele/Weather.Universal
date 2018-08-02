Imports System.Linq
Imports System.Reflection
Imports System.Threading

Namespace ViewModels

    Public Class ViewModelBase
        Inherits ObservableObject

        Private Const APPMANIFESTNAME As String = "WMAppManifest.xml"
        Protected _IsIntilizing As Boolean

        Public Sub New()

        End Sub

        Public Overridable Async Function InitializeAsync(Optional parameter As Object = Nothing) As Task
            _IsIntilizing = True
            Await Task.Delay(0)
            _IsIntilizing = False
        End Function

#Region "Properties"

        Private _applicationTitle As String
        Public ReadOnly Property ApplicationTitle As String
            Get
                Return _applicationTitle
            End Get
        End Property

#Region "Title"

        Dim _Title As String
        Public Property Title As String
            Get
                Return _Title
            End Get
            Set(value As String)
                _Title = value
                OnPropertyChanged("Title")
            End Set
        End Property

#End Region

#Region "Status"

        Dim _Status As String
        Public Property Status As String
            Get
                Return _Status
            End Get
            Set(value As String)
                _Status = value
                OnPropertyChanged("Status")
            End Set
        End Property

#End Region


#End Region

    End Class

End Namespace

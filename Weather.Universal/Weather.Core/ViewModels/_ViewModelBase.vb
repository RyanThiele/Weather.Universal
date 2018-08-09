Imports System.Linq
Imports System.Reflection
Imports System.Threading
Imports Microsoft.Extensions.Logging

Namespace ViewModels

    Public Class ViewModelBase
        Inherits ObservableObject
        Implements IDisposable

        Private Const APPMANIFESTNAME As String = "WMAppManifest.xml"
        Protected _IsIntilizing As Boolean
        Protected _cancellationTokenSource As CancellationTokenSource
        Protected _logger As ILogger

        Public Sub New()

        End Sub

        Public Overridable Async Function InitializeAsync(Optional parameter As Object = Nothing) As Task
            _IsIntilizing = True
            Await Task.Delay(0)
            _IsIntilizing = False
        End Function

        Protected Function CheckIsCanceled() As Boolean
            If _cancellationTokenSource.IsCancellationRequested AndAlso _logger IsNot Nothing Then _logger.LogInformation("Transaction Canceled")

            Return _cancellationTokenSource.IsCancellationRequested
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    If _cancellationTokenSource IsNot Nothing Then _cancellationTokenSource.Cancel()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region

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

Imports System.Runtime.CompilerServices
Imports Microsoft.Extensions.Logging


Public Class TestContextLogger(Of T)
    Implements ILogger(Of T)


    Public Shared Function Instance(testContext As TestContext)
        Return New TestContextLogger(Of T)(testContext)
    End Function


    Private ReadOnly _name As String
    Private ReadOnly _testContext As TestContext
    Private ReadOnly _scopes As New List(Of String)


    Public Sub New(textContext As TestContext)
        _testContext = textContext
        _name = GetType(T).UnderlyingSystemType.Name
    End Sub

    Public Sub Log(Of TState)(logLevel As LogLevel, eventId As EventId, state As TState, exception As Exception, formatter As Func(Of TState, Exception, String)) Implements ILogger.Log
        Dim message = formatter(state, exception)
        If String.IsNullOrWhiteSpace(message) Then Return

        message = logLevel.ToString & ": " & String.Join(" => ", _scopes) & " => " & message
        If exception IsNot Nothing Then message += Environment.NewLine & Environment.NewLine & exception.ToString

        _testContext.WriteLine(message)
    End Sub

    Public Function BeginScope(Of TState)(state As TState) As IDisposable Implements ILogger.BeginScope

        Dim scope As String = state.ToString
        Dim scopeInstance As New Scope(scope)
        _scopes.Add(scope)
        AddHandler scopeInstance.Disposing, Sub(name)
                                                _scopes.Remove(name)
                                            End Sub
        Return scopeInstance
    End Function

    Public Function IsEnabled(logLevel As LogLevel) As Boolean Implements ILogger.IsEnabled
        Return logLevel <> LogLevel.None
    End Function


    Private Class Scope
        Implements IDisposable

        Private ReadOnly _name As String
        Friend Event Disposing(name As String)

        Friend Sub New(name As String)
            _name = name
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            RaiseEvent Disposing(_name)
        End Sub

    End Class
End Class

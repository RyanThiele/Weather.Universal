Namespace ViewModels

    Public Class EditViewModelBase
        Inherits ViewModelBase
        Implements INotifyDataErrorInfo


        Private Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged


        ' A dictionary to hold all the errors.
        Private ReadOnly _errors As New Dictionary(Of String, ObservableCollection(Of String))
        ''' <summary>
        ''' Returns a <see cref="Dictionary(Of String, ObservableCollection(Of String))"/> containing all the error messages.
        ''' </summary>
        ''' <returns>A <see cref="Dictionary(Of String, ObservableCollection(Of String))"/> containing all the error messages</returns>
        Public ReadOnly Property Errors As Dictionary(Of String, ObservableCollection(Of String))

            Get
                Return _errors
            End Get
        End Property


        Private ReadOnly _allErrors As New ObservableCollection(Of String)
        ''' <summary>
        ''' A collection of all the errors as strings.
        ''' </summary>
        ''' <returns>An <see cref="ObservableCollection(Of String)"/> containing all the error messages.</returns>
        Public ReadOnly Property AllErrors As ObservableCollection(Of String)
            Get
                Return _allErrors
            End Get
        End Property

        ''' <summary>
        ''' Adds an error message to the underlying collection using the given property name.
        ''' </summary>
        ''' <param name="errorMessage">The error message for the property.</param>
        ''' <param name="propertyName">The property that is invalid.</param>
        Protected Sub AddError(errorMessage As String, <CallerMemberName> Optional propertyName As String = Nothing)
            If Not _errors.ContainsKey(propertyName) Then
                _errors.Add(propertyName, New ObservableCollection(Of String))
            End If

            If Not _errors(propertyName).Contains(errorMessage) Then _errors(propertyName).Add(errorMessage)
            If Not _allErrors.Contains(errorMessage) Then _allErrors.Add(errorMessage)
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' Removed an error message to the underlying collection using the given property name, if it exists.
        ''' </summary>
        ''' <param name="errorMessage">The error message for the property.</param>
        ''' <param name="propertyName">The property that the error message relates to.</param>
        Protected Sub RemoveError(errorMessage As String, <CallerMemberName> Optional propertyName As String = Nothing)
            If _errors.ContainsKey(propertyName) AndAlso _errors(propertyName).Contains(errorMessage) Then
                _errors(propertyName).Remove(errorMessage)
                If _errors(propertyName).Count = 0 Then _errors.Remove(propertyName)
            End If

            If _allErrors.Contains(errorMessage) Then _allErrors.Remove(errorMessage)

            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' Implements <see cref="INotifyDataErrorInfo.GetErrors"/>.  Returns all the errors that are related to a given property.
        ''' </summary>
        ''' <param name="propertyName">The property name.</param>
        ''' <returns>An <see cref="IEnumerable"/> containing errors; Otherwise Nothing.</returns>
        Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
            If String.IsNullOrEmpty(propertyName) Then Return Nothing
            If Not _errors.ContainsKey(propertyName) Then Return Nothing
            Return _errors(propertyName)
        End Function

        ''' <summary>
        ''' Occurs when the error collections have changed.
        ''' </summary>
        ''' <param name="sender">The sender of the event.</param>
        ''' <param name="e">The <see cref="DataErrorsChangedEventArgs"/> containing the changed event.</param>
        Private Sub EditViewModelBase_ErrorsChanged(sender As Object, e As DataErrorsChangedEventArgs) Handles Me.ErrorsChanged
            OnPropertyChanged("AllErrors")
            OnPropertyChanged("Errors")
            OnPropertyChanged("HasErrors")
            OnPropertyChanged("IsValid")
        End Sub

        ''' <summary>
        ''' Implements <see cref="INotifyDataErrorInfo.HasErrors"/>. Returns whether or not the view model has errors.
        ''' </summary>
        ''' <returns>True if there are errors; Otherwise False.</returns>
        Private ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
            Get
                Return Errors.Count > 0
            End Get
        End Property

        ''' <summary>
        ''' Used to bind to a property or a command to become available when the view model contains not errors.
        ''' </summary>
        ''' <returns>True if valid; Otherwise False.</returns>
        Public ReadOnly Property IsValid As Boolean
            Get
                Return _errors.Count = 0
            End Get
        End Property

    End Class

End Namespace


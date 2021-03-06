﻿Imports System.Windows.Data

Namespace Converters

    Public Class CountToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            If value Is Nothing Then Return False

            Dim count As Integer = 0
            Dim isInverted As Boolean = False

            Integer.TryParse(value, count)
            Boolean.TryParse(parameter, isInverted)

            If isInverted Then
                Return If(count = 0, Visibility.Collapsed, Visibility.Visible)
            Else
                Return If(count = 0, Visibility.Visible, Visibility.Collapsed)
            End If

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException
        End Function
    End Class

End Namespace

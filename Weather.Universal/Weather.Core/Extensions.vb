Imports System.Runtime.CompilerServices
Imports System.Xml.Linq
Imports System.Globalization
Imports System.Xml
Imports System.Text.RegularExpressions

Friend Module Extensions

    Public Structure CityAndRegion
        Dim City As String
        Dim Region As String
    End Structure

    Public Structure LatLong
        Dim Latitude As Double
        Dim Longitude As Double
    End Structure

    Public Enum Countries
        UnitesStatesOfAmerica
        UnitedKingdom
        India
        Canada
    End Enum

    <Extension>
    Public Function ToDouble(s As String, Optional defaultValue As Double = 0) As Double
        Dim value As Double = defaultValue
        If Not String.IsNullOrWhiteSpace(s) Then Double.TryParse(s, value)
        Return value
    End Function

    <Extension>
    Public Function ToDecimal(s As String, Optional defaultValue As Decimal = 0) As Decimal
        Dim value As Decimal = defaultValue
        If Not String.IsNullOrWhiteSpace(s) Then Decimal.TryParse(s, value)
        Return value
    End Function

    <Extension>
    Public Function ToInteger(s As String, Optional defaultValue As Integer = 0) As Integer
        Dim value As Integer = defaultValue
        If Not String.IsNullOrWhiteSpace(s) Then Integer.TryParse(s, value)
        Return value
    End Function

    <Extension>
    Public Function ValueIfExists(element As XElement) As String
        If element IsNot Nothing Then
            Return element.Value
        Else
            Return Nothing
        End If
    End Function

    <Extension>
    Public Function ValueIfExists(attribute As XAttribute) As String
        If attribute IsNot Nothing Then
            Return attribute.Value
        Else
            Return Nothing
        End If
    End Function

    '<Extension>
    'Public Function FromXmlDateTimeToDateTime(s As String, Optional defaultValue As DateTime? = Nothing) As DateTime
    '    If Not defaultValue.HasValue Then defaultValue = New DateTime
    '    Dim value As DateTime? = XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local)
    '    If value Is Nothing Then value = defaultValue.Value
    '    Return value
    'End Function

    <Extension>
    Public Function FromRfc22StringToDateTime(s As String, Optional defaultValue As DateTime? = Nothing) As DateTime
        Dim provider As CultureInfo = CultureInfo.InvariantCulture
        '                              Sun, 08 Jul 2018 02:55:00 -0400
        '                              Sat, 07 Jul 2018 17:48:00 -0400
        Return DateTime.ParseExact(s, "ddd, dd MMM yyyy HH:mm:ss zzz", provider)
    End Function

    <Extension>
    Public Function PostalCodeRegionFromPostalCode(s As String) As Countries?
        If Regex.IsMatch(s, "^\d{3}\s?\d{3}$") Then
            'Indian style pincodes / postal codes used by the Indian postal departments which are 6 digits long and may have space after the 3rd digit
            Return Countries.India
        ElseIf Regex.IsMatch(s, "^[A-Za-z]{1,2}[\d]{1,2}([A-Za-z])?\s?[\d][A-Za-z]{2}$") Then
            ' UK Postal Codes
            Return Countries.UnitedKingdom
        ElseIf Regex.IsMatch(s, "^([0-9]{5})(?:[-\s]*([0-9]{4}))?$") Then
            ' US Postal Codes
            Return Countries.UnitesStatesOfAmerica
        ElseIf Regex.IsMatch(s, "^([A-Z][0-9][A-Z])\s*([0-9][A-Z][0-9])$") Then
            ' Canada
            Return Countries.Canada
        Else
            Return Nothing
        End If

    End Function


    <Extension>
    Public Function ToLatLong(s As String) As LatLong?
        If Not s.Contains(","c) Then Return Nothing

        Dim parts() As String = s.Split(","c)
        If parts.Length < 2 Then Return Nothing

        Dim lat As Double = 0
        Dim lon As Double = 0

        Double.TryParse(parts(0), lat)
        Double.TryParse(parts(1), lon)

        Return New LatLong() With {.Latitude = lat, .Longitude = lon}
    End Function

    <Extension>
    Public Function ToCityAndRegion(s As String) As CityAndRegion?
        If Not s.Contains(","c) Then Return Nothing

        Dim parts() As String = s.Split(","c)
        If parts.Length < 2 Then Return Nothing

        Return New CityAndRegion() With {.City = parts(0), .Region = parts(1)}
    End Function

End Module

Imports System.Reflection
Imports FastMember
Imports System.Linq
Imports System.Collections.Generic

Public Class Parser

    ''' <summary>
    ''' Parse the tag value into a object
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="tag"></param>
    ''' <returns></returns>
    Public Shared Function Parse(Of T As New)(tag As String) As T
        Dim type As Type = GetType(T)
        Dim accessor As TypeAccessor = TypeAccessor.Create(type)
        'Dim member As MemberSet = accessor.GetMembers()
        Dim props As PropertyInfo() = type.GetProperties()
        Dim result As New T()
        For Each p As PropertyInfo In props
            Dim attribute As TagValueAttribute = p.GetCustomAttribute(Of TagValueAttribute)(True)
            If attribute IsNot Nothing Then
                Dim value As String = DecodeStr(tag, attribute.Tag, attribute.StartSeparator, attribute.EndSeparator, 2)
                If attribute.DynamicProperty Then
                    Dim paramNameList As String() = value.Split(","c)
                    Dim dict = New Dictionary(Of String, String)
                    For Each paramName In paramNameList
                        Dim formmattedParamName As String = If(String.IsNullOrEmpty(attribute.DynamicPropertyStringPattern), paramName, attribute.DynamicPropertyStringPattern.Replace("[key]", paramName))
                        If Not dict.ContainsKey(formmattedParamName) Then
                            dict.Add(formmattedParamName, DecodeStr(tag, formmattedParamName, attribute.StartSeparator, attribute.EndSeparator, 2))
                        End If
                    Next
                    accessor(result, p.Name) = dict
                Else
                    Try
                        accessor(result, p.Name) = Convert.ChangeType(value, p.PropertyType)
                    Catch ex As Exception
                        ' no op
                    End Try
                End If


            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' Returns a decoded string
    ''' </summary>
    ''' <param name="inputString">A string value</param>
    ''' <param name="key">A string key value</param>
    ''' <param name="leftSeparator">Left separator character</param>
    ''' <param name="rightSeparator">Right separator character</param>
    ''' <param name="mode">1- keyIn string that was found between separator -"="; 2- keyIn value (right of the keyIn '='); 3- keyIn value (same as 2 but don't require/check the '=';4-keyIn value; right of the keyIn|")</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DecodeStr(inputString As String, key As String, leftSeparator As String, rightSeparator As String, mode As Short) As String
        ' Mode =
        ' 1 - key string that was found between separator
        ' 2 - key value; right of the key="
        ' 3 - key value; same as 2 but don't require/check the =
        ' 4 - key value; right of the key|"
        ' 5 - key value; right of the key<>"
        ' 6 - key value; right of the key:"

        Dim keySeparator As String = ""
        Dim strLine As String
        inputString &= "" ' to avoid nothing error
        Dim result As String = String.Empty
        Select Case mode
            Case 5
                keySeparator = "<>"
            Case 6
                keySeparator = ":"
            Case 4
                keySeparator = "|"
            Case Else
                keySeparator = "="
        End Select

        If leftSeparator.EndsWith(":") OrElse mode = 3 Then
            strLine = String.Format("{0}{1}", leftSeparator, key.Trim).ToUpperInvariant()
        Else
            strLine = String.Format("{0}{1}{2}", leftSeparator, key.Trim, keySeparator).ToUpperInvariant()
        End If

        If inputString.ToUpperInvariant().Contains(strLine) Then
            Dim startIndex As Integer = inputString.ToUpperInvariant().IndexOf(strLine) + leftSeparator.Length
            Dim endIndex As Integer = inputString.IndexOf(rightSeparator, startIndex)

            If endIndex > 0 Then
                strLine = inputString.Substring(startIndex, endIndex - startIndex)

                If mode = 1 Then
                    result = strLine
                Else
                    Dim separatorIndex As Integer = strLine.IndexOf(keySeparator) + If(mode = 5, 2, 1)
                    If separatorIndex > 0 Then result = strLine.Substring(separatorIndex).Trim
                End If
            End If
        End If

        Return result
    End Function
End Class

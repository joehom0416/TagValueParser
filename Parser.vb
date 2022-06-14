Imports System.Reflection
Imports FastMember
Imports System.Linq
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
                Dim value As String = DecodeStr(tag, attribute.Tag, "[/", "/]", 2)
                accessor(result, p.Name) = Convert.ChangeType(value, p.PropertyType)
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' Returns a decoded string (this function copy from
    ''' </summary>
    ''' <param name="stringIn">A string value</param>
    ''' <param name="keyIn">A string key value</param>
    ''' <param name="leftSep">Left separator character</param>
    ''' <param name="rightSep">Right separator character</param>
    ''' <param name="mode">1- keyIn string that was found between separator -"="; 2- keyIn value (right of the keyIn '='); 3- keyIn value (same as 2 but don't require/check the '=';4-keyIn value; right of the keyIn|")</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DecodeStr(ByVal stringIn As String, ByVal keyIn As String, ByVal leftSep As String, ByVal rightSep As String, ByVal mode As Short) As String
        'Mode =
        '1 - keyIn string that was found between separator
        '2 - keyIn value; right of the keyIn="
        '3 - keyIn value; same as 2 but don't require/check the =
        '4 - keyIn value; right of the keyIn|"
        '5 - keyIn value; right of the keyIn<>"
        '6 - keyIn value; right of the keyIn:"
        Dim loc As Integer
        Dim loc1 As Integer
        Dim lenLSep As Integer     ' length left separator
        Dim keySeparator As String
        Dim strLine As String
        stringIn &= ""  ' to avoid nothing error

        If mode = 5 Then
            keySeparator = "<>"
        ElseIf mode = 6 Then
            keySeparator = ":"
        ElseIf mode = 4 Then
            keySeparator = "|"
        Else
            keySeparator = "="
        End If

        If leftSep.EndsWith(":") OrElse mode = 3 Then
            strLine = System.Convert.ToString(leftSep & keyIn.Trim).ToUpperInvariant()
        Else

            strLine = System.Convert.ToString(leftSep & keyIn.Trim & keySeparator).ToUpperInvariant()
        End If

        loc = (stringIn.ToUpperInvariant().IndexOf(strLine.ToUpperInvariant(), 0) + 1)
        lenLSep = leftSep.Length
        DecodeStr = ""

        If loc > 0 Then
            loc1 = (stringIn.IndexOf(rightSep, loc + lenLSep - 1) + 1)
            If loc1 > 0 Then
                strLine = stringIn.Substring(loc + lenLSep - 1, loc1 - loc - lenLSep)
                If mode = 1 Then
                    DecodeStr = strLine
                Else
                    loc = (strLine.IndexOf(keySeparator, 0) + IIf(mode = 5, 2, 1))
                    If loc > 0 Then DecodeStr = System.Convert.ToString(strLine.Substring(loc)).Trim
                End If
            End If
        End If
        '      Exit Function

        'Err_Handler:
        '      SetError(Err.Number, Err.Description, FunctionName, False) ' In the Same Class
    End Function
End Class

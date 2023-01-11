<AttributeUsage(AttributeTargets.Property, Inherited:=False)>
Public Class TagValueAttribute
    Inherits Attribute

    Public Property Tag As String
    Public Property DynamicProperty As Boolean = False
    Public Property DynamicPropertyStringPattern As String ' the parameter place holder must use [key], example parameter.[key]
    Public Property StartSeparator As String = "[/" 'Left separator character
    Public Property EndSeparator As String = "/]" 'Left separator character
    Public Sub New(tag As String)
        Me.Tag = tag
    End Sub

    ''' <summary>
    ''' specific start and end separator
    ''' </summary>
    ''' <param name="tag">tag value</param>
    ''' <param name="startSeparator">start separator</param>
    ''' <param name="endSeparator">end separator</param>
    Public Sub New(tag As String, format As TagFormat)
        Me.Tag = tag

        Select Case format
            Case TagFormat.Parentheses
                StartSeparator = "("
                EndSeparator = ")"
            Case TagFormat.Bracket
                StartSeparator = "["
                EndSeparator = "]"
        End Select
    End Sub

    Public Enum TagFormat As Integer
        BracketSlash = 1
        Bracket = 2
        Parentheses = 3
    End Enum
End Class

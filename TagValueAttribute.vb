<AttributeUsage(AttributeTargets.Property, Inherited:=False)>
Public Class TagValueAttribute
    Inherits Attribute

    Public Property Tag As String
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
    Public Sub New(tag As String, startSeparator As String, endSeparator As String)
        Me.Tag = tag
        Me.StartSeparator = startSeparator
        Me.EndSeparator = endSeparator
    End Sub

End Class

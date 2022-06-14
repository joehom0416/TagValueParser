<AttributeUsage(AttributeTargets.Property, Inherited:=False)>
Public Class TagValueAttribute
    Inherits Attribute

    Public Property Tag As String
    Public Property WithSlash As Boolean = True
    Public Sub New(tag As String)
        Me.Tag = tag
    End Sub

    Public Sub New(tag As String, withSlash As Boolean)
        Me.Tag = tag
        Me.WithSlash = withSlash
    End Sub

End Class

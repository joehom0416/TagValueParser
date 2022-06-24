# TagValueParser
ViewPoint Tag Value parser

# Parse tag value string to object, default tag format will be bracket splash ([/..../] 
parser
```vb
    Dim tag As String = "[/Name=John/]
                             [/Title=Software Engineer/]
                             [/Age=30/] [Other=sasas]"
        Dim human As Human = Parser.Parse(Of Human)(tag)
```
model class
```vb
 Public Class Human
        ''' <summary>
        ''' this tag value is use to get name of user
        ''' </summary>
        ''' <returns></returns>
        <TagValue("Name")>
        Public Property Name As String
        <TagValue("Ttle")>
        Public Property Title As String
        <TagValue("Age")>
        Public Property Age As Integer
        <TagValue("Other","[","]")>
        Public Property Age As Integer
    End Class
```
# Parse tag value string to object with bracket ([...])
```vb
Dim tag As String = "[Parameters=Subject]"
Dim setting As JobTagsSettingSpecial = Parser.Parse(Of JobTagsSettingSpecial)(tag)
```

```vb
    Public Class JobTagsSettingSpecial
        <TagValue("Parameters",TagValueAttribute.TagFormat.Bracket)>
        Public Property Parameters As String
    End Class
```

# Parse tag value string to object with parentheses
```vb
 Dim tag As String = "(Parameters=Subject)"
        Dim setting As JobTagsSettingSpecial = Parser.Parse(Of JobTagsSettingSpecial)(tag)
```
model class
```vb
    Public Class JobTagsSettingSpecial
        <TagValue("Parameters",TagValueAttribute.TagFormat.Parentheses)>
        Public Property Parameters As String
    End Class
```

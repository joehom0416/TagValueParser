# TagValueParser
ViewPoint Tag Value parser

# Parse tag value string to object 
parser
```vb
    Dim tag As String = "[/Name=John/]
                             [/Title=Software Engineer/]
                             [/Age=30/]"
        Dim human As Human = Parser.Parse(Of Human)(tag)
```
model class
```
 Public Class Human
        ''' <summary>
        ''' this tag value is use to get name of user
        ''' </summary>
        ''' <returns></returns>
        <TagValue("Name")>
        Public Property Name As String
        <TagValue("title")>
        Public Property Title As String
        <TagValue("age")>
        Public Property Age As Integer
    End Class
```

# Parse tag value string to object with specific start separator and end separator
parser
```vb
 Dim tag As String = "(Parameters=Subject,Form)"
        Dim setting As JobTagsSettingSpecial = Parser.Parse(Of JobTagsSettingSpecial)(tag)
```
model class
```vb
    Public Class JobTagsSettingSpecial
        <TagValue("Parameters", "(", ")")>
        Public Property Parameters As String
    End Class
```

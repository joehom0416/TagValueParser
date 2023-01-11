# TagValueParser
ViewPoint Tag Value parser

# Parse tag value string to object, default tag format will be bracket splash [/..../] 
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
# Parse tag value string to object with bracket [...]
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

# Parse tag value string to object with parentheses (...)
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
# Parse tag value string from dynamic proprty
```vb
  Dim tag As String = "[/Parameters=Subject,Form/]
                            [/Propety.Subject.1=CSP Password Recovery/]  
                            [/Propety.Form.1=CSPFGO01/] "
        Dim setting As JobTagsSettingChild2 = Parser.Parse(Of JobTagsSettingChild2)(tag)
        Assert.IsTrue(setting.Parameters.Count = 2)
        Assert.IsTrue(setting.Parameters("Propety.Subject.1") = "CSP Password Recovery")
        Assert.IsTrue(setting.Parameters("Propety.Form.1") = "CSPFGO01")
```

model class
```vb
 Public Class JobTagsSettingChild2
        <TagValue("Parameters", DynamicProperty:=True, DynamicPropertyStringPattern:="Propety.[key].1")>
        Public Property Parameters As Dictionary(Of String, String)
    End Class
```

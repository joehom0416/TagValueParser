Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports TagValueParser
<TestClass()> Public Class UnitTest1

    <TestMethod()>
    Public Sub TestParseTagValueStringInt_CorrectBehavior()
        Dim tag As String = "[/Name=John/]
                             [/Title=Software Engineer/]
                             [/Age=30/]"
        Dim obj As Human = Parser.Parse(Of Human)(tag)
        Assert.IsTrue(obj.Name = "John")
        Assert.IsTrue(obj.Title = "Software Engineer")
        Assert.IsTrue(obj.Age = 30)
    End Sub
    <TestMethod()>
    Public Sub TestParseTagValueDateString_CorrectBehavior()
        Dim tag As String = "[/DAILY_RECURS=1/]
                             [/DAILYF_EVERY=1H/]
                             [/DAILYF_ENDTIME=9/6/2022 10:57:27 AM/]
                             [/DAILYF_STARTTIME=8/6/2022 10:03:51 PM/]"

        Dim setting As SchedulerTagSetting = Parser.Parse(Of SchedulerTagSetting)(tag)
        Assert.IsTrue(setting.DailyRecurring = 1)
        Assert.IsTrue(setting.DailyEvery = "1H")
        Assert.IsTrue(setting.EndDate = New DateTime(2022, 6, 9, 10, 57, 27))
        Assert.IsTrue(setting.StartTime = New DateTime(2022, 6, 8, 22, 3, 51))
    End Sub
    <TestMethod()>
    Public Sub TestParseTagValue_IncorrectTypeMapping()
        Dim tag As String = "[/DAILY_RECURS=1/]
                             [/DAILYF_EVERY=1H/]
                             [/DAILYF_ENDTIME=This is string/]
                             [/DAILYF_STARTTIME=8/6/2022 10:03:51 PM/]"

        Dim setting As SchedulerTagSetting = Parser.Parse(Of SchedulerTagSetting)(tag)
        Assert.IsTrue(setting.DailyRecurring = 1)
        Assert.IsTrue(setting.DailyEvery = "1H")
        Assert.IsTrue(setting.EndDate = Nothing)
        Assert.IsTrue(setting.StartTime = New DateTime(2022, 6, 8, 22, 3, 51))
    End Sub

    <TestMethod()>
    Public Sub TestParseTagValueArray_CorrectBehavior()
        Dim tag As String = "[/Parameters=Subject,Form/]"
        Dim setting As JobTagsSetting = Parser.Parse(Of JobTagsSetting)(tag)
        Assert.IsTrue(setting.ParametersInArray.Length = 2)
    End Sub

    <TestMethod()>
    Public Sub TestParseTagValueSpecificSeparator_CorrectBehavior()
        Dim tag As String = "(Parameters=Subject,Form)"
        Dim setting As JobTagsSettingSpecial = Parser.Parse(Of JobTagsSettingSpecial)(tag)
        Assert.IsTrue(setting.Parameters.Split(",").Length = 2)
    End Sub

    <TestMethod()>
    Public Sub TestParseTagValueInherits_CorrectBehavior()
        Dim tag As String = "[/Parameters=Subject,Form/]
                            [/Subject=CSP Password Recovery/]  
                            [/Form=CSPFGO01/] "
        Dim setting As JobTagsSettingChild = Parser.Parse(Of JobTagsSettingChild)(tag)
        Assert.IsTrue(setting.ParametersInArray.Length = 2)
        Assert.IsTrue(setting.Subject = "CSP Password Recovery")
        Assert.IsTrue(setting.Form = "CSPFGO01")
    End Sub


    <TestMethod()>
    Public Sub TestParams()
        Dim tag As String = "[/Parameters=Subject,Form/]
                            [/Subject=CSP Password Recovery/]  
                            [/Form=CSPFGO01/] "
        Dim setting As JobTagsSettingChild2 = Parser.Parse(Of JobTagsSettingChild2)(tag)
        Assert.IsTrue(setting.Parameters.Count = 2)
        Assert.IsTrue(setting.Parameters("Subject") = "CSP Password Recovery")
        Assert.IsTrue(setting.Parameters("Form") = "CSPFGO01")


    End Sub

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

    ''' <summary>
    ''' Configure in TSNA configuration screen, tag is auto generate based on user configuration.
    ''' Stored in AgentJobSchedule.Schedule
    ''' </summary>
    Public Class SchedulerTagSetting
        ''' <summary>
        ''' recurring per day, 1=once per day, 2= 2 times per day
        ''' </summary>
        <TagValue("DAILY_RECURS")>
        Public Property DailyRecurring As Integer = -1
        <TagValue("DAILYF_EVERY")>
        Public Property DailyEvery As String
        ''' <summary>
        ''' end time of the day
        ''' </summary>
        <TagValue("DAILYF_ENDTIME")>
        Public Property EndDate As DateTime
        ''' <summary>
        ''' start time of the day
        ''' </summary>
        <TagValue("DAILYF_STARTTIME")>
        Public Property StartTime As DateTime
    End Class


    Public Class JobTagsSetting
        <TagValue("Parameters")>
        Public Property Parameters As String
        Public ReadOnly Property ParametersInArray As String()
            Get
                Return Parameters.Split(",")
            End Get
        End Property
    End Class
    Public Class JobTagsSettingSpecial
        <TagValue("Parameters", TagValueAttribute.TagFormat.Parentheses)>
        Public Property Parameters As String
    End Class
    Public Class JobTagsSettingChild
        Inherits JobTagsSetting
        <TagValue("Subject")>
        Public Property Subject As String

        <TagValue("Form")>
        Public Property Form As String
    End Class

    Public Class JobTagsSettingChild2
        <TagValue("Parameters", DynamicProperty:=True)>
        Public Property Parameters As Dictionary(Of String, String)
    End Class
End Class